namespace Tanks.Lobby.ClientNotifications.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientUserProfile.API;

    [SerialVersionUID(0x16193703d8aL)]
    public interface EnergyCompensationPersistentNotificationTemplate : IgnoreBattleScreenNotificationTemplate, LockScreenNotificationTemplate, NotificationTemplate, Template
    {
    }
}

