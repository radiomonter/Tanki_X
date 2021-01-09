namespace Platform.Library.ClientResources.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class AssetBundleLoadingChannelsCountComponent : Component
    {
        public int ChannelsCount { get; set; }

        public int DefaultChannelsCount { get; set; }
    }
}

