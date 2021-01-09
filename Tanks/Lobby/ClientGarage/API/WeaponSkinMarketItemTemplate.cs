namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x1562b6ffcb5L)]
    public interface WeaponSkinMarketItemTemplate : WeaponSkinItemTemplate, SkinMarketItemTemplate, Template, SkinItemTemplate, MarketItemTemplate, GarageItemImagedTemplate, GarageItemTemplate, ItemImagedTemplate
    {
    }
}

