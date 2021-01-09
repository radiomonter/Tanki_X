namespace WebSocketSharp.Server
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;
    using WebSocketSharp;
    using WebSocketSharp.Net;
    using WebSocketSharp.Net.WebSockets;

    public class WebSocketServer
    {
        private IPAddress _address;
        private WebSocketSharp.Net.AuthenticationSchemes _authSchemes;
        private static readonly string _defaultRealm = "SECRET AREA";
        private bool _dnsStyle;
        private string _hostname;
        private TcpListener _listener;
        private Logger _logger;
        private int _port;
        private string _realm;
        private Thread _receiveThread;
        private bool _reuseAddress;
        private bool _secure;
        private WebSocketServiceManager _services;
        private ServerSslConfiguration _sslConfig;
        private volatile ServerState _state;
        private object _sync;
        private Func<IIdentity, NetworkCredential> _userCredFinder;

        public WebSocketServer()
        {
            this.init(null, IPAddress.Any, 80, false);
        }

        public WebSocketServer(int port) : this(port, port == 0x1bb)
        {
        }

        public WebSocketServer(string url)
        {
            Uri uri;
            string str;
            if (url == null)
            {
                throw new ArgumentNullException("url");
            }
            if (url.Length == 0)
            {
                throw new ArgumentException("An empty string.", "url");
            }
            if (!tryCreateUri(url, out uri, out str))
            {
                throw new ArgumentException(str, "url");
            }
            string dnsSafeHost = uri.DnsSafeHost;
            IPAddress address = dnsSafeHost.ToIPAddress();
            if (!address.IsLocal())
            {
                throw new ArgumentException("The host part isn't a local host name: " + url, "url");
            }
            this.init(dnsSafeHost, address, uri.Port, uri.Scheme == "wss");
        }

        public WebSocketServer(int port, bool secure)
        {
            if (!port.IsPortNumber())
            {
                throw new ArgumentOutOfRangeException("port", "Not between 1 and 65535 inclusive: " + port);
            }
            this.init(null, IPAddress.Any, port, secure);
        }

        public WebSocketServer(IPAddress address, int port) : this(address, port, port == 0x1bb)
        {
        }

        public WebSocketServer(IPAddress address, int port, bool secure)
        {
            if (address == null)
            {
                throw new ArgumentNullException("address");
            }
            if (!address.IsLocal())
            {
                throw new ArgumentException("Not a local IP address: " + address, "address");
            }
            if (!port.IsPortNumber())
            {
                throw new ArgumentOutOfRangeException("port", "Not between 1 and 65535 inclusive: " + port);
            }
            this.init(null, address, port, secure);
        }

        [CompilerGenerated]
        private static TBehaviorWithNew <AddWebSocketService`1>m__0<TBehaviorWithNew>() where TBehaviorWithNew: WebSocketBehavior, new() => 
            Activator.CreateInstance<TBehaviorWithNew>();

        private void abort()
        {
            lock (this._sync)
            {
                if (this.IsListening)
                {
                    this._state = 2;
                }
                else
                {
                    return;
                }
            }
            this._listener.Stop();
            this._services.Stop(new CloseEventArgs(CloseStatusCode.ServerError), true, false);
            this._state = 3;
        }

        public void AddWebSocketService<TBehaviorWithNew>(string path) where TBehaviorWithNew: WebSocketBehavior, new()
        {
            this.AddWebSocketService<TBehaviorWithNew>(path, new Func<TBehaviorWithNew>(WebSocketServer.<AddWebSocketService`1>m__0<TBehaviorWithNew>));
        }

        public void AddWebSocketService<TBehavior>(string path, Func<TBehavior> initializer) where TBehavior: WebSocketBehavior
        {
            string message = path.CheckIfValidServicePath() ?? ((initializer != null) ? null : "'initializer' is null.");
            if (message != null)
            {
                this._logger.Error(message);
            }
            else
            {
                this._services.Add<TBehavior>(path, initializer);
            }
        }

        private string checkIfCertificateExists() => 
            (!this._secure || ((this._sslConfig != null) && (this._sslConfig.ServerCertificate != null))) ? null : "The secure connection requires a server certificate.";

        private string getRealm()
        {
            string str = this._realm;
            return (((str == null) || (str.Length <= 0)) ? _defaultRealm : str);
        }

        private void init(string hostname, IPAddress address, int port, bool secure)
        {
            string text1 = hostname;
            if (hostname == null)
            {
                string local1 = hostname;
                text1 = address.ToString();
            }
            this._hostname = text1;
            this._address = address;
            this._port = port;
            this._secure = secure;
            this._authSchemes = WebSocketSharp.Net.AuthenticationSchemes.Anonymous;
            this._dnsStyle = Uri.CheckHostName(hostname) == UriHostNameType.Dns;
            this._listener = new TcpListener(address, port);
            this._logger = new Logger();
            this._services = new WebSocketServiceManager(this._logger);
            this._sync = new object();
        }

        private void processRequest(TcpListenerWebSocketContext context)
        {
            Uri requestUri = context.RequestUri;
            if ((requestUri == null) || (requestUri.Port != this._port))
            {
                context.Close(HttpStatusCode.BadRequest);
            }
            else
            {
                WebSocketServiceHost host;
                if (this._dnsStyle)
                {
                    string dnsSafeHost = requestUri.DnsSafeHost;
                    if ((Uri.CheckHostName(dnsSafeHost) == UriHostNameType.Dns) && (dnsSafeHost != this._hostname))
                    {
                        context.Close(HttpStatusCode.NotFound);
                        return;
                    }
                }
                if (!this._services.InternalTryGetServiceHost(requestUri.AbsolutePath, out host))
                {
                    context.Close(HttpStatusCode.NotImplemented);
                }
                else
                {
                    host.StartSession(context);
                }
            }
        }

        private void receiveRequest()
        {
            while (true)
            {
                try
                {
                    <receiveRequest>c__AnonStorey0 storey = new <receiveRequest>c__AnonStorey0 {
                        $this = this,
                        cl = this._listener.AcceptTcpClient()
                    };
                    ThreadPool.QueueUserWorkItem(new WaitCallback(storey.<>m__0));
                    continue;
                }
                catch (SocketException exception)
                {
                    this._logger.Warn("Receiving has been stopped.\n  reason: " + exception.Message);
                }
                catch (Exception exception2)
                {
                    this._logger.Fatal(exception2.ToString());
                }
                if (this.IsListening)
                {
                    this.abort();
                }
                return;
            }
        }

        public bool RemoveWebSocketService(string path)
        {
            string message = path.CheckIfValidServicePath();
            if (message == null)
            {
                return this._services.Remove(path);
            }
            this._logger.Error(message);
            return false;
        }

        public void Start()
        {
            lock (this._sync)
            {
                string message = this._state.CheckIfAvailable(true, false, false) ?? this.checkIfCertificateExists();
                if (message != null)
                {
                    this._logger.Error(message);
                }
                else
                {
                    this._services.Start();
                    this.startReceiving();
                    this._state = 1;
                }
            }
        }

        private void startReceiving()
        {
            if (this._reuseAddress)
            {
                this._listener.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            }
            this._listener.Start();
            this._receiveThread = new Thread(new ThreadStart(this.receiveRequest));
            this._receiveThread.IsBackground = true;
            this._receiveThread.Start();
        }

        public void Stop()
        {
            lock (this._sync)
            {
                string message = this._state.CheckIfAvailable(false, true, false);
                if (message == null)
                {
                    this._state = 2;
                }
                else
                {
                    this._logger.Error(message);
                    return;
                }
            }
            this.stopReceiving(0x1388);
            this._services.Stop(new CloseEventArgs(), true, true);
            this._state = 3;
        }

        public void Stop(ushort code, string reason)
        {
            lock (this._sync)
            {
                string message = this._state.CheckIfAvailable(false, true, false) ?? WebSocket.CheckCloseParameters(code, reason, false);
                if (message == null)
                {
                    this._state = 2;
                }
                else
                {
                    this._logger.Error(message);
                    return;
                }
            }
            this.stopReceiving(0x1388);
            if (code == 0x3ed)
            {
                this._services.Stop(new CloseEventArgs(), true, true);
            }
            else
            {
                bool send = !code.IsReserved();
                this._services.Stop(new CloseEventArgs(code, reason), send, send);
            }
            this._state = 3;
        }

        public void Stop(CloseStatusCode code, string reason)
        {
            lock (this._sync)
            {
                string message = this._state.CheckIfAvailable(false, true, false) ?? WebSocket.CheckCloseParameters(code, reason, false);
                if (message == null)
                {
                    this._state = 2;
                }
                else
                {
                    this._logger.Error(message);
                    return;
                }
            }
            this.stopReceiving(0x1388);
            if (code == CloseStatusCode.NoStatus)
            {
                this._services.Stop(new CloseEventArgs(), true, true);
            }
            else
            {
                bool send = !code.IsReserved();
                this._services.Stop(new CloseEventArgs(code, reason), send, send);
            }
            this._state = 3;
        }

        private void stopReceiving(int millisecondsTimeout)
        {
            this._listener.Stop();
            this._receiveThread.Join(millisecondsTimeout);
        }

        private static bool tryCreateUri(string uriString, out Uri result, out string message)
        {
            if (uriString.TryCreateWebSocketUri(out result, out message))
            {
                if (result.PathAndQuery == "/")
                {
                    return true;
                }
                result = null;
                message = "Includes the path or query component: " + uriString;
            }
            return false;
        }

        public IPAddress Address =>
            this._address;

        public WebSocketSharp.Net.AuthenticationSchemes AuthenticationSchemes
        {
            get => 
                this._authSchemes;
            set
            {
                string message = this._state.CheckIfAvailable(true, false, false);
                if (message != null)
                {
                    this._logger.Error(message);
                }
                else
                {
                    this._authSchemes = value;
                }
            }
        }

        public bool IsListening =>
            this._state == 1;

        public bool IsSecure =>
            this._secure;

        public bool KeepClean
        {
            get => 
                this._services.KeepClean;
            set
            {
                string message = this._state.CheckIfAvailable(true, false, false);
                if (message != null)
                {
                    this._logger.Error(message);
                }
                else
                {
                    this._services.KeepClean = value;
                }
            }
        }

        public Logger Log =>
            this._logger;

        public int Port =>
            this._port;

        public string Realm
        {
            get => 
                this._realm;
            set
            {
                string message = this._state.CheckIfAvailable(true, false, false);
                if (message != null)
                {
                    this._logger.Error(message);
                }
                else
                {
                    this._realm = value;
                }
            }
        }

        public bool ReuseAddress
        {
            get => 
                this._reuseAddress;
            set
            {
                string message = this._state.CheckIfAvailable(true, false, false);
                if (message != null)
                {
                    this._logger.Error(message);
                }
                else
                {
                    this._reuseAddress = value;
                }
            }
        }

        public ServerSslConfiguration SslConfiguration
        {
            get
            {
                ServerSslConfiguration configuration2 = this._sslConfig;
                if (this._sslConfig == null)
                {
                    ServerSslConfiguration local1 = this._sslConfig;
                    configuration2 = this._sslConfig = new ServerSslConfiguration(null);
                }
                return configuration2;
            }
            set
            {
                string message = this._state.CheckIfAvailable(true, false, false);
                if (message != null)
                {
                    this._logger.Error(message);
                }
                else
                {
                    this._sslConfig = value;
                }
            }
        }

        public Func<IIdentity, NetworkCredential> UserCredentialsFinder
        {
            get => 
                this._userCredFinder;
            set
            {
                string message = this._state.CheckIfAvailable(true, false, false);
                if (message != null)
                {
                    this._logger.Error(message);
                }
                else
                {
                    this._userCredFinder = value;
                }
            }
        }

        public TimeSpan WaitTime
        {
            get => 
                this._services.WaitTime;
            set
            {
                string message = this._state.CheckIfAvailable(true, false, false) ?? value.CheckIfValidWaitTime();
                if (message != null)
                {
                    this._logger.Error(message);
                }
                else
                {
                    this._services.WaitTime = value;
                }
            }
        }

        public WebSocketServiceManager WebSocketServices =>
            this._services;

        [CompilerGenerated]
        private sealed class <receiveRequest>c__AnonStorey0
        {
            internal TcpClient cl;
            internal WebSocketServer $this;

            internal void <>m__0(object state)
            {
                try
                {
                    TcpListenerWebSocketContext context = this.cl.GetWebSocketContext(null, this.$this._secure, this.$this._sslConfig, this.$this._logger);
                    if (context.Authenticate(this.$this._authSchemes, this.$this.getRealm(), this.$this._userCredFinder))
                    {
                        this.$this.processRequest(context);
                    }
                }
                catch (Exception exception)
                {
                    this.$this._logger.Fatal(exception.ToString());
                    this.cl.Close();
                }
            }
        }
    }
}

