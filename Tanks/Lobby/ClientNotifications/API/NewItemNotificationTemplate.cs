namespace Tanks.Lobby.ClientNotifications.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientUserProfile.API;

    [SerialVersionUID(0x158dcf8fe5cL)]
    public interface NewItemNotificationTemplate : LockScreenNotificationTemplate, IgnoreBattleScreenNotificationTemplate, NotificationTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        NewItemNotificationTextComponent newItemNotificationText();
        [AutoAdded, PersistentConfig("", false)]
        NewPaintItemNotificationTextComponent newPaintItemNotificationText();
    }
}

