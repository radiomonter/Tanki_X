namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientGarage.Impl;

    [SerialVersionUID(0x14dfb613e06L)]
    public interface TankItemTemplate : GarageItemTemplate, Template
    {
        [AutoAdded]
        MountableItemComponent mountableItem();
        [AutoAdded]
        TankItemComponent tankItem();
        [AutoAdded, PersistentConfig("", true)]
        VisualPropertiesComponent visualProperties();
    }
}

