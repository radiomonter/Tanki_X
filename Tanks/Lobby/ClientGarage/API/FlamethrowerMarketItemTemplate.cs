namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x14e24e607dfL)]
    public interface FlamethrowerMarketItemTemplate : FlamethrowerItemTemplate, WeaponMarketItemTemplate, WeaponItemTemplate, GarageItemTemplate, Template, MarketItemTemplate
    {
    }
}

