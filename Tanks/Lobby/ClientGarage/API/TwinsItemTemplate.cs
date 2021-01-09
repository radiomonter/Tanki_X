namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x14ebe6a7be8L)]
    public interface TwinsItemTemplate : WeaponItemTemplate, GarageItemTemplate, Template
    {
    }
}

