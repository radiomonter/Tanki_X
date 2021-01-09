namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Battle.ClientCore.Impl;

    [SerialVersionUID(0x70a2155124a5202dL)]
    public interface BonusRegionTemplate : Template
    {
        BonusRegionComponent BonusRegion();
        BonusRegionInstanceComponent BonusRegionInstance();
        GoldBonusRegionComponent GoldBonusRegion();
        [AutoAdded]
        OpacityBonusRegionComponent OpacityBonusRegion();
        SpatialGeometryComponent SpatialGeometry();
        SupplyBonusRegionComponent SupplyBonusRegion();
    }
}

