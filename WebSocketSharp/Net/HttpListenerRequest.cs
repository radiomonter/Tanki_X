namespace WebSocketSharp.Net
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Runtime.InteropServices;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using WebSocketSharp;

    public sealed class HttpListenerRequest
    {
        private static readonly byte[] _100continue = Encoding.ASCII.GetBytes("HTTP/1.1 100 Continue\r\n\r\n");
        private string[] _acceptTypes;
        private bool _chunked;
        private Encoding _contentEncoding;
        private long _contentLength;
        private bool _contentLengthSet;
        private HttpListenerContext _context;
        private CookieCollection _cookies;
        private WebHeaderCollection _headers;
        private Guid _identifier;
        private Stream _inputStream;
        private bool _keepAlive;
        private bool _keepAliveSet;
        private string _method;
        private NameValueCollection _queryString;
        private Uri _referer;
        private string _uri;
        private Uri _url;
        private string[] _userLanguages;
        private Version _version;
        private bool _websocketRequest;
        private bool _websocketRequestSet;

        internal HttpListenerRequest(HttpListenerContext context)
        {
            this._context = context;
            this._contentLength = -1L;
            this._headers = new WebHeaderCollection();
            this._identifier = Guid.NewGuid();
        }

        internal void AddHeader(string header)
        {
            int index = header.IndexOf(':');
            if (index == -1)
            {
                this._context.ErrorMessage = "Invalid header";
            }
            else
            {
                string name = header.Substring(0, index).Trim();
                string str2 = header.Substring(index + 1).Trim();
                this._headers.InternalSet(name, str2, false);
                string str3 = name.ToLower(CultureInfo.InvariantCulture);
                if (str3 == "accept")
                {
                    char[] separators = new char[] { ',' };
                    this._acceptTypes = new List<string>(str2.SplitHeaderValue(separators)).ToArray();
                }
                else if (str3 == "accept-language")
                {
                    char[] separator = new char[] { ',' };
                    this._userLanguages = str2.Split(separator);
                }
                else if (str3 == "content-length")
                {
                    long num2;
                    if (!long.TryParse(str2, out num2) || (num2 < 0L))
                    {
                        this._context.ErrorMessage = "Invalid Content-Length header";
                    }
                    else
                    {
                        this._contentLength = num2;
                        this._contentLengthSet = true;
                    }
                }
                else if (str3 != "content-type")
                {
                    if (str3 == "referer")
                    {
                        this._referer = str2.ToUri();
                    }
                }
                else
                {
                    try
                    {
                        this._contentEncoding = HttpUtility.GetEncoding(str2);
                    }
                    catch
                    {
                        this._context.ErrorMessage = "Invalid Content-Type header";
                    }
                }
            }
        }

        public IAsyncResult BeginGetClientCertificate(AsyncCallback requestCallback, object state)
        {
            throw new NotImplementedException();
        }

        public X509Certificate2 EndGetClientCertificate(IAsyncResult asyncResult)
        {
            throw new NotImplementedException();
        }

        internal void FinishInitialization()
        {
            string host = this._headers["Host"];
            bool flag = (host == null) || (host.Length == 0);
            if ((this._version > HttpVersion.Version10) && flag)
            {
                this._context.ErrorMessage = "Invalid Host header";
            }
            else
            {
                if (flag)
                {
                    host = this.UserHostAddress;
                }
                this._url = HttpUtility.CreateRequestUrl(this._uri, host, this.IsWebSocketRequest, this.IsSecureConnection);
                if (this._url == null)
                {
                    this._context.ErrorMessage = "Invalid request url";
                }
                else
                {
                    string str2 = this.Headers["Transfer-Encoding"];
                    if ((this._version > HttpVersion.Version10) && ((str2 != null) && (str2.Length > 0)))
                    {
                        this._chunked = str2.ToLower() == "chunked";
                        if (!this._chunked)
                        {
                            this._context.ErrorMessage = string.Empty;
                            this._context.ErrorStatus = 0x1f5;
                            return;
                        }
                    }
                    if (!this._chunked && !this._contentLengthSet)
                    {
                        string str3 = this._method.ToLower();
                        if ((str3 == "post") || (str3 == "put"))
                        {
                            this._context.ErrorMessage = string.Empty;
                            this._context.ErrorStatus = 0x19b;
                            return;
                        }
                    }
                    string str4 = this.Headers["Expect"];
                    if ((str4 != null) && ((str4.Length > 0) && (str4.ToLower() == "100-continue")))
                    {
                        this._context.Connection.GetResponseStream().InternalWrite(_100continue, 0, _100continue.Length);
                    }
                }
            }
        }

        internal bool FlushInput()
        {
            if (!this.HasEntityBody)
            {
                return true;
            }
            int count = 0x800;
            if (this._contentLength > 0L)
            {
                count = (int) Math.Min(this._contentLength, (long) count);
            }
            byte[] buffer = new byte[count];
            while (true)
            {
                bool flag;
                try
                {
                    IAsyncResult asyncResult = this.InputStream.BeginRead(buffer, 0, count, null, null);
                    if (!asyncResult.IsCompleted && !asyncResult.AsyncWaitHandle.WaitOne(100))
                    {
                        flag = false;
                    }
                    else
                    {
                        if (this.InputStream.EndRead(asyncResult) > 0)
                        {
                            continue;
                        }
                        flag = true;
                    }
                }
                catch
                {
                    flag = false;
                }
                return flag;
            }
        }

        public X509Certificate2 GetClientCertificate()
        {
            throw new NotImplementedException();
        }

        internal void SetRequestLine(string requestLine)
        {
            char[] separator = new char[] { ' ' };
            string[] strArray = requestLine.Split(separator, 3);
            if (strArray.Length != 3)
            {
                this._context.ErrorMessage = "Invalid request line (parts)";
            }
            else
            {
                this._method = strArray[0];
                if (!this._method.IsToken())
                {
                    this._context.ErrorMessage = "Invalid request line (method)";
                }
                else
                {
                    this._uri = strArray[1];
                    string str = strArray[2];
                    if ((str.Length != 8) || (!str.StartsWith("HTTP/") || (!tryCreateVersion(str.Substring(5), out this._version) || (this._version.Major < 1))))
                    {
                        this._context.ErrorMessage = "Invalid request line (version)";
                    }
                }
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder(0x40);
            builder.AppendFormat("{0} {1} HTTP/{2}\r\n", this._method, this._uri, this._version);
            builder.Append(this._headers.ToString());
            return builder.ToString();
        }

        private static bool tryCreateVersion(string version, out Version result)
        {
            try
            {
                result = new Version(version);
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        public string[] AcceptTypes =>
            this._acceptTypes;

        public int ClientCertificateError =>
            0;

        public Encoding ContentEncoding
        {
            get
            {
                Encoding encoding2 = this._contentEncoding;
                if (this._contentEncoding == null)
                {
                    Encoding local1 = this._contentEncoding;
                    encoding2 = this._contentEncoding = Encoding.Default;
                }
                return encoding2;
            }
        }

        public long ContentLength64 =>
            this._contentLength;

        public string ContentType =>
            this._headers["Content-Type"];

        public CookieCollection Cookies
        {
            get
            {
                CookieCollection collection2 = this._cookies;
                if (this._cookies == null)
                {
                    CookieCollection local1 = this._cookies;
                    collection2 = this._cookies = this._headers.GetCookies(false);
                }
                return collection2;
            }
        }

        public bool HasEntityBody =>
            (this._contentLength > 0L) || this._chunked;

        public NameValueCollection Headers =>
            this._headers;

        public string HttpMethod =>
            this._method;

        public Stream InputStream
        {
            get
            {
                Stream stream2 = this._inputStream;
                if (this._inputStream == null)
                {
                    Stream local1 = this._inputStream;
                    stream2 = this._inputStream = !this.HasEntityBody ? Stream.Null : this._context.Connection.GetRequestStream(this._contentLength, this._chunked);
                }
                return stream2;
            }
        }

        public bool IsAuthenticated =>
            !ReferenceEquals(this._context.User, null);

        public bool IsLocal =>
            this.RemoteEndPoint.Address.IsLocal();

        public bool IsSecureConnection =>
            this._context.Connection.IsSecure;

        public bool IsWebSocketRequest
        {
            get
            {
                if (!this._websocketRequestSet)
                {
                    this._websocketRequest = ((this._method == "GET") && ((this._version > HttpVersion.Version10) && this._headers.Contains("Upgrade", "websocket"))) && this._headers.Contains("Connection", "Upgrade");
                    this._websocketRequestSet = true;
                }
                return this._websocketRequest;
            }
        }

        public bool KeepAlive
        {
            get
            {
                if (!this._keepAliveSet)
                {
                    string str;
                    this._keepAlive = ((this._version > HttpVersion.Version10) || this._headers.Contains("Connection", "keep-alive")) || (((str = this._headers["Keep-Alive"]) != null) && (str != "closed"));
                    this._keepAliveSet = true;
                }
                return this._keepAlive;
            }
        }

        public IPEndPoint LocalEndPoint =>
            this._context.Connection.LocalEndPoint;

        public Version ProtocolVersion =>
            this._version;

        public NameValueCollection QueryString
        {
            get
            {
                NameValueCollection collection2 = this._queryString;
                if (this._queryString == null)
                {
                    NameValueCollection local1 = this._queryString;
                    collection2 = this._queryString = HttpUtility.InternalParseQueryString(this._url.Query, Encoding.UTF8);
                }
                return collection2;
            }
        }

        public string RawUrl =>
            this._url.PathAndQuery;

        public IPEndPoint RemoteEndPoint =>
            this._context.Connection.RemoteEndPoint;

        public Guid RequestTraceIdentifier =>
            this._identifier;

        public Uri Url =>
            this._url;

        public Uri UrlReferrer =>
            this._referer;

        public string UserAgent =>
            this._headers["User-Agent"];

        public string UserHostAddress =>
            this.LocalEndPoint.ToString();

        public string UserHostName =>
            this._headers["Host"];

        public string[] UserLanguages =>
            this._userLanguages;
    }
}

