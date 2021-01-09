namespace WebSocketSharp.Net
{
    using System;
    using System.Net;
    using WebSocketSharp;

    internal sealed class HttpListenerPrefix
    {
        private IPAddress[] _addresses;
        private string _host;
        private HttpListener _listener;
        private string _original;
        private string _path;
        private ushort _port;
        private bool _secure;

        internal HttpListenerPrefix(string uriPrefix)
        {
            this._original = uriPrefix;
            this.parse(uriPrefix);
        }

        public static void CheckPrefix(string uriPrefix)
        {
            if (uriPrefix == null)
            {
                throw new ArgumentNullException("uriPrefix");
            }
            int length = uriPrefix.Length;
            if (length == 0)
            {
                throw new ArgumentException("An empty string.");
            }
            if (!uriPrefix.StartsWith("http://") && !uriPrefix.StartsWith("https://"))
            {
                throw new ArgumentException("The scheme isn't 'http' or 'https'.");
            }
            int startIndex = uriPrefix.IndexOf(':') + 3;
            if (startIndex >= length)
            {
                throw new ArgumentException("No host is specified.");
            }
            int num3 = uriPrefix.IndexOf(':', startIndex, length - startIndex);
            if (startIndex == num3)
            {
                throw new ArgumentException("No host is specified.");
            }
            if (num3 <= 0)
            {
                if (uriPrefix.IndexOf('/', startIndex, length - startIndex) == -1)
                {
                    throw new ArgumentException("No path is specified.");
                }
            }
            else
            {
                int num5;
                int num4 = uriPrefix.IndexOf('/', num3, length - num3);
                if (num4 == -1)
                {
                    throw new ArgumentException("No path is specified.");
                }
                if (!int.TryParse(uriPrefix.Substring(num3 + 1, (num4 - num3) - 1), out num5) || !num5.IsPortNumber())
                {
                    throw new ArgumentException("An invalid port is specified.");
                }
            }
            if (uriPrefix[length - 1] != '/')
            {
                throw new ArgumentException("Ends without '/'.");
            }
        }

        public override bool Equals(object obj)
        {
            HttpListenerPrefix prefix = obj as HttpListenerPrefix;
            return ((prefix != null) && (prefix._original == this._original));
        }

        public override int GetHashCode() => 
            this._original.GetHashCode();

        private void parse(string uriPrefix)
        {
            int num = !uriPrefix.StartsWith("https://") ? 80 : 0x1bb;
            if (num == 0x1bb)
            {
                this._secure = true;
            }
            int length = uriPrefix.Length;
            int startIndex = uriPrefix.IndexOf(':') + 3;
            int num4 = uriPrefix.IndexOf(':', startIndex, length - startIndex);
            int num5 = 0;
            if (num4 > 0)
            {
                num5 = uriPrefix.IndexOf('/', num4, length - num4);
                this._host = uriPrefix.Substring(startIndex, num4 - startIndex);
                this._port = (ushort) int.Parse(uriPrefix.Substring(num4 + 1, (num5 - num4) - 1));
            }
            else
            {
                num5 = uriPrefix.IndexOf('/', startIndex, length - startIndex);
                this._host = uriPrefix.Substring(startIndex, num5 - startIndex);
                this._port = (ushort) num;
            }
            this._path = uriPrefix.Substring(num5);
            int num6 = this._path.Length;
            if (num6 > 1)
            {
                this._path = this._path.Substring(0, num6 - 1);
            }
        }

        public override string ToString() => 
            this._original;

        public IPAddress[] Addresses
        {
            get => 
                this._addresses;
            set => 
                this._addresses = value;
        }

        public string Host =>
            this._host;

        public bool IsSecure =>
            this._secure;

        public HttpListener Listener
        {
            get => 
                this._listener;
            set => 
                this._listener = value;
        }

        public string Path =>
            this._path;

        public int Port =>
            this._port;
    }
}

