namespace Platform.Library.ClientDataStructures.API
{
    using System;

    public interface CacheMultisizeArray<T> : AbstractCache
    {
        void Dispose();
        void Dispose(int length);
        void Free(T[] item);
        void FreeAll(int length);
        T[] GetInstanceArray(int length);
    }
}

