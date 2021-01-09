namespace Platform.Library.ClientDataStructures.Impl.Cache
{
    using System;

    public class CacheForTypeNotFoundException : Exception
    {
        public CacheForTypeNotFoundException(Type type) : base("Type: " + type)
        {
        }
    }
}

