namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Lobby.ClientUserProfile.API;
    using Tanks.Lobby.ClientUserProfile.Impl;

    [SerialVersionUID(0x154a4001b51L)]
    public interface BattleShutdownNotificationTemplate : NotificationTemplate, Template
    {
        [PersistentConfig("", false), AutoAdded]
        BattleShutdownTextComponent battleShutdownText();
        [AutoAdded]
        NotClickableNotificationComponent notClickableNotification();
        [AutoAdded]
        UpdatedNotificationComponent updatedNotification();
    }
}

