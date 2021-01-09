namespace Platform.Library.ClientResources.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientResources.API;
    using System;
    using System.Runtime.CompilerServices;

    public class AsyncLoadingAssetComponent : Component
    {
        public LoadAssetFromBundleRequest Request { get; set; }
    }
}

