namespace WebSocketSharp.Net
{
    using System;
    using System.IO;

    internal class ChunkedRequestStream : RequestStream
    {
        private const int _bufferLength = 0x2000;
        private HttpListenerContext _context;
        private ChunkStream _decoder;
        private bool _disposed;
        private bool _noMoreData;

        internal ChunkedRequestStream(Stream stream, byte[] buffer, int offset, int count, HttpListenerContext context) : base(stream, buffer, offset, count)
        {
            this._context = context;
            this._decoder = new ChunkStream((WebHeaderCollection) context.Request.Headers);
        }

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            if (this._disposed)
            {
                throw new ObjectDisposedException(base.GetType().ToString());
            }
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
            HttpStreamAsyncResult asyncResult = new HttpStreamAsyncResult(callback, state);
            if (this._noMoreData)
            {
                asyncResult.Complete();
                return asyncResult;
            }
            int num2 = this._decoder.Read(buffer, offset, count);
            offset += num2;
            count -= num2;
            if (count == 0)
            {
                asyncResult.Count = num2;
                asyncResult.Complete();
                return asyncResult;
            }
            if (!this._decoder.WantMore)
            {
                this._noMoreData = num2 == 0;
                asyncResult.Count = num2;
                asyncResult.Complete();
                return asyncResult;
            }
            asyncResult.Buffer = new byte[0x2000];
            asyncResult.Offset = 0;
            asyncResult.Count = 0x2000;
            ReadBufferState state2 = new ReadBufferState(buffer, offset, count, asyncResult);
            state2.InitialCount += num2;
            base.BeginRead(asyncResult.Buffer, asyncResult.Offset, asyncResult.Count, new AsyncCallback(this.onRead), state2);
            return asyncResult;
        }

        public override void Close()
        {
            if (!this._disposed)
            {
                this._disposed = true;
                base.Close();
            }
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
            HttpStreamAsyncResult result = asyncResult as HttpStreamAsyncResult;
            if (result == null)
            {
                throw new ArgumentException("A wrong IAsyncResult.", "asyncResult");
            }
            if (!result.IsCompleted)
            {
                result.AsyncWaitHandle.WaitOne();
            }
            if (result.HasException)
            {
                throw new HttpListenerException(400, "I/O operation aborted.");
            }
            return result.Count;
        }

        private void onRead(IAsyncResult asyncResult)
        {
            ReadBufferState asyncState = (ReadBufferState) asyncResult.AsyncState;
            HttpStreamAsyncResult result = asyncState.AsyncResult;
            try
            {
                int count = base.EndRead(asyncResult);
                this._decoder.Write(result.Buffer, result.Offset, count);
                count = this._decoder.Read(asyncState.Buffer, asyncState.Offset, asyncState.Count);
                asyncState.Offset += count;
                asyncState.Count -= count;
                if ((asyncState.Count != 0) && (this._decoder.WantMore && (count != 0)))
                {
                    result.Offset = 0;
                    result.Count = Math.Min(0x2000, this._decoder.ChunkLeft + 6);
                    base.BeginRead(result.Buffer, result.Offset, result.Count, new AsyncCallback(this.onRead), asyncState);
                }
                else
                {
                    this._noMoreData = !this._decoder.WantMore && (count == 0);
                    result.Count = asyncState.InitialCount - asyncState.Count;
                    result.Complete();
                }
            }
            catch (Exception exception)
            {
                this._context.Connection.SendError(exception.Message, 400);
                result.Complete(exception);
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            IAsyncResult asyncResult = this.BeginRead(buffer, offset, count, null, null);
            return this.EndRead(asyncResult);
        }

        internal ChunkStream Decoder
        {
            get => 
                this._decoder;
            set => 
                this._decoder = value;
        }
    }
}

