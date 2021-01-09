namespace WebSocketSharp.Net
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Security.Cryptography;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading;

    internal sealed class EndPointListener
    {
        private List<HttpListenerPrefix> _all;
        private static readonly string _defaultCertFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private IPEndPoint _endpoint;
        private Dictionary<HttpListenerPrefix, HttpListener> _prefixes;
        private bool _secure;
        private Socket _socket;
        private ServerSslConfiguration _sslConfig;
        private List<HttpListenerPrefix> _unhandled;
        private Dictionary<HttpConnection, HttpConnection> _unregistered;
        private object _unregisteredSync;
        [CompilerGenerated]
        private static AsyncCallback <>f__mg$cache0;
        [CompilerGenerated]
        private static AsyncCallback <>f__mg$cache1;

        internal EndPointListener(IPAddress address, int port, bool reuseAddress, bool secure, string certificateFolderPath, ServerSslConfiguration sslConfig)
        {
            if (secure)
            {
                X509Certificate2 certificate = getCertificate(port, certificateFolderPath, sslConfig.ServerCertificate);
                if (certificate == null)
                {
                    throw new ArgumentException("No server certificate could be found.");
                }
                this._secure = secure;
                this._sslConfig = sslConfig;
                this._sslConfig.ServerCertificate = certificate;
            }
            this._prefixes = new Dictionary<HttpListenerPrefix, HttpListener>();
            this._unregistered = new Dictionary<HttpConnection, HttpConnection>();
            this._unregisteredSync = ((ICollection) this._unregistered).SyncRoot;
            this._socket = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            if (reuseAddress)
            {
                this._socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            }
            this._endpoint = new IPEndPoint(address, port);
            this._socket.Bind(this._endpoint);
            this._socket.Listen(500);
            if (<>f__mg$cache0 == null)
            {
                <>f__mg$cache0 = new AsyncCallback(EndPointListener.onAccept);
            }
            this._socket.BeginAccept(<>f__mg$cache0, this);
        }

        public void AddPrefix(HttpListenerPrefix prefix, HttpListener listener)
        {
            List<HttpListenerPrefix> list;
            List<HttpListenerPrefix> list2;
            if (prefix.Host == "*")
            {
                while (true)
                {
                    list = this._unhandled;
                    list2 = (list == null) ? new List<HttpListenerPrefix>() : new List<HttpListenerPrefix>(list);
                    prefix.Listener = listener;
                    addSpecial(list2, prefix);
                    if (Interlocked.CompareExchange<List<HttpListenerPrefix>>(ref this._unhandled, list2, list) == list)
                    {
                        return;
                    }
                }
            }
            if (prefix.Host == "+")
            {
                while (true)
                {
                    list = this._all;
                    list2 = (list == null) ? new List<HttpListenerPrefix>() : new List<HttpListenerPrefix>(list);
                    prefix.Listener = listener;
                    addSpecial(list2, prefix);
                    if (Interlocked.CompareExchange<List<HttpListenerPrefix>>(ref this._all, list2, list) == list)
                    {
                        return;
                    }
                }
            }
            while (true)
            {
                Dictionary<HttpListenerPrefix, HttpListener> dictionary = this._prefixes;
                if (dictionary.ContainsKey(prefix))
                {
                    if (dictionary[prefix] != listener)
                    {
                        throw new HttpListenerException(400, $"There's another listener for {prefix}.");
                    }
                    return;
                }
                Dictionary<HttpListenerPrefix, HttpListener> dictionary2 = new Dictionary<HttpListenerPrefix, HttpListener>(dictionary) {
                    [prefix] = listener
                };
                if (Interlocked.CompareExchange<Dictionary<HttpListenerPrefix, HttpListener>>(ref this._prefixes, dictionary2, dictionary) == dictionary)
                {
                    return;
                }
            }
        }

        private static void addSpecial(List<HttpListenerPrefix> prefixes, HttpListenerPrefix prefix)
        {
            string path = prefix.Path;
            foreach (HttpListenerPrefix prefix2 in prefixes)
            {
                if (prefix2.Path == path)
                {
                    throw new HttpListenerException(400, "The prefix is already in use.");
                }
            }
            prefixes.Add(prefix);
        }

        public bool BindContext(HttpListenerContext context)
        {
            HttpListenerPrefix prefix;
            HttpListener listener = this.searchListener(context.Request.Url, out prefix);
            if (listener == null)
            {
                return false;
            }
            context.Listener = listener;
            context.Connection.Prefix = prefix;
            return true;
        }

        internal static bool CertificateExists(int port, string certificateFolderPath)
        {
            if ((certificateFolderPath == null) || (certificateFolderPath.Length == 0))
            {
                certificateFolderPath = _defaultCertFolderPath;
            }
            string path = Path.Combine(certificateFolderPath, $"{port}.key");
            return (File.Exists(Path.Combine(certificateFolderPath, $"{port}.cer")) && File.Exists(path));
        }

        private void checkIfRemove()
        {
            if (this._prefixes.Count <= 0)
            {
                List<HttpListenerPrefix> list = this._unhandled;
                if ((list == null) || (list.Count <= 0))
                {
                    list = this._all;
                    if ((list == null) || (list.Count <= 0))
                    {
                        EndPointManager.RemoveEndPoint(this);
                    }
                }
            }
        }

        public void Close()
        {
            this._socket.Close();
            HttpConnection[] array = null;
            lock (this._unregisteredSync)
            {
                if (this._unregistered.Count != 0)
                {
                    Dictionary<HttpConnection, HttpConnection>.KeyCollection keys = this._unregistered.Keys;
                    array = new HttpConnection[keys.Count];
                    keys.CopyTo(array, 0);
                    this._unregistered.Clear();
                }
                else
                {
                    return;
                }
            }
            for (int i = array.Length - 1; i >= 0; i--)
            {
                array[i].Close(true);
            }
        }

        private static RSACryptoServiceProvider createRSAFromFile(string filename)
        {
            byte[] buffer = null;
            using (FileStream stream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
            }
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            provider.ImportCspBlob(buffer);
            return provider;
        }

        private static X509Certificate2 getCertificate(int port, string certificateFolderPath, X509Certificate2 defaultCertificate)
        {
            if ((certificateFolderPath == null) || (certificateFolderPath.Length == 0))
            {
                certificateFolderPath = _defaultCertFolderPath;
            }
            try
            {
                string path = Path.Combine(certificateFolderPath, $"{port}.cer");
                string str2 = Path.Combine(certificateFolderPath, $"{port}.key");
                if (File.Exists(path) && File.Exists(str2))
                {
                    return new X509Certificate2(path) { PrivateKey = createRSAFromFile(str2) };
                }
            }
            catch
            {
            }
            return defaultCertificate;
        }

        private static HttpListener matchFromList(string host, string path, List<HttpListenerPrefix> list, out HttpListenerPrefix prefix)
        {
            prefix = null;
            if (list == null)
            {
                return null;
            }
            HttpListener listener = null;
            int length = -1;
            foreach (HttpListenerPrefix prefix2 in list)
            {
                string str = prefix2.Path;
                if ((str.Length >= length) && path.StartsWith(str))
                {
                    length = str.Length;
                    listener = prefix2.Listener;
                    prefix = prefix2;
                }
            }
            return listener;
        }

        private static void onAccept(IAsyncResult asyncResult)
        {
            EndPointListener asyncState = (EndPointListener) asyncResult.AsyncState;
            Socket socket = null;
            try
            {
                socket = asyncState._socket.EndAccept(asyncResult);
            }
            catch (SocketException)
            {
            }
            catch (ObjectDisposedException)
            {
                return;
            }
            try
            {
                if (<>f__mg$cache1 == null)
                {
                    <>f__mg$cache1 = new AsyncCallback(EndPointListener.onAccept);
                }
                asyncState._socket.BeginAccept(<>f__mg$cache1, asyncState);
            }
            catch
            {
                if (socket != null)
                {
                    socket.Close();
                }
                return;
            }
            if (socket != null)
            {
                processAccepted(socket, asyncState);
            }
        }

        private static void processAccepted(Socket socket, EndPointListener listener)
        {
            HttpConnection connection = null;
            try
            {
                connection = new HttpConnection(socket, listener);
                lock (listener._unregisteredSync)
                {
                    listener._unregistered[connection] = connection;
                }
                connection.BeginReadRequest();
            }
            catch
            {
                if (connection != null)
                {
                    connection.Close(true);
                }
                else
                {
                    socket.Close();
                }
            }
        }

        internal void RemoveConnection(HttpConnection connection)
        {
            lock (this._unregisteredSync)
            {
                this._unregistered.Remove(connection);
            }
        }

        public void RemovePrefix(HttpListenerPrefix prefix, HttpListener listener)
        {
            List<HttpListenerPrefix> list;
            List<HttpListenerPrefix> list2;
            if (prefix.Host == "*")
            {
                while (true)
                {
                    list = this._unhandled;
                    if (list != null)
                    {
                        list2 = new List<HttpListenerPrefix>(list);
                        if (removeSpecial(list2, prefix) && (Interlocked.CompareExchange<List<HttpListenerPrefix>>(ref this._unhandled, list2, list) != list))
                        {
                            continue;
                        }
                    }
                    this.checkIfRemove();
                    return;
                }
            }
            if (prefix.Host == "+")
            {
                while (true)
                {
                    list = this._all;
                    if (list != null)
                    {
                        list2 = new List<HttpListenerPrefix>(list);
                        if (removeSpecial(list2, prefix) && (Interlocked.CompareExchange<List<HttpListenerPrefix>>(ref this._all, list2, list) != list))
                        {
                            continue;
                        }
                    }
                    this.checkIfRemove();
                    return;
                }
            }
            while (true)
            {
                Dictionary<HttpListenerPrefix, HttpListener> dictionary = this._prefixes;
                if (dictionary.ContainsKey(prefix))
                {
                    Dictionary<HttpListenerPrefix, HttpListener> dictionary2 = new Dictionary<HttpListenerPrefix, HttpListener>(dictionary);
                    dictionary2.Remove(prefix);
                    if (Interlocked.CompareExchange<Dictionary<HttpListenerPrefix, HttpListener>>(ref this._prefixes, dictionary2, dictionary) != dictionary)
                    {
                        continue;
                    }
                }
                this.checkIfRemove();
                return;
            }
        }

        private static bool removeSpecial(List<HttpListenerPrefix> prefixes, HttpListenerPrefix prefix)
        {
            string path = prefix.Path;
            int count = prefixes.Count;
            for (int i = 0; i < count; i++)
            {
                if (prefixes[i].Path == path)
                {
                    prefixes.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        private HttpListener searchListener(Uri uri, out HttpListenerPrefix prefix)
        {
            prefix = null;
            if (uri == null)
            {
                return null;
            }
            string host = uri.Host;
            bool flag = Uri.CheckHostName(host) == UriHostNameType.Dns;
            int port = uri.Port;
            string path = HttpUtility.UrlDecode(uri.AbsolutePath);
            string str3 = (path[path.Length - 1] != '/') ? (path + "/") : path;
            HttpListener listener = null;
            int length = -1;
            if ((host != null) && (host.Length > 0))
            {
                foreach (HttpListenerPrefix prefix2 in this._prefixes.Keys)
                {
                    string str4 = prefix2.Path;
                    if ((str4.Length >= length) && (prefix2.Port == port))
                    {
                        if (flag)
                        {
                            string name = prefix2.Host;
                            if ((Uri.CheckHostName(name) == UriHostNameType.Dns) && (name != host))
                            {
                                continue;
                            }
                        }
                        if (path.StartsWith(str4) || str3.StartsWith(str4))
                        {
                            length = str4.Length;
                            listener = this._prefixes[prefix2];
                            prefix = prefix2;
                        }
                    }
                }
                if (length != -1)
                {
                    return listener;
                }
            }
            List<HttpListenerPrefix> list = this._unhandled;
            listener = matchFromList(host, path, list, out prefix);
            if (path != str3)
            {
                listener ??= matchFromList(host, str3, list, out prefix);
            }
            if (listener != null)
            {
                return listener;
            }
            list = this._all;
            listener = matchFromList(host, path, list, out prefix);
            if (path != str3)
            {
                listener ??= matchFromList(host, str3, list, out prefix);
            }
            return ((listener == null) ? null : listener);
        }

        public void UnbindContext(HttpListenerContext context)
        {
            if ((context != null) && (context.Listener != null))
            {
                context.Listener.UnregisterContext(context);
            }
        }

        public IPAddress Address =>
            this._endpoint.Address;

        public bool IsSecure =>
            this._secure;

        public int Port =>
            this._endpoint.Port;

        public ServerSslConfiguration SslConfiguration =>
            this._sslConfig;
    }
}

