namespace SharpCompress.IO
{
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;

    internal class CountingWritableSubStream : Stream
    {
        private Stream writableStream;

        internal CountingWritableSubStream(Stream stream)
        {
            this.writableStream = stream;
        }

        public override void Flush()
        {
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            this.writableStream.Write(buffer, offset, count);
            this.Count += (uint) count;
        }

        public uint Count { get; private set; }

        public override bool CanRead =>
            false;

        public override bool CanSeek =>
            false;

        public override bool CanWrite =>
            true;

        public override long Length
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override long Position
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}

