namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientUserProfile.API;

    [SerialVersionUID(0x8d40c755e3ad9ddL)]
    public interface EmailConfirmationNotificationTemplate : NotificationTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        EmailConfirmationNotificationComponent emailConfirmationNotification();
    }
}

