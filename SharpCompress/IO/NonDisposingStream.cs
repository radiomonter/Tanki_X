namespace SharpCompress.IO
{
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;

    internal class NonDisposingStream : System.IO.Stream
    {
        public NonDisposingStream(System.IO.Stream stream)
        {
            this.Stream = stream;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
        }

        public override void Flush()
        {
            this.Stream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count) => 
            this.Stream.Read(buffer, offset, count);

        public override long Seek(long offset, SeekOrigin origin) => 
            this.Stream.Seek(offset, origin);

        public override void SetLength(long value)
        {
            this.Stream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            this.Stream.Write(buffer, offset, count);
        }

        public System.IO.Stream Stream { get; private set; }

        public override bool CanRead =>
            this.Stream.CanRead;

        public override bool CanSeek =>
            this.Stream.CanSeek;

        public override bool CanWrite =>
            this.Stream.CanWrite;

        public override long Length =>
            this.Stream.Length;

        public override long Position
        {
            get => 
                this.Stream.Position;
            set => 
                this.Stream.Position = value;
        }
    }
}

