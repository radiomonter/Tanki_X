namespace WebSocketSharp.Net
{
    using System;
    using System.Security.Principal;
    using WebSocketSharp;
    using WebSocketSharp.Net.WebSockets;

    public sealed class HttpListenerContext
    {
        private HttpConnection _connection;
        private string _error;
        private int _errorStatus;
        private HttpListener _listener;
        private HttpListenerRequest _request;
        private HttpListenerResponse _response;
        private IPrincipal _user;
        private HttpListenerWebSocketContext _websocketContext;

        internal HttpListenerContext(HttpConnection connection)
        {
            this._connection = connection;
            this._errorStatus = 400;
            this._request = new HttpListenerRequest(this);
            this._response = new HttpListenerResponse(this);
        }

        public HttpListenerWebSocketContext AcceptWebSocket(string protocol)
        {
            if (this._websocketContext != null)
            {
                throw new InvalidOperationException("The accepting is already in progress.");
            }
            if (protocol != null)
            {
                if (protocol.Length == 0)
                {
                    throw new ArgumentException("An empty string.", "protocol");
                }
                if (!protocol.IsToken())
                {
                    throw new ArgumentException("Contains an invalid character.", "protocol");
                }
            }
            this._websocketContext = new HttpListenerWebSocketContext(this, protocol);
            return this._websocketContext;
        }

        internal bool Authenticate()
        {
            AuthenticationSchemes scheme = this._listener.SelectAuthenticationScheme(this._request);
            if (scheme == AuthenticationSchemes.Anonymous)
            {
                return true;
            }
            if (scheme == AuthenticationSchemes.None)
            {
                this._response.Close(HttpStatusCode.Forbidden);
                return false;
            }
            string realm = this._listener.GetRealm();
            IPrincipal principal = HttpUtility.CreateUser(this._request.Headers["Authorization"], scheme, realm, this._request.HttpMethod, this._listener.GetUserCredentialsFinder());
            if ((principal != null) && principal.Identity.IsAuthenticated)
            {
                this._user = principal;
                return true;
            }
            this._response.CloseWithAuthChallenge(new AuthenticationChallenge(scheme, realm).ToString());
            return false;
        }

        internal HttpConnection Connection =>
            this._connection;

        internal string ErrorMessage
        {
            get => 
                this._error;
            set => 
                this._error = value;
        }

        internal int ErrorStatus
        {
            get => 
                this._errorStatus;
            set => 
                this._errorStatus = value;
        }

        internal bool HasError =>
            !ReferenceEquals(this._error, null);

        internal HttpListener Listener
        {
            get => 
                this._listener;
            set => 
                this._listener = value;
        }

        public HttpListenerRequest Request =>
            this._request;

        public HttpListenerResponse Response =>
            this._response;

        public IPrincipal User =>
            this._user;
    }
}

