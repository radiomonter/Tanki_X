namespace WebSocketSharp.Server
{
    using System;
    using System.IO;
    using WebSocketSharp;
    using WebSocketSharp.Net;
    using WebSocketSharp.Net.WebSockets;

    public abstract class WebSocketBehavior : IWebSocketSession
    {
        private WebSocketContext _context;
        private Func<CookieCollection, CookieCollection, bool> _cookiesValidator;
        private bool _emitOnPing;
        private string _id;
        private bool _ignoreExtensions;
        private Func<string, bool> _originValidator;
        private string _protocol;
        private WebSocketSessionManager _sessions;
        private DateTime _startTime = DateTime.MaxValue;
        private WebSocket _websocket;

        protected WebSocketBehavior()
        {
        }

        private string checkHandshakeRequest(WebSocketContext context) => 
            ((this._originValidator == null) || this._originValidator(context.Origin)) ? (((this._cookiesValidator == null) || this._cookiesValidator(context.CookieCollection, context.WebSocket.CookieCollection)) ? null : "Includes no cookie, or an invalid cookie exists.") : "Includes no Origin header, or it has an invalid value.";

        protected void Error(string message, Exception exception)
        {
            if ((message != null) && (message.Length > 0))
            {
                this.OnError(new ErrorEventArgs(message, exception));
            }
        }

        private void onClose(object sender, CloseEventArgs e)
        {
            if (this._id != null)
            {
                this._sessions.Remove(this._id);
                this.OnClose(e);
            }
        }

        protected virtual void OnClose(CloseEventArgs e)
        {
        }

        private void onError(object sender, ErrorEventArgs e)
        {
            this.OnError(e);
        }

        protected virtual void OnError(ErrorEventArgs e)
        {
        }

        private void onMessage(object sender, MessageEventArgs e)
        {
            this.OnMessage(e);
        }

        protected virtual void OnMessage(MessageEventArgs e)
        {
        }

        private void onOpen(object sender, EventArgs e)
        {
            this._id = this._sessions.Add(this);
            if (this._id == null)
            {
                this._websocket.Close(CloseStatusCode.Away);
            }
            else
            {
                this._startTime = DateTime.Now;
                this.OnOpen();
            }
        }

        protected virtual void OnOpen()
        {
        }

        protected void Send(byte[] data)
        {
            if (this._websocket != null)
            {
                this._websocket.Send(data);
            }
        }

        protected void Send(FileInfo file)
        {
            if (this._websocket != null)
            {
                this._websocket.Send(file);
            }
        }

        protected void Send(string data)
        {
            if (this._websocket != null)
            {
                this._websocket.Send(data);
            }
        }

        protected void SendAsync(byte[] data, Action<bool> completed)
        {
            if (this._websocket != null)
            {
                this._websocket.SendAsync(data, completed);
            }
        }

        protected void SendAsync(FileInfo file, Action<bool> completed)
        {
            if (this._websocket != null)
            {
                this._websocket.SendAsync(file, completed);
            }
        }

        protected void SendAsync(string data, Action<bool> completed)
        {
            if (this._websocket != null)
            {
                this._websocket.SendAsync(data, completed);
            }
        }

        protected void SendAsync(Stream stream, int length, Action<bool> completed)
        {
            if (this._websocket != null)
            {
                this._websocket.SendAsync(stream, length, completed);
            }
        }

        internal void Start(WebSocketContext context, WebSocketSessionManager sessions)
        {
            if (this._websocket != null)
            {
                this._websocket.Log.Error("This session has already been started.");
                context.WebSocket.Close(HttpStatusCode.ServiceUnavailable);
            }
            else
            {
                this._context = context;
                this._sessions = sessions;
                this._websocket = context.WebSocket;
                this._websocket.CustomHandshakeRequestChecker = new Func<WebSocketContext, string>(this.checkHandshakeRequest);
                this._websocket.EmitOnPing = this._emitOnPing;
                this._websocket.IgnoreExtensions = this._ignoreExtensions;
                this._websocket.Protocol = this._protocol;
                TimeSpan waitTime = sessions.WaitTime;
                if (waitTime != this._websocket.WaitTime)
                {
                    this._websocket.WaitTime = waitTime;
                }
                this._websocket.OnOpen += new EventHandler(this.onOpen);
                this._websocket.OnMessage += new EventHandler<MessageEventArgs>(this.onMessage);
                this._websocket.OnError += new EventHandler<ErrorEventArgs>(this.onError);
                this._websocket.OnClose += new EventHandler<CloseEventArgs>(this.onClose);
                this._websocket.InternalAccept();
            }
        }

        protected Logger Log =>
            this._websocket?.Log;

        protected WebSocketSessionManager Sessions =>
            this._sessions;

        public WebSocketContext Context =>
            this._context;

        public Func<CookieCollection, CookieCollection, bool> CookiesValidator
        {
            get => 
                this._cookiesValidator;
            set => 
                this._cookiesValidator = value;
        }

        public bool EmitOnPing
        {
            get => 
                (this._websocket == null) ? this._emitOnPing : this._websocket.EmitOnPing;
            set
            {
                if (this._websocket != null)
                {
                    this._websocket.EmitOnPing = value;
                }
                else
                {
                    this._emitOnPing = value;
                }
            }
        }

        public string ID =>
            this._id;

        public bool IgnoreExtensions
        {
            get => 
                this._ignoreExtensions;
            set => 
                this._ignoreExtensions = value;
        }

        public Func<string, bool> OriginValidator
        {
            get => 
                this._originValidator;
            set => 
                this._originValidator = value;
        }

        public string Protocol
        {
            get => 
                (this._websocket == null) ? (this._protocol ?? string.Empty) : this._websocket.Protocol;
            set
            {
                if ((this.State == WebSocketState.Connecting) && ((value == null) || ((value.Length != 0) && value.IsToken())))
                {
                    this._protocol = value;
                }
            }
        }

        public DateTime StartTime =>
            this._startTime;

        public WebSocketState State =>
            (this._websocket == null) ? WebSocketState.Connecting : this._websocket.ReadyState;
    }
}

