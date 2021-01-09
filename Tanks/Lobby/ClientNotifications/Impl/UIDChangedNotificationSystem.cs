namespace Tanks.Lobby.ClientNotifications.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientGarage.Impl;
    using Tanks.Lobby.ClientNotifications.API;
    using Tanks.Lobby.ClientUserProfile.API;

    public class UIDChangedNotificationSystem : ECSSystem
    {
        [OnEventFire]
        public void PrepareNotificationText(NodeAddedEvent e, NotificationNode notification, [JoinAll] ChangeUIDSystem.SelfUserNode user)
        {
            string message = string.Format(notification.uIDChangedNotificationText.MessageTemplate, user.userUid.Uid);
            notification.Entity.AddComponent(new NotificationMessageComponent(message));
        }

        public class NotificationNode : Node
        {
            public UIDChangedNotificationComponent uIDChangedNotification;
            public UIDChangedNotificationTextComponent uIDChangedNotificationText;
        }
    }
}

