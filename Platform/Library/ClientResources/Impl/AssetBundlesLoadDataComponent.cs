namespace Platform.Library.ClientResources.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class AssetBundlesLoadDataComponent : Component
    {
        public override string ToString() => 
            $"AssetBundlesLoadDataComponent Loaded: {this.Loaded},

LoadingBundles: {EcsToStringUtil.EnumerableToString(this.LoadingBundles, "\n")},

LoadedBundles: {EcsToStringUtil.EnumerableToString(this.LoadedBundles, "\n")}";

        public bool Loaded { get; set; }

        public HashSet<AssetBundleInfo> AllBundles { get; set; }

        public List<AssetBundleInfo> BundlesToLoad { get; set; }

        public HashSet<AssetBundleInfo> LoadingBundles { get; set; }

        public Dictionary<AssetBundleInfo, AssetBundle> LoadedBundles { get; set; }
    }
}

