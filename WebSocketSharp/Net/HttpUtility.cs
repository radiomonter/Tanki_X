namespace WebSocketSharp.Net
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.IO;
    using System.Security.Principal;
    using System.Text;
    using WebSocketSharp;

    internal sealed class HttpUtility
    {
        private static Dictionary<string, char> _entities;
        private static char[] _hexChars = "0123456789abcdef".ToCharArray();
        private static object _sync = new object();

        internal static Uri CreateRequestUrl(string requestUri, string host, bool websocketRequest, bool secure)
        {
            Uri uri2;
            if ((requestUri == null) || ((requestUri.Length == 0) || ((host == null) || (host.Length == 0))))
            {
                return null;
            }
            string str = null;
            string pathAndQuery = null;
            if (requestUri.StartsWith("/"))
            {
                pathAndQuery = requestUri;
            }
            else if (!requestUri.MaybeUri())
            {
                if (requestUri != "*")
                {
                    host = requestUri;
                }
            }
            else
            {
                Uri uri;
                if (!(Uri.TryCreate(requestUri, UriKind.Absolute, out uri) && ((!(str = uri.Scheme).StartsWith("http") || websocketRequest) ? (str.StartsWith("ws") && websocketRequest) : true)))
                {
                    return null;
                }
                host = uri.Authority;
                pathAndQuery = uri.PathAndQuery;
            }
            str ??= ((!websocketRequest ? "http" : "ws") + (!secure ? string.Empty : "s"));
            if (host.IndexOf(':') == -1)
            {
                host = $"{host}:{((str == "http") || (str == "ws")) ? ((int) 80) : 0x1bb}";
            }
            return (Uri.TryCreate($"{str}://{host}{pathAndQuery}", UriKind.Absolute, out uri2) ? uri2 : null);
        }

        internal static IPrincipal CreateUser(string response, AuthenticationSchemes scheme, string realm, string method, Func<IIdentity, NetworkCredential> credentialsFinder)
        {
            if ((response == null) || (response.Length == 0))
            {
                return null;
            }
            if (credentialsFinder == null)
            {
                return null;
            }
            if ((scheme != AuthenticationSchemes.Basic) && (scheme != AuthenticationSchemes.Digest))
            {
                return null;
            }
            if (scheme == AuthenticationSchemes.Digest)
            {
                if ((realm == null) || (realm.Length == 0))
                {
                    return null;
                }
                if ((method == null) || (method.Length == 0))
                {
                    return null;
                }
            }
            if (!response.StartsWith(scheme.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }
            AuthenticationResponse response2 = AuthenticationResponse.Parse(response);
            if (response2 == null)
            {
                return null;
            }
            IIdentity identity = response2.ToIdentity();
            if (identity == null)
            {
                return null;
            }
            NetworkCredential credential = null;
            try
            {
                credential = credentialsFinder(identity);
            }
            catch
            {
            }
            return ((credential != null) ? (((scheme != AuthenticationSchemes.Basic) || (((HttpBasicIdentity) identity).Password == credential.Password)) ? (((scheme != AuthenticationSchemes.Digest) || ((HttpDigestIdentity) identity).IsValid(credential.Password, realm, method, null)) ? new GenericPrincipal(identity, credential.Roles) : null) : null) : null);
        }

        private static int getChar(byte[] bytes, int offset, int length)
        {
            int num = 0;
            int num2 = length + offset;
            for (int i = offset; i < num2; i++)
            {
                int num4 = getInt(bytes[i]);
                if (num4 == -1)
                {
                    return -1;
                }
                num = (num << 4) + num4;
            }
            return num;
        }

        private static int getChar(string s, int offset, int length)
        {
            int num = 0;
            int num2 = length + offset;
            for (int i = offset; i < num2; i++)
            {
                char ch = s[i];
                if (ch > '\x007f')
                {
                    return -1;
                }
                int num4 = getInt((byte) ch);
                if (num4 == -1)
                {
                    return -1;
                }
                num = (num << 4) + num4;
            }
            return num;
        }

        private static char[] getChars(MemoryStream buffer, Encoding encoding) => 
            encoding.GetChars(buffer.GetBuffer(), 0, (int) buffer.Length);

        internal static Encoding GetEncoding(string contentType)
        {
            char[] separator = new char[] { ';' };
            foreach (string str in contentType.Split(separator))
            {
                string nameAndValue = str.Trim();
                if (nameAndValue.StartsWith("charset", StringComparison.OrdinalIgnoreCase))
                {
                    return Encoding.GetEncoding(nameAndValue.GetValue('=', true));
                }
            }
            return null;
        }

        private static Dictionary<string, char> getEntities()
        {
            lock (_sync)
            {
                if (_entities == null)
                {
                    initEntities();
                }
                return _entities;
            }
        }

        private static int getInt(byte b)
        {
            char ch = (char) b;
            return (((ch < '0') || (ch > '9')) ? (((ch < 'a') || (ch > 'f')) ? (((ch < 'A') || (ch > 'F')) ? -1 : ((ch - 'A') + 10)) : ((ch - 'a') + 10)) : (ch - '0'));
        }

        public static string HtmlAttributeEncode(string s)
        {
            if ((s != null) && (s.Length != 0))
            {
                char[] chars = new char[] { '&', '"', '<', '>' };
                if (s.Contains(chars))
                {
                    StringBuilder builder = new StringBuilder();
                    foreach (char ch in s)
                    {
                        builder.Append((ch != '&') ? ((ch != '"') ? ((ch != '<') ? ((ch != '>') ? ch.ToString() : "&gt;") : "&lt;") : "&quot;") : "&amp;");
                    }
                    return builder.ToString();
                }
            }
            return s;
        }

        public static void HtmlAttributeEncode(string s, TextWriter output)
        {
            if (output == null)
            {
                throw new ArgumentNullException("output");
            }
            output.Write(HtmlAttributeEncode(s));
        }

        public static string HtmlDecode(string s)
        {
            if ((s != null) && (s.Length != 0))
            {
                char[] chars = new char[] { '&' };
                if (s.Contains(chars))
                {
                    StringBuilder builder = new StringBuilder();
                    StringBuilder builder2 = new StringBuilder();
                    int num = 0;
                    int num2 = 0;
                    bool flag = false;
                    foreach (char ch in s)
                    {
                        if (num == 0)
                        {
                            if (ch != '&')
                            {
                                builder2.Append(ch);
                            }
                            else
                            {
                                builder.Append(ch);
                                num = 1;
                            }
                        }
                        else if (ch == '&')
                        {
                            num = 1;
                            if (flag)
                            {
                                builder.Append(num2.ToString(CultureInfo.InvariantCulture));
                                flag = false;
                            }
                            builder2.Append(builder.ToString());
                            builder.Length = 0;
                            builder.Append('&');
                        }
                        else if (num == 1)
                        {
                            if (ch != ';')
                            {
                                num2 = 0;
                                num = (ch == '#') ? 3 : 2;
                                builder.Append(ch);
                            }
                            else
                            {
                                num = 0;
                                builder2.Append(builder.ToString());
                                builder2.Append(ch);
                                builder.Length = 0;
                            }
                        }
                        else if (num == 2)
                        {
                            builder.Append(ch);
                            if (ch == ';')
                            {
                                string str2 = builder.ToString();
                                Dictionary<string, char> dictionary = getEntities();
                                if ((str2.Length > 1) && dictionary.ContainsKey(str2.Substring(1, str2.Length - 2)))
                                {
                                    str2 = dictionary[str2.Substring(1, str2.Length - 2)].ToString();
                                }
                                builder2.Append(str2);
                                num = 0;
                                builder.Length = 0;
                            }
                        }
                        else if (num == 3)
                        {
                            if (ch == ';')
                            {
                                if (num2 <= 0xffff)
                                {
                                    builder2.Append((char) num2);
                                }
                                else
                                {
                                    builder2.Append("&#");
                                    builder2.Append(num2.ToString(CultureInfo.InvariantCulture));
                                    builder2.Append(";");
                                }
                                num = 0;
                                builder.Length = 0;
                                flag = false;
                            }
                            else if (char.IsDigit(ch))
                            {
                                num2 = (num2 * 10) + (ch - '0');
                                flag = true;
                            }
                            else
                            {
                                num = 2;
                                if (flag)
                                {
                                    builder.Append(num2.ToString(CultureInfo.InvariantCulture));
                                    flag = false;
                                }
                                builder.Append(ch);
                            }
                        }
                    }
                    if (builder.Length > 0)
                    {
                        builder2.Append(builder.ToString());
                    }
                    else if (flag)
                    {
                        builder2.Append(num2.ToString(CultureInfo.InvariantCulture));
                    }
                    return builder2.ToString();
                }
            }
            return s;
        }

        public static void HtmlDecode(string s, TextWriter output)
        {
            if (output == null)
            {
                throw new ArgumentNullException("output");
            }
            output.Write(HtmlDecode(s));
        }

        public static string HtmlEncode(string s)
        {
            if ((s == null) || (s.Length == 0))
            {
                return s;
            }
            bool flag = false;
            string str = s;
            int num = 0;
            while (true)
            {
                if (num < str.Length)
                {
                    char ch = str[num];
                    if ((ch != '&') && ((ch != '"') && ((ch != '<') && ((ch != '>') && (ch <= '\x009f')))))
                    {
                        num++;
                        continue;
                    }
                    flag = true;
                }
                if (!flag)
                {
                    return s;
                }
                StringBuilder builder = new StringBuilder();
                foreach (char ch2 in s)
                {
                    if (ch2 == '&')
                    {
                        builder.Append("&amp;");
                    }
                    else if (ch2 == '"')
                    {
                        builder.Append("&quot;");
                    }
                    else if (ch2 == '<')
                    {
                        builder.Append("&lt;");
                    }
                    else if (ch2 == '>')
                    {
                        builder.Append("&gt;");
                    }
                    else if (ch2 <= '\x009f')
                    {
                        builder.Append(ch2);
                    }
                    else
                    {
                        builder.Append("&#");
                        builder.Append(((int) ch2).ToString(CultureInfo.InvariantCulture));
                        builder.Append(";");
                    }
                }
                return builder.ToString();
            }
        }

        public static void HtmlEncode(string s, TextWriter output)
        {
            if (output == null)
            {
                throw new ArgumentNullException("output");
            }
            output.Write(HtmlEncode(s));
        }

        private static void initEntities()
        {
            _entities = new Dictionary<string, char>();
            _entities.Add("nbsp", '\x00a0');
            _entities.Add("iexcl", '\x00a1');
            _entities.Add("cent", '\x00a2');
            _entities.Add("pound", '\x00a3');
            _entities.Add("curren", '\x00a4');
            _entities.Add("yen", '\x00a5');
            _entities.Add("brvbar", '\x00a6');
            _entities.Add("sect", '\x00a7');
            _entities.Add("uml", '\x00a8');
            _entities.Add("copy", '\x00a9');
            _entities.Add("ordf", '\x00aa');
            _entities.Add("laquo", '\x00ab');
            _entities.Add("not", '\x00ac');
            _entities.Add("shy", '\x00ad');
            _entities.Add("reg", '\x00ae');
            _entities.Add("macr", '\x00af');
            _entities.Add("deg", '\x00b0');
            _entities.Add("plusmn", '\x00b1');
            _entities.Add("sup2", '\x00b2');
            _entities.Add("sup3", '\x00b3');
            _entities.Add("acute", '\x00b4');
            _entities.Add("micro", '\x00b5');
            _entities.Add("para", '\x00b6');
            _entities.Add("middot", '\x00b7');
            _entities.Add("cedil", '\x00b8');
            _entities.Add("sup1", '\x00b9');
            _entities.Add("ordm", '\x00ba');
            _entities.Add("raquo", '\x00bb');
            _entities.Add("frac14", '\x00bc');
            _entities.Add("frac12", '\x00bd');
            _entities.Add("frac34", '\x00be');
            _entities.Add("iquest", '\x00bf');
            _entities.Add("Agrave", '\x00c0');
            _entities.Add("Aacute", '\x00c1');
            _entities.Add("Acirc", '\x00c2');
            _entities.Add("Atilde", '\x00c3');
            _entities.Add("Auml", '\x00c4');
            _entities.Add("Aring", '\x00c5');
            _entities.Add("AElig", '\x00c6');
            _entities.Add("Ccedil", '\x00c7');
            _entities.Add("Egrave", '\x00c8');
            _entities.Add("Eacute", '\x00c9');
            _entities.Add("Ecirc", '\x00ca');
            _entities.Add("Euml", '\x00cb');
            _entities.Add("Igrave", '\x00cc');
            _entities.Add("Iacute", '\x00cd');
            _entities.Add("Icirc", '\x00ce');
            _entities.Add("Iuml", '\x00cf');
            _entities.Add("ETH", '\x00d0');
            _entities.Add("Ntilde", '\x00d1');
            _entities.Add("Ograve", '\x00d2');
            _entities.Add("Oacute", '\x00d3');
            _entities.Add("Ocirc", '\x00d4');
            _entities.Add("Otilde", '\x00d5');
            _entities.Add("Ouml", '\x00d6');
            _entities.Add("times", '\x00d7');
            _entities.Add("Oslash", '\x00d8');
            _entities.Add("Ugrave", '\x00d9');
            _entities.Add("Uacute", '\x00da');
            _entities.Add("Ucirc", '\x00db');
            _entities.Add("Uuml", '\x00dc');
            _entities.Add("Yacute", '\x00dd');
            _entities.Add("THORN", '\x00de');
            _entities.Add("szlig", '\x00df');
            _entities.Add("agrave", '\x00e0');
            _entities.Add("aacute", '\x00e1');
            _entities.Add("acirc", '\x00e2');
            _entities.Add("atilde", '\x00e3');
            _entities.Add("auml", '\x00e4');
            _entities.Add("aring", '\x00e5');
            _entities.Add("aelig", '\x00e6');
            _entities.Add("ccedil", '\x00e7');
            _entities.Add("egrave", '\x00e8');
            _entities.Add("eacute", '\x00e9');
            _entities.Add("ecirc", '\x00ea');
            _entities.Add("euml", '\x00eb');
            _entities.Add("igrave", '\x00ec');
            _entities.Add("iacute", '\x00ed');
            _entities.Add("icirc", '\x00ee');
            _entities.Add("iuml", '\x00ef');
            _entities.Add("eth", '\x00f0');
            _entities.Add("ntilde", '\x00f1');
            _entities.Add("ograve", '\x00f2');
            _entities.Add("oacute", '\x00f3');
            _entities.Add("ocirc", '\x00f4');
            _entities.Add("otilde", '\x00f5');
            _entities.Add("ouml", '\x00f6');
            _entities.Add("divide", '\x00f7');
            _entities.Add("oslash", '\x00f8');
            _entities.Add("ugrave", '\x00f9');
            _entities.Add("uacute", '\x00fa');
            _entities.Add("ucirc", '\x00fb');
            _entities.Add("uuml", '\x00fc');
            _entities.Add("yacute", '\x00fd');
            _entities.Add("thorn", '\x00fe');
            _entities.Add("yuml", '\x00ff');
            _entities.Add("fnof", 'ƒ');
            _entities.Add("Alpha", 'Α');
            _entities.Add("Beta", 'Β');
            _entities.Add("Gamma", 'Γ');
            _entities.Add("Delta", 'Δ');
            _entities.Add("Epsilon", 'Ε');
            _entities.Add("Zeta", 'Ζ');
            _entities.Add("Eta", 'Η');
            _entities.Add("Theta", 'Θ');
            _entities.Add("Iota", 'Ι');
            _entities.Add("Kappa", 'Κ');
            _entities.Add("Lambda", 'Λ');
            _entities.Add("Mu", 'Μ');
            _entities.Add("Nu", 'Ν');
            _entities.Add("Xi", 'Ξ');
            _entities.Add("Omicron", 'Ο');
            _entities.Add("Pi", 'Π');
            _entities.Add("Rho", 'Ρ');
            _entities.Add("Sigma", 'Σ');
            _entities.Add("Tau", 'Τ');
            _entities.Add("Upsilon", 'Υ');
            _entities.Add("Phi", 'Φ');
            _entities.Add("Chi", 'Χ');
            _entities.Add("Psi", 'Ψ');
            _entities.Add("Omega", 'Ω');
            _entities.Add("alpha", 'α');
            _entities.Add("beta", 'β');
            _entities.Add("gamma", 'γ');
            _entities.Add("delta", 'δ');
            _entities.Add("epsilon", 'ε');
            _entities.Add("zeta", 'ζ');
            _entities.Add("eta", 'η');
            _entities.Add("theta", 'θ');
            _entities.Add("iota", 'ι');
            _entities.Add("kappa", 'κ');
            _entities.Add("lambda", 'λ');
            _entities.Add("mu", 'μ');
            _entities.Add("nu", 'ν');
            _entities.Add("xi", 'ξ');
            _entities.Add("omicron", 'ο');
            _entities.Add("pi", 'π');
            _entities.Add("rho", 'ρ');
            _entities.Add("sigmaf", 'ς');
            _entities.Add("sigma", 'σ');
            _entities.Add("tau", 'τ');
            _entities.Add("upsilon", 'υ');
            _entities.Add("phi", 'φ');
            _entities.Add("chi", 'χ');
            _entities.Add("psi", 'ψ');
            _entities.Add("omega", 'ω');
            _entities.Add("thetasym", 'ϑ');
            _entities.Add("upsih", 'ϒ');
            _entities.Add("piv", 'ϖ');
            _entities.Add("bull", '•');
            _entities.Add("hellip", '…');
            _entities.Add("prime", '′');
            _entities.Add("Prime", '″');
            _entities.Add("oline", '‾');
            _entities.Add("frasl", '⁄');
            _entities.Add("weierp", '℘');
            _entities.Add("image", 'ℑ');
            _entities.Add("real", 'ℜ');
            _entities.Add("trade", '™');
            _entities.Add("alefsym", 'ℵ');
            _entities.Add("larr", '←');
            _entities.Add("uarr", '↑');
            _entities.Add("rarr", '→');
            _entities.Add("darr", '↓');
            _entities.Add("harr", '↔');
            _entities.Add("crarr", '↵');
            _entities.Add("lArr", '⇐');
            _entities.Add("uArr", '⇑');
            _entities.Add("rArr", '⇒');
            _entities.Add("dArr", '⇓');
            _entities.Add("hArr", '⇔');
            _entities.Add("forall", '∀');
            _entities.Add("part", '∂');
            _entities.Add("exist", '∃');
            _entities.Add("empty", '∅');
            _entities.Add("nabla", '∇');
            _entities.Add("isin", '∈');
            _entities.Add("notin", '∉');
            _entities.Add("ni", '∋');
            _entities.Add("prod", '∏');
            _entities.Add("sum", '∑');
            _entities.Add("minus", '−');
            _entities.Add("lowast", '∗');
            _entities.Add("radic", '√');
            _entities.Add("prop", '∝');
            _entities.Add("infin", '∞');
            _entities.Add("ang", '∠');
            _entities.Add("and", '∧');
            _entities.Add("or", '∨');
            _entities.Add("cap", '∩');
            _entities.Add("cup", '∪');
            _entities.Add("int", '∫');
            _entities.Add("there4", '∴');
            _entities.Add("sim", '∼');
            _entities.Add("cong", '≅');
            _entities.Add("asymp", '≈');
            _entities.Add("ne", '≠');
            _entities.Add("equiv", '≡');
            _entities.Add("le", '≤');
            _entities.Add("ge", '≥');
            _entities.Add("sub", '⊂');
            _entities.Add("sup", '⊃');
            _entities.Add("nsub", '⊄');
            _entities.Add("sube", '⊆');
            _entities.Add("supe", '⊇');
            _entities.Add("oplus", '⊕');
            _entities.Add("otimes", '⊗');
            _entities.Add("perp", '⊥');
            _entities.Add("sdot", '⋅');
            _entities.Add("lceil", '⌈');
            _entities.Add("rceil", '⌉');
            _entities.Add("lfloor", '⌊');
            _entities.Add("rfloor", '⌋');
            _entities.Add("lang", '〈');
            _entities.Add("rang", '〉');
            _entities.Add("loz", '◊');
            _entities.Add("spades", '♠');
            _entities.Add("clubs", '♣');
            _entities.Add("hearts", '♥');
            _entities.Add("diams", '♦');
            _entities.Add("quot", '"');
            _entities.Add("amp", '&');
            _entities.Add("lt", '<');
            _entities.Add("gt", '>');
            _entities.Add("OElig", 'Œ');
            _entities.Add("oelig", 'œ');
            _entities.Add("Scaron", 'Š');
            _entities.Add("scaron", 'š');
            _entities.Add("Yuml", 'Ÿ');
            _entities.Add("circ", 'ˆ');
            _entities.Add("tilde", '˜');
            _entities.Add("ensp", ' ');
            _entities.Add("emsp", ' ');
            _entities.Add("thinsp", ' ');
            _entities.Add("zwnj", '‌');
            _entities.Add("zwj", '‍');
            _entities.Add("lrm", '‎');
            _entities.Add("rlm", '‏');
            _entities.Add("ndash", '–');
            _entities.Add("mdash", '—');
            _entities.Add("lsquo", '‘');
            _entities.Add("rsquo", '’');
            _entities.Add("sbquo", '‚');
            _entities.Add("ldquo", '“');
            _entities.Add("rdquo", '”');
            _entities.Add("bdquo", '„');
            _entities.Add("dagger", '†');
            _entities.Add("Dagger", '‡');
            _entities.Add("permil", '‰');
            _entities.Add("lsaquo", '‹');
            _entities.Add("rsaquo", '›');
            _entities.Add("euro", '€');
        }

        internal static NameValueCollection InternalParseQueryString(string query, Encoding encoding)
        {
            int num;
            if ((query == null) || (((num = query.Length) == 0) || ((num == 1) && (query[0] == '?'))))
            {
                return new NameValueCollection(1);
            }
            if (query[0] == '?')
            {
                query = query.Substring(1);
            }
            QueryStringCollection strings = new QueryStringCollection();
            char[] separator = new char[] { '&' };
            foreach (string str in query.Split(separator))
            {
                int index = str.IndexOf('=');
                if (index <= -1)
                {
                    strings.Add(null, UrlDecode(str, encoding));
                }
                else
                {
                    string name = UrlDecode(str.Substring(0, index), encoding);
                    strings.Add(name, (str.Length <= (index + 1)) ? string.Empty : UrlDecode(str.Substring(index + 1), encoding));
                }
            }
            return strings;
        }

        internal static string InternalUrlDecode(byte[] bytes, int offset, int count, Encoding encoding)
        {
            StringBuilder builder = new StringBuilder();
            using (MemoryStream stream = new MemoryStream())
            {
                int num = count + offset;
                int index = offset;
                while (true)
                {
                    while (true)
                    {
                        if (index < num)
                        {
                            if ((bytes[index] == 0x25) && (((index + 2) < count) && (bytes[index + 1] != 0x25)))
                            {
                                int num3;
                                if ((bytes[index + 1] != 0x75) || ((index + 5) >= num))
                                {
                                    num3 = getChar(bytes, index + 1, 2);
                                    if (num3 != -1)
                                    {
                                        stream.WriteByte((byte) num3);
                                        index += 2;
                                        break;
                                    }
                                }
                                else
                                {
                                    if (stream.Length > 0L)
                                    {
                                        builder.Append(getChars(stream, encoding));
                                        stream.SetLength(0L);
                                    }
                                    num3 = getChar(bytes, index + 2, 4);
                                    if (num3 != -1)
                                    {
                                        builder.Append((char) num3);
                                        index += 5;
                                        break;
                                    }
                                }
                            }
                            if (stream.Length > 0L)
                            {
                                builder.Append(getChars(stream, encoding));
                                stream.SetLength(0L);
                            }
                            if (bytes[index] == 0x2b)
                            {
                                builder.Append(' ');
                            }
                            else
                            {
                                builder.Append((char) bytes[index]);
                            }
                        }
                        else
                        {
                            if (stream.Length > 0L)
                            {
                                builder.Append(getChars(stream, encoding));
                            }
                            goto TR_0003;
                        }
                        break;
                    }
                    index++;
                }
            }
        TR_0003:
            return builder.ToString();
        }

        internal static byte[] InternalUrlDecodeToBytes(byte[] bytes, int offset, int count)
        {
            byte[] buffer;
            using (MemoryStream stream = new MemoryStream())
            {
                int num = offset + count;
                int index = offset;
                while (true)
                {
                    if (index >= num)
                    {
                        stream.Close();
                        buffer = stream.ToArray();
                        break;
                    }
                    char ch = (char) bytes[index];
                    if (ch == '+')
                    {
                        ch = ' ';
                    }
                    else if ((ch == '%') && (index < (num - 2)))
                    {
                        int num3 = getChar(bytes, index + 1, 2);
                        if (num3 != -1)
                        {
                            ch = (char) num3;
                            index += 2;
                        }
                    }
                    stream.WriteByte((byte) ch);
                    index++;
                }
            }
            return buffer;
        }

        internal static byte[] InternalUrlEncodeToBytes(byte[] bytes, int offset, int count)
        {
            byte[] buffer;
            using (MemoryStream stream = new MemoryStream())
            {
                int num = offset + count;
                int index = offset;
                while (true)
                {
                    if (index >= num)
                    {
                        stream.Close();
                        buffer = stream.ToArray();
                        break;
                    }
                    urlEncode((char) bytes[index], stream, false);
                    index++;
                }
            }
            return buffer;
        }

        internal static byte[] InternalUrlEncodeUnicodeToBytes(string s)
        {
            byte[] buffer;
            using (MemoryStream stream = new MemoryStream())
            {
                string str = s;
                int num = 0;
                while (true)
                {
                    if (num >= str.Length)
                    {
                        stream.Close();
                        buffer = stream.ToArray();
                        break;
                    }
                    char c = str[num];
                    urlEncode(c, stream, true);
                    num++;
                }
            }
            return buffer;
        }

        private static bool notEncoded(char c) => 
            ((c == '!') || ((c == '\'') || ((c == '(') || ((c == ')') || ((c == '*') || ((c == '-') || (c == '.'))))))) || (c == '_');

        public static NameValueCollection ParseQueryString(string query) => 
            ParseQueryString(query, Encoding.UTF8);

        public static NameValueCollection ParseQueryString(string query, Encoding encoding)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            Encoding encoding1 = encoding;
            if (encoding == null)
            {
                Encoding local1 = encoding;
                encoding1 = Encoding.UTF8;
            }
            return InternalParseQueryString(query, encoding1);
        }

        public static string UrlDecode(string s) => 
            UrlDecode(s, Encoding.UTF8);

        public static string UrlDecode(string s, Encoding encoding)
        {
            if ((s != null) && (s.Length != 0))
            {
                char[] chars = new char[] { '%', '+' };
                if (s.Contains(chars))
                {
                    encoding ??= Encoding.UTF8;
                    List<byte> buffer = new List<byte>();
                    int length = s.Length;
                    for (int i = 0; i < length; i++)
                    {
                        char c = s[i];
                        if ((c != '%') || (((i + 2) >= length) || (s[i + 1] == '%')))
                        {
                            if (c == '+')
                            {
                                writeCharBytes(' ', buffer, encoding);
                            }
                            else
                            {
                                writeCharBytes(c, buffer, encoding);
                            }
                        }
                        else
                        {
                            int num3;
                            if ((s[i + 1] != 'u') || ((i + 5) >= length))
                            {
                                num3 = getChar(s, i + 1, 2);
                                if (num3 == -1)
                                {
                                    writeCharBytes('%', buffer, encoding);
                                }
                                else
                                {
                                    writeCharBytes((char) num3, buffer, encoding);
                                    i += 2;
                                }
                            }
                            else
                            {
                                num3 = getChar(s, i + 2, 4);
                                if (num3 == -1)
                                {
                                    writeCharBytes('%', buffer, encoding);
                                }
                                else
                                {
                                    writeCharBytes((char) num3, buffer, encoding);
                                    i += 5;
                                }
                            }
                        }
                    }
                    return encoding.GetString(buffer.ToArray());
                }
            }
            return s;
        }

        public static string UrlDecode(byte[] bytes, Encoding encoding)
        {
            string text1;
            if (bytes == null)
            {
                text1 = null;
            }
            else
            {
                int length = bytes.Length;
                if (length == 0)
                {
                    text1 = string.Empty;
                }
                else
                {
                    Encoding encoding1 = encoding;
                    if (encoding == null)
                    {
                        Encoding local1 = encoding;
                        encoding1 = Encoding.UTF8;
                    }
                    text1 = InternalUrlDecode(bytes, 0, length, encoding1);
                }
            }
            return text1;
        }

        public static string UrlDecode(byte[] bytes, int offset, int count, Encoding encoding)
        {
            if (bytes == null)
            {
                return null;
            }
            int length = bytes.Length;
            if ((length == 0) || (count == 0))
            {
                return string.Empty;
            }
            if ((offset < 0) || (offset >= length))
            {
                throw new ArgumentOutOfRangeException("offset");
            }
            if ((count < 0) || (count > (length - offset)))
            {
                throw new ArgumentOutOfRangeException("count");
            }
            Encoding encoding1 = encoding;
            if (encoding == null)
            {
                Encoding local1 = encoding;
                encoding1 = Encoding.UTF8;
            }
            return InternalUrlDecode(bytes, offset, count, encoding1);
        }

        public static byte[] UrlDecodeToBytes(byte[] bytes)
        {
            int num;
            return (((bytes == null) || ((num = bytes.Length) <= 0)) ? bytes : InternalUrlDecodeToBytes(bytes, 0, num));
        }

        public static byte[] UrlDecodeToBytes(string s) => 
            UrlDecodeToBytes(s, Encoding.UTF8);

        public static byte[] UrlDecodeToBytes(string s, Encoding encoding)
        {
            if (s == null)
            {
                return null;
            }
            if (s.Length == 0)
            {
                return new byte[0];
            }
            byte[] bytes = (encoding ?? Encoding.UTF8).GetBytes(s);
            return InternalUrlDecodeToBytes(bytes, 0, bytes.Length);
        }

        public static byte[] UrlDecodeToBytes(byte[] bytes, int offset, int count)
        {
            int num;
            if ((bytes == null) || ((num = bytes.Length) == 0))
            {
                return bytes;
            }
            if (count == 0)
            {
                return new byte[0];
            }
            if ((offset < 0) || (offset >= num))
            {
                throw new ArgumentOutOfRangeException("offset");
            }
            if ((count < 0) || (count > (num - offset)))
            {
                throw new ArgumentOutOfRangeException("count");
            }
            return InternalUrlDecodeToBytes(bytes, offset, count);
        }

        private static void urlEncode(char c, Stream result, bool unicode)
        {
            if (c > '\x00ff')
            {
                result.WriteByte(0x25);
                result.WriteByte(0x75);
                int num = c;
                int index = num >> 12;
                result.WriteByte((byte) _hexChars[index]);
                index = (num >> 8) & 15;
                result.WriteByte((byte) _hexChars[index]);
                index = (num >> 4) & 15;
                result.WriteByte((byte) _hexChars[index]);
                index = num & 15;
                result.WriteByte((byte) _hexChars[index]);
            }
            else if ((c > ' ') && notEncoded(c))
            {
                result.WriteByte((byte) c);
            }
            else if (c == ' ')
            {
                result.WriteByte(0x2b);
            }
            else if ((((c >= '0') && ((c >= 'A') || (c <= '9'))) && ((c <= 'Z') || (c >= 'a'))) && (c <= 'z'))
            {
                result.WriteByte((byte) c);
            }
            else
            {
                if (!unicode || (c <= '\x007f'))
                {
                    result.WriteByte(0x25);
                }
                else
                {
                    result.WriteByte(0x25);
                    result.WriteByte(0x75);
                    result.WriteByte(0x30);
                    result.WriteByte(0x30);
                }
                int num3 = c;
                int index = num3 >> 4;
                result.WriteByte((byte) _hexChars[index]);
                index = num3 & 15;
                result.WriteByte((byte) _hexChars[index]);
            }
        }

        public static string UrlEncode(byte[] bytes)
        {
            int num;
            return ((bytes != null) ? (((num = bytes.Length) != 0) ? Encoding.ASCII.GetString(InternalUrlEncodeToBytes(bytes, 0, num)) : string.Empty) : null);
        }

        public static string UrlEncode(string s) => 
            UrlEncode(s, Encoding.UTF8);

        public static string UrlEncode(string s, Encoding encoding)
        {
            int num;
            if ((s == null) || ((num = s.Length) == 0))
            {
                return s;
            }
            bool flag = false;
            string str = s;
            int num2 = 0;
            while (true)
            {
                if (num2 < str.Length)
                {
                    char c = str[num2];
                    if (((((c >= '0') && ((c >= 'A') || (c <= '9'))) && ((c <= 'Z') || (c >= 'a'))) && (c <= 'z')) || notEncoded(c))
                    {
                        num2++;
                        continue;
                    }
                    flag = true;
                }
                if (!flag)
                {
                    return s;
                }
                encoding ??= Encoding.UTF8;
                byte[] bytes = new byte[encoding.GetMaxByteCount(num)];
                return Encoding.ASCII.GetString(InternalUrlEncodeToBytes(bytes, 0, encoding.GetBytes(s, 0, num, bytes, 0)));
            }
        }

        public static string UrlEncode(byte[] bytes, int offset, int count)
        {
            byte[] buffer = UrlEncodeToBytes(bytes, offset, count);
            return ((buffer?.Length != 0) ? Encoding.ASCII.GetString(buffer) : string.Empty);
        }

        public static byte[] UrlEncodeToBytes(byte[] bytes)
        {
            int num;
            return (((bytes == null) || ((num = bytes.Length) <= 0)) ? bytes : InternalUrlEncodeToBytes(bytes, 0, num));
        }

        public static byte[] UrlEncodeToBytes(string s) => 
            UrlEncodeToBytes(s, Encoding.UTF8);

        public static byte[] UrlEncodeToBytes(string s, Encoding encoding)
        {
            if (s == null)
            {
                return null;
            }
            if (s.Length == 0)
            {
                return new byte[0];
            }
            byte[] bytes = (encoding ?? Encoding.UTF8).GetBytes(s);
            return InternalUrlEncodeToBytes(bytes, 0, bytes.Length);
        }

        public static byte[] UrlEncodeToBytes(byte[] bytes, int offset, int count)
        {
            int num;
            if ((bytes == null) || ((num = bytes.Length) == 0))
            {
                return bytes;
            }
            if (count == 0)
            {
                return new byte[0];
            }
            if ((offset < 0) || (offset >= num))
            {
                throw new ArgumentOutOfRangeException("offset");
            }
            if ((count < 0) || (count > (num - offset)))
            {
                throw new ArgumentOutOfRangeException("count");
            }
            return InternalUrlEncodeToBytes(bytes, offset, count);
        }

        public static string UrlEncodeUnicode(string s) => 
            ((s == null) || (s.Length <= 0)) ? s : Encoding.ASCII.GetString(InternalUrlEncodeUnicodeToBytes(s));

        public static byte[] UrlEncodeUnicodeToBytes(string s) => 
            (s?.Length != 0) ? InternalUrlEncodeUnicodeToBytes(s) : new byte[0];

        private static void urlPathEncode(char c, Stream result)
        {
            if ((c >= '!') && (c <= '~'))
            {
                if (c != ' ')
                {
                    result.WriteByte((byte) c);
                }
                else
                {
                    result.WriteByte(0x25);
                    result.WriteByte(50);
                    result.WriteByte(0x30);
                }
            }
            else
            {
                foreach (byte num in Encoding.UTF8.GetBytes(c.ToString()))
                {
                    result.WriteByte(0x25);
                    int num3 = num;
                    int index = num3 >> 4;
                    result.WriteByte((byte) _hexChars[index]);
                    index = num3 & 15;
                    result.WriteByte((byte) _hexChars[index]);
                }
            }
        }

        public static string UrlPathEncode(string s)
        {
            string str2;
            if ((s == null) || (s.Length == 0))
            {
                return s;
            }
            using (MemoryStream stream = new MemoryStream())
            {
                string str = s;
                int num = 0;
                while (true)
                {
                    if (num >= str.Length)
                    {
                        stream.Close();
                        str2 = Encoding.ASCII.GetString(stream.ToArray());
                        break;
                    }
                    char c = str[num];
                    urlPathEncode(c, stream);
                    num++;
                }
            }
            return str2;
        }

        private static void writeCharBytes(char c, IList buffer, Encoding encoding)
        {
            if (c <= '\x00ff')
            {
                buffer.Add((byte) c);
            }
            else
            {
                char[] chars = new char[] { c };
                foreach (byte num in encoding.GetBytes(chars))
                {
                    buffer.Add(num);
                }
            }
        }
    }
}

