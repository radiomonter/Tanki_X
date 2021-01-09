namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x1588b66da1fL)]
    public interface ContainerMarketItemTemplate : ContainerItemTemplate, MarketItemTemplate, GarageItemImagedTemplate, GarageItemTemplate, ItemImagedTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        ItemPacksComponent itemPacks();
        [AutoAdded, PersistentConfig("", false)]
        ItemsContainerItemComponent itemsContainerItem();
    }
}

