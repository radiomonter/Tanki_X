namespace Platform.Library.ClientDataStructures.Impl.Cache
{
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Collections.Generic;

    public class CacheServiceImpl : CacheService
    {
        private readonly Dictionary<Type, object> caches = new Dictionary<Type, object>();

        public void Dispose()
        {
            Dictionary<Type, object>.Enumerator enumerator = this.caches.GetEnumerator();
            while (enumerator.MoveNext())
            {
                KeyValuePair<Type, object> current = enumerator.Current;
                ((AbstractCache) current.Value).Dispose();
            }
        }

        public Cache<T> GetCache<T>()
        {
            object obj2;
            if (!this.caches.TryGetValue(typeof(T), out obj2))
            {
                throw new CacheForTypeNotFoundException(typeof(T));
            }
            return (Cache<T>) obj2;
        }

        public Cache<T> GetCache<T>(T o) => 
            this.GetCache<T>();

        public Cache<T> RegisterTypeCache<T>() => 
            this.RegisterTypeCache<T>(null);

        public Cache<T> RegisterTypeCache<T>(Action<T> cleaner)
        {
            Cache<T> cache = new CacheImpl<T>(cleaner, 0);
            this.caches.Add(typeof(T), cache);
            return cache;
        }
    }
}

