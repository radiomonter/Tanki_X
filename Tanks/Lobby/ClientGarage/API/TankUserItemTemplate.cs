namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x14ef373674aL)]
    public interface TankUserItemTemplate : TankItemTemplate, UpgradableUserItemTemplate, GarageItemTemplate, Template, UserItemTemplate
    {
    }
}

