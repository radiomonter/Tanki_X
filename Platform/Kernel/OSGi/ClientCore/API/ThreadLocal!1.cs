namespace Platform.Kernel.OSGi.ClientCore.API
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    public class ThreadLocal<TValue>
    {
        private object Lock;
        private Dictionary<long, TValue> map;

        public ThreadLocal()
        {
            this.Lock = new object();
        }

        public bool Exists()
        {
            lock (this.Lock)
            {
                return ((this.map != null) && this.map.ContainsKey((long) Thread.CurrentThread.ManagedThreadId));
            }
        }

        public TValue Get()
        {
            lock (this.Lock)
            {
                if (this.map == null)
                {
                    throw new ArgumentException();
                }
                return this.map[(long) Thread.CurrentThread.ManagedThreadId];
            }
        }

        public void Set(TValue value)
        {
            lock (this.Lock)
            {
                this.map ??= new Dictionary<long, TValue>();
                this.map[(long) Thread.CurrentThread.ManagedThreadId] = value;
            }
        }
    }
}

