namespace Tanks.Lobby.ClientNotifications.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientUserProfile.API;

    [SerialVersionUID(0x162d25e2df8L)]
    public interface LoginRewardNotificationTemplate : IgnoreBattleScreenNotificationTemplate, LockScreenNotificationTemplate, NotificationTemplate, Template
    {
    }
}

