namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class BonusRegionAssetComponent : Component
    {
        public BonusRegionAssetComponent()
        {
        }

        public BonusRegionAssetComponent(string assetGuid)
        {
            this.AssetGuid = assetGuid;
        }

        public string AssetGuid { get; set; }
    }
}

