namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x15a36ef9590L)]
    public interface PassiveModuleUserItemTemplate : ModuleUserItemTemplate, ModuleItemTemplate, UserItemTemplate, GarageItemTemplate, Template
    {
        [AutoAdded]
        PassiveModuleComponent passiveModule();
    }
}

