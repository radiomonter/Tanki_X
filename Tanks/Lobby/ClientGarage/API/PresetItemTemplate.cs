namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x15bd7e7894cL)]
    public interface PresetItemTemplate : GarageItemTemplate, Template
    {
        [AutoAdded]
        PresetItemComponent presetItem();
    }
}

