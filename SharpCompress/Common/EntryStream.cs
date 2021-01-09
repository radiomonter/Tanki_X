namespace SharpCompress.Common
{
    using System;
    using System.IO;

    public class EntryStream : Stream
    {
        private Stream stream;
        private bool completed;

        internal EntryStream(Stream stream)
        {
            this.stream = stream;
        }

        protected override void Dispose(bool disposing)
        {
            if (!this.completed)
            {
                throw new InvalidOperationException("EntryStream has not been fully consumed.  Read the entire stream or use SkipEntry.");
            }
            base.Dispose(disposing);
            this.stream.Dispose();
        }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int num = this.stream.Read(buffer, offset, count);
            if (num <= 0)
            {
                this.completed = true;
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

        public void SkipEntry()
        {
            byte[] buffer = new byte[0x1000];
            while (this.Read(buffer, 0, buffer.Length) > 0)
            {
            }
            this.completed = true;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

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

