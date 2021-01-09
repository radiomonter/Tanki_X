namespace Platform.Library.ClientResources.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class AssetGroupComponent : GroupComponent
    {
        public AssetGroupComponent(Entity keyEntity) : base(keyEntity)
        {
        }

        public AssetGroupComponent(long key) : base(key)
        {
        }
    }
}

