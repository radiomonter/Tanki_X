namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x14e24efd48eL)]
    public interface HammerMarketItemTemplate : HammerItemTemplate, WeaponMarketItemTemplate, WeaponItemTemplate, GarageItemTemplate, Template, MarketItemTemplate
    {
    }
}

