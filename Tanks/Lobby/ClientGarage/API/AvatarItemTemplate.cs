namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x167a6f32a25L)]
    public interface AvatarItemTemplate : GarageItemImagedTemplate, GarageItemTemplate, ItemImagedTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        AvatarItemComponent avatarItem();
        [AutoAdded]
        MountableItemComponent mountableItem();
    }
}

