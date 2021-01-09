namespace Tanks.Lobby.ClientNotifications.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientUserProfile.API;

    [SerialVersionUID(0x157999125a8L)]
    public interface UIDChangedNotificationTemplate : NotificationTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        UIDChangedNotificationTextComponent uIDChangedNotificationText();
    }
}

