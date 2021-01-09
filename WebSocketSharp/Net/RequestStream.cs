namespace WebSocketSharp.Net
{
    using System;
    using System.IO;

    internal class RequestStream : Stream
    {
        private long _bodyLeft;
        private byte[] _buffer;
        private int _count;
        private bool _disposed;
        private int _offset;
        private Stream _stream;

        internal RequestStream(Stream stream, byte[] buffer, int offset, int count) : this(stream, buffer, offset, count, -1L)
        {
        }

        internal RequestStream(Stream stream, byte[] buffer, int offset, int count, long contentLength)
        {
            this._stream = stream;
            this._buffer = buffer;
            this._offset = offset;
            this._count = count;
            this._bodyLeft = contentLength;
        }

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            if (this._disposed)
            {
                throw new ObjectDisposedException(base.GetType().ToString());
            }
            int num = this.fillFromBuffer(buffer, offset, count);
            if ((num <= 0) && (num != -1))
            {
                if ((this._bodyLeft >= 0L) && (count > this._bodyLeft))
                {
                    count = (int) this._bodyLeft;
                }
                return this._stream.BeginRead(buffer, offset, count, callback, state);
            }
            HttpStreamAsyncResult result = new HttpStreamAsyncResult(callback, state) {
                Buffer = buffer,
                Offset = offset,
                Count = count,
                SyncRead = (num <= 0) ? 0 : num
            };
            result.Complete();
            return result;
        }

        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            throw new NotSupportedException();
        }

        public override void Close()
        {
            this._disposed = true;
        }

        public override int EndRead(IAsyncResult asyncResult)
        {
            if (this._disposed)
            {
                throw new ObjectDisposedException(base.GetType().ToString());
            }
            if (asyncResult == null)
            {
                throw new ArgumentNullException("asyncResult");
            }
            if (asyncResult is HttpStreamAsyncResult)
            {
                HttpStreamAsyncResult result = (HttpStreamAsyncResult) asyncResult;
                if (!result.IsCompleted)
                {
                    result.AsyncWaitHandle.WaitOne();
                }
                return result.SyncRead;
            }
            int num = this._stream.EndRead(asyncResult);
            if ((num > 0) && (this._bodyLeft > 0L))
            {
                this._bodyLeft -= num;
            }
            return num;
        }

        public override void EndWrite(IAsyncResult asyncResult)
        {
            throw new NotSupportedException();
        }

        private int fillFromBuffer(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }
            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException("offset", "A negative value.");
            }
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count", "A negative value.");
            }
            int length = buffer.Length;
            if ((offset + count) > length)
            {
                throw new ArgumentException("The sum of 'offset' and 'count' is greater than 'buffer' length.");
            }
            if (this._bodyLeft == 0L)
            {
                return -1;
            }
            if ((this._count == 0) || (count == 0))
            {
                return 0;
            }
            if (count > this._count)
            {
                count = this._count;
            }
            if ((this._bodyLeft > 0L) && (count > this._bodyLeft))
            {
                count = (int) this._bodyLeft;
            }
            Buffer.BlockCopy(this._buffer, this._offset, buffer, offset, count);
            this._offset += count;
            this._count -= count;
            if (this._bodyLeft > 0L)
            {
                this._bodyLeft -= count;
            }
            return count;
        }

        public override void Flush()
        {
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (this._disposed)
            {
                throw new ObjectDisposedException(base.GetType().ToString());
            }
            int num = this.fillFromBuffer(buffer, offset, count);
            if (num == -1)
            {
                return 0;
            }
            if (num <= 0)
            {
                num = this._stream.Read(buffer, offset, count);
                if ((num > 0) && (this._bodyLeft > 0L))
                {
                    this._bodyLeft -= num;
                }
            }
            return num;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
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
                throw new NotSupportedException();
            }
        }

        public override long Position
        {
            get
            {
                throw new NotSupportedException();
            }
            set
            {
                throw new NotSupportedException();
            }
        }
    }
}

