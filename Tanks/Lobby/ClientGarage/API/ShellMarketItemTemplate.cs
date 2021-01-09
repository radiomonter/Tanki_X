namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x9f0633b75b09474L)]
    public interface ShellMarketItemTemplate : ShellItemTemplate, MarketItemTemplate, GarageItemImagedTemplate, GarageItemTemplate, ItemImagedTemplate, Template
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

