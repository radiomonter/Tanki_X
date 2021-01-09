namespace WebSocketSharp.Net
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;
    using WebSocketSharp;

    public sealed class HttpListenerResponse : IDisposable
    {
        private bool _closeConnection;
        private Encoding _contentEncoding;
        private long _contentLength;
        private string _contentType;
        private HttpListenerContext _context;
        private CookieCollection _cookies;
        private bool _disposed;
        private WebHeaderCollection _headers;
        private bool _headersSent;
        private bool _keepAlive;
        private string _location;
        private ResponseStream _outputStream;
        private bool _sendChunked;
        private int _statusCode;
        private string _statusDescription;
        private Version _version;

        internal HttpListenerResponse(HttpListenerContext context)
        {
            this._context = context;
            this._keepAlive = true;
            this._statusCode = 200;
            this._statusDescription = "OK";
            this._version = HttpVersion.Version11;
        }

        public void Abort()
        {
            if (!this._disposed)
            {
                this.close(true);
            }
        }

        public void AddHeader(string name, string value)
        {
            this.Headers.Set(name, value);
        }

        public void AppendCookie(Cookie cookie)
        {
            this.Cookies.Add(cookie);
        }

        public void AppendHeader(string name, string value)
        {
            this.Headers.Add(name, value);
        }

        private bool canAddOrUpdate(Cookie cookie)
        {
            bool flag;
            if ((this._cookies == null) || (this._cookies.Count == 0))
            {
                return true;
            }
            List<Cookie> list = this.findCookie(cookie).ToList<Cookie>();
            if (list.Count == 0)
            {
                return true;
            }
            int version = cookie.Version;
            using (List<Cookie>.Enumerator enumerator = list.GetEnumerator())
            {
                while (true)
                {
                    if (enumerator.MoveNext())
                    {
                        Cookie current = enumerator.Current;
                        if (current.Version != version)
                        {
                            continue;
                        }
                        flag = true;
                    }
                    else
                    {
                        return false;
                    }
                    break;
                }
            }
            return flag;
        }

        private void checkDisposed()
        {
            if (this._disposed)
            {
                throw new ObjectDisposedException(base.GetType().ToString());
            }
        }

        private void checkDisposedOrHeadersSent()
        {
            if (this._disposed)
            {
                throw new ObjectDisposedException(base.GetType().ToString());
            }
            if (this._headersSent)
            {
                throw new InvalidOperationException("Cannot be changed after the headers are sent.");
            }
        }

        private void close(bool force)
        {
            this._disposed = true;
            this._context.Connection.Close(force);
        }

        public void Close()
        {
            if (!this._disposed)
            {
                this.close(false);
            }
        }

        public void Close(byte[] responseEntity, bool willBlock)
        {
            <Close>c__AnonStorey1 storey = new <Close>c__AnonStorey1 {
                $this = this
            };
            this.checkDisposed();
            if (responseEntity == null)
            {
                throw new ArgumentNullException("responseEntity");
            }
            int length = responseEntity.Length;
            storey.output = this.OutputStream;
            if (!willBlock)
            {
                storey.output.BeginWrite(responseEntity, 0, length, new AsyncCallback(storey.<>m__0), null);
            }
            else
            {
                storey.output.Write(responseEntity, 0, length);
                this.close(false);
            }
        }

        public void CopyFrom(HttpListenerResponse templateResponse)
        {
            if (templateResponse == null)
            {
                throw new ArgumentNullException("templateResponse");
            }
            if (templateResponse._headers == null)
            {
                if (this._headers != null)
                {
                    this._headers = null;
                }
            }
            else
            {
                if (this._headers != null)
                {
                    this._headers.Clear();
                }
                this.Headers.Add(templateResponse._headers);
            }
            this._contentLength = templateResponse._contentLength;
            this._statusCode = templateResponse._statusCode;
            this._statusDescription = templateResponse._statusDescription;
            this._keepAlive = templateResponse._keepAlive;
            this._version = templateResponse._version;
        }

        [DebuggerHidden]
        private IEnumerable<Cookie> findCookie(Cookie cookie) => 
            new <findCookie>c__Iterator0 { 
                cookie = cookie,
                $this = this,
                $PC = -2
            };

        public void Redirect(string url)
        {
            this.checkDisposedOrHeadersSent();
            if (url == null)
            {
                throw new ArgumentNullException("url");
            }
            Uri result = null;
            if (!url.MaybeUri() || !Uri.TryCreate(url, UriKind.Absolute, out result))
            {
                throw new ArgumentException("Not an absolute URL.", "url");
            }
            this._location = url;
            this._statusCode = 0x12e;
            this._statusDescription = "Found";
        }

        public void SetCookie(Cookie cookie)
        {
            if (cookie == null)
            {
                throw new ArgumentNullException("cookie");
            }
            if (!this.canAddOrUpdate(cookie))
            {
                throw new ArgumentException("Cannot be replaced.", "cookie");
            }
            this.Cookies.Add(cookie);
        }

        void IDisposable.Dispose()
        {
            if (!this._disposed)
            {
                this.close(true);
            }
        }

        internal WebHeaderCollection WriteHeadersTo(MemoryStream destination)
        {
            WebHeaderCollection headers = new WebHeaderCollection(HttpHeaderType.Response, true);
            if (this._headers != null)
            {
                headers.Add(this._headers);
            }
            if (this._contentType != null)
            {
                string str = ((this._contentType.IndexOf("charset=", StringComparison.Ordinal) != -1) || (this._contentEncoding == null)) ? this._contentType : $"{this._contentType}; charset={this._contentEncoding.WebName}";
                headers.InternalSet("Content-Type", str, true);
            }
            if (headers["Server"] == null)
            {
                headers.InternalSet("Server", "websocket-sharp/1.0", true);
            }
            CultureInfo invariantCulture = CultureInfo.InvariantCulture;
            if (headers["Date"] == null)
            {
                headers.InternalSet("Date", DateTime.UtcNow.ToString("r", invariantCulture), true);
            }
            if (!this._sendChunked)
            {
                headers.InternalSet("Content-Length", this._contentLength.ToString(invariantCulture), true);
            }
            else
            {
                headers.InternalSet("Transfer-Encoding", "chunked", true);
            }
            int reuses = this._context.Connection.Reuses;
            if (((!this._context.Request.KeepAlive || (!this._keepAlive || ((this._statusCode == 400) || ((this._statusCode == 0x198) || ((this._statusCode == 0x19b) || ((this._statusCode == 0x19d) || ((this._statusCode == 0x19e) || (this._statusCode == 500)))))))) || (this._statusCode == 0x1f7)) || (reuses >= 100))
            {
                headers.InternalSet("Connection", "close", true);
            }
            else
            {
                headers.InternalSet("Keep-Alive", $"timeout=15,max={100 - reuses}", true);
                if (this._context.Request.ProtocolVersion < HttpVersion.Version11)
                {
                    headers.InternalSet("Connection", "keep-alive", true);
                }
            }
            if (this._location != null)
            {
                headers.InternalSet("Location", this._location, true);
            }
            if (this._cookies != null)
            {
                IEnumerator enumerator = this._cookies.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        headers.InternalSet("Set-Cookie", ((Cookie) enumerator.Current).ToResponseString(), true);
                    }
                }
                finally
                {
                    IDisposable disposable = enumerator as IDisposable;
                    if (disposable != null)
                    {
                        disposable.Dispose();
                    }
                }
            }
            Encoding encoding = this._contentEncoding ?? Encoding.Default;
            StreamWriter writer = new StreamWriter(destination, encoding, 0x100);
            writer.Write("HTTP/{0} {1} {2}\r\n", this._version, this._statusCode, this._statusDescription);
            writer.Write(headers.ToStringMultiValue(true));
            writer.Flush();
            destination.Position = encoding.GetPreamble().Length;
            return headers;
        }

        internal bool CloseConnection
        {
            get => 
                this._closeConnection;
            set => 
                this._closeConnection = value;
        }

        internal bool HeadersSent
        {
            get => 
                this._headersSent;
            set => 
                this._headersSent = value;
        }

        public Encoding ContentEncoding
        {
            get => 
                this._contentEncoding;
            set
            {
                this.checkDisposed();
                this._contentEncoding = value;
            }
        }

        public long ContentLength64
        {
            get => 
                this._contentLength;
            set
            {
                this.checkDisposedOrHeadersSent();
                if (value < 0L)
                {
                    throw new ArgumentOutOfRangeException("Less than zero.", "value");
                }
                this._contentLength = value;
            }
        }

        public string ContentType
        {
            get => 
                this._contentType;
            set
            {
                this.checkDisposed();
                if ((value != null) && (value.Length == 0))
                {
                    throw new ArgumentException("An empty string.", "value");
                }
                this._contentType = value;
            }
        }

        public CookieCollection Cookies
        {
            get
            {
                CookieCollection collection2 = this._cookies;
                if (this._cookies == null)
                {
                    CookieCollection local1 = this._cookies;
                    collection2 = this._cookies = new CookieCollection();
                }
                return collection2;
            }
            set => 
                this._cookies = value;
        }

        public WebHeaderCollection Headers
        {
            get
            {
                WebHeaderCollection collection2 = this._headers;
                if (this._headers == null)
                {
                    WebHeaderCollection local1 = this._headers;
                    collection2 = this._headers = new WebHeaderCollection(HttpHeaderType.Response, false);
                }
                return collection2;
            }
            set
            {
                if ((value != null) && (value.State != HttpHeaderType.Response))
                {
                    throw new InvalidOperationException("The specified headers aren't valid for a response.");
                }
                this._headers = value;
            }
        }

        public bool KeepAlive
        {
            get => 
                this._keepAlive;
            set
            {
                this.checkDisposedOrHeadersSent();
                this._keepAlive = value;
            }
        }

        public Stream OutputStream
        {
            get
            {
                this.checkDisposed();
                ResponseStream stream2 = this._outputStream;
                if (this._outputStream == null)
                {
                    ResponseStream local1 = this._outputStream;
                    stream2 = this._outputStream = this._context.Connection.GetResponseStream();
                }
                return stream2;
            }
        }

        public Version ProtocolVersion
        {
            get => 
                this._version;
            set
            {
                this.checkDisposedOrHeadersSent();
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                if ((value.Major != 1) || ((value.Minor != 0) && (value.Minor != 1)))
                {
                    throw new ArgumentException("Not 1.0 or 1.1.", "value");
                }
                this._version = value;
            }
        }

        public string RedirectLocation
        {
            get => 
                this._location;
            set
            {
                this.checkDisposed();
                if (value == null)
                {
                    this._location = null;
                }
                else
                {
                    Uri result = null;
                    if (!value.MaybeUri() || !Uri.TryCreate(value, UriKind.Absolute, out result))
                    {
                        throw new ArgumentException("Not an absolute URL.", "value");
                    }
                    this._location = value;
                }
            }
        }

        public bool SendChunked
        {
            get => 
                this._sendChunked;
            set
            {
                this.checkDisposedOrHeadersSent();
                this._sendChunked = value;
            }
        }

        public int StatusCode
        {
            get => 
                this._statusCode;
            set
            {
                this.checkDisposedOrHeadersSent();
                if ((value < 100) || (value > 0x3e7))
                {
                    throw new ProtocolViolationException("A value isn't between 100 and 999 inclusive.");
                }
                this._statusCode = value;
                this._statusDescription = value.GetStatusDescription();
            }
        }

        public string StatusDescription
        {
            get => 
                this._statusDescription;
            set
            {
                this.checkDisposedOrHeadersSent();
                if ((value == null) || (value.Length == 0))
                {
                    this._statusDescription = this._statusCode.GetStatusDescription();
                }
                else
                {
                    if (value.IsText())
                    {
                        char[] anyOf = new char[] { '\r', '\n' };
                        if (value.IndexOfAny(anyOf) <= -1)
                        {
                            this._statusDescription = value;
                            return;
                        }
                    }
                    throw new ArgumentException("Contains invalid characters.", "value");
                }
            }
        }

        [CompilerGenerated]
        private sealed class <Close>c__AnonStorey1
        {
            internal Stream output;
            internal HttpListenerResponse $this;

            internal void <>m__0(IAsyncResult ar)
            {
                this.output.EndWrite(ar);
                this.$this.close(false);
            }
        }

        [CompilerGenerated]
        private sealed class <findCookie>c__Iterator0 : IEnumerable, IEnumerable<Cookie>, IEnumerator, IDisposable, IEnumerator<Cookie>
        {
            internal Cookie cookie;
            internal string <name>__0;
            internal string <domain>__0;
            internal string <path>__0;
            internal IEnumerator $locvar0;
            internal Cookie <c>__1;
            internal IDisposable $locvar1;
            internal HttpListenerResponse $this;
            internal Cookie $current;
            internal bool $disposing;
            internal int $PC;

            [DebuggerHidden]
            public void Dispose()
            {
                uint num = (uint) this.$PC;
                this.$disposing = true;
                this.$PC = -1;
                switch (num)
                {
                    case 1:
                        try
                        {
                        }
                        finally
                        {
                            this.$locvar1 = this.$locvar0 as IDisposable;
                            if (this.$locvar1 != null)
                            {
                                this.$locvar1.Dispose();
                            }
                        }
                        break;

                    default:
                        break;
                }
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                bool flag = false;
                switch (num)
                {
                    case 0:
                        this.<name>__0 = this.cookie.Name;
                        this.<domain>__0 = this.cookie.Domain;
                        this.<path>__0 = this.cookie.Path;
                        if (this.$this._cookies == null)
                        {
                            goto TR_0007;
                        }
                        else
                        {
                            this.$locvar0 = this.$this._cookies.GetEnumerator();
                            num = 0xfffffffd;
                        }
                        break;

                    case 1:
                        break;

                    default:
                        goto TR_0000;
                }
                try
                {
                    switch (num)
                    {
                        default:
                            while (true)
                            {
                                if (!this.$locvar0.MoveNext())
                                {
                                    break;
                                }
                                this.<c>__1 = (Cookie) this.$locvar0.Current;
                                if (this.<c>__1.Name.Equals(this.<name>__0, StringComparison.OrdinalIgnoreCase) && (this.<c>__1.Domain.Equals(this.<domain>__0, StringComparison.OrdinalIgnoreCase) && this.<c>__1.Path.Equals(this.<path>__0, StringComparison.Ordinal)))
                                {
                                    this.$current = this.<c>__1;
                                    if (!this.$disposing)
                                    {
                                        this.$PC = 1;
                                    }
                                    flag = true;
                                    return true;
                                }
                            }
                            break;
                    }
                }
                finally
                {
                    if (flag)
                    {
                    }
                    this.$locvar1 = this.$locvar0 as IDisposable;
                    if (this.$locvar1 != null)
                    {
                        this.$locvar1.Dispose();
                    }
                }
                goto TR_0007;
            TR_0000:
                return false;
            TR_0007:
                this.$PC = -1;
                goto TR_0000;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            [DebuggerHidden]
            IEnumerator<Cookie> IEnumerable<Cookie>.GetEnumerator()
            {
                if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
                {
                    return this;
                }
                return new HttpListenerResponse.<findCookie>c__Iterator0 { 
                    $this = this.$this,
                    cookie = this.cookie
                };
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator() => 
                this.System.Collections.Generic.IEnumerable<WebSocketSharp.Net.Cookie>.GetEnumerator();

            Cookie IEnumerator<Cookie>.Current =>
                this.$current;

            object IEnumerator.Current =>
                this.$current;
        }
    }
}

