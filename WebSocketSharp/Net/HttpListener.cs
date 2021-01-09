namespace WebSocketSharp.Net
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using WebSocketSharp;

    public sealed class HttpListener : IDisposable
    {
        private WebSocketSharp.Net.AuthenticationSchemes _authSchemes = WebSocketSharp.Net.AuthenticationSchemes.Anonymous;
        private Func<HttpListenerRequest, WebSocketSharp.Net.AuthenticationSchemes> _authSchemeSelector;
        private string _certFolderPath;
        private Dictionary<HttpConnection, HttpConnection> _connections = new Dictionary<HttpConnection, HttpConnection>();
        private object _connectionsSync;
        private List<HttpListenerContext> _ctxQueue;
        private object _ctxQueueSync;
        private Dictionary<HttpListenerContext, HttpListenerContext> _ctxRegistry;
        private object _ctxRegistrySync;
        private static readonly string _defaultRealm = "SECRET AREA";
        private bool _disposed;
        private bool _ignoreWriteExceptions;
        private volatile bool _listening;
        private Logger _logger;
        private HttpListenerPrefixCollection _prefixes;
        private string _realm;
        private bool _reuseAddress;
        private ServerSslConfiguration _sslConfig;
        private Func<IIdentity, NetworkCredential> _userCredFinder;
        private List<HttpListenerAsyncResult> _waitQueue;
        private object _waitQueueSync;

        public HttpListener()
        {
            this._connectionsSync = ((ICollection) this._connections).SyncRoot;
            this._ctxQueue = new List<HttpListenerContext>();
            this._ctxQueueSync = ((ICollection) this._ctxQueue).SyncRoot;
            this._ctxRegistry = new Dictionary<HttpListenerContext, HttpListenerContext>();
            this._ctxRegistrySync = ((ICollection) this._ctxRegistry).SyncRoot;
            this._logger = new Logger();
            this._prefixes = new HttpListenerPrefixCollection(this);
            this._waitQueue = new List<HttpListenerAsyncResult>();
            this._waitQueueSync = ((ICollection) this._waitQueue).SyncRoot;
        }

        public void Abort()
        {
            if (!this._disposed)
            {
                this.close(true);
            }
        }

        internal bool AddConnection(HttpConnection connection)
        {
            bool flag;
            if (!this._listening)
            {
                return false;
            }
            lock (this._connectionsSync)
            {
                if (!this._listening)
                {
                    flag = false;
                }
                else
                {
                    this._connections[connection] = connection;
                    flag = true;
                }
            }
            return flag;
        }

        internal HttpListenerAsyncResult BeginGetContext(HttpListenerAsyncResult asyncResult)
        {
            lock (this._ctxRegistrySync)
            {
                if (!this._listening)
                {
                    throw new HttpListenerException(0x3e3);
                }
                HttpListenerContext context = this.getContextFromQueue();
                if (context == null)
                {
                    this._waitQueue.Add(asyncResult);
                }
                else
                {
                    asyncResult.Complete(context, true);
                }
                return asyncResult;
            }
        }

        public IAsyncResult BeginGetContext(AsyncCallback callback, object state)
        {
            this.CheckDisposed();
            if (this._prefixes.Count == 0)
            {
                throw new InvalidOperationException("The listener has no URI prefix on which listens.");
            }
            if (!this._listening)
            {
                throw new InvalidOperationException("The listener hasn't been started.");
            }
            return this.BeginGetContext(new HttpListenerAsyncResult(callback, state));
        }

        internal void CheckDisposed()
        {
            if (this._disposed)
            {
                throw new ObjectDisposedException(base.GetType().ToString());
            }
        }

        private void cleanupConnections()
        {
            HttpConnection[] array = null;
            lock (this._connectionsSync)
            {
                if (this._connections.Count != 0)
                {
                    Dictionary<HttpConnection, HttpConnection>.KeyCollection keys = this._connections.Keys;
                    array = new HttpConnection[keys.Count];
                    keys.CopyTo(array, 0);
                    this._connections.Clear();
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

        private void cleanupContextQueue(bool sendServiceUnavailable)
        {
            HttpListenerContext[] contextArray = null;
            lock (this._ctxQueueSync)
            {
                if (this._ctxQueue.Count != 0)
                {
                    contextArray = this._ctxQueue.ToArray();
                    this._ctxQueue.Clear();
                }
                else
                {
                    return;
                }
            }
            if (sendServiceUnavailable)
            {
                foreach (HttpListenerContext context in contextArray)
                {
                    HttpListenerResponse response = context.Response;
                    response.StatusCode = 0x1f7;
                    response.Close();
                }
            }
        }

        private void cleanupContextRegistry()
        {
            HttpListenerContext[] array = null;
            lock (this._ctxRegistrySync)
            {
                if (this._ctxRegistry.Count != 0)
                {
                    Dictionary<HttpListenerContext, HttpListenerContext>.KeyCollection keys = this._ctxRegistry.Keys;
                    array = new HttpListenerContext[keys.Count];
                    keys.CopyTo(array, 0);
                    this._ctxRegistry.Clear();
                }
                else
                {
                    return;
                }
            }
            for (int i = array.Length - 1; i >= 0; i--)
            {
                array[i].Connection.Close(true);
            }
        }

        private void cleanupWaitQueue(Exception exception)
        {
            HttpListenerAsyncResult[] resultArray = null;
            lock (this._waitQueueSync)
            {
                if (this._waitQueue.Count != 0)
                {
                    resultArray = this._waitQueue.ToArray();
                    this._waitQueue.Clear();
                }
                else
                {
                    return;
                }
            }
            foreach (HttpListenerAsyncResult result in resultArray)
            {
                result.Complete(exception);
            }
        }

        private void close(bool force)
        {
            if (this._listening)
            {
                this._listening = false;
                EndPointManager.RemoveListener(this);
            }
            lock (this._ctxRegistrySync)
            {
                this.cleanupContextQueue(!force);
            }
            this.cleanupContextRegistry();
            this.cleanupConnections();
            this.cleanupWaitQueue(new ObjectDisposedException(base.GetType().ToString()));
            this._disposed = true;
        }

        public void Close()
        {
            if (!this._disposed)
            {
                this.close(false);
            }
        }

        public HttpListenerContext EndGetContext(IAsyncResult asyncResult)
        {
            this.CheckDisposed();
            if (asyncResult == null)
            {
                throw new ArgumentNullException("asyncResult");
            }
            HttpListenerAsyncResult result = asyncResult as HttpListenerAsyncResult;
            if (result == null)
            {
                throw new ArgumentException("A wrong IAsyncResult.", "asyncResult");
            }
            if (result.EndCalled)
            {
                throw new InvalidOperationException("This IAsyncResult cannot be reused.");
            }
            result.EndCalled = true;
            if (!result.IsCompleted)
            {
                result.AsyncWaitHandle.WaitOne();
            }
            return result.GetContext();
        }

        private HttpListenerAsyncResult getAsyncResultFromQueue()
        {
            if (this._waitQueue.Count == 0)
            {
                return null;
            }
            HttpListenerAsyncResult result = this._waitQueue[0];
            this._waitQueue.RemoveAt(0);
            return result;
        }

        public HttpListenerContext GetContext()
        {
            this.CheckDisposed();
            if (this._prefixes.Count == 0)
            {
                throw new InvalidOperationException("The listener has no URI prefix on which listens.");
            }
            if (!this._listening)
            {
                throw new InvalidOperationException("The listener hasn't been started.");
            }
            HttpListenerAsyncResult asyncResult = this.BeginGetContext(new HttpListenerAsyncResult(null, null));
            asyncResult.InGet = true;
            return this.EndGetContext(asyncResult);
        }

        private HttpListenerContext getContextFromQueue()
        {
            if (this._ctxQueue.Count == 0)
            {
                return null;
            }
            HttpListenerContext context = this._ctxQueue[0];
            this._ctxQueue.RemoveAt(0);
            return context;
        }

        internal string GetRealm()
        {
            string str = this._realm;
            return (((str == null) || (str.Length <= 0)) ? _defaultRealm : str);
        }

        internal Func<IIdentity, NetworkCredential> GetUserCredentialsFinder() => 
            this._userCredFinder;

        internal bool RegisterContext(HttpListenerContext context)
        {
            bool flag;
            if (!this._listening)
            {
                return false;
            }
            if (!context.Authenticate())
            {
                return false;
            }
            lock (this._ctxRegistrySync)
            {
                if (!this._listening)
                {
                    flag = false;
                }
                else
                {
                    this._ctxRegistry[context] = context;
                    HttpListenerAsyncResult result = this.getAsyncResultFromQueue();
                    if (result == null)
                    {
                        this._ctxQueue.Add(context);
                    }
                    else
                    {
                        result.Complete(context);
                    }
                    flag = true;
                }
            }
            return flag;
        }

        internal void RemoveConnection(HttpConnection connection)
        {
            lock (this._connectionsSync)
            {
                this._connections.Remove(connection);
            }
        }

        internal WebSocketSharp.Net.AuthenticationSchemes SelectAuthenticationScheme(HttpListenerRequest request)
        {
            Func<HttpListenerRequest, WebSocketSharp.Net.AuthenticationSchemes> func = this._authSchemeSelector;
            if (func == null)
            {
                return this._authSchemes;
            }
            try
            {
                return func(request);
            }
            catch
            {
                return WebSocketSharp.Net.AuthenticationSchemes.None;
            }
        }

        public void Start()
        {
            this.CheckDisposed();
            if (!this._listening)
            {
                EndPointManager.AddListener(this);
                this._listening = true;
            }
        }

        public void Stop()
        {
            this.CheckDisposed();
            if (this._listening)
            {
                this._listening = false;
                EndPointManager.RemoveListener(this);
                lock (this._ctxRegistrySync)
                {
                    this.cleanupContextQueue(true);
                }
                this.cleanupContextRegistry();
                this.cleanupConnections();
                this.cleanupWaitQueue(new HttpListenerException(0x3e3, "The listener is stopped."));
            }
        }

        void IDisposable.Dispose()
        {
            if (!this._disposed)
            {
                this.close(true);
            }
        }

        internal void UnregisterContext(HttpListenerContext context)
        {
            lock (this._ctxRegistrySync)
            {
                this._ctxRegistry.Remove(context);
            }
        }

        internal bool IsDisposed =>
            this._disposed;

        internal bool ReuseAddress
        {
            get => 
                this._reuseAddress;
            set => 
                this._reuseAddress = value;
        }

        public WebSocketSharp.Net.AuthenticationSchemes AuthenticationSchemes
        {
            get
            {
                this.CheckDisposed();
                return this._authSchemes;
            }
            set
            {
                this.CheckDisposed();
                this._authSchemes = value;
            }
        }

        public Func<HttpListenerRequest, WebSocketSharp.Net.AuthenticationSchemes> AuthenticationSchemeSelector
        {
            get
            {
                this.CheckDisposed();
                return this._authSchemeSelector;
            }
            set
            {
                this.CheckDisposed();
                this._authSchemeSelector = value;
            }
        }

        public string CertificateFolderPath
        {
            get
            {
                this.CheckDisposed();
                return this._certFolderPath;
            }
            set
            {
                this.CheckDisposed();
                this._certFolderPath = value;
            }
        }

        public bool IgnoreWriteExceptions
        {
            get
            {
                this.CheckDisposed();
                return this._ignoreWriteExceptions;
            }
            set
            {
                this.CheckDisposed();
                this._ignoreWriteExceptions = value;
            }
        }

        public bool IsListening =>
            this._listening;

        public static bool IsSupported =>
            true;

        public Logger Log =>
            this._logger;

        public HttpListenerPrefixCollection Prefixes
        {
            get
            {
                this.CheckDisposed();
                return this._prefixes;
            }
        }

        public string Realm
        {
            get
            {
                this.CheckDisposed();
                return this._realm;
            }
            set
            {
                this.CheckDisposed();
                this._realm = value;
            }
        }

        public ServerSslConfiguration SslConfiguration
        {
            get
            {
                this.CheckDisposed();
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
                this.CheckDisposed();
                this._sslConfig = value;
            }
        }

        public bool UnsafeConnectionNtlmAuthentication
        {
            get
            {
                throw new NotSupportedException();
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        public Func<IIdentity, NetworkCredential> UserCredentialsFinder
        {
            get
            {
                this.CheckDisposed();
                return this._userCredFinder;
            }
            set
            {
                this.CheckDisposed();
                this._userCredFinder = value;
            }
        }
    }
}

