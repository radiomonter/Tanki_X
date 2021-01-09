namespace Platform.Library.ClientDataStructures.API
{
    using System;

    public interface AbstractCache
    {
        void Dispose();
        void FreeAll();
    }
}

