namespace SharpCompress.IO
{
    using SharpCompress.Common;
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;

    internal class ListeningStream : System.IO.Stream
    {
        private long currentEntryTotalReadBytes;
        private IStreamListener listener;

        public ListeningStream(IStreamListener listener, System.IO.Stream stream)
        {
            this.Stream = stream;
            this.listener = listener;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Stream.Dispose();
            }
        }

        public override void Flush()
        {
            this.Stream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int num = this.Stream.Read(buffer, offset, count);
            this.currentEntryTotalReadBytes += num;
            this.listener.FireCompressedBytesRead(this.currentEntryTotalReadBytes, this.currentEntryTotalReadBytes);
            return num;
        }

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

