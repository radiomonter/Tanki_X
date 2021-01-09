namespace WebSocketSharp.Server
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;
    using WebSocketSharp;
    using WebSocketSharp.Net;
    using WebSocketSharp.Net.WebSockets;

    public class HttpServer
    {
        private IPAddress _address;
        private string _hostname;
        private HttpListener _listener;
        private Logger _logger;
        private int _port;
        private Thread _receiveThread;
        private string _rootPath;
        private bool _secure;
        private WebSocketServiceManager _services;
        private volatile ServerState _state;
        private object _sync;
        private bool _windows;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private EventHandler<HttpRequestEventArgs> OnConnect;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private EventHandler<HttpRequestEventArgs> OnDelete;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private EventHandler<HttpRequestEventArgs> OnGet;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private EventHandler<HttpRequestEventArgs> OnHead;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private EventHandler<HttpRequestEventArgs> OnOptions;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private EventHandler<HttpRequestEventArgs> OnPatch;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private EventHandler<HttpRequestEventArgs> OnPost;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private EventHandler<HttpRequestEventArgs> OnPut;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private EventHandler<HttpRequestEventArgs> OnTrace;

        public event EventHandler<HttpRequestEventArgs> OnConnect
        {
            add
            {
                EventHandler<HttpRequestEventArgs> onConnect = this.OnConnect;
                while (true)
                {
                    EventHandler<HttpRequestEventArgs> objB = onConnect;
                    onConnect = Interlocked.CompareExchange<EventHandler<HttpRequestEventArgs>>(ref this.OnConnect, objB + value, onConnect);
                    if (ReferenceEquals(onConnect, objB))
                    {
                        return;
                    }
                }
            }
            remove
            {
                EventHandler<HttpRequestEventArgs> onConnect = this.OnConnect;
                while (true)
                {
                    EventHandler<HttpRequestEventArgs> objB = onConnect;
                    onConnect = Interlocked.CompareExchange<EventHandler<HttpRequestEventArgs>>(ref this.OnConnect, objB - value, onConnect);
                    if (ReferenceEquals(onConnect, objB))
                    {
                        return;
                    }
                }
            }
        }

        public event EventHandler<HttpRequestEventArgs> OnDelete
        {
            add
            {
                EventHandler<HttpRequestEventArgs> onDelete = this.OnDelete;
                while (true)
                {
                    EventHandler<HttpRequestEventArgs> objB = onDelete;
                    onDelete = Interlocked.CompareExchange<EventHandler<HttpRequestEventArgs>>(ref this.OnDelete, objB + value, onDelete);
                    if (ReferenceEquals(onDelete, objB))
                    {
                        return;
                    }
                }
            }
            remove
            {
                EventHandler<HttpRequestEventArgs> onDelete = this.OnDelete;
                while (true)
                {
                    EventHandler<HttpRequestEventArgs> objB = onDelete;
                    onDelete = Interlocked.CompareExchange<EventHandler<HttpRequestEventArgs>>(ref this.OnDelete, objB - value, onDelete);
                    if (ReferenceEquals(onDelete, objB))
                    {
                        return;
                    }
                }
            }
        }

        public event EventHandler<HttpRequestEventArgs> OnGet
        {
            add
            {
                EventHandler<HttpRequestEventArgs> onGet = this.OnGet;
                while (true)
                {
                    EventHandler<HttpRequestEventArgs> objB = onGet;
                    onGet = Interlocked.CompareExchange<EventHandler<HttpRequestEventArgs>>(ref this.OnGet, objB + value, onGet);
                    if (ReferenceEquals(onGet, objB))
                    {
                        return;
                    }
                }
            }
            remove
            {
                EventHandler<HttpRequestEventArgs> onGet = this.OnGet;
                while (true)
                {
                    EventHandler<HttpRequestEventArgs> objB = onGet;
                    onGet = Interlocked.CompareExchange<EventHandler<HttpRequestEventArgs>>(ref this.OnGet, objB - value, onGet);
                    if (ReferenceEquals(onGet, objB))
                    {
                        return;
                    }
                }
            }
        }

        public event EventHandler<HttpRequestEventArgs> OnHead
        {
            add
            {
                EventHandler<HttpRequestEventArgs> onHead = this.OnHead;
                while (true)
                {
                    EventHandler<HttpRequestEventArgs> objB = onHead;
                    onHead = Interlocked.CompareExchange<EventHandler<HttpRequestEventArgs>>(ref this.OnHead, objB + value, onHead);
                    if (ReferenceEquals(onHead, objB))
                    {
                        return;
                    }
                }
            }
            remove
            {
                EventHandler<HttpRequestEventArgs> onHead = this.OnHead;
                while (true)
                {
                    EventHandler<HttpRequestEventArgs> objB = onHead;
                    onHead = Interlocked.CompareExchange<EventHandler<HttpRequestEventArgs>>(ref this.OnHead, objB - value, onHead);
                    if (ReferenceEquals(onHead, objB))
                    {
                        return;
                    }
                }
            }
        }

        public event EventHandler<HttpRequestEventArgs> OnOptions
        {
            add
            {
                EventHandler<HttpRequestEventArgs> onOptions = this.OnOptions;
                while (true)
                {
                    EventHandler<HttpRequestEventArgs> objB = onOptions;
                    onOptions = Interlocked.CompareExchange<EventHandler<HttpRequestEventArgs>>(ref this.OnOptions, objB + value, onOptions);
                    if (ReferenceEquals(onOptions, objB))
                    {
                        return;
                    }
                }
            }
            remove
            {
                EventHandler<HttpRequestEventArgs> onOptions = this.OnOptions;
                while (true)
                {
                    EventHandler<HttpRequestEventArgs> objB = onOptions;
                    onOptions = Interlocked.CompareExchange<EventHandler<HttpRequestEventArgs>>(ref this.OnOptions, objB - value, onOptions);
                    if (ReferenceEquals(onOptions, objB))
                    {
                        return;
                    }
                }
            }
        }

        public event EventHandler<HttpRequestEventArgs> OnPatch
        {
            add
            {
                EventHandler<HttpRequestEventArgs> onPatch = this.OnPatch;
                while (true)
                {
                    EventHandler<HttpRequestEventArgs> objB = onPatch;
                    onPatch = Interlocked.CompareExchange<EventHandler<HttpRequestEventArgs>>(ref this.OnPatch, objB + value, onPatch);
                    if (ReferenceEquals(onPatch, objB))
                    {
                        return;
                    }
                }
            }
            remove
            {
                EventHandler<HttpRequestEventArgs> onPatch = this.OnPatch;
                while (true)
                {
                    EventHandler<HttpRequestEventArgs> objB = onPatch;
                    onPatch = Interlocked.CompareExchange<EventHandler<HttpRequestEventArgs>>(ref this.OnPatch, objB - value, onPatch);
                    if (ReferenceEquals(onPatch, objB))
                    {
                        return;
                    }
                }
            }
        }

        public event EventHandler<HttpRequestEventArgs> OnPost
        {
            add
            {
                EventHandler<HttpRequestEventArgs> onPost = this.OnPost;
                while (true)
                {
                    EventHandler<HttpRequestEventArgs> objB = onPost;
                    onPost = Interlocked.CompareExchange<EventHandler<HttpRequestEventArgs>>(ref this.OnPost, objB + value, onPost);
                    if (ReferenceEquals(onPost, objB))
                    {
                        return;
                    }
                }
            }
            remove
            {
                EventHandler<HttpRequestEventArgs> onPost = this.OnPost;
                while (true)
                {
                    EventHandler<HttpRequestEventArgs> objB = onPost;
                    onPost = Interlocked.CompareExchange<EventHandler<HttpRequestEventArgs>>(ref this.OnPost, objB - value, onPost);
                    if (ReferenceEquals(onPost, objB))
                    {
                        return;
                    }
                }
            }
        }

        public event EventHandler<HttpRequestEventArgs> OnPut
        {
            add
            {
                EventHandler<HttpRequestEventArgs> onPut = this.OnPut;
                while (true)
                {
                    EventHandler<HttpRequestEventArgs> objB = onPut;
                    onPut = Interlocked.CompareExchange<EventHandler<HttpRequestEventArgs>>(ref this.OnPut, objB + value, onPut);
                    if (ReferenceEquals(onPut, objB))
                    {
                        return;
                    }
                }
            }
            remove
            {
                EventHandler<HttpRequestEventArgs> onPut = this.OnPut;
                while (true)
                {
                    EventHandler<HttpRequestEventArgs> objB = onPut;
                    onPut = Interlocked.CompareExchange<EventHandler<HttpRequestEventArgs>>(ref this.OnPut, objB - value, onPut);
                    if (ReferenceEquals(onPut, objB))
                    {
                        return;
                    }
                }
            }
        }

        public event EventHandler<HttpRequestEventArgs> OnTrace
        {
            add
            {
                EventHandler<HttpRequestEventArgs> onTrace = this.OnTrace;
                while (true)
                {
                    EventHandler<HttpRequestEventArgs> objB = onTrace;
                    onTrace = Interlocked.CompareExchange<EventHandler<HttpRequestEventArgs>>(ref this.OnTrace, objB + value, onTrace);
                    if (ReferenceEquals(onTrace, objB))
                    {
                        return;
                    }
                }
            }
            remove
            {
                EventHandler<HttpRequestEventArgs> onTrace = this.OnTrace;
                while (true)
                {
                    EventHandler<HttpRequestEventArgs> objB = onTrace;
                    onTrace = Interlocked.CompareExchange<EventHandler<HttpRequestEventArgs>>(ref this.OnTrace, objB - value, onTrace);
                    if (ReferenceEquals(onTrace, objB))
                    {
                        return;
                    }
                }
            }
        }

        public HttpServer()
        {
            this.init("*", IPAddress.Any, 80, false);
        }

        public HttpServer(int port) : this(port, port == 0x1bb)
        {
        }

        public HttpServer(string url)
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
            this.init(dnsSafeHost, address, uri.Port, uri.Scheme == "https");
        }

        public HttpServer(int port, bool secure)
        {
            if (!port.IsPortNumber())
            {
                throw new ArgumentOutOfRangeException("port", "Not between 1 and 65535 inclusive: " + port);
            }
            this.init("*", IPAddress.Any, port, secure);
        }

        public HttpServer(IPAddress address, int port) : this(address, port, port == 0x1bb)
        {
        }

        public HttpServer(IPAddress address, int port, bool secure)
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
            this._services.Stop(new CloseEventArgs(CloseStatusCode.ServerError), true, false);
            this._listener.Abort();
            this._state = 3;
        }

        public void AddWebSocketService<TBehaviorWithNew>(string path) where TBehaviorWithNew: WebSocketBehavior, new()
        {
            this.AddWebSocketService<TBehaviorWithNew>(path, new Func<TBehaviorWithNew>(HttpServer.<AddWebSocketService`1>m__0<TBehaviorWithNew>));
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

        private string checkIfCertificateExists()
        {
            if (this._secure)
            {
                bool flag = !ReferenceEquals(this._listener.SslConfiguration.ServerCertificate, null);
                bool flag2 = EndPointListener.CertificateExists(this._port, this._listener.CertificateFolderPath);
                if (!flag || !flag2)
                {
                    return ((flag || flag2) ? null : "The secure connection requires a server certificate.");
                }
                this._logger.Warn("The server certificate associated with the port number already exists.");
            }
            return null;
        }

        public byte[] GetFile(string path)
        {
            path = this.RootPath + path;
            if (this._windows)
            {
                path = path.Replace("/", @"\");
            }
            return (!File.Exists(path) ? null : File.ReadAllBytes(path));
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
            this._listener = new HttpListener();
            this._listener.Prefixes.Add($"http{!secure ? string.Empty : "s"}://{this._hostname}:{port}/");
            this._logger = this._listener.Log;
            this._services = new WebSocketServiceManager(this._logger);
            this._sync = new object();
            OperatingSystem oSVersion = Environment.OSVersion;
            this._windows = (oSVersion.Platform != PlatformID.Unix) && (oSVersion.Platform != PlatformID.MacOSX);
        }

        private void processRequest(HttpListenerContext context)
        {
            string httpMethod = context.Request.HttpMethod;
            EventHandler<HttpRequestEventArgs> handler = (httpMethod != "GET") ? ((httpMethod != "HEAD") ? ((httpMethod != "POST") ? ((httpMethod != "PUT") ? ((httpMethod != "DELETE") ? ((httpMethod != "OPTIONS") ? ((httpMethod != "TRACE") ? ((httpMethod != "CONNECT") ? ((httpMethod != "PATCH") ? null : this.OnPatch) : this.OnConnect) : this.OnTrace) : this.OnOptions) : this.OnDelete) : this.OnPut) : this.OnPost) : this.OnHead) : this.OnGet;
            if (handler != null)
            {
                handler(this, new HttpRequestEventArgs(context));
            }
            else
            {
                context.Response.StatusCode = 0x1f5;
            }
            context.Response.Close();
        }

        private void processRequest(HttpListenerWebSocketContext context)
        {
            WebSocketServiceHost host;
            if (!this._services.InternalTryGetServiceHost(context.RequestUri.AbsolutePath, out host))
            {
                context.Close(HttpStatusCode.NotImplemented);
            }
            else
            {
                host.StartSession(context);
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
                        ctx = this._listener.GetContext()
                    };
                    ThreadPool.QueueUserWorkItem(new WaitCallback(storey.<>m__0));
                    continue;
                }
                catch (HttpListenerException exception)
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
            this._services.Stop(new CloseEventArgs(), true, true);
            this.stopReceiving(0x1388);
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
            if (code == 0x3ed)
            {
                this._services.Stop(new CloseEventArgs(), true, true);
            }
            else
            {
                bool send = !code.IsReserved();
                this._services.Stop(new CloseEventArgs(code, reason), send, send);
            }
            this.stopReceiving(0x1388);
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
            if (code == CloseStatusCode.NoStatus)
            {
                this._services.Stop(new CloseEventArgs(), true, true);
            }
            else
            {
                bool send = !code.IsReserved();
                this._services.Stop(new CloseEventArgs(code, reason), send, send);
            }
            this.stopReceiving(0x1388);
            this._state = 3;
        }

        private void stopReceiving(int millisecondsTimeout)
        {
            this._listener.Close();
            this._receiveThread.Join(millisecondsTimeout);
        }

        private static bool tryCreateUri(string uriString, out Uri result, out string message)
        {
            result = null;
            Uri uri = uriString.ToUri();
            if (uri == null)
            {
                message = "An invalid URI string: " + uriString;
                return false;
            }
            if (!uri.IsAbsoluteUri)
            {
                message = "Not an absolute URI: " + uriString;
                return false;
            }
            string scheme = uri.Scheme;
            if ((scheme != "http") && (scheme != "https"))
            {
                message = "The scheme part isn't 'http' or 'https': " + uriString;
                return false;
            }
            if (uri.PathAndQuery != "/")
            {
                message = "Includes the path or query component: " + uriString;
                return false;
            }
            if (uri.Fragment.Length > 0)
            {
                message = "Includes the fragment component: " + uriString;
                return false;
            }
            if (uri.Port == 0)
            {
                message = "The port part is zero: " + uriString;
                return false;
            }
            result = uri;
            message = string.Empty;
            return true;
        }

        public IPAddress Address =>
            this._address;

        public WebSocketSharp.Net.AuthenticationSchemes AuthenticationSchemes
        {
            get => 
                this._listener.AuthenticationSchemes;
            set
            {
                string message = this._state.CheckIfAvailable(true, false, false);
                if (message != null)
                {
                    this._logger.Error(message);
                }
                else
                {
                    this._listener.AuthenticationSchemes = value;
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
                this._listener.Realm;
            set
            {
                string message = this._state.CheckIfAvailable(true, false, false);
                if (message != null)
                {
                    this._logger.Error(message);
                }
                else
                {
                    this._listener.Realm = value;
                }
            }
        }

        public bool ReuseAddress
        {
            get => 
                this._listener.ReuseAddress;
            set
            {
                string message = this._state.CheckIfAvailable(true, false, false);
                if (message != null)
                {
                    this._logger.Error(message);
                }
                else
                {
                    this._listener.ReuseAddress = value;
                }
            }
        }

        public string RootPath
        {
            get
            {
                string text2;
                if ((this._rootPath != null) && (this._rootPath.Length > 0))
                {
                    text2 = this._rootPath;
                }
                else
                {
                    text2 = this._rootPath = "./Public";
                }
                return text2;
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
                    this._rootPath = value;
                }
            }
        }

        public ServerSslConfiguration SslConfiguration
        {
            get => 
                this._listener.SslConfiguration;
            set
            {
                string message = this._state.CheckIfAvailable(true, false, false);
                if (message != null)
                {
                    this._logger.Error(message);
                }
                else
                {
                    this._listener.SslConfiguration = value;
                }
            }
        }

        public Func<IIdentity, NetworkCredential> UserCredentialsFinder
        {
            get => 
                this._listener.UserCredentialsFinder;
            set
            {
                string message = this._state.CheckIfAvailable(true, false, false);
                if (message != null)
                {
                    this._logger.Error(message);
                }
                else
                {
                    this._listener.UserCredentialsFinder = value;
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
            internal HttpListenerContext ctx;
            internal HttpServer $this;

            internal void <>m__0(object state)
            {
                try
                {
                    if (this.ctx.Request.IsUpgradeTo("websocket"))
                    {
                        this.$this.processRequest(this.ctx.AcceptWebSocket(null));
                    }
                    else
                    {
                        this.$this.processRequest(this.ctx);
                    }
                }
                catch (Exception exception)
                {
                    this.$this._logger.Fatal(exception.ToString());
                    this.ctx.Connection.Close(true);
                }
            }
        }
    }
}

