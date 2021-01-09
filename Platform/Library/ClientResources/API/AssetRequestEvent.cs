namespace Platform.Library.ClientResources.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class AssetRequestEvent : Event
    {
        public AssetRequestEvent()
        {
        }

        public AssetRequestEvent(AssetReference reference)
        {
            this.AssetGuid = reference.AssetGuid;
        }

        public AssetRequestEvent Init<T>(string assetGuid) where T: ResourceDataComponent
        {
            this.AssetGuid = assetGuid;
            this.ResourceDataComponentType = typeof(T);
            this.StoreLevel = AssetStoreLevel.MANAGED;
            return this;
        }

        public AssetRequestEvent Init<T>(string assetGuid, int priority, AssetStoreLevel level) where T: ResourceDataComponent
        {
            this.AssetGuid = assetGuid;
            this.ResourceDataComponentType = typeof(T);
            this.Priority = priority;
            this.StoreLevel = level;
            return this;
        }

        public string AssetGuid { get; set; }

        public Type ResourceDataComponentType { get; set; }

        public int Priority { get; set; }

        public AssetStoreLevel StoreLevel { get; set; }

        public Entity LoaderEntity { get; set; }
    }
}

