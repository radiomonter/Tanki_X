namespace Platform.Library.ClientDataStructures.Impl.Cache
{
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class CacheMultisizeArrayImpl<T> : CacheMultisizeArray<T>, AbstractCache
    {
        private readonly Dictionary<int, CacheImpl<T[]>> elementsCountToArrayCache;
        private readonly Action<T[]> cleaner;
        [CompilerGenerated]
        private static Action<KeyValuePair<int, CacheImpl<T[]>>> <>f__am$cache0;

        public CacheMultisizeArrayImpl()
        {
            this.elementsCountToArrayCache = new Dictionary<int, CacheImpl<T[]>>();
        }

        public CacheMultisizeArrayImpl(Action<T[]> cleaner)
        {
            this.elementsCountToArrayCache = new Dictionary<int, CacheImpl<T[]>>();
            this.cleaner = cleaner;
        }

        public void Dispose()
        {
            this.elementsCountToArrayCache.Clear();
        }

        public void Dispose(int length)
        {
            this.elementsCountToArrayCache.Remove(length);
        }

        public void Free(T[] item)
        {
            CacheImpl<T[]> impl;
            int length = item.Length;
            if (this.elementsCountToArrayCache.TryGetValue(length, out impl))
            {
                impl.Free(item);
            }
        }

        public void FreeAll()
        {
            if (CacheMultisizeArrayImpl<T>.<>f__am$cache0 == null)
            {
                CacheMultisizeArrayImpl<T>.<>f__am$cache0 = kv => kv.Value.FreeAll();
            }
            this.elementsCountToArrayCache.ForEach<KeyValuePair<int, CacheImpl<T[]>>>(CacheMultisizeArrayImpl<T>.<>f__am$cache0);
        }

        public void FreeAll(int length)
        {
            CacheImpl<T[]> impl;
            if (this.elementsCountToArrayCache.TryGetValue(length, out impl))
            {
                impl.FreeAll();
            }
        }

        public void FreeObjectInstance(object item)
        {
            throw new NotImplementedException();
        }

        public int GetArrayInstancesCount(int length)
        {
            CacheImpl<T[]> impl;
            if (!this.elementsCountToArrayCache.TryGetValue(length, out impl))
            {
                throw new NoCachesForArrayWithLengthException<T>(length);
            }
            return impl.InstanceCount;
        }

        public int GetCapacityInCache(int length)
        {
            CacheImpl<T[]> impl;
            if (!this.elementsCountToArrayCache.TryGetValue(length, out impl))
            {
                throw new NoCachesForArrayWithLengthException<T>(length);
            }
            return impl.Capacity;
        }

        public int GetFreeCountInCache(int length)
        {
            CacheImpl<T[]> impl;
            if (!this.elementsCountToArrayCache.TryGetValue(length, out impl))
            {
                throw new NoCachesForArrayWithLengthException<T>(length);
            }
            return impl.FreeCount;
        }

        public T[] GetInstanceArray(int length)
        {
            CacheImpl<T[]> impl;
            if (!this.elementsCountToArrayCache.TryGetValue(length, out impl))
            {
                impl = new CacheImpl<T[]>(this.cleaner, length);
                this.elementsCountToArrayCache.Add(length, impl);
            }
            return impl.GetInstance();
        }

        public object GetObjectInstance()
        {
            throw new NotImplementedException();
        }

        public int CacheArrayCount =>
            this.elementsCountToArrayCache.Count;

        public class NoCachesForArrayWithLengthException : Exception
        {
            private const string EXCEPTION_FORMAT = "Length = {0}";

            public NoCachesForArrayWithLengthException(int length) : base($"Length = {length}")
            {
            }
        }
    }
}

