namespace Platform.Library.ClientResources.API
{
    using System;

    public class AssetNotFoundException : Exception
    {
        public AssetNotFoundException(string guid) : base("GUID=" + guid)
        {
        }
    }
}

