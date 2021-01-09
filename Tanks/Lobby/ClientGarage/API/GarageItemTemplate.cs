namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x8d32d680967a30bL)]
    public interface GarageItemTemplate : Template
    {
        [AutoAdded, PersistentConfig("default", true)]
        DefaultItemComponent defaultItem();
        [AutoAdded, PersistentConfig("", false)]
        DescriptionItemComponent descriptionItem();
        [AutoAdded]
        GarageItemComponent garageItem();
        [AutoAdded, PersistentConfig("order", false)]
        OrderItemComponent OrderItem();
    }
}

