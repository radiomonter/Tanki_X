namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x14ebe6c15d4L)]
    public interface ShaftItemTemplate : WeaponItemTemplate, GarageItemTemplate, Template
    {
    }
}

