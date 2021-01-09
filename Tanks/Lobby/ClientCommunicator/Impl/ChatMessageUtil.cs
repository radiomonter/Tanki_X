namespace Tanks.Lobby.ClientCommunicator.Impl
{
    using System;
    using System.Text;
    using System.Text.RegularExpressions;

    public static class ChatMessageUtil
    {
        public static string RemoveTags(string text, string[] tags)
        {
            StringBuilder builder = new StringBuilder();
            foreach (string str in tags)
            {
                if (builder.Length > 0)
                {
                    builder.Append("|");
                }
                builder.AppendFormat("(<{0}.*?>)|(</{0}.*?>)", str);
            }
            return new Regex(builder.ToString(), RegexOptions.IgnoreCase).Replace(text, string.Empty);
        }

        public static string RemoveWhiteSpaces(string text) => 
            text.Trim();
    }
}

