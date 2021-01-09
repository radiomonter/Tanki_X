namespace Platform.Library.ClientDataStructures.Impl.Cache
{
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class CacheImpl<T> : Cache<T>, AbstractCache
    {
        private static readonly int DEFAULT_MAX_SIZE;
        private readonly Stack<T> cache;
        private readonly IList<T> inuse;
        private readonly Action<T> cleaner;
        private readonly int elementsCount;
        private int maxSize;

        static CacheImpl()
        {
            CacheImpl<T>.DEFAULT_MAX_SIZE = 100;
        }

        public CacheImpl()
        {
            this.cache = new Stack<T>();
            this.inuse = new List<T>();
            this.maxSize = CacheImpl<T>.DEFAULT_MAX_SIZE;
        }

        public CacheImpl(Action<T> cleaner)
        {
            this.cache = new Stack<T>();
            this.inuse = new List<T>();
            this.maxSize = CacheImpl<T>.DEFAULT_MAX_SIZE;
            this.cleaner = cleaner;
        }

        public CacheImpl(Action<T> cleaner, int elementsCount)
        {
            this.cache = new Stack<T>();
            this.inuse = new List<T>();
            this.maxSize = CacheImpl<T>.DEFAULT_MAX_SIZE;
            this.cleaner = cleaner;
            this.elementsCount = elementsCount;
        }

        public void Dispose()
        {
            this.cache.Clear();
        }

        public void Free(T o)
        {
            if (this.cleaner != null)
            {
                this.cleaner(o);
            }
            if (!this.ReturnToCache(o))
            {
                this.OverflowCount++;
            }
            this.FreeCount++;
        }

        public void FreeAll()
        {
            int count = this.inuse.Count;
            for (int i = 0; i < count; i++)
            {
                this.Free(this.inuse[i]);
            }
            this.inuse.Clear();
        }

        public T GetInstance()
        {
            this.UseCount++;
            T item = (this.cache.Count == 0) ? this.NewInstance() : this.cache.Pop();
            if (this.inuse.Count <= this.maxSize)
            {
                this.inuse.Add(item);
            }
            else
            {
                this.OverflowCount++;
            }
            return item;
        }

        protected T NewInstance()
        {
            Type type = typeof(T);
            object obj2 = !type.IsArray ? ((object) Activator.CreateInstance<T>()) : ((object) Array.CreateInstance(type.GetElementType(), this.elementsCount));
            this.InstanceCount++;
            return (T) obj2;
        }

        private bool ReturnToCache(T o)
        {
            if (this.cache.Count > this.maxSize)
            {
                return false;
            }
            this.cache.Push(o);
            return true;
        }

        public void SetMaxSize(int maxSize)
        {
            this.maxSize = maxSize;
        }

        public int InstanceCount { get; private set; }

        public int UseCount { get; private set; }

        public int FreeCount { get; private set; }

        public int OverflowCount { get; private set; }

        public int Capacity =>
            this.cache.Count;
    }
}

