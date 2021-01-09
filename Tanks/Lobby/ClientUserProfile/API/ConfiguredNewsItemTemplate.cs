namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x158e1d03fc2L)]
    public interface ConfiguredNewsItemTemplate : Template
    {
        NewsItemComponent newsItem();
    }
}

