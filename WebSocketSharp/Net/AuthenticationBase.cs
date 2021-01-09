namespace WebSocketSharp.Net
{
    using System;
    using System.Collections.Specialized;
    using System.Text;
    using WebSocketSharp;

    internal abstract class AuthenticationBase
    {
        private AuthenticationSchemes _scheme;
        internal NameValueCollection Parameters;

        protected AuthenticationBase(AuthenticationSchemes scheme, NameValueCollection parameters)
        {
            this._scheme = scheme;
            this.Parameters = parameters;
        }

        internal static string CreateNonceValue()
        {
            byte[] buffer = new byte[0x10];
            new Random().NextBytes(buffer);
            StringBuilder builder = new StringBuilder(0x20);
            foreach (byte num in buffer)
            {
                builder.Append(num.ToString("x2"));
            }
            return builder.ToString();
        }

        internal static NameValueCollection ParseParameters(string value)
        {
            NameValueCollection values = new NameValueCollection();
            char[] separators = new char[] { ',' };
            foreach (string str in value.SplitHeaderValue(separators))
            {
                string text1;
                int index = str.IndexOf('=');
                string name = (index <= 0) ? null : str.Substring(0, index).Trim();
                if (index < 0)
                {
                    char[] trimChars = new char[] { '"' };
                    text1 = str.Trim().Trim(trimChars);
                }
                else if (index >= (str.Length - 1))
                {
                    text1 = string.Empty;
                }
                else
                {
                    char[] trimChars = new char[] { '"' };
                    text1 = str.Substring(index + 1).Trim().Trim(trimChars);
                }
                values.Add(name, text1);
            }
            return values;
        }

        internal abstract string ToBasicString();
        internal abstract string ToDigestString();
        public override string ToString() => 
            (this._scheme != AuthenticationSchemes.Basic) ? ((this._scheme != AuthenticationSchemes.Digest) ? string.Empty : this.ToDigestString()) : this.ToBasicString();

        public string Algorithm =>
            this.Parameters["algorithm"];

        public string Nonce =>
            this.Parameters["nonce"];

        public string Opaque =>
            this.Parameters["opaque"];

        public string Qop =>
            this.Parameters["qop"];

        public string Realm =>
            this.Parameters["realm"];

        public AuthenticationSchemes Scheme =>
            this._scheme;
    }
}

