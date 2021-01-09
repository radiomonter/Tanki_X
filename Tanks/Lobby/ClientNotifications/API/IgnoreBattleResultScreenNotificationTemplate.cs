namespace Tanks.Lobby.ClientNotifications.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientUserProfile.API;

    [SerialVersionUID(0x8d4121422bbbd11L)]
    public interface IgnoreBattleResultScreenNotificationTemplate : NotificationTemplate, Template
    {
        [AutoAdded]
        IgnoreBattleResultScreenNotificationComponent ignoreBattleResultScreenNotification();
    }
}

