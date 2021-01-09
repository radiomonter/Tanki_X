namespace Tanks.Lobby.ClientNotifications.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientUserProfile.API;

    [SerialVersionUID(0x161bc8b195eL)]
    public interface EulaPersistentNotificationTemplate : IgnoreBattleScreenNotificationTemplate, LockScreenNotificationTemplate, NotificationTemplate, Template
    {
    }
}

