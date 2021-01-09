namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientGarage.Impl;

    [SerialVersionUID(0x15bd7b61524L)]
    public interface PresetUserItemTemplate : PresetItemTemplate, UserItemTemplate, GarageItemTemplate, Template
    {
        PresetNameComponent presetName();
    }
}

