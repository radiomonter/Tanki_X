namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x14e52ea5c98L)]
    public interface IsisUserItemTemplate : IsisItemTemplate, UpgradableUserItemTemplate, WeaponItemTemplate, GarageItemTemplate, Template, UserItemTemplate
    {
    }
}

