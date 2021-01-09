namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientNotifications.API;
    using Tanks.Lobby.ClientUserProfile.API;

    [SerialVersionUID(0x1683173f503L)]
    public interface FractionsCompetitionRewardNotificationTemplate : LockScreenNotificationTemplate, NotificationTemplate, Template
    {
    }
}

