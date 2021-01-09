namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x14e24ee718dL)]
    public interface TwinsUserItemTemplate : TwinsItemTemplate, UpgradableUserItemTemplate, WeaponItemTemplate, GarageItemTemplate, Template, UserItemTemplate
    {
    }
}

