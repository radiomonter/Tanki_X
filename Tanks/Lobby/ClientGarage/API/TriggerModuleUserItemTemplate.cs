namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x8d49b7225b43fe4L)]
    public interface TriggerModuleUserItemTemplate : PassiveModuleUserItemTemplate, ModuleUserItemTemplate, ModuleItemTemplate, UserItemTemplate, GarageItemTemplate, Template
    {
    }
}

