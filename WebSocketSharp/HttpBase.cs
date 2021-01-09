namespace WebSocketSharp
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;
    using WebSocketSharp.Net;

    internal abstract class HttpBase
    {
        private NameValueCollection _headers;
        private const int _headersMaxLength = 0x2000;
        private Version _version;
        internal byte[] EntityBodyData;
        protected const string CrLf = "\r\n";

        protected HttpBase(Version version, NameValueCollection headers)
        {
            this._version = version;
            this._headers = headers;
        }

        protected static T Read<T>(Stream stream, Func<string[], T> parser, int millisecondsTimeout) where T: HttpBase
        {
            <Read>c__AnonStorey1<T> storey = new <Read>c__AnonStorey1<T> {
                stream = stream,
                timeout = false
            };
            Timer timer = new Timer(new TimerCallback(storey.<>m__0), null, millisecondsTimeout, -1);
            T local = null;
            Exception innerException = null;
            try
            {
                local = parser(readHeaders(storey.stream, 0x2000));
                string length = local.Headers["Content-Length"];
                if ((length != null) && (length.Length > 0))
                {
                    local.EntityBodyData = readEntityBody(storey.stream, length);
                }
            }
            catch (Exception exception1)
            {
                innerException = exception1;
            }
            finally
            {
                timer.Change(-1, -1);
                timer.Dispose();
            }
            string message = !storey.timeout ? ((innerException == null) ? null : "An exception has occurred while reading an HTTP request/response.") : "A timeout has occurred while reading an HTTP request/response.";
            if (message != null)
            {
                throw new WebSocketException(message, innerException);
            }
            return local;
        }

        private static byte[] readEntityBody(Stream stream, string length)
        {
            long num;
            if (!long.TryParse(length, out num))
            {
                throw new ArgumentException("Cannot be parsed.", "length");
            }
            if (num < 0L)
            {
                throw new ArgumentOutOfRangeException("length", "Less than zero.");
            }
            return ((num <= 0x400L) ? ((num <= 0L) ? null : stream.ReadBytes(((int) num))) : stream.ReadBytes(num, 0x400));
        }

        private static string[] readHeaders(Stream stream, int maxLength)
        {
            <readHeaders>c__AnonStorey0 storey = new <readHeaders>c__AnonStorey0 {
                buff = new List<byte>(),
                cnt = 0
            };
            Action<int> action = new Action<int>(storey.<>m__0);
            bool flag = false;
            while (true)
            {
                if (storey.cnt < maxLength)
                {
                    if (!stream.ReadByte().EqualsWith('\r', action) || (!stream.ReadByte().EqualsWith('\n', action) || (!stream.ReadByte().EqualsWith('\r', action) || !stream.ReadByte().EqualsWith('\n', action))))
                    {
                        continue;
                    }
                    flag = true;
                }
                if (!flag)
                {
                    throw new WebSocketException("The length of header part is greater than the max length.");
                }
                string[] separator = new string[] { "\r\n" };
                return Encoding.UTF8.GetString(storey.buff.ToArray()).Replace("\r\n ", " ").Replace("\r\n\t", " ").Split(separator, StringSplitOptions.RemoveEmptyEntries);
            }
        }

        public byte[] ToByteArray() => 
            Encoding.UTF8.GetBytes(this.ToString());

        public string EntityBody
        {
            get
            {
                if ((this.EntityBodyData == null) || (this.EntityBodyData.LongLength == 0L))
                {
                    return string.Empty;
                }
                Encoding encoding = null;
                string contentType = this._headers["Content-Type"];
                if ((contentType != null) && (contentType.Length > 0))
                {
                    encoding = HttpUtility.GetEncoding(contentType);
                }
                return (encoding ?? Encoding.UTF8).GetString(this.EntityBodyData);
            }
        }

        public NameValueCollection Headers =>
            this._headers;

        public Version ProtocolVersion =>
            this._version;

        [CompilerGenerated]
        private sealed class <Read>c__AnonStorey1<T> where T: HttpBase
        {
            internal bool timeout;
            internal Stream stream;

            internal void <>m__0(object state)
            {
                this.timeout = true;
                this.stream.Close();
            }
        }

        [CompilerGenerated]
        private sealed class <readHeaders>c__AnonStorey0
        {
            internal List<byte> buff;
            internal int cnt;

            internal void <>m__0(int i)
            {
                if (i == -1)
                {
                    throw new EndOfStreamException("The header cannot be read from the data source.");
                }
                this.buff.Add((byte) i);
                this.cnt++;
            }
        }
    }
}

