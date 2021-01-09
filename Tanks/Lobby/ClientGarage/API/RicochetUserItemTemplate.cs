namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x14e24e85f08L)]
    public interface RicochetUserItemTemplate : RicochetItemTemplate, UpgradableUserItemTemplate, WeaponItemTemplate, GarageItemTemplate, Template, UserItemTemplate
    {
    }
}

