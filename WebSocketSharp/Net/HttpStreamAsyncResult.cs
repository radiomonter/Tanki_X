namespace WebSocketSharp.Net
{
    using System;
    using System.Threading;

    internal class HttpStreamAsyncResult : IAsyncResult
    {
        private byte[] _buffer;
        private AsyncCallback _callback;
        private bool _completed;
        private int _count;
        private System.Exception _exception;
        private int _offset;
        private object _state;
        private object _sync;
        private int _syncRead;
        private ManualResetEvent _waitHandle;

        internal HttpStreamAsyncResult(AsyncCallback callback, object state)
        {
            this._callback = callback;
            this._state = state;
            this._sync = new object();
        }

        internal void Complete()
        {
            lock (this._sync)
            {
                if (!this._completed)
                {
                    this._completed = true;
                    if (this._waitHandle != null)
                    {
                        this._waitHandle.Set();
                    }
                    if (this._callback != null)
                    {
                        this._callback.BeginInvoke(this, ar => this._callback.EndInvoke(ar), null);
                    }
                }
            }
        }

        internal void Complete(System.Exception exception)
        {
            this._exception = exception;
            this.Complete();
        }

        internal byte[] Buffer
        {
            get => 
                this._buffer;
            set => 
                this._buffer = value;
        }

        internal int Count
        {
            get => 
                this._count;
            set => 
                this._count = value;
        }

        internal System.Exception Exception =>
            this._exception;

        internal bool HasException =>
            !ReferenceEquals(this._exception, null);

        internal int Offset
        {
            get => 
                this._offset;
            set => 
                this._offset = value;
        }

        internal int SyncRead
        {
            get => 
                this._syncRead;
            set => 
                this._syncRead = value;
        }

        public object AsyncState =>
            this._state;

        public WaitHandle AsyncWaitHandle
        {
            get
            {
                lock (this._sync)
                {
                    ManualResetEvent event3 = this._waitHandle;
                    if (this._waitHandle == null)
                    {
                        ManualResetEvent local1 = this._waitHandle;
                        event3 = this._waitHandle = new ManualResetEvent(this._completed);
                    }
                    return event3;
                }
            }
        }

        public bool CompletedSynchronously =>
            this._syncRead == this._count;

        public bool IsCompleted
        {
            get
            {
                lock (this._sync)
                {
                    return this._completed;
                }
            }
        }
    }
}

