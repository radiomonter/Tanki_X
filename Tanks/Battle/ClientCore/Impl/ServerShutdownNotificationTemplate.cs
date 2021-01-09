namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientUserProfile.API;
    using Tanks.Lobby.ClientUserProfile.Impl;

    [SerialVersionUID(0x1549f553c37L)]
    public interface ServerShutdownNotificationTemplate : NotificationTemplate, Template
    {
        [AutoAdded]
        NotClickableNotificationComponent notClickableNotification();
        [PersistentConfig("", false), AutoAdded]
        ServerShutdownTextComponent serverShutdownText();
        [AutoAdded]
        UpdatedNotificationComponent updatedNotification();
    }
}

