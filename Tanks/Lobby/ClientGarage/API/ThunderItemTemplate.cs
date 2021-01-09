namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x14ebe6b6496L)]
    public interface ThunderItemTemplate : WeaponItemTemplate, GarageItemTemplate, Template
    {
    }
}

