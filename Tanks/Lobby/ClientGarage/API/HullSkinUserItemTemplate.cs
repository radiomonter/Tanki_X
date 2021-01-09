namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x1562b75d820L)]
    public interface HullSkinUserItemTemplate : HullSkinItemTemplate, SkinUserItemTemplate, Template, SkinItemTemplate, UserItemTemplate, GarageItemImagedTemplate, GarageItemTemplate, ItemImagedTemplate
    {
    }
}

