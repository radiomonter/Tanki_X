namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x14dbdb3dcbcL)]
    public interface FreezeMarketItemTemplate : FreezeItemTemplete, WeaponMarketItemTemplate, WeaponItemTemplate, GarageItemTemplate, Template, MarketItemTemplate
    {
    }
}

