namespace Tanks.Lobby.ClientNotifications.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientUserProfile.API;

    [SerialVersionUID(0x15a35f1aa7fL)]
    public interface NewItemClientNotificationTemplate : NewItemNotificationTemplate, LockScreenNotificationTemplate, IgnoreBattleScreenNotificationTemplate, NotificationTemplate, Template
    {
        [AutoAdded]
        NewItemClientNotificationComponent newItemClientNotification();
    }
}

