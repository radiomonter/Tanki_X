namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x14ebe77a0d1L)]
    public interface HammerItemTemplate : WeaponItemTemplate, GarageItemTemplate, Template
    {
    }
}

