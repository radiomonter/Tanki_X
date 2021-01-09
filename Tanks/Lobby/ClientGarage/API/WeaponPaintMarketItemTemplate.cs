namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x8d48bcb93910c4cL)]
    public interface WeaponPaintMarketItemTemplate : WeaponPaintItemTemplate, MarketItemTemplate, PaintItemTemplate, GarageItemImagedTemplate, GarageItemTemplate, ItemImagedTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        PurchaseUpgradeLevelRestrictionComponent purchaseUpgradeLevelRestriction();
        [AutoAdded, PersistentConfig("", false)]
        PurchaseUserRankRestrictionComponent purchaseUserRankRestriction();
    }
}

