namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientGarage.Impl;

    [SerialVersionUID(0x15bd7b5a16aL)]
    public interface PresetMarketItemTemplate : PresetItemTemplate, MarketItemTemplate, GarageItemTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        CreateByRankConfigComponent createByRankConfig();
        [AutoAdded, PersistentConfig("", false)]
        FirstBuySaleComponent firstBuySale();
        [AutoAdded, PersistentConfig("", false)]
        ItemAutoIncreasePriceComponent itemsAutoIncreasePrice();
        [AutoAdded, PersistentConfig("", false)]
        ItemsBuyCountLimitComponent itemsBuyCountLimit();
    }
}

