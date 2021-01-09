namespace Platform.Library.ClientResources.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientResources.Impl;
    using System;
    using System.Runtime.CompilerServices;

    public class AssetBundleDiskCacheComponent : Component
    {
        public Platform.Library.ClientResources.Impl.AssetBundleDiskCache AssetBundleDiskCache { get; set; }
    }
}

