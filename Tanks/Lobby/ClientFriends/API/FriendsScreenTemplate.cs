namespace Tanks.Lobby.ClientFriends.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientFriends.Impl;
    using Tanks.Lobby.ClientNavigation.API;

    [SerialVersionUID(0x151aa145f25L)]
    public interface FriendsScreenTemplate : ScreenTemplate, Template
    {
        [PersistentConfig("", false)]
        FriendsScreenLocalizationComponent friendsScreenLocalization();
    }
}

