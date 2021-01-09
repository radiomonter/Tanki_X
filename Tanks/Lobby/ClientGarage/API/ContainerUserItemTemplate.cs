namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x1588b67a1b8L)]
    public interface ContainerUserItemTemplate : ContainerItemTemplate, UserItemTemplate, GarageItemImagedTemplate, GarageItemTemplate, ItemImagedTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        ItemsContainerItemComponent itemsContainerItem();
    }
}

