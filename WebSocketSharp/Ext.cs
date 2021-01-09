namespace WebSocketSharp
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.IO;
    using System.IO.Compression;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;
    using WebSocketSharp.Net;
    using WebSocketSharp.Net.WebSockets;
    using WebSocketSharp.Server;

    public static class Ext
    {
        private static readonly byte[] _last = new byte[1];
        private const string _tspecials = "()<>@,;:\\\"/[]?={} \t";
        [CompilerGenerated]
        private static Func<string, bool> <>f__am$cache0;

        internal static byte[] Append(this ushort code, string reason)
        {
            byte[] collection = code.InternalToByteArray(ByteOrder.Big);
            if ((reason != null) && (reason.Length > 0))
            {
                List<byte> list = new List<byte>(collection);
                list.AddRange(Encoding.UTF8.GetBytes(reason));
                collection = list.ToArray();
            }
            return collection;
        }

        internal static string CheckIfAvailable(this ServerState state, bool ready, bool start, bool shutting)
        {
            object obj1;
            if (((!ready && ((state == ServerState.Ready) || (state == ServerState.Stop))) || (!start && (state == ServerState.Start))) || (!shutting && (state == ServerState.ShuttingDown)))
            {
                obj1 = "This operation isn't available in: " + state.ToString().ToLower();
            }
            else
            {
                obj1 = null;
            }
            return (string) obj1;
        }

        internal static string CheckIfAvailable(this WebSocketState state, bool connecting, bool open, bool closing, bool closed)
        {
            object obj1;
            if (((!connecting && (state == WebSocketState.Connecting)) || ((!open && (state == WebSocketState.Open)) || (!closing && (state == WebSocketState.Closing)))) || (!closed && (state == WebSocketState.Closed)))
            {
                obj1 = "This operation isn't available in: " + state.ToString().ToLower();
            }
            else
            {
                obj1 = null;
            }
            return (string) obj1;
        }

        internal static string CheckIfValidProtocols(this string[] protocols)
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = protocol => ((protocol == null) || (protocol.Length == 0)) || !protocol.IsToken();
            }
            return (!protocols.Contains<string>(<>f__am$cache0) ? (!protocols.ContainsTwice() ? null : "Contains a value twice.") : "Contains an invalid value.");
        }

        internal static string CheckIfValidServicePath(this string path)
        {
            string text1;
            if ((path == null) || (path.Length == 0))
            {
                text1 = "'path' is null or empty.";
            }
            else if (path[0] != '/')
            {
                text1 = "'path' isn't an absolute path.";
            }
            else
            {
                char[] anyOf = new char[] { '?', '#' };
                text1 = (path.IndexOfAny(anyOf) <= -1) ? null : "'path' includes either or both query and fragment components.";
            }
            return text1;
        }

        internal static string CheckIfValidSessionID(this string id) => 
            ((id == null) || (id.Length == 0)) ? "'id' is null or empty." : null;

        internal static string CheckIfValidWaitTime(this TimeSpan time) => 
            (time > TimeSpan.Zero) ? null : "A wait time is zero or less.";

        internal static bool CheckWaitTime(this TimeSpan time, out string message)
        {
            message = null;
            if (time > TimeSpan.Zero)
            {
                return true;
            }
            message = "A wait time is zero or less.";
            return false;
        }

        internal static void Close(this HttpListenerResponse response, HttpStatusCode code)
        {
            response.StatusCode = (int) code;
            response.OutputStream.Close();
        }

        internal static void CloseWithAuthChallenge(this HttpListenerResponse response, string challenge)
        {
            response.Headers.InternalSet("WWW-Authenticate", challenge, true);
            response.Close(HttpStatusCode.Unauthorized);
        }

        private static byte[] compress(this byte[] data)
        {
            if (data.LongLength == 0L)
            {
                return data;
            }
            using (MemoryStream stream = new MemoryStream(data))
            {
                return stream.compressToArray();
            }
        }

        private static MemoryStream compress(this Stream stream)
        {
            MemoryStream compressedStream = new MemoryStream();
            if (stream.Length == 0L)
            {
                return compressedStream;
            }
            stream.Position = 0L;
            using (DeflateStream stream3 = new DeflateStream(compressedStream, CompressionMode.Compress, true))
            {
                stream.CopyTo(stream3, 0x400);
                stream3.Close();
                compressedStream.Write(_last, 0, 1);
                compressedStream.Position = 0L;
                return compressedStream;
            }
        }

        internal static byte[] Compress(this byte[] data, CompressionMethod method) => 
            (method != CompressionMethod.Deflate) ? data : data.compress();

        internal static Stream Compress(this Stream stream, CompressionMethod method) => 
            (method != CompressionMethod.Deflate) ? stream : stream.compress();

        private static byte[] compressToArray(this Stream stream)
        {
            using (MemoryStream stream2 = stream.compress())
            {
                stream2.Close();
                return stream2.ToArray();
            }
        }

        internal static byte[] CompressToArray(this Stream stream, CompressionMethod method) => 
            (method != CompressionMethod.Deflate) ? stream.ToByteArray() : stream.compressToArray();

        internal static bool Contains<T>(this IEnumerable<T> source, Func<T, bool> condition)
        {
            bool flag;
            using (IEnumerator<T> enumerator = source.GetEnumerator())
            {
                while (true)
                {
                    if (enumerator.MoveNext())
                    {
                        T current = enumerator.Current;
                        if (!condition(current))
                        {
                            continue;
                        }
                        flag = true;
                    }
                    else
                    {
                        return false;
                    }
                    break;
                }
            }
            return flag;
        }

        public static bool Contains(this NameValueCollection collection, string name) => 
            ((collection != null) && (collection.Count > 0)) && !ReferenceEquals(collection[name], null);

        public static bool Contains(this string value, params char[] chars) => 
            ((chars == null) || (chars.Length == 0)) || (((value != null) && (value.Length != 0)) && (value.IndexOfAny(chars) > -1));

        public static bool Contains(this NameValueCollection collection, string name, string value)
        {
            if ((collection != null) && (collection.Count != 0))
            {
                string str = collection[name];
                if (str == null)
                {
                    return false;
                }
                char[] separator = new char[] { ',' };
                foreach (string str2 in str.Split(separator))
                {
                    if (str2.Trim().Equals(value, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        internal static bool ContainsTwice(this string[] values)
        {
            <ContainsTwice>c__AnonStorey1 storey = new <ContainsTwice>c__AnonStorey1 {
                values = values
            };
            storey.len = storey.values.Length;
            storey.contains = null;
            storey.contains = new Func<int, bool>(storey.<>m__0);
            return storey.contains(0);
        }

        internal static T[] Copy<T>(this T[] source, long length)
        {
            T[] destinationArray = new T[length];
            Array.Copy(source, 0L, destinationArray, 0L, length);
            return destinationArray;
        }

        internal static void CopyTo(this Stream source, Stream destination, int bufferLength)
        {
            byte[] buffer = new byte[bufferLength];
            int count = 0;
            while ((count = source.Read(buffer, 0, bufferLength)) > 0)
            {
                destination.Write(buffer, 0, count);
            }
        }

        internal static void CopyToAsync(this Stream source, Stream destination, int bufferLength, Action completed, Action<Exception> error)
        {
            <CopyToAsync>c__AnonStorey2 storey = new <CopyToAsync>c__AnonStorey2 {
                source = source,
                completed = completed,
                destination = destination,
                bufferLength = bufferLength,
                error = error
            };
            storey.buff = new byte[storey.bufferLength];
            storey.callback = null;
            storey.callback = new AsyncCallback(storey.<>m__0);
            try
            {
                storey.source.BeginRead(storey.buff, 0, storey.bufferLength, storey.callback, null);
            }
            catch (Exception exception)
            {
                if (storey.error != null)
                {
                    storey.error(exception);
                }
            }
        }

        private static byte[] decompress(this byte[] data)
        {
            if (data.LongLength == 0L)
            {
                return data;
            }
            using (MemoryStream stream = new MemoryStream(data))
            {
                return stream.decompressToArray();
            }
        }

        private static MemoryStream decompress(this Stream stream)
        {
            MemoryStream destination = new MemoryStream();
            if (stream.Length == 0L)
            {
                return destination;
            }
            stream.Position = 0L;
            using (DeflateStream stream3 = new DeflateStream(stream, CompressionMode.Decompress, true))
            {
                stream3.CopyTo(destination, 0x400);
                destination.Position = 0L;
                return destination;
            }
        }

        internal static byte[] Decompress(this byte[] data, CompressionMethod method) => 
            (method != CompressionMethod.Deflate) ? data : data.decompress();

        internal static Stream Decompress(this Stream stream, CompressionMethod method) => 
            (method != CompressionMethod.Deflate) ? stream : stream.decompress();

        private static byte[] decompressToArray(this Stream stream)
        {
            using (MemoryStream stream2 = stream.decompress())
            {
                stream2.Close();
                return stream2.ToArray();
            }
        }

        internal static byte[] DecompressToArray(this Stream stream, CompressionMethod method) => 
            (method != CompressionMethod.Deflate) ? stream.ToByteArray() : stream.decompressToArray();

        public static void Emit(this EventHandler eventHandler, object sender, EventArgs e)
        {
            if (eventHandler != null)
            {
                eventHandler(sender, e);
            }
        }

        public static void Emit<TEventArgs>(this EventHandler<TEventArgs> eventHandler, object sender, TEventArgs e) where TEventArgs: EventArgs
        {
            if (eventHandler != null)
            {
                eventHandler(sender, e);
            }
        }

        internal static bool EqualsWith(this int value, char c, Action<int> action)
        {
            action(value);
            return (value == c);
        }

        internal static string GetAbsolutePath(this Uri uri)
        {
            if (uri.IsAbsoluteUri)
            {
                return uri.AbsolutePath;
            }
            string originalString = uri.OriginalString;
            if (originalString[0] != '/')
            {
                return null;
            }
            char[] anyOf = new char[] { '?', '#' };
            int length = originalString.IndexOfAny(anyOf);
            return ((length <= 0) ? originalString : originalString.Substring(0, length));
        }

        public static CookieCollection GetCookies(this NameValueCollection headers, bool response)
        {
            string name = !response ? "Cookie" : "Set-Cookie";
            return (((headers == null) || !headers.Contains(name)) ? new CookieCollection() : CookieCollection.Parse(headers[name], response));
        }

        public static string GetDescription(this HttpStatusCode code) => 
            ((int) code).GetStatusDescription();

        internal static string GetMessage(this CloseStatusCode code) => 
            (code != CloseStatusCode.ProtocolError) ? ((code != CloseStatusCode.UnsupportedData) ? ((code != CloseStatusCode.Abnormal) ? ((code != CloseStatusCode.InvalidData) ? ((code != CloseStatusCode.PolicyViolation) ? ((code != CloseStatusCode.TooBig) ? ((code != CloseStatusCode.MandatoryExtension) ? ((code != CloseStatusCode.ServerError) ? ((code != CloseStatusCode.TlsHandshakeFailure) ? string.Empty : "An error has occurred during a TLS handshake.") : "WebSocket server got an internal error.") : "WebSocket client didn't receive expected extension(s).") : "A too big message has been received.") : "A policy violation has occurred.") : "Invalid data has been received.") : "An exception has occurred.") : "Unsupported data has been received.") : "A WebSocket protocol error has occurred.";

        internal static string GetName(this string nameAndValue, char separator)
        {
            int index = nameAndValue.IndexOf(separator);
            return ((index <= 0) ? null : nameAndValue.Substring(0, index).Trim());
        }

        public static string GetStatusDescription(this int code)
        {
            switch (code)
            {
                case 400:
                    return "Bad Request";

                case 0x191:
                    return "Unauthorized";

                case 0x192:
                    return "Payment Required";

                case 0x193:
                    return "Forbidden";

                case 0x194:
                    return "Not Found";

                case 0x195:
                    return "Method Not Allowed";

                case 0x196:
                    return "Not Acceptable";

                case 0x197:
                    return "Proxy Authentication Required";

                case 0x198:
                    return "Request Timeout";

                case 0x199:
                    return "Conflict";

                case 410:
                    return "Gone";

                case 0x19b:
                    return "Length Required";

                case 0x19c:
                    return "Precondition Failed";

                case 0x19d:
                    return "Request Entity Too Large";

                case 0x19e:
                    return "Request-Uri Too Long";

                case 0x19f:
                    return "Unsupported Media Type";

                case 0x1a0:
                    return "Requested Range Not Satisfiable";

                case 0x1a1:
                    return "Expectation Failed";

                case 0x1a6:
                    return "Unprocessable Entity";

                case 0x1a7:
                    return "Locked";

                case 0x1a8:
                    return "Failed Dependency";
            }
            switch (code)
            {
                case 200:
                    return "OK";

                case 0xc9:
                    return "Created";

                case 0xca:
                    return "Accepted";

                case 0xcb:
                    return "Non-Authoritative Information";

                case 0xcc:
                    return "No Content";

                case 0xcd:
                    return "Reset Content";

                case 0xce:
                    return "Partial Content";

                case 0xcf:
                    return "Multi-Status";
            }
            switch (code)
            {
                case 300:
                    return "Multiple Choices";

                case 0x12d:
                    return "Moved Permanently";

                case 0x12e:
                    return "Found";

                case 0x12f:
                    return "See Other";

                case 0x130:
                    return "Not Modified";

                case 0x131:
                    return "Use Proxy";

                case 0x133:
                    return "Temporary Redirect";
            }
            switch (code)
            {
                case 500:
                    return "Internal Server Error";

                case 0x1f5:
                    return "Not Implemented";

                case 0x1f6:
                    return "Bad Gateway";

                case 0x1f7:
                    return "Service Unavailable";

                case 0x1f8:
                    return "Gateway Timeout";

                case 0x1f9:
                    return "Http Version Not Supported";

                case 0x1fb:
                    return "Insufficient Storage";
            }
            switch (code)
            {
                case 100:
                    return "Continue";

                case 0x65:
                    return "Switching Protocols";

                case 0x66:
                    return "Processing";
            }
            return string.Empty;
        }

        internal static string GetValue(this string nameAndValue, char separator)
        {
            int index = nameAndValue.IndexOf(separator);
            return (((index <= -1) || (index >= (nameAndValue.Length - 1))) ? null : nameAndValue.Substring(index + 1).Trim());
        }

        internal static string GetValue(this string nameAndValue, char separator, bool unquote)
        {
            int index = nameAndValue.IndexOf(separator);
            if ((index < 0) || (index == (nameAndValue.Length - 1)))
            {
                return null;
            }
            string str = nameAndValue.Substring(index + 1).Trim();
            return (!unquote ? str : str.Unquote());
        }

        internal static TcpListenerWebSocketContext GetWebSocketContext(this TcpClient tcpClient, string protocol, bool secure, ServerSslConfiguration sslConfig, Logger logger) => 
            new TcpListenerWebSocketContext(tcpClient, protocol, secure, sslConfig, logger);

        internal static byte[] InternalToByteArray(this ushort value, ByteOrder order)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (!order.IsHostOrder())
            {
                Array.Reverse(bytes);
            }
            return bytes;
        }

        internal static byte[] InternalToByteArray(this ulong value, ByteOrder order)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (!order.IsHostOrder())
            {
                Array.Reverse(bytes);
            }
            return bytes;
        }

        public static bool IsCloseStatusCode(this ushort value) => 
            (value > 0x3e7) && (value < 0x1388);

        internal static bool IsCompressionExtension(this string value, CompressionMethod method) => 
            value.StartsWith(method.ToExtensionString(new string[0]));

        internal static bool IsControl(this byte opcode) => 
            (opcode > 7) && (opcode < 0x10);

        internal static bool IsControl(this Opcode opcode) => 
            opcode >= Opcode.Close;

        internal static bool IsData(this byte opcode) => 
            (opcode == 1) || (opcode == 2);

        internal static bool IsData(this Opcode opcode) => 
            (opcode == Opcode.Text) || (opcode == Opcode.Binary);

        public static bool IsEnclosedIn(this string value, char c) => 
            ((value != null) && ((value.Length > 1) && (value[0] == c))) && (value[value.Length - 1] == c);

        public static bool IsHostOrder(this ByteOrder order) => 
            !(BitConverter.IsLittleEndian ^ (order == ByteOrder.Little));

        public static bool IsLocal(this IPAddress address)
        {
            if (address != null)
            {
                if (address.Equals(IPAddress.Any))
                {
                    return true;
                }
                if (address.Equals(IPAddress.Loopback))
                {
                    return true;
                }
                if (Socket.OSSupportsIPv6)
                {
                    if (address.Equals(IPAddress.IPv6Any))
                    {
                        return true;
                    }
                    if (address.Equals(IPAddress.IPv6Loopback))
                    {
                        return true;
                    }
                }
                foreach (IPAddress address2 in Dns.GetHostAddresses(Dns.GetHostName()))
                {
                    if (address.Equals(address2))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool IsNullOrEmpty(this string value) => 
            (value == null) || (value.Length == 0);

        internal static bool IsPortNumber(this int value) => 
            (value > 0) && (value < 0x10000);

        public static bool IsPredefinedScheme(this string value)
        {
            if ((value == null) || (value.Length < 2))
            {
                return false;
            }
            char ch = value[0];
            return ((ch != 'h') ? ((ch != 'w') ? ((ch != 'f') ? ((ch != 'n') ? (((ch != 'g') || (value != "gopher")) ? ((ch == 'm') && (value == "mailto")) : true) : ((value[1] != 'e') ? (value == "nntp") : (((value == "news") || (value == "net.pipe")) || (value == "net.tcp")))) : ((value == "file") || (value == "ftp"))) : ((value == "ws") || (value == "wss"))) : ((value == "http") || (value == "https")));
        }

        internal static bool IsReserved(this ushort code) => 
            ((code == 0x3ec) || ((code == 0x3ed) || (code == 0x3ee))) || (code == 0x3f7);

        internal static bool IsReserved(this CloseStatusCode code) => 
            ((code == CloseStatusCode.Undefined) || ((code == CloseStatusCode.NoStatus) || (code == CloseStatusCode.Abnormal))) || (code == CloseStatusCode.TlsHandshakeFailure);

        internal static bool IsSupported(this byte opcode) => 
            Enum.IsDefined(typeof(Opcode), opcode);

        internal static bool IsText(this string value)
        {
            int length = value.Length;
            for (int i = 0; i < length; i++)
            {
                char ch = value[i];
                if (ch < ' ')
                {
                    char[] chars = new char[] { ch };
                    if (!"\r\n\t".Contains(chars))
                    {
                        return false;
                    }
                }
                if (ch == '\x007f')
                {
                    return false;
                }
                if ((ch == '\n') && (++i < length))
                {
                    ch = value[i];
                    char[] chars = new char[] { ch };
                    if (!" \t".Contains(chars))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        internal static bool IsToken(this string value)
        {
            string str = value;
            int num = 0;
            while (num < str.Length)
            {
                char ch = str[num];
                if ((ch >= ' ') && (ch < '\x007f'))
                {
                    char[] chars = new char[] { ch };
                    if (!"()<>@,;:\\\"/[]?={} \t".Contains(chars))
                    {
                        num++;
                        continue;
                    }
                }
                return false;
            }
            return true;
        }

        public static bool IsUpgradeTo(this HttpListenerRequest request, string protocol)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            if (protocol == null)
            {
                throw new ArgumentNullException("protocol");
            }
            if (protocol.Length == 0)
            {
                throw new ArgumentException("An empty string.", "protocol");
            }
            return (request.Headers.Contains("Upgrade", protocol) && request.Headers.Contains("Connection", "Upgrade"));
        }

        public static bool MaybeUri(this string value)
        {
            if ((value == null) || (value.Length == 0))
            {
                return false;
            }
            int index = value.IndexOf(':');
            return ((index != -1) ? ((index < 10) ? value.Substring(0, index).IsPredefinedScheme() : false) : false);
        }

        internal static string Quote(this string value) => 
            $""{value.Replace("\"", "\\\"")}"";

        internal static byte[] ReadBytes(this Stream stream, int length)
        {
            byte[] buffer = new byte[length];
            int offset = 0;
            try
            {
                int num2 = 0;
                while (length > 0)
                {
                    num2 = stream.Read(buffer, offset, length);
                    if (num2 == 0)
                    {
                        break;
                    }
                    offset += num2;
                    length -= num2;
                }
            }
            catch
            {
            }
            return buffer.SubArray<byte>(0, offset);
        }

        internal static byte[] ReadBytes(this Stream stream, long length, int bufferLength)
        {
            using (MemoryStream stream2 = new MemoryStream())
            {
                try
                {
                    byte[] buffer = new byte[bufferLength];
                    int count = 0;
                    while (length > 0L)
                    {
                        if (length < bufferLength)
                        {
                            bufferLength = (int) length;
                        }
                        count = stream.Read(buffer, 0, bufferLength);
                        if (count == 0)
                        {
                            break;
                        }
                        stream2.Write(buffer, 0, count);
                        length -= count;
                    }
                }
                catch
                {
                }
                stream2.Close();
                return stream2.ToArray();
            }
        }

        internal static void ReadBytesAsync(this Stream stream, int length, Action<byte[]> completed, Action<Exception> error)
        {
            <ReadBytesAsync>c__AnonStorey3 storey = new <ReadBytesAsync>c__AnonStorey3 {
                stream = stream,
                length = length,
                completed = completed,
                error = error
            };
            storey.buff = new byte[storey.length];
            storey.offset = 0;
            storey.callback = null;
            storey.callback = new AsyncCallback(storey.<>m__0);
            try
            {
                storey.stream.BeginRead(storey.buff, storey.offset, storey.length, storey.callback, null);
            }
            catch (Exception exception)
            {
                if (storey.error != null)
                {
                    storey.error(exception);
                }
            }
        }

        internal static void ReadBytesAsync(this Stream stream, long length, int bufferLength, Action<byte[]> completed, Action<Exception> error)
        {
            <ReadBytesAsync>c__AnonStorey4 storey = new <ReadBytesAsync>c__AnonStorey4 {
                bufferLength = bufferLength,
                stream = stream,
                completed = completed,
                error = error,
                dest = new MemoryStream()
            };
            storey.buff = new byte[storey.bufferLength];
            storey.read = null;
            storey.read = new Action<long>(storey.<>m__0);
            try
            {
                storey.read(length);
            }
            catch (Exception exception)
            {
                storey.dest.Dispose();
                if (storey.error != null)
                {
                    storey.error(exception);
                }
            }
        }

        internal static string RemovePrefix(this string value, params string[] prefixes)
        {
            int startIndex = 0;
            string[] strArray = prefixes;
            int index = 0;
            while (true)
            {
                if (index < strArray.Length)
                {
                    string str = strArray[index];
                    if (!value.StartsWith(str))
                    {
                        index++;
                        continue;
                    }
                    startIndex = str.Length;
                }
                return ((startIndex <= 0) ? value : value.Substring(startIndex));
            }
        }

        internal static T[] Reverse<T>(this T[] array)
        {
            int length = array.Length;
            T[] localArray = new T[length];
            int num2 = length - 1;
            for (int i = 0; i <= num2; i++)
            {
                localArray[i] = array[num2 - i];
            }
            return localArray;
        }

        [DebuggerHidden]
        internal static IEnumerable<string> SplitHeaderValue(this string value, params char[] separators) => 
            new <SplitHeaderValue>c__Iterator0 { 
                value = value,
                separators = separators,
                $PC = -2
            };

        public static T[] SubArray<T>(this T[] array, int startIndex, int length)
        {
            int num;
            if ((array == null) || ((num = array.Length) == 0))
            {
                return new T[0];
            }
            if ((startIndex < 0) || ((length <= 0) || ((startIndex + length) > num)))
            {
                return new T[0];
            }
            if ((startIndex == 0) && (length == num))
            {
                return array;
            }
            T[] destinationArray = new T[length];
            Array.Copy(array, startIndex, destinationArray, 0, length);
            return destinationArray;
        }

        public static T[] SubArray<T>(this T[] array, long startIndex, long length)
        {
            long num;
            if ((array == null) || ((num = array.LongLength) == 0L))
            {
                return new T[0];
            }
            if ((startIndex < 0L) || ((length <= 0L) || ((startIndex + length) > num)))
            {
                return new T[0];
            }
            if ((startIndex == 0L) && (length == num))
            {
                return array;
            }
            T[] destinationArray = new T[length];
            Array.Copy(array, startIndex, destinationArray, 0L, length);
            return destinationArray;
        }

        private static void times(this ulong n, Action action)
        {
            for (ulong i = 0UL; i < n; i += (ulong) 1L)
            {
                action();
            }
        }

        public static void Times(this int n, Action<int> action)
        {
            if ((n > 0) && (action != null))
            {
                for (int i = 0; i < n; i++)
                {
                    action(i);
                }
            }
        }

        public static void Times(this int n, Action action)
        {
            if ((n > 0) && (action != null))
            {
                ((ulong) n).times(action);
            }
        }

        public static void Times(this long n, Action<long> action)
        {
            if ((n > 0L) && (action != null))
            {
                for (long i = 0L; i < n; i += 1L)
                {
                    action(i);
                }
            }
        }

        public static void Times(this long n, Action action)
        {
            if ((n > 0L) && (action != null))
            {
                ((ulong) n).times(action);
            }
        }

        public static void Times(this uint n, Action<uint> action)
        {
            if ((n > 0) && (action != null))
            {
                for (uint i = 0; i < n; i++)
                {
                    action(i);
                }
            }
        }

        public static void Times(this uint n, Action action)
        {
            if ((n > 0) && (action != null))
            {
                ((ulong) n).times(action);
            }
        }

        public static void Times(this ulong n, Action<ulong> action)
        {
            if ((n > 0L) && (action != null))
            {
                for (ulong i = 0UL; i < n; i += (ulong) 1L)
                {
                    action(i);
                }
            }
        }

        public static void Times(this ulong n, Action action)
        {
            if ((n > 0L) && (action != null))
            {
                n.times(action);
            }
        }

        public static T To<T>(this byte[] source, ByteOrder sourceOrder) where T: struct
        {
            T local1;
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (source.Length == 0)
            {
                return default(T);
            }
            Type objA = typeof(T);
            byte[] buffer = source.ToHostOrder(sourceOrder);
            if (ReferenceEquals(objA, typeof(bool)))
            {
                local1 = (T) BitConverter.ToBoolean(buffer, 0);
            }
            else if (ReferenceEquals(objA, typeof(char)))
            {
                local1 = (T) BitConverter.ToChar(buffer, 0);
            }
            else if (ReferenceEquals(objA, typeof(double)))
            {
                local1 = (T) BitConverter.ToDouble(buffer, 0);
            }
            else if (ReferenceEquals(objA, typeof(short)))
            {
                local1 = (T) BitConverter.ToInt16(buffer, 0);
            }
            else if (ReferenceEquals(objA, typeof(int)))
            {
                local1 = (T) BitConverter.ToInt32(buffer, 0);
            }
            else if (ReferenceEquals(objA, typeof(long)))
            {
                local1 = (T) BitConverter.ToInt64(buffer, 0);
            }
            else if (ReferenceEquals(objA, typeof(float)))
            {
                local1 = (T) BitConverter.ToSingle(buffer, 0);
            }
            else if (ReferenceEquals(objA, typeof(ushort)))
            {
                local1 = (T) BitConverter.ToUInt16(buffer, 0);
            }
            else if (ReferenceEquals(objA, typeof(uint)))
            {
                local1 = (T) BitConverter.ToUInt32(buffer, 0);
            }
            else if (ReferenceEquals(objA, typeof(ulong)))
            {
                local1 = (T) BitConverter.ToUInt64(buffer, 0);
            }
            else
            {
                local1 = default(T);
            }
            return local1;
        }

        internal static byte[] ToByteArray(this Stream stream)
        {
            using (MemoryStream stream2 = new MemoryStream())
            {
                stream.Position = 0L;
                stream.CopyTo(stream2, 0x400);
                stream2.Close();
                return stream2.ToArray();
            }
        }

        public static byte[] ToByteArray<T>(this T value, ByteOrder order) where T: struct
        {
            byte[] bytes;
            Type objA = typeof(T);
            if (ReferenceEquals(objA, typeof(bool)))
            {
                bytes = BitConverter.GetBytes((bool) value);
            }
            else if (!ReferenceEquals(objA, typeof(byte)))
            {
                bytes = !ReferenceEquals(objA, typeof(char)) ? (!ReferenceEquals(objA, typeof(double)) ? (!ReferenceEquals(objA, typeof(short)) ? (!ReferenceEquals(objA, typeof(int)) ? (!ReferenceEquals(objA, typeof(long)) ? (!ReferenceEquals(objA, typeof(float)) ? (!ReferenceEquals(objA, typeof(ushort)) ? (!ReferenceEquals(objA, typeof(uint)) ? (!ReferenceEquals(objA, typeof(ulong)) ? WebSocket.EmptyBytes : BitConverter.GetBytes((ulong) value)) : BitConverter.GetBytes((uint) value)) : BitConverter.GetBytes((ushort) value)) : BitConverter.GetBytes((float) value)) : BitConverter.GetBytes((long) value)) : BitConverter.GetBytes((int) value)) : BitConverter.GetBytes((short) value)) : BitConverter.GetBytes((double) value)) : BitConverter.GetBytes((char) value);
            }
            else
            {
                bytes = new byte[] { (byte) value };
            }
            byte[] array = bytes;
            if ((array.Length > 1) && !order.IsHostOrder())
            {
                Array.Reverse(array);
            }
            return array;
        }

        internal static CompressionMethod ToCompressionMethod(this string value)
        {
            CompressionMethod method2;
            IEnumerator enumerator = Enum.GetValues(typeof(CompressionMethod)).GetEnumerator();
            try
            {
                while (true)
                {
                    if (enumerator.MoveNext())
                    {
                        CompressionMethod current = (CompressionMethod) enumerator.Current;
                        if (current.ToExtensionString(new string[0]) != value)
                        {
                            continue;
                        }
                        method2 = current;
                    }
                    else
                    {
                        return CompressionMethod.None;
                    }
                    break;
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
            return method2;
        }

        internal static string ToExtensionString(this CompressionMethod method, params string[] parameters)
        {
            if (method == CompressionMethod.None)
            {
                return string.Empty;
            }
            string str = $"permessage-{method.ToString().ToLower()}";
            return (((parameters == null) || (parameters.Length == 0)) ? str : $"{str}; {parameters.ToString<string>("; ")}");
        }

        public static byte[] ToHostOrder(this byte[] source, ByteOrder sourceOrder)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return (((source.Length <= 1) || sourceOrder.IsHostOrder()) ? source : source.Reverse<byte>());
        }

        internal static IPAddress ToIPAddress(this string hostnameOrAddress)
        {
            try
            {
                return Dns.GetHostAddresses(hostnameOrAddress)[0];
            }
            catch
            {
                return null;
            }
        }

        internal static List<TSource> ToList<TSource>(this IEnumerable<TSource> source) => 
            new List<TSource>(source);

        public static string ToString<T>(this T[] array, string separator)
        {
            <ToString>c__AnonStorey7<T> storey = new <ToString>c__AnonStorey7<T> {
                array = array,
                separator = separator
            };
            if (storey.array == null)
            {
                throw new ArgumentNullException("array");
            }
            int length = storey.array.Length;
            if (length == 0)
            {
                return string.Empty;
            }
            storey.separator ??= string.Empty;
            storey.buff = new StringBuilder(0x40);
            (length - 1).Times(new Action<int>(storey.<>m__0));
            storey.buff.Append(storey.array[length - 1].ToString());
            return storey.buff.ToString();
        }

        internal static ushort ToUInt16(this byte[] source, ByteOrder sourceOrder) => 
            BitConverter.ToUInt16(source.ToHostOrder(sourceOrder), 0);

        internal static ulong ToUInt64(this byte[] source, ByteOrder sourceOrder) => 
            BitConverter.ToUInt64(source.ToHostOrder(sourceOrder), 0);

        public static Uri ToUri(this string uriString)
        {
            Uri uri;
            Uri.TryCreate(uriString, !uriString.MaybeUri() ? UriKind.Relative : UriKind.Absolute, out uri);
            return uri;
        }

        internal static string TrimEndSlash(this string value)
        {
            char[] trimChars = new char[] { '/' };
            value = value.TrimEnd(trimChars);
            return ((value.Length <= 0) ? "/" : value);
        }

        internal static bool TryCreateWebSocketUri(this string uriString, out Uri result, out string message)
        {
            Uri uri1;
            result = null;
            Uri uri = uriString.ToUri();
            if (uri == null)
            {
                message = "An invalid URI string: " + uriString;
                return false;
            }
            if (!uri.IsAbsoluteUri)
            {
                message = "Not an absolute URI: " + uriString;
                return false;
            }
            string scheme = uri.Scheme;
            if ((scheme != "ws") && (scheme != "wss"))
            {
                message = "The scheme part isn't 'ws' or 'wss': " + uriString;
                return false;
            }
            if (uri.Fragment.Length > 0)
            {
                message = "Includes the fragment component: " + uriString;
                return false;
            }
            int port = uri.Port;
            if (port == 0)
            {
                message = "The port part is zero: " + uriString;
                return false;
            }
            if (port != -1)
            {
                uri1 = uri;
            }
            else
            {
                object[] objArray1 = new object[4];
                objArray1[0] = scheme;
                objArray1[1] = uri.Host;
                objArray1[2] = (scheme != "ws") ? 0x1bb : 80;
                object[] args = objArray1;
                args[3] = uri.PathAndQuery;
                uri1 = new Uri(string.Format("{0}://{1}:{2}{3}", args));
            }
            result = uri1;
            message = string.Empty;
            return true;
        }

        internal static string Unquote(this string value)
        {
            int index = value.IndexOf('"');
            if (index < 0)
            {
                return value;
            }
            int length = (value.LastIndexOf('"') - index) - 1;
            return ((length >= 0) ? ((length != 0) ? value.Substring(index + 1, length).Replace("\\\"", "\"") : string.Empty) : value);
        }

        public static string UrlDecode(this string value) => 
            ((value == null) || (value.Length <= 0)) ? value : HttpUtility.UrlDecode(value);

        public static string UrlEncode(this string value) => 
            ((value == null) || (value.Length <= 0)) ? value : HttpUtility.UrlEncode(value);

        internal static string UTF8Decode(this byte[] bytes)
        {
            try
            {
                return Encoding.UTF8.GetString(bytes);
            }
            catch
            {
                return null;
            }
        }

        internal static byte[] UTF8Encode(this string s) => 
            Encoding.UTF8.GetBytes(s);

        internal static void WriteBytes(this Stream stream, byte[] bytes, int bufferLength)
        {
            using (MemoryStream stream2 = new MemoryStream(bytes))
            {
                stream2.CopyTo(stream, bufferLength);
            }
        }

        internal static void WriteBytesAsync(this Stream stream, byte[] bytes, int bufferLength, Action completed, Action<Exception> error)
        {
            <WriteBytesAsync>c__AnonStorey6 storey = new <WriteBytesAsync>c__AnonStorey6 {
                completed = completed,
                error = error,
                input = new MemoryStream(bytes)
            };
            storey.input.CopyToAsync(stream, bufferLength, new Action(storey.<>m__0), new Action<Exception>(storey.<>m__1));
        }

        public static void WriteContent(this HttpListenerResponse response, byte[] content)
        {
            if (response == null)
            {
                throw new ArgumentNullException("response");
            }
            if (content == null)
            {
                throw new ArgumentNullException("content");
            }
            long longLength = content.LongLength;
            if (longLength == 0L)
            {
                response.Close();
            }
            else
            {
                response.ContentLength64 = longLength;
                Stream outputStream = response.OutputStream;
                if (longLength <= 0x7fffffffL)
                {
                    outputStream.Write(content, 0, (int) longLength);
                }
                else
                {
                    outputStream.WriteBytes(content, 0x400);
                }
                outputStream.Close();
            }
        }

        [CompilerGenerated]
        private sealed class <ContainsTwice>c__AnonStorey1
        {
            internal int len;
            internal string[] values;
            internal Func<int, bool> contains;

            internal bool <>m__0(int idx)
            {
                if (idx >= (this.len - 1))
                {
                    return false;
                }
                for (int i = idx + 1; i < this.len; i++)
                {
                    if (this.values[i] == this.values[idx])
                    {
                        return true;
                    }
                }
                return this.contains(++idx);
            }
        }

        [CompilerGenerated]
        private sealed class <CopyToAsync>c__AnonStorey2
        {
            internal Stream source;
            internal Action completed;
            internal Stream destination;
            internal byte[] buff;
            internal int bufferLength;
            internal AsyncCallback callback;
            internal Action<Exception> error;

            internal void <>m__0(IAsyncResult ar)
            {
                try
                {
                    int count = this.source.EndRead(ar);
                    if (count > 0)
                    {
                        this.destination.Write(this.buff, 0, count);
                        this.source.BeginRead(this.buff, 0, this.bufferLength, this.callback, null);
                    }
                    else if (this.completed != null)
                    {
                        this.completed();
                    }
                }
                catch (Exception exception)
                {
                    if (this.error != null)
                    {
                        this.error(exception);
                    }
                }
            }
        }

        [CompilerGenerated]
        private sealed class <ReadBytesAsync>c__AnonStorey3
        {
            internal Stream stream;
            internal int length;
            internal Action<byte[]> completed;
            internal byte[] buff;
            internal int offset;
            internal AsyncCallback callback;
            internal Action<Exception> error;

            internal void <>m__0(IAsyncResult ar)
            {
                try
                {
                    int num = this.stream.EndRead(ar);
                    if ((num != 0) && (num != this.length))
                    {
                        this.offset += num;
                        this.length -= num;
                        this.stream.BeginRead(this.buff, this.offset, this.length, this.callback, null);
                    }
                    else if (this.completed != null)
                    {
                        this.completed(this.buff.SubArray<byte>(0, this.offset + num));
                    }
                }
                catch (Exception exception)
                {
                    if (this.error != null)
                    {
                        this.error(exception);
                    }
                }
            }
        }

        [CompilerGenerated]
        private sealed class <ReadBytesAsync>c__AnonStorey4
        {
            internal int bufferLength;
            internal Stream stream;
            internal byte[] buff;
            internal MemoryStream dest;
            internal Action<byte[]> completed;
            internal Action<long> read;
            internal Action<Exception> error;

            internal void <>m__0(long len)
            {
                <ReadBytesAsync>c__AnonStorey5 storey = new <ReadBytesAsync>c__AnonStorey5 {
                    <>f__ref$4 = this,
                    len = len
                };
                if (storey.len < this.bufferLength)
                {
                    this.bufferLength = (int) storey.len;
                }
                this.stream.BeginRead(this.buff, 0, this.bufferLength, new AsyncCallback(storey.<>m__0), null);
            }

            private sealed class <ReadBytesAsync>c__AnonStorey5
            {
                internal long len;
                internal Ext.<ReadBytesAsync>c__AnonStorey4 <>f__ref$4;

                internal void <>m__0(IAsyncResult ar)
                {
                    try
                    {
                        int count = this.<>f__ref$4.stream.EndRead(ar);
                        if (count > 0)
                        {
                            this.<>f__ref$4.dest.Write(this.<>f__ref$4.buff, 0, count);
                        }
                        if ((count != 0) && (count != this.len))
                        {
                            this.<>f__ref$4.read(this.len - count);
                        }
                        else
                        {
                            if (this.<>f__ref$4.completed != null)
                            {
                                this.<>f__ref$4.dest.Close();
                                this.<>f__ref$4.completed(this.<>f__ref$4.dest.ToArray());
                            }
                            this.<>f__ref$4.dest.Dispose();
                        }
                    }
                    catch (Exception exception)
                    {
                        this.<>f__ref$4.dest.Dispose();
                        if (this.<>f__ref$4.error != null)
                        {
                            this.<>f__ref$4.error(exception);
                        }
                    }
                }
            }
        }

        [CompilerGenerated]
        private sealed class <SplitHeaderValue>c__Iterator0 : IEnumerable, IEnumerable<string>, IEnumerator, IDisposable, IEnumerator<string>
        {
            internal string value;
            internal int <len>__0;
            internal char[] separators;
            internal string <seps>__0;
            internal StringBuilder <buff>__0;
            internal bool <escaped>__0;
            internal bool <quoted>__0;
            internal int <i>__1;
            internal char <c>__2;
            internal string $current;
            internal bool $disposing;
            internal int $PC;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$disposing = true;
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        this.<len>__0 = this.value.Length;
                        this.<seps>__0 = new string(this.separators);
                        this.<buff>__0 = new StringBuilder(0x20);
                        this.<escaped>__0 = false;
                        this.<quoted>__0 = false;
                        this.<i>__1 = 0;
                        break;

                    case 1:
                        this.<buff>__0.Length = 0;
                        goto TR_0007;

                    case 2:
                        goto TR_0005;

                    default:
                        goto TR_0000;
                }
                goto TR_0017;
            TR_0000:
                return false;
            TR_0001:
                return true;
            TR_0005:
                this.$PC = -1;
                goto TR_0000;
            TR_0007:
                this.<i>__1++;
            TR_0017:
                while (true)
                {
                    if (this.<i>__1 < this.<len>__0)
                    {
                        this.<c>__2 = this.value[this.<i>__1];
                        if (this.<c>__2 == '"')
                        {
                            if (this.<escaped>__0)
                            {
                                this.<escaped>__0 = !this.<escaped>__0;
                            }
                            else
                            {
                                this.<quoted>__0 = !this.<quoted>__0;
                            }
                            break;
                        }
                        if (this.<c>__2 != '\\')
                        {
                            char[] chars = new char[] { this.<c>__2 };
                            if (this.<seps>__0.Contains(chars) && !this.<quoted>__0)
                            {
                                this.$current = this.<buff>__0.ToString();
                                if (!this.$disposing)
                                {
                                    this.$PC = 1;
                                }
                                goto TR_0001;
                            }
                        }
                        else if ((this.<i>__1 < (this.<len>__0 - 1)) && (this.value[this.<i>__1 + 1] == '"'))
                        {
                            this.<escaped>__0 = true;
                        }
                        break;
                    }
                    else if (this.<buff>__0.Length <= 0)
                    {
                        goto TR_0005;
                    }
                    else
                    {
                        this.$current = this.<buff>__0.ToString();
                        if (!this.$disposing)
                        {
                            this.$PC = 2;
                        }
                    }
                    goto TR_0001;
                }
                this.<buff>__0.Append(this.<c>__2);
                goto TR_0007;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            [DebuggerHidden]
            IEnumerator<string> IEnumerable<string>.GetEnumerator()
            {
                if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
                {
                    return this;
                }
                return new Ext.<SplitHeaderValue>c__Iterator0 { 
                    value = this.value,
                    separators = this.separators
                };
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator() => 
                this.System.Collections.Generic.IEnumerable<string>.GetEnumerator();

            string IEnumerator<string>.Current =>
                this.$current;

            object IEnumerator.Current =>
                this.$current;
        }

        [CompilerGenerated]
        private sealed class <ToString>c__AnonStorey7<T>
        {
            internal StringBuilder buff;
            internal T[] array;
            internal string separator;

            internal void <>m__0(int i)
            {
                this.buff.AppendFormat("{0}{1}", this.array[i].ToString(), this.separator);
            }
        }

        [CompilerGenerated]
        private sealed class <WriteBytesAsync>c__AnonStorey6
        {
            internal Action completed;
            internal MemoryStream input;
            internal Action<Exception> error;

            internal void <>m__0()
            {
                if (this.completed != null)
                {
                    this.completed();
                }
                this.input.Dispose();
            }

            internal void <>m__1(Exception ex)
            {
                this.input.Dispose();
                if (this.error != null)
                {
                    this.error(ex);
                }
            }
        }
    }
}

