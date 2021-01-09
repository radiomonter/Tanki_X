namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x158719a379fL)]
    public interface NewsItemTemplate : Template
    {
        NewsItemComponent newsItem();
        NewsItemSaleLabelComponent newsItemSaleLabel();
    }
}

