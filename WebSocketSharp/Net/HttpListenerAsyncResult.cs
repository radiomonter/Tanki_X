namespace WebSocketSharp.Net
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Threading;

    internal class HttpListenerAsyncResult : IAsyncResult
    {
        private AsyncCallback _callback;
        private bool _completed;
        private HttpListenerContext _context;
        private bool _endCalled;
        private Exception _exception;
        private bool _inGet;
        private object _state;
        private object _sync;
        private bool _syncCompleted;
        private ManualResetEvent _waitHandle;

        internal HttpListenerAsyncResult(AsyncCallback callback, object state)
        {
            this._callback = callback;
            this._state = state;
            this._sync = new object();
        }

        private static void complete(HttpListenerAsyncResult asyncResult)
        {
            <complete>c__AnonStorey0 storey = new <complete>c__AnonStorey0 {
                asyncResult = asyncResult
            };
            lock (storey.asyncResult._sync)
            {
                storey.asyncResult._completed = true;
                ManualResetEvent event2 = storey.asyncResult._waitHandle;
                if (event2 != null)
                {
                    event2.Set();
                }
            }
            storey.callback = storey.asyncResult._callback;
            if (storey.callback != null)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(storey.<>m__0), null);
            }
        }

        internal void Complete(Exception exception)
        {
            this._exception = (!this._inGet || !(exception is ObjectDisposedException)) ? exception : new HttpListenerException(0x3e3, "The listener is closed.");
            complete(this);
        }

        internal void Complete(HttpListenerContext context)
        {
            this.Complete(context, false);
        }

        internal void Complete(HttpListenerContext context, bool syncCompleted)
        {
            this._context = context;
            this._syncCompleted = syncCompleted;
            complete(this);
        }

        internal HttpListenerContext GetContext()
        {
            if (this._exception != null)
            {
                throw this._exception;
            }
            return this._context;
        }

        internal bool EndCalled
        {
            get => 
                this._endCalled;
            set => 
                this._endCalled = value;
        }

        internal bool InGet
        {
            get => 
                this._inGet;
            set => 
                this._inGet = value;
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
            this._syncCompleted;

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

        [CompilerGenerated]
        private sealed class <complete>c__AnonStorey0
        {
            internal AsyncCallback callback;
            internal HttpListenerAsyncResult asyncResult;

            internal void <>m__0(object state)
            {
                try
                {
                    this.callback(this.asyncResult);
                }
                catch
                {
                }
            }
        }
    }
}

