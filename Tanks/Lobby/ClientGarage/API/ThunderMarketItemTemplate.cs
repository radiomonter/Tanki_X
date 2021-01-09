namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x14e24f58637L)]
    public interface ThunderMarketItemTemplate : ThunderItemTemplate, WeaponMarketItemTemplate, WeaponItemTemplate, GarageItemTemplate, Template, MarketItemTemplate
    {
    }
}

