namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x155b5b50330L)]
    public interface SkinMarketItemTemplate : SkinItemTemplate, MarketItemTemplate, GarageItemImagedTemplate, GarageItemTemplate, ItemImagedTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        MountUpgradeLevelRestrictionComponent mountUpgradeLevelRestriction();
        [AutoAdded, PersistentConfig("", false)]
        MountUserRankRestrictionComponent mountUserRankRestriction();
        [AutoAdded, PersistentConfig("", false)]
        PurchaseUpgradeLevelRestrictionComponent purchaseUpgradeLevelRestriction();
        [AutoAdded, PersistentConfig("", false)]
        PurchaseUserRankRestrictionComponent purchaseUserRankRestriction();
    }
}

