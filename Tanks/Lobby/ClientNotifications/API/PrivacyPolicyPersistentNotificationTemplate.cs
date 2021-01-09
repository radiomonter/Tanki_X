namespace Tanks.Lobby.ClientNotifications.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientUserProfile.API;

    [SerialVersionUID(0x163aadb3433L)]
    public interface PrivacyPolicyPersistentNotificationTemplate : IgnoreBattleScreenNotificationTemplate, LockScreenNotificationTemplate, NotificationTemplate, Template
    {
    }
}

