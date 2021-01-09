namespace WebSocketSharp.Net.WebSockets
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Net.Security;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;
    using System.Security.Principal;
    using System.Text;
    using System.Threading;
    using WebSocketSharp;
    using WebSocketSharp.Net;

    internal class TcpListenerWebSocketContext : WebSocketContext
    {
        private WebSocketSharp.Net.CookieCollection _cookies;
        private Logger _logger;
        private NameValueCollection _queryString;
        private HttpRequest _request;
        private bool _secure;
        private System.IO.Stream _stream;
        private TcpClient _tcpClient;
        private Uri _uri;
        private IPrincipal _user;
        private WebSocketSharp.WebSocket _websocket;

        internal TcpListenerWebSocketContext(TcpClient tcpClient, string protocol, bool secure, ServerSslConfiguration sslConfig, Logger logger)
        {
            this._tcpClient = tcpClient;
            this._secure = secure;
            this._logger = logger;
            NetworkStream innerStream = tcpClient.GetStream();
            if (!secure)
            {
                this._stream = innerStream;
            }
            else
            {
                SslStream stream2 = new SslStream(innerStream, false, sslConfig.ClientCertificateValidationCallback);
                stream2.AuthenticateAsServer(sslConfig.ServerCertificate, sslConfig.ClientCertificateRequired, sslConfig.EnabledSslProtocols, sslConfig.CheckCertificateRevocation);
                this._stream = stream2;
            }
            this._request = HttpRequest.Read(this._stream, 0x15f90);
            this._uri = HttpUtility.CreateRequestUrl(this._request.RequestUri, this._request.Headers["Host"], this._request.IsWebSocketRequest, secure);
            this._websocket = new WebSocketSharp.WebSocket(this, protocol);
        }

        internal bool Authenticate(AuthenticationSchemes scheme, string realm, Func<IIdentity, NetworkCredential> credentialsFinder)
        {
            <Authenticate>c__AnonStorey1 storey = new <Authenticate>c__AnonStorey1 {
                scheme = scheme,
                realm = realm,
                credentialsFinder = credentialsFinder,
                $this = this
            };
            if (storey.scheme == AuthenticationSchemes.Anonymous)
            {
                return true;
            }
            if (storey.scheme == AuthenticationSchemes.None)
            {
                this.Close(HttpStatusCode.Forbidden);
                return false;
            }
            storey.chal = new AuthenticationChallenge(storey.scheme, storey.realm).ToString();
            storey.retry = -1;
            storey.auth = null;
            storey.auth = new Func<bool>(storey.<>m__0);
            return storey.auth();
        }

        internal void Close()
        {
            this._stream.Close();
            this._tcpClient.Close();
        }

        internal void Close(HttpStatusCode code)
        {
            this._websocket.Close(HttpResponse.CreateCloseResponse(code));
        }

        internal void SendAuthenticationChallenge(string challenge)
        {
            byte[] buffer = HttpResponse.CreateUnauthorizedResponse(challenge).ToByteArray();
            this._stream.Write(buffer, 0, buffer.Length);
            this._request = HttpRequest.Read(this._stream, 0x3a98);
        }

        public override string ToString() => 
            this._request.ToString();

        internal Logger Log =>
            this._logger;

        internal System.IO.Stream Stream =>
            this._stream;

        public override WebSocketSharp.Net.CookieCollection CookieCollection
        {
            get
            {
                WebSocketSharp.Net.CookieCollection collection2 = this._cookies;
                if (this._cookies == null)
                {
                    WebSocketSharp.Net.CookieCollection local1 = this._cookies;
                    collection2 = this._cookies = this._request.Cookies;
                }
                return collection2;
            }
        }

        public override NameValueCollection Headers =>
            this._request.Headers;

        public override string Host =>
            this._request.Headers["Host"];

        public override bool IsAuthenticated =>
            !ReferenceEquals(this._user, null);

        public override bool IsLocal =>
            this.UserEndPoint.Address.IsLocal();

        public override bool IsSecureConnection =>
            this._secure;

        public override bool IsWebSocketRequest =>
            this._request.IsWebSocketRequest;

        public override string Origin =>
            this._request.Headers["Origin"];

        public override NameValueCollection QueryString
        {
            get
            {
                NameValueCollection collection2 = this._queryString;
                if (this._queryString == null)
                {
                    NameValueCollection local1 = this._queryString;
                    collection2 = this._queryString = HttpUtility.InternalParseQueryString(this._uri?.Query, Encoding.UTF8);
                }
                return collection2;
            }
        }

        public override Uri RequestUri =>
            this._uri;

        public override string SecWebSocketKey =>
            this._request.Headers["Sec-WebSocket-Key"];

        public override IEnumerable<string> SecWebSocketProtocols =>
            new <>c__Iterator0 { 
                $this=this,
                $PC=-2
            };

        public override string SecWebSocketVersion =>
            this._request.Headers["Sec-WebSocket-Version"];

        public override IPEndPoint ServerEndPoint =>
            (IPEndPoint) this._tcpClient.Client.LocalEndPoint;

        public override IPrincipal User =>
            this._user;

        public override IPEndPoint UserEndPoint =>
            (IPEndPoint) this._tcpClient.Client.RemoteEndPoint;

        public override WebSocketSharp.WebSocket WebSocket =>
            this._websocket;

        [CompilerGenerated]
        private sealed class <>c__Iterator0 : IEnumerable, IEnumerable<string>, IEnumerator, IDisposable, IEnumerator<string>
        {
            internal string <protocols>__0;
            internal string[] $locvar0;
            internal int $locvar1;
            internal string <protocol>__1;
            internal TcpListenerWebSocketContext $this;
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
                        this.<protocols>__0 = this.$this._request.Headers["Sec-WebSocket-Protocol"];
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
                return new TcpListenerWebSocketContext.<>c__Iterator0 { $this = this.$this };
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator() => 
                this.System.Collections.Generic.IEnumerable<string>.GetEnumerator();

            string IEnumerator<string>.Current =>
                this.$current;

            object IEnumerator.Current =>
                this.$current;
        }

        [CompilerGenerated]
        private sealed class <Authenticate>c__AnonStorey1
        {
            internal int retry;
            internal AuthenticationSchemes scheme;
            internal string realm;
            internal Func<IIdentity, NetworkCredential> credentialsFinder;
            internal string chal;
            internal Func<bool> auth;
            internal TcpListenerWebSocketContext $this;

            internal bool <>m__0()
            {
                this.retry++;
                if (this.retry > 0x63)
                {
                    this.$this.Close(HttpStatusCode.Forbidden);
                    return false;
                }
                IPrincipal principal = HttpUtility.CreateUser(this.$this._request.Headers["Authorization"], this.scheme, this.realm, this.$this._request.HttpMethod, this.credentialsFinder);
                if ((principal != null) && principal.Identity.IsAuthenticated)
                {
                    this.$this._user = principal;
                    return true;
                }
                this.$this.SendAuthenticationChallenge(this.chal);
                return this.auth();
            }
        }
    }
}

