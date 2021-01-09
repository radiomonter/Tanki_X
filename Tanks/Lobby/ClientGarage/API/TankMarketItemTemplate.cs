namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientGarage.Impl;

    [SerialVersionUID(0x14dbdb2f970L)]
    public interface TankMarketItemTemplate : TankItemTemplate, MarketItemTemplate, GarageItemTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        CrystalsPurchaseUserRankRestrictionComponent crystalsPurchaseUserRankRestriction();
    }
}

