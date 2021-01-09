namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientGarage.Impl;

    [SerialVersionUID(0x14dbe6603acL)]
    public interface WeaponItemTemplate : GarageItemTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        ItemIconComponent itemIcon();
        [AutoAdded]
        MountableItemComponent mountableItem();
        [AutoAdded, PersistentConfig("", true)]
        VisualPropertiesComponent visualProperties();
        [AutoAdded]
        WeaponItemComponent weaponItem();
    }
}

