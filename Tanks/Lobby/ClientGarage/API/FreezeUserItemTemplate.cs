namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x14dbdb411d7L)]
    public interface FreezeUserItemTemplate : FreezeItemTemplete, UpgradableUserItemTemplate, WeaponItemTemplate, GarageItemTemplate, Template, UserItemTemplate
    {
    }
}

