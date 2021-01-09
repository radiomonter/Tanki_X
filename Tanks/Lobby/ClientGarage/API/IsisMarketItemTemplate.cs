namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x14e52e970cbL)]
    public interface IsisMarketItemTemplate : IsisItemTemplate, WeaponMarketItemTemplate, WeaponItemTemplate, GarageItemTemplate, Template, MarketItemTemplate
    {
    }
}

