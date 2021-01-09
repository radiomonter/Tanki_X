namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x8d4ea3b753fd0abL)]
    public interface SlotMarketItemTemplate : Template
    {
        [AutoAdded]
        MarketItemComponent marketItem();
        [AutoAdded]
        SlotItemComponent slotItem();
    }
}

