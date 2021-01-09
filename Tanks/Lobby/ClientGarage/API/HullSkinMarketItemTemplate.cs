namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x1562b75fa91L)]
    public interface HullSkinMarketItemTemplate : HullSkinItemTemplate, SkinMarketItemTemplate, Template, SkinItemTemplate, MarketItemTemplate, GarageItemImagedTemplate, GarageItemTemplate, ItemImagedTemplate
    {
    }
}

