namespace WebSocketSharp.Net
{
    using System;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using WebSocketSharp;

    [Serializable]
    public sealed class Cookie
    {
        private string _comment;
        private Uri _commentUri;
        private bool _discard;
        private string _domain;
        private DateTime _expires;
        private bool _httpOnly;
        private string _name;
        private string _path;
        private string _port;
        private int[] _ports;
        private static readonly char[] _reservedCharsForName = new char[] { ' ', '=', ';', ',', '\n', '\r', '\t' };
        private static readonly char[] _reservedCharsForValue = new char[] { ';', ',' };
        private bool _secure;
        private DateTime _timestamp;
        private string _value;
        private int _version;

        public Cookie()
        {
            this._comment = string.Empty;
            this._domain = string.Empty;
            this._expires = DateTime.MinValue;
            this._name = string.Empty;
            this._path = string.Empty;
            this._port = string.Empty;
            this._ports = new int[0];
            this._timestamp = DateTime.Now;
            this._value = string.Empty;
            this._version = 0;
        }

        public Cookie(string name, string value) : this()
        {
            this.Name = name;
            this.Value = value;
        }

        public Cookie(string name, string value, string path) : this(name, value)
        {
            this.Path = path;
        }

        public Cookie(string name, string value, string path, string domain) : this(name, value, path)
        {
            this.Domain = domain;
        }

        private static bool canSetName(string name, out string message)
        {
            if (name.IsNullOrEmpty())
            {
                message = "The value specified for the Name is null or empty.";
                return false;
            }
            if ((name[0] != '$') && !name.Contains(_reservedCharsForName))
            {
                message = string.Empty;
                return true;
            }
            message = "The value specified for the Name contains an invalid character.";
            return false;
        }

        private static bool canSetValue(string value, out string message)
        {
            if (value == null)
            {
                message = "The value specified for the Value is null.";
                return false;
            }
            if (value.Contains(_reservedCharsForValue) && !value.IsEnclosedIn('"'))
            {
                message = "The value specified for the Value contains an invalid character.";
                return false;
            }
            message = string.Empty;
            return true;
        }

        public override bool Equals(object comparand)
        {
            Cookie cookie = comparand as Cookie;
            return (((cookie != null) && (this._name.Equals(cookie.Name, StringComparison.InvariantCultureIgnoreCase) && (this._value.Equals(cookie.Value, StringComparison.InvariantCulture) && (this._path.Equals(cookie.Path, StringComparison.InvariantCulture) && this._domain.Equals(cookie.Domain, StringComparison.InvariantCultureIgnoreCase))))) && (this._version == cookie.Version));
        }

        public override int GetHashCode() => 
            hash(StringComparer.InvariantCultureIgnoreCase.GetHashCode(this._name), this._value.GetHashCode(), this._path.GetHashCode(), StringComparer.InvariantCultureIgnoreCase.GetHashCode(this._domain), this._version);

        private static int hash(int i, int j, int k, int l, int m) => 
            (((i ^ ((j << 13) | (j >> 0x13))) ^ ((k << 0x1a) | (k >> 6))) ^ ((l << 7) | (l >> 0x19))) ^ ((m << 20) | (m >> 12));

        internal string ToRequestString(Uri uri)
        {
            if (this._name.Length == 0)
            {
                return string.Empty;
            }
            if (this._version == 0)
            {
                return $"{this._name}={this._value}";
            }
            StringBuilder builder = new StringBuilder(0x40);
            builder.AppendFormat("$Version={0}; {1}={2}", this._version, this._name, this._value);
            if (!this._path.IsNullOrEmpty())
            {
                builder.AppendFormat("; $Path={0}", this._path);
            }
            else if (uri != null)
            {
                builder.AppendFormat("; $Path={0}", uri.GetAbsolutePath());
            }
            else
            {
                builder.Append("; $Path=/");
            }
            if (((uri == null) || (uri.Host != this._domain)) && !this._domain.IsNullOrEmpty())
            {
                builder.AppendFormat("; $Domain={0}", this._domain);
            }
            if (!this._port.IsNullOrEmpty())
            {
                if (this._port == "\"\"")
                {
                    builder.Append("; $Port");
                }
                else
                {
                    builder.AppendFormat("; $Port={0}", this._port);
                }
            }
            return builder.ToString();
        }

        internal string ToResponseString() => 
            (this._name.Length <= 0) ? string.Empty : ((this._version != 0) ? this.toResponseStringVersion1() : this.toResponseStringVersion0());

        private string toResponseStringVersion0()
        {
            StringBuilder builder = new StringBuilder(0x40);
            builder.AppendFormat("{0}={1}", this._name, this._value);
            if (this._expires != DateTime.MinValue)
            {
                builder.AppendFormat("; Expires={0}", this._expires.ToUniversalTime().ToString("ddd, dd'-'MMM'-'yyyy HH':'mm':'ss 'GMT'", CultureInfo.CreateSpecificCulture("en-US")));
            }
            if (!this._path.IsNullOrEmpty())
            {
                builder.AppendFormat("; Path={0}", this._path);
            }
            if (!this._domain.IsNullOrEmpty())
            {
                builder.AppendFormat("; Domain={0}", this._domain);
            }
            if (this._secure)
            {
                builder.Append("; Secure");
            }
            if (this._httpOnly)
            {
                builder.Append("; HttpOnly");
            }
            return builder.ToString();
        }

        private string toResponseStringVersion1()
        {
            StringBuilder builder = new StringBuilder(0x40);
            builder.AppendFormat("{0}={1}; Version={2}", this._name, this._value, this._version);
            if (this._expires != DateTime.MinValue)
            {
                builder.AppendFormat("; Max-Age={0}", this.MaxAge);
            }
            if (!this._path.IsNullOrEmpty())
            {
                builder.AppendFormat("; Path={0}", this._path);
            }
            if (!this._domain.IsNullOrEmpty())
            {
                builder.AppendFormat("; Domain={0}", this._domain);
            }
            if (!this._port.IsNullOrEmpty())
            {
                if (this._port == "\"\"")
                {
                    builder.Append("; Port");
                }
                else
                {
                    builder.AppendFormat("; Port={0}", this._port);
                }
            }
            if (!this._comment.IsNullOrEmpty())
            {
                builder.AppendFormat("; Comment={0}", this._comment.UrlEncode());
            }
            if (this._commentUri != null)
            {
                string originalString = this._commentUri.OriginalString;
                builder.AppendFormat("; CommentURL={0}", !originalString.IsToken() ? originalString.Quote() : originalString);
            }
            if (this._discard)
            {
                builder.Append("; Discard");
            }
            if (this._secure)
            {
                builder.Append("; Secure");
            }
            return builder.ToString();
        }

        public override string ToString() => 
            this.ToRequestString(null);

        private static bool tryCreatePorts(string value, out int[] result, out string parseError)
        {
            char[] trimChars = new char[] { '"' };
            char[] separator = new char[] { ',' };
            string[] strArray = value.Trim(trimChars).Split(separator);
            int length = strArray.Length;
            int[] numArray = new int[length];
            for (int i = 0; i < length; i++)
            {
                numArray[i] = -2147483648;
                string s = strArray[i].Trim();
                if ((s.Length != 0) && !int.TryParse(s, out numArray[i]))
                {
                    result = new int[0];
                    parseError = s;
                    return false;
                }
            }
            result = numArray;
            parseError = string.Empty;
            return true;
        }

        internal bool ExactDomain { get; set; }

        internal int MaxAge
        {
            get
            {
                if (this._expires == DateTime.MinValue)
                {
                    return 0;
                }
                TimeSpan span = ((this._expires.Kind == DateTimeKind.Local) ? this._expires : this._expires.ToLocalTime()) - DateTime.Now;
                return ((span <= TimeSpan.Zero) ? 0 : ((int) span.TotalSeconds));
            }
        }

        internal int[] Ports =>
            this._ports;

        public string Comment
        {
            get => 
                this._comment;
            set
            {
                string text1 = value;
                if (value == null)
                {
                    string local1 = value;
                    text1 = string.Empty;
                }
                this._comment = text1;
            }
        }

        public Uri CommentUri
        {
            get => 
                this._commentUri;
            set => 
                this._commentUri = value;
        }

        public bool Discard
        {
            get => 
                this._discard;
            set => 
                this._discard = value;
        }

        public string Domain
        {
            get => 
                this._domain;
            set
            {
                if (value.IsNullOrEmpty())
                {
                    this._domain = string.Empty;
                    this.ExactDomain = true;
                }
                else
                {
                    this._domain = value;
                    this.ExactDomain = value[0] != '.';
                }
            }
        }

        public bool Expired
        {
            get => 
                (this._expires != DateTime.MinValue) && (this._expires <= DateTime.Now);
            set => 
                this._expires = !value ? DateTime.MinValue : DateTime.Now;
        }

        public DateTime Expires
        {
            get => 
                this._expires;
            set => 
                this._expires = value;
        }

        public bool HttpOnly
        {
            get => 
                this._httpOnly;
            set => 
                this._httpOnly = value;
        }

        public string Name
        {
            get => 
                this._name;
            set
            {
                string str;
                if (!canSetName(value, out str))
                {
                    throw new CookieException(str);
                }
                this._name = value;
            }
        }

        public string Path
        {
            get => 
                this._path;
            set
            {
                string text1 = value;
                if (value == null)
                {
                    string local1 = value;
                    text1 = string.Empty;
                }
                this._path = text1;
            }
        }

        public string Port
        {
            get => 
                this._port;
            set
            {
                if (value.IsNullOrEmpty())
                {
                    this._port = string.Empty;
                    this._ports = new int[0];
                }
                else
                {
                    string str;
                    if (!value.IsEnclosedIn('"'))
                    {
                        throw new CookieException("The value specified for the Port attribute isn't enclosed in double quotes.");
                    }
                    if (!tryCreatePorts(value, out this._ports, out str))
                    {
                        throw new CookieException($"The value specified for the Port attribute contains an invalid value: {str}");
                    }
                    this._port = value;
                }
            }
        }

        public bool Secure
        {
            get => 
                this._secure;
            set => 
                this._secure = value;
        }

        public DateTime TimeStamp =>
            this._timestamp;

        public string Value
        {
            get => 
                this._value;
            set
            {
                string str;
                if (!canSetValue(value, out str))
                {
                    throw new CookieException(str);
                }
                this._value = (value.Length <= 0) ? "\"\"" : value;
            }
        }

        public int Version
        {
            get => 
                this._version;
            set
            {
                if ((value < 0) || (value > 1))
                {
                    throw new ArgumentOutOfRangeException("value", "Not 0 or 1.");
                }
                this._version = value;
            }
        }
    }
}

