namespace log4net.Util
{
    using System;
    using System.Text.RegularExpressions;
    using System.Xml;

    public sealed class Transform
    {
        private const string CDATA_END = "]]>";
        private const string CDATA_UNESCAPABLE_TOKEN = "]]";
        private static Regex INVALIDCHARS = new Regex(@"[^\x09\x0A\x0D\x20-\uD7FF\uE000-\uFFFD]", RegexOptions.None);

        private Transform()
        {
        }

        private static int CountSubstrings(string text, string substring)
        {
            int num = 0;
            int startIndex = 0;
            int length = text.Length;
            int num4 = substring.Length;
            if (length == 0)
            {
                return 0;
            }
            if (num4 == 0)
            {
                return 0;
            }
            while (true)
            {
                if (startIndex < length)
                {
                    int index = text.IndexOf(substring, startIndex);
                    if (index != -1)
                    {
                        num++;
                        startIndex = index + num4;
                        continue;
                    }
                }
                return num;
            }
        }

        public static string MaskXmlInvalidCharacters(string textData, string mask) => 
            INVALIDCHARS.Replace(textData, mask);

        public static void WriteEscapedXmlString(XmlWriter writer, string textData, string invalidCharReplacement)
        {
            string text = MaskXmlInvalidCharacters(textData, invalidCharReplacement);
            if (((3 * (CountSubstrings(text, "<") + CountSubstrings(text, ">"))) + (4 * CountSubstrings(text, "&"))) <= (12 * (1 + CountSubstrings(text, "]]>"))))
            {
                writer.WriteString(text);
            }
            else
            {
                int index = text.IndexOf("]]>");
                if (index < 0)
                {
                    writer.WriteCData(text);
                }
                else
                {
                    int startIndex = 0;
                    while (true)
                    {
                        if (index > -1)
                        {
                            writer.WriteCData(text.Substring(startIndex, index - startIndex));
                            if (index != (text.Length - 3))
                            {
                                writer.WriteString("]]");
                                startIndex = index + 2;
                                index = text.IndexOf("]]>", startIndex);
                                continue;
                            }
                            startIndex = text.Length;
                            writer.WriteString("]]>");
                        }
                        if (startIndex < text.Length)
                        {
                            writer.WriteCData(text.Substring(startIndex));
                        }
                        break;
                    }
                }
            }
        }
    }
}

