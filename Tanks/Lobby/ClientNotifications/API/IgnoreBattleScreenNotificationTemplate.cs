namespace Tanks.Lobby.ClientNotifications.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientUserProfile.API;

    [SerialVersionUID(0x8d40e159c0ccf82L)]
    public interface IgnoreBattleScreenNotificationTemplate : NotificationTemplate, Template
    {
        [AutoAdded]
        IgnoreBattleScreenNotificationComponent ignoreBattleScreenNotification();
    }
}

