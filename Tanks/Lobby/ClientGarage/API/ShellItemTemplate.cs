namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x63cecd9ff98a89bcL)]
    public interface ShellItemTemplate : GarageItemImagedTemplate, GarageItemTemplate, ItemImagedTemplate, Template
    {
        [AutoAdded]
        MountableItemComponent mountableItem();
        [AutoAdded]
        ShellItemComponent shellItem();
    }
}

