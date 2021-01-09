namespace Tanks.Lobby.ClientNotifications.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientNotifications.API;
    using Tanks.Lobby.ClientUserProfile.API;
    using Tanks.Lobby.ClientUserProfile.Impl;

    public class NotificationsOnBattleResultsScreenSystem : ECSSystem
    {
        [OnEventFire]
        public void NewCardNotificationsClosed(CloseNotificationEvent e, ActiveCardNotificationNode cards, [JoinAll] SingleNode<BattleResultsAwardsScreenComponent> screen)
        {
            screen.component.CardsCount--;
            screen.component.HideNotiffication();
        }

        [OnEventFire]
        public void NewNotificationsOpened(NodeAddedEvent e, SingleNode<NewItemNotificationComponent> notification, [JoinAll] SingleNode<BattleResultsAwardsScreenComponent> screen)
        {
            screen.component.CardsCount++;
            screen.component.ShowNotiffication();
        }

        public class ActiveCardNotificationNode : NotificationsOnBattleResultsScreenSystem.ActiveNotificationNode
        {
            public NewCardItemNotificationComponent newCardItemNotification;
        }

        [Not(typeof(NewCardItemNotificationComponent))]
        public class ActiveItemNotificationNode : NotificationsOnBattleResultsScreenSystem.ActiveNotificationNode
        {
        }

        public class ActiveNotificationNode : NotificationsOnBattleResultsScreenSystem.NotificationNode
        {
            public ActiveNotificationComponent activeNotification;
            public NewItemNotificationUnityComponent newItemNotificationUnity;
        }

        public class NotificationNode : Node
        {
            public NewItemNotificationComponent newItemNotification;
            public NewItemNotificationTextComponent newItemNotificationText;
            public NewPaintItemNotificationTextComponent newPaintItemNotificationText;
        }
    }
}

