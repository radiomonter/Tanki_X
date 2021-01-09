namespace SharpCompress.IO
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class ReadOnlyAppendingStream : Stream
    {
        private Queue<Stream> streams;
        private Stream current;

        public ReadOnlyAppendingStream(IEnumerable<Stream> streams)
        {
            this.streams = new Queue<Stream>(streams);
        }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if ((this.current == null) && (this.streams.Count == 0))
            {
                return -1;
            }
            this.current ??= this.streams.Dequeue();
            int num = 0;
            while (num < count)
            {
                int num2 = this.current.Read(buffer, offset + num, count - num);
                if (num2 <= 0)
                {
                    if (this.streams.Count == 0)
                    {
                        return num;
                    }
                    this.current = this.streams.Dequeue();
                }
                num += num2;
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

        public override bool CanRead =>
            true;

        public override bool CanSeek
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override bool CanWrite
        {
            get
            {
                throw new NotImplementedException();
            }
        }

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

