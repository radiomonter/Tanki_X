﻿namespace WebSocketSharp.Net.WebSockets
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Net;
    using System.Security.Principal;
    using WebSocketSharp;
    using WebSocketSharp.Net;

    public abstract class WebSocketContext
    {
        protected WebSocketContext()
        {
        }

        public abstract WebSocketSharp.Net.CookieCollection CookieCollection { get; }

        public abstract NameValueCollection Headers { get; }

        public abstract string Host { get; }

        public abstract bool IsAuthenticated { get; }

        public abstract bool IsLocal { get; }

        public abstract bool IsSecureConnection { get; }

        public abstract bool IsWebSocketRequest { get; }

        public abstract string Origin { get; }

        public abstract NameValueCollection QueryString { get; }

        public abstract Uri RequestUri { get; }

        public abstract string SecWebSocketKey { get; }

        public abstract IEnumerable<string> SecWebSocketProtocols { get; }

        public abstract string SecWebSocketVersion { get; }

        public abstract IPEndPoint ServerEndPoint { get; }

        public abstract IPrincipal User { get; }

        public abstract IPEndPoint UserEndPoint { get; }

        public abstract WebSocketSharp.WebSocket WebSocket { get; }
    }
}

