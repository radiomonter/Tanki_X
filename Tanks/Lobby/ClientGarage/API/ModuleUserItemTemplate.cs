namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x159bb05ff4cL)]
    public interface ModuleUserItemTemplate : ModuleItemTemplate, UserItemTemplate, GarageItemTemplate, Template
    {
    }
}

