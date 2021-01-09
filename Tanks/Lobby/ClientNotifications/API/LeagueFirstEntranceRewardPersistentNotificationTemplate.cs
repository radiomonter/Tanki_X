namespace Tanks.Lobby.ClientNotifications.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientUserProfile.API;

    [SerialVersionUID(0x15e9f034e92L)]
    public interface LeagueFirstEntranceRewardPersistentNotificationTemplate : IgnoreBattleScreenNotificationTemplate, LockScreenNotificationTemplate, NotificationTemplate, Template
    {
    }
}

