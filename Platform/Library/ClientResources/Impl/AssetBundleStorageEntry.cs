namespace Platform.Library.ClientResources.Impl
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class AssetBundleStorageEntry
    {
        public float LastAccessTime { get; set; }

        public AssetBundle Bundle { get; set; }

        public AssetBundleInfo Info { get; set; }
    }
}

