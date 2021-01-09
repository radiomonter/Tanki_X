namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class BonusRegionAssetsComponent : Component
    {
        public string DoubleDamageAssetGuid { get; set; }

        public string DoubleArmorAssetGuid { get; set; }

        public string RepairKitAssetGuid { get; set; }

        public string SpeedBoostAssetGuid { get; set; }

        public string GoldAssetGuid { get; set; }
    }
}

