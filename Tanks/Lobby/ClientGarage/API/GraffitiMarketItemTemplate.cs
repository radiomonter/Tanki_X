namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x8d3e24f1adf7fdbL)]
    public interface GraffitiMarketItemTemplate : GraffitiItemTemplate, MarketItemTemplate, GarageItemImagedTemplate, GarageItemTemplate, ItemImagedTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        MountUpgradeLevelRestrictionComponent mountUpgradeLevelRestriction();
        [AutoAdded, PersistentConfig("", false)]
        PurchaseUpgradeLevelRestrictionComponent purchaseUpgradeLevelRestriction();
        [AutoAdded, PersistentConfig("", false)]
        PurchaseUserRankRestrictionComponent purchaseUserRankRestriction();
    }
}

