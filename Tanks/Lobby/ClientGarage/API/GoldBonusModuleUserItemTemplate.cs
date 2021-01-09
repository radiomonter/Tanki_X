namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x164ae2333dfL)]
    public interface GoldBonusModuleUserItemTemplate : ModuleUserItemTemplate, ModuleItemTemplate, UserItemTemplate, GarageItemTemplate, Template
    {
        [AutoAdded]
        GoldBonusModuleItemComponent goldBonusModuleItem();
    }
}

