namespace WebSocketSharp
{
    using System;
    using System.Collections.Specialized;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text;
    using WebSocketSharp.Net;

    internal class HttpRequest : HttpBase
    {
        private string _method;
        private string _uri;
        private bool _websocketRequest;
        private bool _websocketRequestSet;
        [CompilerGenerated]
        private static Func<string[], HttpResponse> <>f__mg$cache0;
        [CompilerGenerated]
        private static Func<string[], HttpRequest> <>f__mg$cache1;

        internal HttpRequest(string method, string uri) : this(method, uri, HttpVersion.Version11, new NameValueCollection())
        {
            base.Headers["User-Agent"] = "websocket-sharp/1.0";
        }

        private HttpRequest(string method, string uri, Version version, NameValueCollection headers) : base(version, headers)
        {
            this._method = method;
            this._uri = uri;
        }

        internal static HttpRequest CreateConnectRequest(Uri uri)
        {
            string dnsSafeHost = uri.DnsSafeHost;
            int port = uri.Port;
            string str2 = $"{dnsSafeHost}:{port}";
            return new HttpRequest("CONNECT", str2) { Headers = { ["Host"] = (port != 80) ? str2 : dnsSafeHost } };
        }

        internal static HttpRequest CreateWebSocketRequest(Uri uri)
        {
            string dnsSafeHost;
            HttpRequest request = new HttpRequest("GET", uri.PathAndQuery);
            NameValueCollection headers = request.Headers;
            int port = uri.Port;
            string scheme = uri.Scheme;
            if (((port == 80) && (scheme == "ws")) || ((port == 0x1bb) && (scheme == "wss")))
            {
                dnsSafeHost = uri.DnsSafeHost;
            }
            else
            {
                dnsSafeHost = uri.Authority;
            }
            headers["Host"] = dnsSafeHost;
            headers["Upgrade"] = "websocket";
            headers["Connection"] = "Upgrade";
            return request;
        }

        internal HttpResponse GetResponse(Stream stream, int millisecondsTimeout)
        {
            byte[] buffer = base.ToByteArray();
            stream.Write(buffer, 0, buffer.Length);
            if (<>f__mg$cache0 == null)
            {
                <>f__mg$cache0 = new Func<string[], HttpResponse>(HttpResponse.Parse);
            }
            return Read<HttpResponse>(stream, <>f__mg$cache0, millisecondsTimeout);
        }

        internal static HttpRequest Parse(string[] headerParts)
        {
            char[] separator = new char[] { ' ' };
            string[] strArray = headerParts[0].Split(separator, 3);
            if (strArray.Length != 3)
            {
                throw new ArgumentException("Invalid request line: " + headerParts[0]);
            }
            WebHeaderCollection headers = new WebHeaderCollection();
            for (int i = 1; i < headerParts.Length; i++)
            {
                headers.InternalSet(headerParts[i], false);
            }
            return new HttpRequest(strArray[0], strArray[1], new Version(strArray[2].Substring(5)), headers);
        }

        internal static HttpRequest Read(Stream stream, int millisecondsTimeout)
        {
            if (<>f__mg$cache1 == null)
            {
                <>f__mg$cache1 = new Func<string[], HttpRequest>(HttpRequest.Parse);
            }
            return Read<HttpRequest>(stream, <>f__mg$cache1, millisecondsTimeout);
        }

        public void SetCookies(CookieCollection cookies)
        {
            if ((cookies != null) && (cookies.Count != 0))
            {
                StringBuilder builder = new StringBuilder(0x40);
                foreach (Cookie cookie in cookies.Sorted)
                {
                    if (!cookie.Expired)
                    {
                        builder.AppendFormat("{0}; ", cookie.ToString());
                    }
                }
                int length = builder.Length;
                if (length > 2)
                {
                    builder.Length = length - 2;
                    base.Headers["Cookie"] = builder.ToString();
                }
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder(0x40);
            object[] args = new object[] { this._method, this._uri, base.ProtocolVersion, "\r\n" };
            builder.AppendFormat("{0} {1} HTTP/{2}{3}", args);
            NameValueCollection headers = base.Headers;
            foreach (string str in headers.AllKeys)
            {
                builder.AppendFormat("{0}: {1}{2}", str, headers[str], "\r\n");
            }
            builder.Append("\r\n");
            string entityBody = base.EntityBody;
            if (entityBody.Length > 0)
            {
                builder.Append(entityBody);
            }
            return builder.ToString();
        }

        public WebSocketSharp.Net.AuthenticationResponse AuthenticationResponse
        {
            get
            {
                string str = base.Headers["Authorization"];
                return (((str == null) || (str.Length <= 0)) ? null : WebSocketSharp.Net.AuthenticationResponse.Parse(str));
            }
        }

        public CookieCollection Cookies =>
            base.Headers.GetCookies(false);

        public string HttpMethod =>
            this._method;

        public bool IsWebSocketRequest
        {
            get
            {
                if (!this._websocketRequestSet)
                {
                    NameValueCollection headers = base.Headers;
                    this._websocketRequest = ((this._method == "GET") && ((base.ProtocolVersion > HttpVersion.Version10) && headers.Contains("Upgrade", "websocket"))) && headers.Contains("Connection", "Upgrade");
                    this._websocketRequestSet = true;
                }
                return this._websocketRequest;
            }
        }

        public string RequestUri =>
            this._uri;
    }
}

