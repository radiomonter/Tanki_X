namespace WebSocketSharp
{
    using System;
    using System.Collections.Specialized;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text;
    using WebSocketSharp.Net;

    internal class HttpResponse : HttpBase
    {
        private string _code;
        private string _reason;
        [CompilerGenerated]
        private static Func<string[], HttpResponse> <>f__mg$cache0;

        internal HttpResponse(HttpStatusCode code) : this(code, code.GetDescription())
        {
        }

        internal HttpResponse(HttpStatusCode code, string reason) : this(code.ToString(), reason, HttpVersion.Version11, new NameValueCollection())
        {
            base.Headers["Server"] = "websocket-sharp/1.0";
        }

        private HttpResponse(string code, string reason, Version version, NameValueCollection headers) : base(version, headers)
        {
            this._code = code;
            this._reason = reason;
        }

        internal static HttpResponse CreateCloseResponse(HttpStatusCode code) => 
            new HttpResponse(code) { Headers = { ["Connection"] = "close" } };

        internal static HttpResponse CreateUnauthorizedResponse(string challenge) => 
            new HttpResponse(HttpStatusCode.Unauthorized) { Headers = { ["WWW-Authenticate"] = challenge } };

        internal static HttpResponse CreateWebSocketResponse()
        {
            HttpResponse response = new HttpResponse(HttpStatusCode.SwitchingProtocols);
            NameValueCollection headers = response.Headers;
            headers["Upgrade"] = "websocket";
            headers["Connection"] = "Upgrade";
            return response;
        }

        internal static HttpResponse Parse(string[] headerParts)
        {
            char[] separator = new char[] { ' ' };
            string[] strArray = headerParts[0].Split(separator, 3);
            if (strArray.Length != 3)
            {
                throw new ArgumentException("Invalid status line: " + headerParts[0]);
            }
            WebHeaderCollection headers = new WebHeaderCollection();
            for (int i = 1; i < headerParts.Length; i++)
            {
                headers.InternalSet(headerParts[i], true);
            }
            return new HttpResponse(strArray[1], strArray[2], new Version(strArray[0].Substring(5)), headers);
        }

        internal static HttpResponse Read(Stream stream, int millisecondsTimeout)
        {
            if (<>f__mg$cache0 == null)
            {
                <>f__mg$cache0 = new Func<string[], HttpResponse>(HttpResponse.Parse);
            }
            return Read<HttpResponse>(stream, <>f__mg$cache0, millisecondsTimeout);
        }

        public void SetCookies(CookieCollection cookies)
        {
            if ((cookies != null) && (cookies.Count != 0))
            {
                NameValueCollection headers = base.Headers;
                foreach (Cookie cookie in cookies.Sorted)
                {
                    headers.Add("Set-Cookie", cookie.ToResponseString());
                }
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder(0x40);
            object[] args = new object[] { base.ProtocolVersion, this._code, this._reason, "\r\n" };
            builder.AppendFormat("HTTP/{0} {1} {2}{3}", args);
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

        public CookieCollection Cookies =>
            base.Headers.GetCookies(true);

        public bool HasConnectionClose =>
            base.Headers.Contains("Connection", "close");

        public bool IsProxyAuthenticationRequired =>
            this._code == "407";

        public bool IsRedirect =>
            (this._code == "301") || (this._code == "302");

        public bool IsUnauthorized =>
            this._code == "401";

        public bool IsWebSocketResponse
        {
            get
            {
                NameValueCollection headers = base.Headers;
                return (((base.ProtocolVersion > HttpVersion.Version10) && ((this._code == "101") && headers.Contains("Upgrade", "websocket"))) && headers.Contains("Connection", "Upgrade"));
            }
        }

        public string Reason =>
            this._reason;

        public string StatusCode =>
            this._code;
    }
}

