namespace Platform.Library.ClientResources.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientResources.API;
    using System;
    using System.Runtime.CompilerServices;

    public class AssetBundleLoadingComponent : Component
    {
        public AssetBundleLoadingComponent()
        {
        }

        public AssetBundleLoadingComponent(AssetBundleInfo info)
        {
            this.Info = info;
        }

        public AssetBundleInfo Info { get; set; }

        public float StartTime { get; set; }

        public Platform.Library.ClientResources.API.AssetBundleDiskCacheRequest AssetBundleDiskCacheRequest { get; set; }

        public float Progress { get; set; }
    }
}

