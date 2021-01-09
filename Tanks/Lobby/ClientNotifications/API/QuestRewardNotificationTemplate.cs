namespace Tanks.Lobby.ClientNotifications.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientUserProfile.API;

    [SerialVersionUID(0x15c35433802L)]
    public interface QuestRewardNotificationTemplate : LockScreenNotificationTemplate, IgnoreBattleScreenNotificationTemplate, NotificationTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        NewItemNotificationTextComponent newItemNotificationText();
        [AutoAdded, PersistentConfig("", false)]
        NewPaintItemNotificationTextComponent newPaintItemNotificationText();
    }
}

