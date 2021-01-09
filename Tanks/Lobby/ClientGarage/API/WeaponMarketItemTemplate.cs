namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientGarage.Impl;

    [SerialVersionUID(0x8d333b0e9391ed5L)]
    public interface WeaponMarketItemTemplate : MarketItemTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        CrystalsPurchaseUserRankRestrictionComponent crystalsPurchaseUserRankRestriction();
    }
}

