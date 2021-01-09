namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientNotifications.API;
    using Tanks.Lobby.ClientUserProfile.API;

    [SerialVersionUID(0x167a6a958a0L)]
    public interface FractionsCompetitionStartNotificationTemplate : LockScreenNotificationTemplate, NotificationTemplate, Template
    {
    }
}

