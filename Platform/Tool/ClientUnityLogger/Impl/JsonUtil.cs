namespace Platform.Tool.ClientUnityLogger.Impl
{
    using System;
    using System.Collections.Generic;

    public static class JsonUtil
    {
        public static string ToJSONString(string text)
        {
            List<string> list = new List<string>();
            foreach (char ch in text.ToCharArray())
            {
                if (ch == '\b')
                {
                    list.Add(@"\b");
                }
                else if (ch == '\t')
                {
                    list.Add(@"\t");
                }
                else if (ch == '\n')
                {
                    list.Add(@"\n");
                }
                else if (ch == '\f')
                {
                    list.Add(@"\f");
                }
                else if (ch == '\r')
                {
                    list.Add(@"\n");
                }
                else if (ch == '"')
                {
                    list.Add(@"\" + ch.ToString());
                }
                else if (ch == '/')
                {
                    list.Add(@"\" + ch.ToString());
                }
                else if (ch == '\\')
                {
                    list.Add(@"\" + ch.ToString());
                }
                else if (ch > '\x001f')
                {
                    list.Add(ch.ToString());
                }
            }
            return string.Join(string.Empty, list.ToArray());
        }
    }
}

