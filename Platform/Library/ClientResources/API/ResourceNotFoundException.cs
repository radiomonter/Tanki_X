namespace Platform.Library.ClientResources.API
{
    using System;

    public class ResourceNotFoundException : Exception
    {
        public ResourceNotFoundException(string url) : base("Resource not found: " + url)
        {
        }
    }
}

