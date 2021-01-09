namespace Tanks.Lobby.ClientFriends.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientUserProfile.API;

    [SerialVersionUID(0x15f0a9b0465L)]
    public interface FriendSentNotificationTemplate : NotificationTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        FriendSentNotificationComponent friendSentNotification();
    }
}

