namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x14ebe786de6L)]
    public interface FlamethrowerItemTemplate : WeaponItemTemplate, GarageItemTemplate, Template
    {
    }
}

