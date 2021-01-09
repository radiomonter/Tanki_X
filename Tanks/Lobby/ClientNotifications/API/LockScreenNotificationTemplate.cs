namespace Tanks.Lobby.ClientNotifications.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientUserProfile.API;

    [SerialVersionUID(0x8d41207e0cdefd7L)]
    public interface LockScreenNotificationTemplate : NotificationTemplate, Template
    {
        [AutoAdded]
        LockScreenNotificationComponent lockScreenNotification();
    }
}

