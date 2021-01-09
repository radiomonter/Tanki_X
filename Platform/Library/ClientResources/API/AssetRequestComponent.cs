namespace Platform.Library.ClientResources.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class AssetRequestComponent : Component
    {
        public AssetRequestComponent()
        {
            this.Priority = 0;
            this.AssetStoreLevel = Platform.Library.ClientResources.API.AssetStoreLevel.MANAGED;
        }

        public AssetRequestComponent(int priority)
        {
            this.Priority = priority;
        }

        public int Priority { get; set; }

        public Platform.Library.ClientResources.API.AssetStoreLevel AssetStoreLevel { get; set; }
    }
}

