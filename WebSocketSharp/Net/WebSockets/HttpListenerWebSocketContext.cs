namespace WebSocketSharp.Net.WebSockets
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Security.Principal;
    using System.Threading;
    using WebSocketSharp;
    using WebSocketSharp.Net;

    public class HttpListenerWebSocketContext : WebSocketContext
    {
        private HttpListenerContext _context;
        private WebSocketSharp.WebSocket _websocket;

        internal HttpListenerWebSocketContext(HttpListenerContext context, string protocol)
        {
            this._context = context;
            this._websocket = new WebSocketSharp.WebSocket(this, protocol);
        }

        internal void Close()
        {
            this._context.Connection.Close(true);
        }

        internal void Close(HttpStatusCode code)
        {
            this._context.Response.Close(code);
        }

        public override string ToString() => 
            this._context.Request.ToString();

        internal Logger Log =>
            this._context.Listener.Log;

        internal System.IO.Stream Stream =>
            this._context.Connection.Stream;

        public override WebSocketSharp.Net.CookieCollection CookieCollection =>
            this._context.Request.Cookies;

        public override NameValueCollection Headers =>
            this._context.Request.Headers;

        public override string Host =>
            this._context.Request.Headers["Host"];

        public override bool IsAuthenticated =>
            !ReferenceEquals(this._context.User, null);

        public override bool IsLocal =>
            this._context.Request.IsLocal;

        public override bool IsSecureConnection =>
            this._context.Connection.IsSecure;

        public override bool IsWebSocketRequest =>
            this._context.Request.IsWebSocketRequest;

        public override string Origin =>
            this._context.Request.Headers["Origin"];

        public override NameValueCollection QueryString =>
            this._context.Request.QueryString;

        public override Uri RequestUri =>
            this._context.Request.Url;

        public override string SecWebSocketKey =>
            this._context.Request.Headers["Sec-WebSocket-Key"];

        public override IEnumerable<string> SecWebSocketProtocols =>
            new <>c__Iterator0 { 
                $this=this,
                $PC=-2
            };

        public override string SecWebSocketVersion =>
            this._context.Request.Headers["Sec-WebSocket-Version"];

        public override IPEndPoint ServerEndPoint =>
            this._context.Connection.LocalEndPoint;

        public override IPrincipal User =>
            this._context.User;

        public override IPEndPoint UserEndPoint =>
            this._context.Connection.RemoteEndPoint;

        public override WebSocketSharp.WebSocket WebSocket =>
            this._websocket;

        [CompilerGenerated]
        private sealed class <>c__Iterator0 : IEnumerable, IEnumerable<string>, IEnumerator, IDisposable, IEnumerator<string>
        {
            internal string <protocols>__0;
            internal string[] $locvar0;
            internal int $locvar1;
            internal string <protocol>__1;
            internal HttpListenerWebSocketContext $this;
            internal string $current;
            internal bool $disposing;
            internal int $PC;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$disposing = true;
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        this.<protocols>__0 = this.$this._context.Request.Headers["Sec-WebSocket-Protocol"];
                        if (this.<protocols>__0 == null)
                        {
                            goto TR_0001;
                        }
                        else
                        {
                            char[] separator = new char[] { ',' };
                            this.$locvar0 = this.<protocols>__0.Split(separator);
                            this.$locvar1 = 0;
                        }
                        break;

                    case 1:
                        this.$locvar1++;
                        break;

                    default:
                        goto TR_0000;
                }
                if (this.$locvar1 < this.$locvar0.Length)
                {
                    this.<protocol>__1 = this.$locvar0[this.$locvar1];
                    this.$current = this.<protocol>__1.Trim();
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    return true;
                }
                goto TR_0001;
            TR_0000:
                return false;
            TR_0001:
                this.$PC = -1;
                goto TR_0000;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            [DebuggerHidden]
            IEnumerator<string> IEnumerable<string>.GetEnumerator()
            {
                if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
                {
                    return this;
                }
                return new HttpListenerWebSocketContext.<>c__Iterator0 { $this = this.$this };
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator() => 
                this.System.Collections.Generic.IEnumerable<string>.GetEnumerator();

            string IEnumerator<string>.Current =>
                this.$current;

            object IEnumerator.Current =>
                this.$current;
        }
    }
}

