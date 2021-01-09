namespace WebSocketSharp.Net
{
    using System;
    using System.IO;
    using System.Text;

    internal class ResponseStream : Stream
    {
        private MemoryStream _body;
        private static readonly byte[] _crlf = new byte[] { 13, 10 };
        private bool _disposed;
        private HttpListenerResponse _response;
        private bool _sendChunked;
        private Stream _stream;
        private Action<byte[], int, int> _write;
        private Action<byte[], int, int> _writeBody;
        private Action<byte[], int, int> _writeChunked;

        internal ResponseStream(Stream stream, HttpListenerResponse response, bool ignoreWriteExceptions)
        {
            this._stream = stream;
            this._response = response;
            if (ignoreWriteExceptions)
            {
                this._write = new Action<byte[], int, int>(this.writeWithoutThrowingException);
                this._writeChunked = new Action<byte[], int, int>(this.writeChunkedWithoutThrowingException);
            }
            else
            {
                this._write = new Action<byte[], int, int>(stream.Write);
                this._writeChunked = new Action<byte[], int, int>(this.writeChunked);
            }
            this._body = new MemoryStream();
        }

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            throw new NotSupportedException();
        }

        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            if (this._disposed)
            {
                throw new ObjectDisposedException(base.GetType().ToString());
            }
            return this._body.BeginWrite(buffer, offset, count, callback, state);
        }

        public override void Close()
        {
            this.Close(false);
        }

        internal void Close(bool force)
        {
            if (!this._disposed)
            {
                this._disposed = true;
                if (!force && this.flush(true))
                {
                    this._response.Close();
                }
                else
                {
                    if (this._sendChunked)
                    {
                        byte[] buffer = getChunkSizeBytes(0, true);
                        this._write(buffer, 0, buffer.Length);
                    }
                    this._body.Dispose();
                    this._body = null;
                    this._response.Abort();
                }
                this._response = null;
                this._stream = null;
            }
        }

        protected override void Dispose(bool disposing)
        {
            this.Close(!disposing);
        }

        public override int EndRead(IAsyncResult asyncResult)
        {
            throw new NotSupportedException();
        }

        public override void EndWrite(IAsyncResult asyncResult)
        {
            if (this._disposed)
            {
                throw new ObjectDisposedException(base.GetType().ToString());
            }
            this._body.EndWrite(asyncResult);
        }

        private bool flush(bool closing)
        {
            if (!this._response.HeadersSent)
            {
                if (!this.flushHeaders(closing))
                {
                    if (closing)
                    {
                        this._response.CloseConnection = true;
                    }
                    return false;
                }
                this._sendChunked = this._response.SendChunked;
                this._writeBody = !this._sendChunked ? this._write : this._writeChunked;
            }
            this.flushBody(closing);
            if (closing && this._sendChunked)
            {
                byte[] buffer = getChunkSizeBytes(0, true);
                this._write(buffer, 0, buffer.Length);
            }
            return true;
        }

        public override void Flush()
        {
            if (!this._disposed && (this._sendChunked || this._response.SendChunked))
            {
                this.flush(false);
            }
        }

        private void flushBody(bool closing)
        {
            using (this._body)
            {
                long length = this._body.Length;
                if (length <= 0x7fffffffL)
                {
                    if (length > 0L)
                    {
                        this._writeBody(this._body.GetBuffer(), 0, (int) length);
                    }
                }
                else
                {
                    this._body.Position = 0L;
                    int count = 0x400;
                    byte[] buffer = new byte[count];
                    int num3 = 0;
                    while ((num3 = this._body.Read(buffer, 0, count)) > 0)
                    {
                        this._writeBody(buffer, 0, num3);
                    }
                }
            }
            this._body = closing ? null : new MemoryStream();
        }

        private bool flushHeaders(bool closing)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bool flag;
                WebHeaderCollection headers = this._response.WriteHeadersTo(stream);
                long position = stream.Position;
                long num2 = stream.Length - position;
                if (num2 <= 0x8000L)
                {
                    if (this._response.SendChunked || (this._response.ContentLength64 == this._body.Length))
                    {
                        this._write(stream.GetBuffer(), (int) position, (int) num2);
                        this._response.CloseConnection = headers["Connection"] == "close";
                        this._response.HeadersSent = true;
                        goto TR_0006;
                    }
                    else
                    {
                        flag = false;
                    }
                }
                else
                {
                    flag = false;
                }
                return flag;
            }
        TR_0006:
            return true;
        }

        private static byte[] getChunkSizeBytes(int size, bool final) => 
            Encoding.ASCII.GetBytes($"{size:x}
{!final ? string.Empty : "\r\n"}");

        internal void InternalWrite(byte[] buffer, int offset, int count)
        {
            this._write(buffer, offset, count);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
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
            if (this._disposed)
            {
                throw new ObjectDisposedException(base.GetType().ToString());
            }
            this._body.Write(buffer, offset, count);
        }

        private void writeChunked(byte[] buffer, int offset, int count)
        {
            byte[] buffer2 = getChunkSizeBytes(count, false);
            this._stream.Write(buffer2, 0, buffer2.Length);
            this._stream.Write(buffer, offset, count);
            this._stream.Write(_crlf, 0, 2);
        }

        private void writeChunkedWithoutThrowingException(byte[] buffer, int offset, int count)
        {
            try
            {
                this.writeChunked(buffer, offset, count);
            }
            catch
            {
            }
        }

        private void writeWithoutThrowingException(byte[] buffer, int offset, int count)
        {
            try
            {
                this._stream.Write(buffer, offset, count);
            }
            catch
            {
            }
        }

        public override bool CanRead =>
            false;

        public override bool CanSeek =>
            false;

        public override bool CanWrite =>
            !this._disposed;

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

