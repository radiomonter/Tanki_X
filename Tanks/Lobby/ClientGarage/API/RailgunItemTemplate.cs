namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x14ebe75c2cdL)]
    public interface RailgunItemTemplate : WeaponItemTemplate, GarageItemTemplate, Template
    {
    }
}

