namespace Platform.Library.ClientResources.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class LoadAssetBundlesRequestComponent : Component
    {
        public int LoadingPriority { get; set; }
    }
}

