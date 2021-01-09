namespace SharpCompress.Compressor.BZip2
{
    using SharpCompress.Compressor;
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class BZip2Stream : Stream
    {
        private readonly Stream stream;

        public BZip2Stream(Stream stream, CompressionMode compressionMode, bool leaveOpen = false, bool decompressContacted = false)
        {
            this.Mode = compressionMode;
            this.stream = (this.Mode != CompressionMode.Compress) ? ((Stream) new CBZip2InputStream(stream, decompressContacted, leaveOpen)) : ((Stream) new CBZip2OutputStream(stream, leaveOpen));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.stream.Dispose();
            }
        }

        public override void Flush()
        {
            this.stream.Flush();
        }

        public static bool IsBZip2(Stream stream)
        {
            byte[] buffer = new BinaryReader(stream).ReadBytes(2);
            return ((buffer.Length >= 2) && ((buffer[0] == 0x42) && (buffer[1] == 90)));
        }

        public override int Read(byte[] buffer, int offset, int count) => 
            this.stream.Read(buffer, offset, count);

        public override long Seek(long offset, SeekOrigin origin) => 
            this.stream.Seek(offset, origin);

        public override void SetLength(long value)
        {
            this.stream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            this.stream.Write(buffer, offset, count);
        }

        public CompressionMode Mode { get; private set; }

        public override bool CanRead =>
            this.stream.CanRead;

        public override bool CanSeek =>
            this.stream.CanSeek;

        public override bool CanWrite =>
            this.stream.CanWrite;

        public override long Length =>
            this.stream.Length;

        public override long Position
        {
            get => 
                this.stream.Position;
            set => 
                this.stream.Position = value;
        }
    }
}

