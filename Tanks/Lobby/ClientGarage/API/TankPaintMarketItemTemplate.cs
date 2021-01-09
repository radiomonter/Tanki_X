namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x14e72b1e97cL)]
    public interface TankPaintMarketItemTemplate : TankPaintItemTemplate, MarketItemTemplate, PaintItemTemplate, GarageItemImagedTemplate, GarageItemTemplate, ItemImagedTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        PurchaseUpgradeLevelRestrictionComponent purchaseUpgradeLevelRestriction();
        [AutoAdded, PersistentConfig("", false)]
        PurchaseUserRankRestrictionComponent purchaseUserRankRestriction();
    }
}

