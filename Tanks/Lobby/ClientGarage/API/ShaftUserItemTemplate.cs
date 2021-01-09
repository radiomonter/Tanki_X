namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x14e24ea5adaL)]
    public interface ShaftUserItemTemplate : ShaftItemTemplate, UpgradableUserItemTemplate, WeaponItemTemplate, GarageItemTemplate, Template, UserItemTemplate
    {
    }
}

