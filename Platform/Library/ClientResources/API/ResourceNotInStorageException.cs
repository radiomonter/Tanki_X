namespace Platform.Library.ClientResources.API
{
    using System;

    public class ResourceNotInStorageException : Exception
    {
        public ResourceNotInStorageException(string assetGuid) : base("Guid: " + assetGuid)
        {
        }
    }
}

