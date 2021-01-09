namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x14dd2e745fdL)]
    public interface MarketItemTemplate : Template
    {
        [AutoAdded, PersistentConfig("", true)]
        ItemRarityComponent itemRarity();
        [AutoAdded]
        MarketItemComponent marketItem();
        [AutoAdded, PersistentConfig("", true)]
        PriceItemComponent priceItem();
        [AutoAdded, PersistentConfig("", true)]
        XPriceItemComponent xPriceItem();
    }
}

