namespace SharpCompress.Common.Tar
{
    using SharpCompress;
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;

    internal class TarReadOnlySubStream : System.IO.Stream
    {
        private int amountRead;

        public TarReadOnlySubStream(System.IO.Stream stream, long bytesToRead)
        {
            this.Stream = stream;
            this.BytesLeftToRead = bytesToRead;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                int num = this.amountRead % 0x200;
                if (num != 0)
                {
                    num = 0x200 - num;
                    if (num != 0)
                    {
                        byte[] buffer = new byte[num];
                        this.Stream.ReadFully(buffer);
                    }
                }
            }
        }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (this.BytesLeftToRead < count)
            {
                count = (int) this.BytesLeftToRead;
            }
            int num = this.Stream.Read(buffer, offset, count);
            if (num > 0)
            {
                this.BytesLeftToRead -= num;
                this.amountRead += num;
            }
            return num;
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
            throw new NotImplementedException();
        }

        private long BytesLeftToRead { get; set; }

        public System.IO.Stream Stream { get; private set; }

        public override bool CanRead =>
            true;

        public override bool CanSeek =>
            false;

        public override bool CanWrite =>
            false;

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

