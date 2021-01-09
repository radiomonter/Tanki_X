namespace WebSocketSharp.Net
{
    using System;
    using System.Collections.Specialized;
    using System.Text;

    internal sealed class QueryStringCollection : NameValueCollection
    {
        public override string ToString()
        {
            if (this.Count == 0)
            {
                return string.Empty;
            }
            StringBuilder builder = new StringBuilder();
            foreach (string str in this.AllKeys)
            {
                builder.AppendFormat("{0}={1}&", str, base[str]);
            }
            if (builder.Length > 0)
            {
                builder.Length--;
            }
            return builder.ToString();
        }
    }
}

