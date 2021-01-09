namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x14ebe79aa21L)]
    public interface IsisItemTemplate : WeaponItemTemplate, GarageItemTemplate, Template
    {
    }
}

