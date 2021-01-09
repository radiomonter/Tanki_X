namespace Platform.Library.ClientDataStructures.API
{
    using System;

    public interface Cache<T> : AbstractCache
    {
        void Free(T item);
        T GetInstance();
        void SetMaxSize(int maxSize);
    }
}

