namespace WebSocketSharp
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text;

    internal class WebSocketFrame : IEnumerable<byte>, IEnumerable
    {
        private byte[] _extPayloadLength;
        private WebSocketSharp.Fin _fin;
        private WebSocketSharp.Mask _mask;
        private byte[] _maskingKey;
        private WebSocketSharp.Opcode _opcode;
        private WebSocketSharp.PayloadData _payloadData;
        private byte _payloadLength;
        private Rsv _rsv1;
        private Rsv _rsv2;
        private Rsv _rsv3;
        internal static readonly byte[] EmptyPingBytes = CreatePingFrame(false).ToArray();

        private WebSocketFrame()
        {
        }

        internal WebSocketFrame(WebSocketSharp.Opcode opcode, WebSocketSharp.PayloadData payloadData, bool mask) : this(WebSocketSharp.Fin.Final, opcode, payloadData, false, mask)
        {
        }

        internal WebSocketFrame(WebSocketSharp.Fin fin, WebSocketSharp.Opcode opcode, byte[] data, bool compressed, bool mask) : this(fin, opcode, new WebSocketSharp.PayloadData(data), compressed, mask)
        {
        }

        internal WebSocketFrame(WebSocketSharp.Fin fin, WebSocketSharp.Opcode opcode, WebSocketSharp.PayloadData payloadData, bool compressed, bool mask)
        {
            this._fin = fin;
            this._rsv1 = (!opcode.IsData() || !compressed) ? Rsv.Off : Rsv.On;
            this._rsv2 = Rsv.Off;
            this._rsv3 = Rsv.Off;
            this._opcode = opcode;
            ulong length = payloadData.Length;
            if (length < 0x7e)
            {
                this._payloadLength = (byte) length;
                this._extPayloadLength = WebSocket.EmptyBytes;
            }
            else if (length < 0x10000L)
            {
                this._payloadLength = 0x7e;
                this._extPayloadLength = ((ushort) length).InternalToByteArray(ByteOrder.Big);
            }
            else
            {
                this._payloadLength = 0x7f;
                this._extPayloadLength = length.InternalToByteArray(ByteOrder.Big);
            }
            if (!mask)
            {
                this._mask = WebSocketSharp.Mask.Off;
                this._maskingKey = WebSocket.EmptyBytes;
            }
            else
            {
                this._mask = WebSocketSharp.Mask.On;
                this._maskingKey = createMaskingKey();
                payloadData.Mask(this._maskingKey);
            }
            this._payloadData = payloadData;
        }

        internal static WebSocketFrame CreateCloseFrame(WebSocketSharp.PayloadData payloadData, bool mask) => 
            new WebSocketFrame(WebSocketSharp.Fin.Final, WebSocketSharp.Opcode.Close, payloadData, false, mask);

        private static byte[] createMaskingKey()
        {
            byte[] data = new byte[4];
            WebSocket.RandomNumber.GetBytes(data);
            return data;
        }

        internal static WebSocketFrame CreatePingFrame(bool mask) => 
            new WebSocketFrame(WebSocketSharp.Fin.Final, WebSocketSharp.Opcode.Ping, WebSocketSharp.PayloadData.Empty, false, mask);

        internal static WebSocketFrame CreatePingFrame(byte[] data, bool mask) => 
            new WebSocketFrame(WebSocketSharp.Fin.Final, WebSocketSharp.Opcode.Ping, new WebSocketSharp.PayloadData(data), false, mask);

        private static string dump(WebSocketFrame frame)
        {
            int num4;
            string str;
            <dump>c__AnonStorey1 storey = new <dump>c__AnonStorey1();
            ulong length = frame.Length;
            long num2 = (long) (length / ((ulong) 4L));
            int num3 = (int) (length % ((ulong) 4L));
            if (num2 < 0x2710L)
            {
                num4 = 4;
                str = "{0,4}";
            }
            else if (num2 < 0x10000L)
            {
                num4 = 4;
                str = "{0,4:X}";
            }
            else if (num2 < 0x100000000L)
            {
                num4 = 8;
                str = "{0,8:X}";
            }
            else
            {
                num4 = 0x10;
                str = "{0,16:X}";
            }
            string str2 = $"{{0,{num4}}}";
            string format = string.Format("\n{0} 01234567 89ABCDEF 01234567 89ABCDEF\n{0}+--------+--------+--------+--------+\\n", str2);
            storey.lineFmt = $"{str}|{{1,8}} {{2,8}} {{3,8}} {{4,8}}|
";
            string str4 = $"{str2}+--------+--------+--------+--------+";
            storey.output = new StringBuilder(0x40);
            Action<string, string, string, string> action = new Func<Action<string, string, string, string>>(storey.<>m__0)();
            storey.output.AppendFormat(format, string.Empty);
            byte[] buffer = frame.ToArray();
            for (long i = 0L; i <= num2; i += 1L)
            {
                long num6 = i * 4L;
                if (i < num2)
                {
                    action(Convert.ToString(buffer[(int) ((IntPtr) num6)], 2).PadLeft(8, '0'), Convert.ToString(buffer[(int) ((IntPtr) (num6 + 1L))], 2).PadLeft(8, '0'), Convert.ToString(buffer[(int) ((IntPtr) (num6 + 2L))], 2).PadLeft(8, '0'), Convert.ToString(buffer[(int) ((IntPtr) (num6 + 3L))], 2).PadLeft(8, '0'));
                }
                else if (num3 > 0)
                {
                    action(Convert.ToString(buffer[(int) ((IntPtr) num6)], 2).PadLeft(8, '0'), (num3 < 2) ? string.Empty : Convert.ToString(buffer[(int) ((IntPtr) (num6 + 1L))], 2).PadLeft(8, '0'), (num3 != 3) ? string.Empty : Convert.ToString(buffer[(int) ((IntPtr) (num6 + 2L))], 2).PadLeft(8, '0'), string.Empty);
                }
            }
            storey.output.AppendFormat(str4, string.Empty);
            return storey.output.ToString();
        }

        [DebuggerHidden]
        public IEnumerator<byte> GetEnumerator() => 
            new <GetEnumerator>c__Iterator0 { $this = this };

        private static string print(WebSocketFrame frame)
        {
            byte num = frame._payloadLength;
            string str = (num <= 0x7d) ? string.Empty : frame.FullPayloadLength.ToString();
            string str2 = BitConverter.ToString(frame._maskingKey);
            string str3 = (num != 0) ? ((num <= 0x7d) ? ((!frame.IsText || (frame.IsFragment || (frame.IsMasked || frame.IsCompressed))) ? frame._payloadData.ToString() : frame._payloadData.ApplicationData.UTF8Decode()) : "---") : string.Empty;
            object[] args = new object[10];
            args[0] = frame._fin;
            args[1] = frame._rsv1;
            args[2] = frame._rsv2;
            args[3] = frame._rsv3;
            args[4] = frame._opcode;
            args[5] = frame._mask;
            args[6] = num;
            args[7] = str;
            args[8] = str2;
            args[9] = str3;
            return string.Format("\n                    FIN: {0}\n                   RSV1: {1}\n                   RSV2: {2}\n                   RSV3: {3}\n                 Opcode: {4}\n                   MASK: {5}\n         Payload Length: {6}\nExtended Payload Length: {7}\n            Masking Key: {8}\n           Payload Data: {9}", args);
        }

        public void Print(bool dumped)
        {
            Console.WriteLine(!dumped ? print(this) : dump(this));
        }

        public string PrintToString(bool dumped) => 
            !dumped ? print(this) : dump(this);

        private static WebSocketFrame processHeader(byte[] header)
        {
            if (header.Length != 2)
            {
                throw new WebSocketException("The header of a frame cannot be read from the stream.");
            }
            WebSocketSharp.Fin fin = ((header[0] & 0x80) != 0x80) ? WebSocketSharp.Fin.More : WebSocketSharp.Fin.Final;
            Rsv rsv = ((header[0] & 0x40) != 0x40) ? Rsv.Off : Rsv.On;
            Rsv rsv2 = ((header[0] & 0x20) != 0x20) ? Rsv.Off : Rsv.On;
            Rsv rsv3 = ((header[0] & 0x10) != 0x10) ? Rsv.Off : Rsv.On;
            byte opcode = (byte) (header[0] & 15);
            WebSocketSharp.Mask mask = ((header[1] & 0x80) != 0x80) ? WebSocketSharp.Mask.Off : WebSocketSharp.Mask.On;
            byte num2 = (byte) (header[1] & 0x7f);
            string message = opcode.IsSupported() ? ((opcode.IsData() || (rsv != Rsv.On)) ? ((!opcode.IsControl() || (fin != WebSocketSharp.Fin.More)) ? ((!opcode.IsControl() || (num2 <= 0x7d)) ? null : "A control frame has a long payload length.") : "A control frame is fragmented.") : "A non data frame is compressed.") : "An unsupported opcode.";
            if (message != null)
            {
                throw new WebSocketException(CloseStatusCode.ProtocolError, message);
            }
            return new WebSocketFrame { 
                _fin = fin,
                _rsv1 = rsv,
                _rsv2 = rsv2,
                _rsv3 = rsv3,
                _opcode = (WebSocketSharp.Opcode) opcode,
                _mask = mask,
                _payloadLength = num2
            };
        }

        private static WebSocketFrame readExtendedPayloadLength(Stream stream, WebSocketFrame frame)
        {
            int extendedPayloadLengthCount = frame.ExtendedPayloadLengthCount;
            if (extendedPayloadLengthCount == 0)
            {
                frame._extPayloadLength = WebSocket.EmptyBytes;
                return frame;
            }
            byte[] buffer = stream.ReadBytes(extendedPayloadLengthCount);
            if (buffer.Length != extendedPayloadLengthCount)
            {
                throw new WebSocketException("The extended payload length of a frame cannot be read from the stream.");
            }
            frame._extPayloadLength = buffer;
            return frame;
        }

        private static void readExtendedPayloadLengthAsync(Stream stream, WebSocketFrame frame, Action<WebSocketFrame> completed, Action<Exception> error)
        {
            <readExtendedPayloadLengthAsync>c__AnonStorey3 storey = new <readExtendedPayloadLengthAsync>c__AnonStorey3 {
                frame = frame,
                completed = completed
            };
            storey.len = storey.frame.ExtendedPayloadLengthCount;
            if (storey.len != 0)
            {
                stream.ReadBytesAsync(storey.len, new Action<byte[]>(storey.<>m__0), error);
            }
            else
            {
                storey.frame._extPayloadLength = WebSocket.EmptyBytes;
                storey.completed(storey.frame);
            }
        }

        internal static WebSocketFrame ReadFrame(Stream stream, bool unmask)
        {
            WebSocketFrame frame = readHeader(stream);
            readExtendedPayloadLength(stream, frame);
            readMaskingKey(stream, frame);
            readPayloadData(stream, frame);
            if (unmask)
            {
                frame.Unmask();
            }
            return frame;
        }

        internal static void ReadFrameAsync(Stream stream, bool unmask, Action<WebSocketFrame> completed, Action<Exception> error)
        {
            <ReadFrameAsync>c__AnonStorey7 storey = new <ReadFrameAsync>c__AnonStorey7 {
                stream = stream,
                error = error,
                unmask = unmask,
                completed = completed
            };
            readHeaderAsync(storey.stream, new Action<WebSocketFrame>(storey.<>m__0), storey.error);
        }

        private static WebSocketFrame readHeader(Stream stream) => 
            processHeader(stream.ReadBytes(2));

        private static void readHeaderAsync(Stream stream, Action<WebSocketFrame> completed, Action<Exception> error)
        {
            <readHeaderAsync>c__AnonStorey4 storey = new <readHeaderAsync>c__AnonStorey4 {
                completed = completed
            };
            stream.ReadBytesAsync(2, new Action<byte[]>(storey.<>m__0), error);
        }

        private static WebSocketFrame readMaskingKey(Stream stream, WebSocketFrame frame)
        {
            int length = !frame.IsMasked ? 0 : 4;
            if (length == 0)
            {
                frame._maskingKey = WebSocket.EmptyBytes;
                return frame;
            }
            byte[] buffer = stream.ReadBytes(length);
            if (buffer.Length != length)
            {
                throw new WebSocketException("The masking key of a frame cannot be read from the stream.");
            }
            frame._maskingKey = buffer;
            return frame;
        }

        private static void readMaskingKeyAsync(Stream stream, WebSocketFrame frame, Action<WebSocketFrame> completed, Action<Exception> error)
        {
            <readMaskingKeyAsync>c__AnonStorey5 storey = new <readMaskingKeyAsync>c__AnonStorey5 {
                frame = frame,
                completed = completed
            };
            storey.len = !storey.frame.IsMasked ? 0 : 4;
            if (storey.len != 0)
            {
                stream.ReadBytesAsync(storey.len, new Action<byte[]>(storey.<>m__0), error);
            }
            else
            {
                storey.frame._maskingKey = WebSocket.EmptyBytes;
                storey.completed(storey.frame);
            }
        }

        private static WebSocketFrame readPayloadData(Stream stream, WebSocketFrame frame)
        {
            ulong fullPayloadLength = frame.FullPayloadLength;
            if (fullPayloadLength == 0L)
            {
                frame._payloadData = WebSocketSharp.PayloadData.Empty;
                return frame;
            }
            if (fullPayloadLength > WebSocketSharp.PayloadData.MaxLength)
            {
                throw new WebSocketException(CloseStatusCode.TooBig, "A frame has a long payload length.");
            }
            long length = (long) fullPayloadLength;
            byte[] data = (frame._payloadLength >= 0x7f) ? stream.ReadBytes(length, 0x400) : stream.ReadBytes(((int) fullPayloadLength));
            if (data.LongLength != length)
            {
                throw new WebSocketException("The payload data of a frame cannot be read from the stream.");
            }
            frame._payloadData = new WebSocketSharp.PayloadData(data, length);
            return frame;
        }

        private static void readPayloadDataAsync(Stream stream, WebSocketFrame frame, Action<WebSocketFrame> completed, Action<Exception> error)
        {
            <readPayloadDataAsync>c__AnonStorey6 storey = new <readPayloadDataAsync>c__AnonStorey6 {
                frame = frame,
                completed = completed
            };
            ulong fullPayloadLength = storey.frame.FullPayloadLength;
            if (fullPayloadLength == 0L)
            {
                storey.frame._payloadData = WebSocketSharp.PayloadData.Empty;
                storey.completed(storey.frame);
            }
            else
            {
                if (fullPayloadLength > WebSocketSharp.PayloadData.MaxLength)
                {
                    throw new WebSocketException(CloseStatusCode.TooBig, "A frame has a long payload length.");
                }
                storey.llen = (long) fullPayloadLength;
                Action<byte[]> action = new Action<byte[]>(storey.<>m__0);
                if (storey.frame._payloadLength < 0x7f)
                {
                    stream.ReadBytesAsync((int) fullPayloadLength, action, error);
                }
                else
                {
                    stream.ReadBytesAsync(storey.llen, 0x400, action, error);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => 
            this.GetEnumerator();

        public byte[] ToArray()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                stream.Write(((ushort) ((((byte) ((((((((((this._fin << 1) + this._rsv1) << 1) + this._rsv2) << 1) + this._rsv3) << 4) + this._opcode) << 1) + this._mask)) << 7) + this._payloadLength)).InternalToByteArray(ByteOrder.Big), 0, 2);
                if (this._payloadLength > 0x7d)
                {
                    stream.Write(this._extPayloadLength, 0, (this._payloadLength != 0x7e) ? 8 : 2);
                }
                if (this._mask == WebSocketSharp.Mask.On)
                {
                    stream.Write(this._maskingKey, 0, 4);
                }
                if (this._payloadLength > 0)
                {
                    byte[] buffer = this._payloadData.ToArray();
                    if (this._payloadLength < 0x7f)
                    {
                        stream.Write(buffer, 0, buffer.Length);
                    }
                    else
                    {
                        stream.WriteBytes(buffer, 0x400);
                    }
                }
                stream.Close();
                return stream.ToArray();
            }
        }

        public override string ToString() => 
            BitConverter.ToString(this.ToArray());

        internal void Unmask()
        {
            if (this._mask != WebSocketSharp.Mask.Off)
            {
                this._mask = WebSocketSharp.Mask.Off;
                this._payloadData.Mask(this._maskingKey);
                this._maskingKey = WebSocket.EmptyBytes;
            }
        }

        internal int ExtendedPayloadLengthCount =>
            (this._payloadLength >= 0x7e) ? ((this._payloadLength != 0x7e) ? 8 : 2) : 0;

        internal ulong FullPayloadLength =>
            (this._payloadLength >= 0x7e) ? ((this._payloadLength != 0x7e) ? this._extPayloadLength.ToUInt64(ByteOrder.Big) : ((ulong) this._extPayloadLength.ToUInt16(ByteOrder.Big))) : ((ulong) this._payloadLength);

        public byte[] ExtendedPayloadLength =>
            this._extPayloadLength;

        public WebSocketSharp.Fin Fin =>
            this._fin;

        public bool IsBinary =>
            this._opcode == WebSocketSharp.Opcode.Binary;

        public bool IsClose =>
            this._opcode == WebSocketSharp.Opcode.Close;

        public bool IsCompressed =>
            this._rsv1 == Rsv.On;

        public bool IsContinuation =>
            this._opcode == WebSocketSharp.Opcode.Cont;

        public bool IsControl =>
            this._opcode >= WebSocketSharp.Opcode.Close;

        public bool IsData =>
            (this._opcode == WebSocketSharp.Opcode.Text) || (this._opcode == WebSocketSharp.Opcode.Binary);

        public bool IsFinal =>
            this._fin == WebSocketSharp.Fin.Final;

        public bool IsFragment =>
            (this._fin == WebSocketSharp.Fin.More) || (this._opcode == WebSocketSharp.Opcode.Cont);

        public bool IsMasked =>
            this._mask == WebSocketSharp.Mask.On;

        public bool IsPing =>
            this._opcode == WebSocketSharp.Opcode.Ping;

        public bool IsPong =>
            this._opcode == WebSocketSharp.Opcode.Pong;

        public bool IsText =>
            this._opcode == WebSocketSharp.Opcode.Text;

        public ulong Length =>
            ((ulong) (2L + (this._extPayloadLength.Length + this._maskingKey.Length))) + this._payloadData.Length;

        public WebSocketSharp.Mask Mask =>
            this._mask;

        public byte[] MaskingKey =>
            this._maskingKey;

        public WebSocketSharp.Opcode Opcode =>
            this._opcode;

        public WebSocketSharp.PayloadData PayloadData =>
            this._payloadData;

        public byte PayloadLength =>
            this._payloadLength;

        public Rsv Rsv1 =>
            this._rsv1;

        public Rsv Rsv2 =>
            this._rsv2;

        public Rsv Rsv3 =>
            this._rsv3;

        [CompilerGenerated]
        private sealed class <dump>c__AnonStorey1
        {
            internal StringBuilder output;
            internal string lineFmt;

            internal Action<string, string, string, string> <>m__0()
            {
                <dump>c__AnonStorey2 storey = new <dump>c__AnonStorey2 {
                    <>f__ref$1 = this,
                    lineCnt = 0L
                };
                return new Action<string, string, string, string>(storey.<>m__0);
            }

            private sealed class <dump>c__AnonStorey2
            {
                internal long lineCnt;
                internal WebSocketFrame.<dump>c__AnonStorey1 <>f__ref$1;

                internal void <>m__0(string arg1, string arg2, string arg3, string arg4)
                {
                    long num;
                    this.lineCnt = num = this.lineCnt + 1L;
                    object[] args = new object[] { num, arg1, arg2, arg3, arg4 };
                    this.<>f__ref$1.output.AppendFormat(this.<>f__ref$1.lineFmt, args);
                }
            }
        }

        [CompilerGenerated]
        private sealed class <GetEnumerator>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<byte>
        {
            internal byte[] $locvar0;
            internal int $locvar1;
            internal byte <b>__1;
            internal WebSocketFrame $this;
            internal byte $current;
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
                        this.$locvar0 = this.$this.ToArray();
                        this.$locvar1 = 0;
                        break;

                    case 1:
                        this.$locvar1++;
                        break;

                    default:
                        goto TR_0000;
                }
                if (this.$locvar1 < this.$locvar0.Length)
                {
                    this.<b>__1 = this.$locvar0[this.$locvar1];
                    this.$current = this.<b>__1;
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    return true;
                }
                this.$PC = -1;
            TR_0000:
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            byte IEnumerator<byte>.Current =>
                this.$current;

            object IEnumerator.Current =>
                this.$current;
        }

        [CompilerGenerated]
        private sealed class <readExtendedPayloadLengthAsync>c__AnonStorey3
        {
            internal int len;
            internal WebSocketFrame frame;
            internal Action<WebSocketFrame> completed;

            internal void <>m__0(byte[] bytes)
            {
                if (bytes.Length != this.len)
                {
                    throw new WebSocketException("The extended payload length of a frame cannot be read from the stream.");
                }
                this.frame._extPayloadLength = bytes;
                this.completed(this.frame);
            }
        }

        [CompilerGenerated]
        private sealed class <ReadFrameAsync>c__AnonStorey7
        {
            internal Stream stream;
            internal Action<Exception> error;
            internal bool unmask;
            internal Action<WebSocketFrame> completed;

            internal void <>m__0(WebSocketFrame frame)
            {
                WebSocketFrame.readExtendedPayloadLengthAsync(this.stream, frame, frame1 => WebSocketFrame.readMaskingKeyAsync(this.stream, frame1, frame2 => WebSocketFrame.readPayloadDataAsync(this.stream, frame2, delegate (WebSocketFrame frame3) {
                    if (this.unmask)
                    {
                        frame3.Unmask();
                    }
                    this.completed(frame3);
                }, this.error), this.error), this.error);
            }

            internal void <>m__1(WebSocketFrame frame1)
            {
                WebSocketFrame.readMaskingKeyAsync(this.stream, frame1, frame2 => WebSocketFrame.readPayloadDataAsync(this.stream, frame2, delegate (WebSocketFrame frame3) {
                    if (this.unmask)
                    {
                        frame3.Unmask();
                    }
                    this.completed(frame3);
                }, this.error), this.error);
            }

            internal void <>m__2(WebSocketFrame frame2)
            {
                WebSocketFrame.readPayloadDataAsync(this.stream, frame2, delegate (WebSocketFrame frame3) {
                    if (this.unmask)
                    {
                        frame3.Unmask();
                    }
                    this.completed(frame3);
                }, this.error);
            }

            internal void <>m__3(WebSocketFrame frame3)
            {
                if (this.unmask)
                {
                    frame3.Unmask();
                }
                this.completed(frame3);
            }
        }

        [CompilerGenerated]
        private sealed class <readHeaderAsync>c__AnonStorey4
        {
            internal Action<WebSocketFrame> completed;

            internal void <>m__0(byte[] bytes)
            {
                this.completed(WebSocketFrame.processHeader(bytes));
            }
        }

        [CompilerGenerated]
        private sealed class <readMaskingKeyAsync>c__AnonStorey5
        {
            internal int len;
            internal WebSocketFrame frame;
            internal Action<WebSocketFrame> completed;

            internal void <>m__0(byte[] bytes)
            {
                if (bytes.Length != this.len)
                {
                    throw new WebSocketException("The masking key of a frame cannot be read from the stream.");
                }
                this.frame._maskingKey = bytes;
                this.completed(this.frame);
            }
        }

        [CompilerGenerated]
        private sealed class <readPayloadDataAsync>c__AnonStorey6
        {
            internal long llen;
            internal WebSocketFrame frame;
            internal Action<WebSocketFrame> completed;

            internal void <>m__0(byte[] bytes)
            {
                if (bytes.LongLength != this.llen)
                {
                    throw new WebSocketException("The payload data of a frame cannot be read from the stream.");
                }
                this.frame._payloadData = new PayloadData(bytes, this.llen);
                this.completed(this.frame);
            }
        }
    }
}

