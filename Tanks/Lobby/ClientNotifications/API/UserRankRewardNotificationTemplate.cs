namespace Tanks.Lobby.ClientNotifications.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientUserProfile.API;

    [SerialVersionUID(0x8d40c8773c9fe38L)]
    public interface UserRankRewardNotificationTemplate : IgnoreBattleScreenNotificationTemplate, LockScreenNotificationTemplate, IgnoreBattleResultScreenNotificationTemplate, NotificationTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        UserRankRewardNotificationTextComponent userRankRewardNotificationText();
    }
}

