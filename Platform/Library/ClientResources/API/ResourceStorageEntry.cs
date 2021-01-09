namespace Platform.Library.ClientResources.API
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class ResourceStorageEntry
    {
        public float LastAccessTime { get; set; }

        public Object Asset { get; set; }
    }
}

