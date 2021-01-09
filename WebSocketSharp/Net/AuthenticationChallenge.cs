namespace WebSocketSharp.Net
{
    using System;
    using System.Collections.Specialized;
    using System.Text;

    internal class AuthenticationChallenge : AuthenticationBase
    {
        private AuthenticationChallenge(AuthenticationSchemes scheme, NameValueCollection parameters) : base(scheme, parameters)
        {
        }

        internal AuthenticationChallenge(AuthenticationSchemes scheme, string realm) : base(scheme, new NameValueCollection())
        {
            base.Parameters["realm"] = realm;
            if (scheme == AuthenticationSchemes.Digest)
            {
                base.Parameters["nonce"] = CreateNonceValue();
                base.Parameters["algorithm"] = "MD5";
                base.Parameters["qop"] = "auth";
            }
        }

        internal static AuthenticationChallenge CreateBasicChallenge(string realm) => 
            new AuthenticationChallenge(AuthenticationSchemes.Basic, realm);

        internal static AuthenticationChallenge CreateDigestChallenge(string realm) => 
            new AuthenticationChallenge(AuthenticationSchemes.Digest, realm);

        internal static AuthenticationChallenge Parse(string value)
        {
            char[] separator = new char[] { ' ' };
            string[] strArray = value.Split(separator, 2);
            if (strArray.Length != 2)
            {
                return null;
            }
            string str = strArray[0].ToLower();
            return ((str != "basic") ? ((str != "digest") ? null : new AuthenticationChallenge(AuthenticationSchemes.Digest, ParseParameters(strArray[1]))) : new AuthenticationChallenge(AuthenticationSchemes.Basic, ParseParameters(strArray[1])));
        }

        internal override string ToBasicString() => 
            $"Basic realm="{base.Parameters["realm"]}"";

        internal override string ToDigestString()
        {
            StringBuilder builder = new StringBuilder(0x80);
            string str = base.Parameters["domain"];
            if (str != null)
            {
                builder.AppendFormat("Digest realm=\"{0}\", domain=\"{1}\", nonce=\"{2}\"", base.Parameters["realm"], str, base.Parameters["nonce"]);
            }
            else
            {
                builder.AppendFormat("Digest realm=\"{0}\", nonce=\"{1}\"", base.Parameters["realm"], base.Parameters["nonce"]);
            }
            string str2 = base.Parameters["opaque"];
            if (str2 != null)
            {
                builder.AppendFormat(", opaque=\"{0}\"", str2);
            }
            string str3 = base.Parameters["stale"];
            if (str3 != null)
            {
                builder.AppendFormat(", stale={0}", str3);
            }
            string str4 = base.Parameters["algorithm"];
            if (str4 != null)
            {
                builder.AppendFormat(", algorithm={0}", str4);
            }
            string str5 = base.Parameters["qop"];
            if (str5 != null)
            {
                builder.AppendFormat(", qop=\"{0}\"", str5);
            }
            return builder.ToString();
        }

        public string Domain =>
            base.Parameters["domain"];

        public string Stale =>
            base.Parameters["stale"];
    }
}

