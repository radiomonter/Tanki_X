namespace Platform.Library.ClientDataStructures.API
{
    using System;

    public interface CacheService
    {
        void Dispose();
        Cache<T> GetCache<T>();
        Cache<T> GetCache<T>(T o);
        Cache<T> RegisterTypeCache<T>();
        Cache<T> RegisterTypeCache<T>(Action<T> cleaner);
    }
}

