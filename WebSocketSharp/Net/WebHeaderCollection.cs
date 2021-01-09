namespace WebSocketSharp.Net
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization;
    using System.Security.Permissions;
    using System.Text;
    using WebSocketSharp;

    [Serializable, ComVisible(true)]
    public class WebHeaderCollection : NameValueCollection, ISerializable
    {
        private static readonly Dictionary<string, HttpHeaderInfo> _headers;
        private bool _internallyUsed;
        private HttpHeaderType _state;

        static WebHeaderCollection()
        {
            Dictionary<string, HttpHeaderInfo> dictionary = new Dictionary<string, HttpHeaderInfo>(StringComparer.InvariantCultureIgnoreCase) {
                { 
                    "Accept",
                    new HttpHeaderInfo("Accept", HttpHeaderType.MultiValue | HttpHeaderType.Restricted | HttpHeaderType.Request)
                },
                { 
                    "AcceptCharset",
                    new HttpHeaderInfo("Accept-Charset", HttpHeaderType.MultiValue | HttpHeaderType.Request)
                },
                { 
                    "AcceptEncoding",
                    new HttpHeaderInfo("Accept-Encoding", HttpHeaderType.MultiValue | HttpHeaderType.Request)
                },
                { 
                    "AcceptLanguage",
                    new HttpHeaderInfo("Accept-Language", HttpHeaderType.MultiValue | HttpHeaderType.Request)
                },
                { 
                    "AcceptRanges",
                    new HttpHeaderInfo("Accept-Ranges", HttpHeaderType.MultiValue | HttpHeaderType.Response)
                },
                { 
                    "Age",
                    new HttpHeaderInfo("Age", HttpHeaderType.Response)
                },
                { 
                    "Allow",
                    new HttpHeaderInfo("Allow", HttpHeaderType.MultiValue | HttpHeaderType.Response | HttpHeaderType.Request)
                },
                { 
                    "Authorization",
                    new HttpHeaderInfo("Authorization", HttpHeaderType.MultiValue | HttpHeaderType.Request)
                },
                { 
                    "CacheControl",
                    new HttpHeaderInfo("Cache-Control", HttpHeaderType.MultiValue | HttpHeaderType.Response | HttpHeaderType.Request)
                },
                { 
                    "Connection",
                    new HttpHeaderInfo("Connection", HttpHeaderType.MultiValue | HttpHeaderType.Restricted | HttpHeaderType.Response | HttpHeaderType.Request)
                },
                { 
                    "ContentEncoding",
                    new HttpHeaderInfo("Content-Encoding", HttpHeaderType.MultiValue | HttpHeaderType.Response | HttpHeaderType.Request)
                },
                { 
                    "ContentLanguage",
                    new HttpHeaderInfo("Content-Language", HttpHeaderType.MultiValue | HttpHeaderType.Response | HttpHeaderType.Request)
                },
                { 
                    "ContentLength",
                    new HttpHeaderInfo("Content-Length", HttpHeaderType.Restricted | HttpHeaderType.Response | HttpHeaderType.Request)
                },
                { 
                    "ContentLocation",
                    new HttpHeaderInfo("Content-Location", HttpHeaderType.Response | HttpHeaderType.Request)
                },
                { 
                    "ContentMd5",
                    new HttpHeaderInfo("Content-MD5", HttpHeaderType.Response | HttpHeaderType.Request)
                },
                { 
                    "ContentRange",
                    new HttpHeaderInfo("Content-Range", HttpHeaderType.Response | HttpHeaderType.Request)
                },
                { 
                    "ContentType",
                    new HttpHeaderInfo("Content-Type", HttpHeaderType.Restricted | HttpHeaderType.Response | HttpHeaderType.Request)
                },
                { 
                    "Cookie",
                    new HttpHeaderInfo("Cookie", HttpHeaderType.Request)
                },
                { 
                    "Cookie2",
                    new HttpHeaderInfo("Cookie2", HttpHeaderType.Request)
                },
                { 
                    "Date",
                    new HttpHeaderInfo("Date", HttpHeaderType.Restricted | HttpHeaderType.Response | HttpHeaderType.Request)
                },
                { 
                    "Expect",
                    new HttpHeaderInfo("Expect", HttpHeaderType.MultiValue | HttpHeaderType.Restricted | HttpHeaderType.Request)
                },
                { 
                    "Expires",
                    new HttpHeaderInfo("Expires", HttpHeaderType.Response | HttpHeaderType.Request)
                },
                { 
                    "ETag",
                    new HttpHeaderInfo("ETag", HttpHeaderType.Response)
                },
                { 
                    "From",
                    new HttpHeaderInfo("From", HttpHeaderType.Request)
                },
                { 
                    "Host",
                    new HttpHeaderInfo("Host", HttpHeaderType.Restricted | HttpHeaderType.Request)
                },
                { 
                    "IfMatch",
                    new HttpHeaderInfo("If-Match", HttpHeaderType.MultiValue | HttpHeaderType.Request)
                },
                { 
                    "IfModifiedSince",
                    new HttpHeaderInfo("If-Modified-Since", HttpHeaderType.Restricted | HttpHeaderType.Request)
                },
                { 
                    "IfNoneMatch",
                    new HttpHeaderInfo("If-None-Match", HttpHeaderType.MultiValue | HttpHeaderType.Request)
                },
                { 
                    "IfRange",
                    new HttpHeaderInfo("If-Range", HttpHeaderType.Request)
                },
                { 
                    "IfUnmodifiedSince",
                    new HttpHeaderInfo("If-Unmodified-Since", HttpHeaderType.Request)
                },
                { 
                    "KeepAlive",
                    new HttpHeaderInfo("Keep-Alive", HttpHeaderType.MultiValue | HttpHeaderType.Response | HttpHeaderType.Request)
                },
                { 
                    "LastModified",
                    new HttpHeaderInfo("Last-Modified", HttpHeaderType.Response | HttpHeaderType.Request)
                },
                { 
                    "Location",
                    new HttpHeaderInfo("Location", HttpHeaderType.Response)
                },
                { 
                    "MaxForwards",
                    new HttpHeaderInfo("Max-Forwards", HttpHeaderType.Request)
                },
                { 
                    "Pragma",
                    new HttpHeaderInfo("Pragma", HttpHeaderType.Response | HttpHeaderType.Request)
                },
                { 
                    "ProxyAuthenticate",
                    new HttpHeaderInfo("Proxy-Authenticate", HttpHeaderType.MultiValue | HttpHeaderType.Response)
                },
                { 
                    "ProxyAuthorization",
                    new HttpHeaderInfo("Proxy-Authorization", HttpHeaderType.Request)
                },
                { 
                    "ProxyConnection",
                    new HttpHeaderInfo("Proxy-Connection", HttpHeaderType.Restricted | HttpHeaderType.Response | HttpHeaderType.Request)
                },
                { 
                    "Public",
                    new HttpHeaderInfo("Public", HttpHeaderType.MultiValue | HttpHeaderType.Response)
                },
                { 
                    "Range",
                    new HttpHeaderInfo("Range", HttpHeaderType.MultiValue | HttpHeaderType.Restricted | HttpHeaderType.Request)
                },
                { 
                    "Referer",
                    new HttpHeaderInfo("Referer", HttpHeaderType.Restricted | HttpHeaderType.Request)
                },
                { 
                    "RetryAfter",
                    new HttpHeaderInfo("Retry-After", HttpHeaderType.Response)
                },
                { 
                    "SecWebSocketAccept",
                    new HttpHeaderInfo("Sec-WebSocket-Accept", HttpHeaderType.Restricted | HttpHeaderType.Response)
                },
                { 
                    "SecWebSocketExtensions",
                    new HttpHeaderInfo("Sec-WebSocket-Extensions", HttpHeaderType.MultiValueInRequest | HttpHeaderType.Restricted | HttpHeaderType.Response | HttpHeaderType.Request)
                },
                { 
                    "SecWebSocketKey",
                    new HttpHeaderInfo("Sec-WebSocket-Key", HttpHeaderType.Restricted | HttpHeaderType.Request)
                },
                { 
                    "SecWebSocketProtocol",
                    new HttpHeaderInfo("Sec-WebSocket-Protocol", HttpHeaderType.MultiValueInRequest | HttpHeaderType.Response | HttpHeaderType.Request)
                },
                { 
                    "SecWebSocketVersion",
                    new HttpHeaderInfo("Sec-WebSocket-Version", HttpHeaderType.MultiValueInResponse | HttpHeaderType.Restricted | HttpHeaderType.Response | HttpHeaderType.Request)
                },
                { 
                    "Server",
                    new HttpHeaderInfo("Server", HttpHeaderType.Response)
                },
                { 
                    "SetCookie",
                    new HttpHeaderInfo("Set-Cookie", HttpHeaderType.MultiValue | HttpHeaderType.Response)
                },
                { 
                    "SetCookie2",
                    new HttpHeaderInfo("Set-Cookie2", HttpHeaderType.MultiValue | HttpHeaderType.Response)
                },
                { 
                    "Te",
                    new HttpHeaderInfo("TE", HttpHeaderType.Request)
                },
                { 
                    "Trailer",
                    new HttpHeaderInfo("Trailer", HttpHeaderType.Response | HttpHeaderType.Request)
                },
                { 
                    "TransferEncoding",
                    new HttpHeaderInfo("Transfer-Encoding", HttpHeaderType.MultiValue | HttpHeaderType.Restricted | HttpHeaderType.Response | HttpHeaderType.Request)
                },
                { 
                    "Translate",
                    new HttpHeaderInfo("Translate", HttpHeaderType.Request)
                },
                { 
                    "Upgrade",
                    new HttpHeaderInfo("Upgrade", HttpHeaderType.MultiValue | HttpHeaderType.Response | HttpHeaderType.Request)
                },
                { 
                    "UserAgent",
                    new HttpHeaderInfo("User-Agent", HttpHeaderType.Restricted | HttpHeaderType.Request)
                },
                { 
                    "Vary",
                    new HttpHeaderInfo("Vary", HttpHeaderType.MultiValue | HttpHeaderType.Response)
                },
                { 
                    "Via",
                    new HttpHeaderInfo("Via", HttpHeaderType.MultiValue | HttpHeaderType.Response | HttpHeaderType.Request)
                },
                { 
                    "Warning",
                    new HttpHeaderInfo("Warning", HttpHeaderType.MultiValue | HttpHeaderType.Response | HttpHeaderType.Request)
                },
                { 
                    "WwwAuthenticate",
                    new HttpHeaderInfo("WWW-Authenticate", HttpHeaderType.MultiValue | HttpHeaderType.Restricted | HttpHeaderType.Response)
                }
            };
            _headers = dictionary;
        }

        public WebHeaderCollection()
        {
        }

        protected WebHeaderCollection(SerializationInfo serializationInfo, StreamingContext streamingContext)
        {
            if (serializationInfo == null)
            {
                throw new ArgumentNullException("serializationInfo");
            }
            try
            {
                this._internallyUsed = serializationInfo.GetBoolean("InternallyUsed");
                this._state = (HttpHeaderType) serializationInfo.GetInt32("State");
                int num = serializationInfo.GetInt32("Count");
                for (int i = 0; i < num; i++)
                {
                    int num3 = num + i;
                    base.Add(serializationInfo.GetString(i.ToString()), serializationInfo.GetString(num3.ToString()));
                }
            }
            catch (SerializationException exception)
            {
                throw new ArgumentException(exception.Message, "serializationInfo", exception);
            }
        }

        internal WebHeaderCollection(HttpHeaderType state, bool internallyUsed)
        {
            this._state = state;
            this._internallyUsed = internallyUsed;
        }

        private void add(string name, string value, bool ignoreRestricted)
        {
            Action<string, string> action = !ignoreRestricted ? new Action<string, string>(this.addWithoutCheckingName) : new Action<string, string>(this.addWithoutCheckingNameAndRestricted);
            this.doWithCheckingState(action, checkName(name), value, true);
        }

        public void Add(string header)
        {
            if ((header == null) || (header.Length == 0))
            {
                throw new ArgumentNullException("header");
            }
            int length = checkColonSeparated(header);
            this.add(header.Substring(0, length), header.Substring(length + 1), false);
        }

        public override void Add(string name, string value)
        {
            this.add(name, value, false);
        }

        public void Add(HttpRequestHeader header, string value)
        {
            this.doWithCheckingState(new Action<string, string>(this.addWithoutCheckingName), Convert(header), value, false, true);
        }

        public void Add(HttpResponseHeader header, string value)
        {
            this.doWithCheckingState(new Action<string, string>(this.addWithoutCheckingName), Convert(header), value, true, true);
        }

        private void addWithoutCheckingName(string name, string value)
        {
            this.doWithoutCheckingName(new Action<string, string>(this.Add), name, value);
        }

        private void addWithoutCheckingNameAndRestricted(string name, string value)
        {
            base.Add(name, checkValue(value));
        }

        protected void AddWithoutValidate(string headerName, string headerValue)
        {
            this.add(headerName, headerValue, true);
        }

        private static int checkColonSeparated(string header)
        {
            int index = header.IndexOf(':');
            if (index == -1)
            {
                throw new ArgumentException("No colon could be found.", "header");
            }
            return index;
        }

        private static HttpHeaderType checkHeaderType(string name)
        {
            HttpHeaderInfo info = getHeaderInfo(name);
            return ((info != null) ? ((!info.IsRequest || info.IsResponse) ? ((info.IsRequest || !info.IsResponse) ? HttpHeaderType.Unspecified : HttpHeaderType.Response) : HttpHeaderType.Request) : HttpHeaderType.Unspecified);
        }

        private static string checkName(string name)
        {
            if ((name == null) || (name.Length == 0))
            {
                throw new ArgumentNullException("name");
            }
            name = name.Trim();
            if (!IsHeaderName(name))
            {
                throw new ArgumentException("Contains invalid characters.", "name");
            }
            return name;
        }

        private void checkRestricted(string name)
        {
            if (!this._internallyUsed && isRestricted(name, true))
            {
                throw new ArgumentException("This header must be modified with the appropiate property.");
            }
        }

        private void checkState(bool response)
        {
            if (this._state != HttpHeaderType.Unspecified)
            {
                if (response && (this._state == HttpHeaderType.Request))
                {
                    throw new InvalidOperationException("This collection has already been used to store the request headers.");
                }
                if (!response && (this._state == HttpHeaderType.Response))
                {
                    throw new InvalidOperationException("This collection has already been used to store the response headers.");
                }
            }
        }

        private static string checkValue(string value)
        {
            if ((value == null) || (value.Length == 0))
            {
                return string.Empty;
            }
            value = value.Trim();
            if (value.Length > 0xffff)
            {
                throw new ArgumentOutOfRangeException("value", "Greater than 65,535 characters.");
            }
            if (!IsHeaderValue(value))
            {
                throw new ArgumentException("Contains invalid characters.", "value");
            }
            return value;
        }

        public override void Clear()
        {
            base.Clear();
            this._state = HttpHeaderType.Unspecified;
        }

        private static string convert(string key)
        {
            HttpHeaderInfo info;
            return (!_headers.TryGetValue(key, out info) ? string.Empty : info.Name);
        }

        internal static string Convert(HttpRequestHeader header) => 
            convert(header.ToString());

        internal static string Convert(HttpResponseHeader header) => 
            convert(header.ToString());

        private void doWithCheckingState(Action<string, string> action, string name, string value, bool setState)
        {
            HttpHeaderType type = checkHeaderType(name);
            if (type == HttpHeaderType.Request)
            {
                this.doWithCheckingState(action, name, value, false, setState);
            }
            else if (type == HttpHeaderType.Response)
            {
                this.doWithCheckingState(action, name, value, true, setState);
            }
            else
            {
                action(name, value);
            }
        }

        private void doWithCheckingState(Action<string, string> action, string name, string value, bool response, bool setState)
        {
            this.checkState(response);
            action(name, value);
            if (setState)
            {
                this._state ??= (!response ? HttpHeaderType.Request : HttpHeaderType.Response);
            }
        }

        private void doWithoutCheckingName(Action<string, string> action, string name, string value)
        {
            this.checkRestricted(name);
            action(name, checkValue(value));
        }

        public override string Get(int index) => 
            base.Get(index);

        public override string Get(string name) => 
            base.Get(name);

        public override IEnumerator GetEnumerator() => 
            base.GetEnumerator();

        private static HttpHeaderInfo getHeaderInfo(string name)
        {
            HttpHeaderInfo info2;
            using (Dictionary<string, HttpHeaderInfo>.ValueCollection.Enumerator enumerator = _headers.Values.GetEnumerator())
            {
                while (true)
                {
                    if (enumerator.MoveNext())
                    {
                        HttpHeaderInfo current = enumerator.Current;
                        if (!current.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                        {
                            continue;
                        }
                        info2 = current;
                    }
                    else
                    {
                        return null;
                    }
                    break;
                }
            }
            return info2;
        }

        public override string GetKey(int index) => 
            base.GetKey(index);

        [SecurityPermission(SecurityAction.LinkDemand, Flags=SecurityPermissionFlag.SerializationFormatter)]
        public override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
        {
            <GetObjectData>c__AnonStorey1 storey = new <GetObjectData>c__AnonStorey1 {
                serializationInfo = serializationInfo,
                $this = this
            };
            if (storey.serializationInfo == null)
            {
                throw new ArgumentNullException("serializationInfo");
            }
            storey.serializationInfo.AddValue("InternallyUsed", this._internallyUsed);
            storey.serializationInfo.AddValue("State", (int) this._state);
            storey.cnt = this.Count;
            storey.serializationInfo.AddValue("Count", storey.cnt);
            storey.cnt.Times(new Action<int>(storey.<>m__0));
        }

        public override string[] GetValues(int index)
        {
            string[] values = base.GetValues(index);
            return (((values == null) || (values.Length <= 0)) ? null : values);
        }

        public override string[] GetValues(string header)
        {
            string[] values = base.GetValues(header);
            return (((values == null) || (values.Length <= 0)) ? null : values);
        }

        internal void InternalRemove(string name)
        {
            base.Remove(name);
        }

        internal void InternalSet(string header, bool response)
        {
            int length = checkColonSeparated(header);
            this.InternalSet(header.Substring(0, length), header.Substring(length + 1), response);
        }

        internal void InternalSet(string name, string value, bool response)
        {
            value = checkValue(value);
            if (IsMultiValue(name, response))
            {
                base.Add(name, value);
            }
            else
            {
                base.Set(name, value);
            }
        }

        internal static bool IsHeaderName(string name) => 
            ((name != null) && (name.Length > 0)) && name.IsToken();

        internal static bool IsHeaderValue(string value) => 
            value.IsText();

        internal static bool IsMultiValue(string headerName, bool response)
        {
            if ((headerName == null) || (headerName.Length == 0))
            {
                return false;
            }
            HttpHeaderInfo info = getHeaderInfo(headerName);
            return ((info != null) && info.IsMultiValue(response));
        }

        private static bool isRestricted(string name, bool response)
        {
            HttpHeaderInfo info = getHeaderInfo(name);
            return ((info != null) && info.IsRestricted(response));
        }

        public static bool IsRestricted(string headerName) => 
            isRestricted(checkName(headerName), false);

        public static bool IsRestricted(string headerName, bool response) => 
            isRestricted(checkName(headerName), response);

        public override void OnDeserialization(object sender)
        {
        }

        public override void Remove(string name)
        {
            this.doWithCheckingState(new Action<string, string>(this.removeWithoutCheckingName), checkName(name), null, false);
        }

        public void Remove(HttpRequestHeader header)
        {
            this.doWithCheckingState(new Action<string, string>(this.removeWithoutCheckingName), Convert(header), null, false, false);
        }

        public void Remove(HttpResponseHeader header)
        {
            this.doWithCheckingState(new Action<string, string>(this.removeWithoutCheckingName), Convert(header), null, true, false);
        }

        private void removeWithoutCheckingName(string name, string unuse)
        {
            this.checkRestricted(name);
            base.Remove(name);
        }

        public override void Set(string name, string value)
        {
            this.doWithCheckingState(new Action<string, string>(this.setWithoutCheckingName), checkName(name), value, true);
        }

        public void Set(HttpRequestHeader header, string value)
        {
            this.doWithCheckingState(new Action<string, string>(this.setWithoutCheckingName), Convert(header), value, false, true);
        }

        public void Set(HttpResponseHeader header, string value)
        {
            this.doWithCheckingState(new Action<string, string>(this.setWithoutCheckingName), Convert(header), value, true, true);
        }

        private void setWithoutCheckingName(string name, string value)
        {
            this.doWithoutCheckingName(new Action<string, string>(this.Set), name, value);
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags=SecurityPermissionFlag.SerializationFormatter, SerializationFormatter=true)]
        void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
        {
            this.GetObjectData(serializationInfo, streamingContext);
        }

        public byte[] ToByteArray() => 
            Encoding.UTF8.GetBytes(this.ToString());

        public override string ToString()
        {
            <ToString>c__AnonStorey2 storey = new <ToString>c__AnonStorey2 {
                $this = this,
                buff = new StringBuilder()
            };
            this.Count.Times(new Action<int>(storey.<>m__0));
            return storey.buff.Append("\r\n").ToString();
        }

        internal string ToStringMultiValue(bool response)
        {
            <ToStringMultiValue>c__AnonStorey0 storey = new <ToStringMultiValue>c__AnonStorey0 {
                response = response,
                $this = this,
                buff = new StringBuilder()
            };
            this.Count.Times(new Action<int>(storey.<>m__0));
            return storey.buff.Append("\r\n").ToString();
        }

        internal HttpHeaderType State =>
            this._state;

        public override string[] AllKeys =>
            base.AllKeys;

        public override int Count =>
            base.Count;

        public string this[HttpRequestHeader header]
        {
            get => 
                this.Get(Convert(header));
            set => 
                this.Add(header, value);
        }

        public string this[HttpResponseHeader header]
        {
            get => 
                this.Get(Convert(header));
            set => 
                this.Add(header, value);
        }

        public override NameObjectCollectionBase.KeysCollection Keys =>
            base.Keys;

        [CompilerGenerated]
        private sealed class <GetObjectData>c__AnonStorey1
        {
            internal SerializationInfo serializationInfo;
            internal int cnt;
            internal WebHeaderCollection $this;

            internal void <>m__0(int i)
            {
                this.serializationInfo.AddValue(i.ToString(), this.$this.GetKey(i));
                this.serializationInfo.AddValue((this.cnt + i).ToString(), this.$this.Get(i));
            }
        }

        [CompilerGenerated]
        private sealed class <ToString>c__AnonStorey2
        {
            internal StringBuilder buff;
            internal WebHeaderCollection $this;

            internal void <>m__0(int i)
            {
                this.buff.AppendFormat("{0}: {1}\r\n", this.$this.GetKey(i), this.$this.Get(i));
            }
        }

        [CompilerGenerated]
        private sealed class <ToStringMultiValue>c__AnonStorey0
        {
            internal bool response;
            internal StringBuilder buff;
            internal WebHeaderCollection $this;

            internal void <>m__0(int i)
            {
                string key = this.$this.GetKey(i);
                if (!WebHeaderCollection.IsMultiValue(key, this.response))
                {
                    this.buff.AppendFormat("{0}: {1}\r\n", key, this.$this.Get(i));
                }
                else
                {
                    foreach (string str2 in this.$this.GetValues(i))
                    {
                        this.buff.AppendFormat("{0}: {1}\r\n", key, str2);
                    }
                }
            }
        }
    }
}

