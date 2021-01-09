namespace Platform.Library.ClientResources.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class AsyncLoadingAssetListComponent : Component
    {
        public AsyncLoadingAssetListComponent(List<LoadAssetFromBundleRequest> assetListRequest)
        {
            this.AssetListRequest = assetListRequest;
        }

        public List<LoadAssetFromBundleRequest> AssetListRequest { get; set; }
    }
}

