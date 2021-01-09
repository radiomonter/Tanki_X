namespace WebSocketSharp.Net
{
    using System;
    using System.Collections.Specialized;
    using System.Runtime.CompilerServices;
    using System.Security.Cryptography;
    using System.Security.Principal;
    using System.Text;
    using WebSocketSharp;

    internal class AuthenticationResponse : AuthenticationBase
    {
        private uint _nonceCount;
        [CompilerGenerated]
        private static Func<string, bool> <>f__am$cache0;

        internal AuthenticationResponse(NetworkCredential credentials) : this(AuthenticationSchemes.Basic, new NameValueCollection(), credentials, 0)
        {
        }

        private AuthenticationResponse(AuthenticationSchemes scheme, NameValueCollection parameters) : base(scheme, parameters)
        {
        }

        internal AuthenticationResponse(AuthenticationChallenge challenge, NetworkCredential credentials, uint nonceCount) : this(challenge.Scheme, challenge.Parameters, credentials, nonceCount)
        {
        }

        internal AuthenticationResponse(AuthenticationSchemes scheme, NameValueCollection parameters, NetworkCredential credentials, uint nonceCount) : base(scheme, parameters)
        {
            base.Parameters["username"] = credentials.UserName;
            base.Parameters["password"] = credentials.Password;
            base.Parameters["uri"] = credentials.Domain;
            this._nonceCount = nonceCount;
            if (scheme == AuthenticationSchemes.Digest)
            {
                this.initAsDigest();
            }
        }

        private static string createA1(string username, string password, string realm) => 
            $"{username}:{realm}:{password}";

        private static string createA1(string username, string password, string realm, string nonce, string cnonce) => 
            $"{hash(createA1(username, password, realm))}:{nonce}:{cnonce}";

        private static string createA2(string method, string uri) => 
            $"{method}:{uri}";

        private static string createA2(string method, string uri, string entity) => 
            $"{method}:{uri}:{hash(entity)}";

        internal static string CreateRequestDigest(NameValueCollection parameters)
        {
            string text1;
            string username = parameters["username"];
            string password = parameters["password"];
            string realm = parameters["realm"];
            string nonce = parameters["nonce"];
            string uri = parameters["uri"];
            string str6 = parameters["algorithm"];
            string str7 = parameters["qop"];
            string cnonce = parameters["cnonce"];
            string str9 = parameters["nc"];
            string method = parameters["method"];
            string str11 = ((str6 == null) || (str6.ToLower() != "md5-sess")) ? createA1(username, password, realm) : createA1(username, password, realm, nonce, cnonce);
            string str12 = ((str7 == null) || (str7.ToLower() != "auth-int")) ? createA2(method, uri) : createA2(method, uri, parameters["entity"]);
            string str13 = hash(str11);
            if (str7 == null)
            {
                text1 = $"{nonce}:{hash(str12)}";
            }
            else
            {
                text1 = $"{nonce}:{str9}:{cnonce}:{str7}:{hash(str12)}";
            }
            return hash($"{str13}:{text1}");
        }

        private static string hash(string value)
        {
            StringBuilder builder = new StringBuilder(0x40);
            foreach (byte num in MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(value)))
            {
                builder.Append(num.ToString("x2"));
            }
            return builder.ToString();
        }

        private void initAsDigest()
        {
            string str = base.Parameters["qop"];
            if (str != null)
            {
                char[] separator = new char[] { ',' };
                if (<>f__am$cache0 == null)
                {
                    <>f__am$cache0 = qop => qop.Trim().ToLower() == "auth";
                }
                if (!str.Split(separator).Contains<string>(<>f__am$cache0))
                {
                    base.Parameters["qop"] = null;
                }
                else
                {
                    uint num;
                    base.Parameters["qop"] = "auth";
                    base.Parameters["cnonce"] = CreateNonceValue();
                    this._nonceCount = num = this._nonceCount + 1;
                    base.Parameters["nc"] = $"{num:x8}";
                }
            }
            base.Parameters["method"] = "GET";
            base.Parameters["response"] = CreateRequestDigest(base.Parameters);
        }

        internal static AuthenticationResponse Parse(string value)
        {
            AuthenticationResponse response;
            try
            {
                char[] separator = new char[] { ' ' };
                string[] strArray = value.Split(separator, 2);
                if (strArray.Length != 2)
                {
                    response = null;
                }
                else
                {
                    string str = strArray[0].ToLower();
                    response = (str != "basic") ? ((str != "digest") ? null : new AuthenticationResponse(AuthenticationSchemes.Digest, ParseParameters(strArray[1]))) : new AuthenticationResponse(AuthenticationSchemes.Basic, ParseBasicCredentials(strArray[1]));
                }
            }
            catch
            {
                return null;
            }
            return response;
        }

        internal static NameValueCollection ParseBasicCredentials(string value)
        {
            string str = Encoding.Default.GetString(Convert.FromBase64String(value));
            int index = str.IndexOf(':');
            string str2 = str.Substring(0, index);
            string str3 = (index >= (str.Length - 1)) ? string.Empty : str.Substring(index + 1);
            index = str2.IndexOf('\\');
            if (index > -1)
            {
                str2 = str2.Substring(index + 1);
            }
            return new NameValueCollection { 
                ["username"] = str2,
                ["password"] = str3
            };
        }

        internal override string ToBasicString()
        {
            string s = $"{base.Parameters["username"]}:{base.Parameters["password"]}";
            string str2 = Convert.ToBase64String(Encoding.UTF8.GetBytes(s));
            return ("Basic " + str2);
        }

        internal override string ToDigestString()
        {
            StringBuilder builder = new StringBuilder(0x100);
            object[] args = new object[] { base.Parameters["username"], base.Parameters["realm"], base.Parameters["nonce"], base.Parameters["uri"], base.Parameters["response"] };
            builder.AppendFormat("Digest username=\"{0}\", realm=\"{1}\", nonce=\"{2}\", uri=\"{3}\", response=\"{4}\"", args);
            string str = base.Parameters["opaque"];
            if (str != null)
            {
                builder.AppendFormat(", opaque=\"{0}\"", str);
            }
            string str2 = base.Parameters["algorithm"];
            if (str2 != null)
            {
                builder.AppendFormat(", algorithm={0}", str2);
            }
            string str3 = base.Parameters["qop"];
            if (str3 != null)
            {
                builder.AppendFormat(", qop={0}, cnonce=\"{1}\", nc={2}", str3, base.Parameters["cnonce"], base.Parameters["nc"]);
            }
            return builder.ToString();
        }

        public IIdentity ToIdentity()
        {
            AuthenticationSchemes scheme = base.Scheme;
            return ((scheme != AuthenticationSchemes.Basic) ? ((scheme != AuthenticationSchemes.Digest) ? null : ((IIdentity) new HttpDigestIdentity(base.Parameters))) : ((IIdentity) new HttpBasicIdentity(base.Parameters["username"], base.Parameters["password"])));
        }

        internal uint NonceCount =>
            (this._nonceCount >= uint.MaxValue) ? 0 : this._nonceCount;

        public string Cnonce =>
            base.Parameters["cnonce"];

        public string Nc =>
            base.Parameters["nc"];

        public string Password =>
            base.Parameters["password"];

        public string Response =>
            base.Parameters["response"];

        public string Uri =>
            base.Parameters["uri"];

        public string UserName =>
            base.Parameters["username"];
    }
}

