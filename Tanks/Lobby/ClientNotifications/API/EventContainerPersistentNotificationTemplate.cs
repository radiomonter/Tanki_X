namespace Tanks.Lobby.ClientNotifications.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientUserProfile.API;

    [SerialVersionUID(0x8d58478bc408b15L)]
    public interface EventContainerPersistentNotificationTemplate : IgnoreBattleScreenNotificationTemplate, NotificationTemplate, Template
    {
    }
}

