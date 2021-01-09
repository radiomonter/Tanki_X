namespace Tanks.Lobby.ClientNotifications.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientBattleSelect.Impl;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientNavigation.API;
    using Tanks.Lobby.ClientNotifications.API;
    using Tanks.Lobby.ClientUserProfile.API;
    using Tanks.Lobby.ClientUserProfile.Impl;

    public class IgnoreBattleResultsScreenNotificationSystem : ECSSystem
    {
        [OnEventFire]
        public void CloseNotificationOnBattleScreen(NodeAddedEvent evt, [Combine] ActiveNotificationNode notification, BattleResultsScreenNode screen)
        {
            base.ScheduleEvent<CloseNotificationEvent>(notification);
        }

        [OnEventFire]
        public void IgnoredNotificationShown(NodeRemoveEvent e, ActiveReadyNotificationWithGroupNode notification, [JoinBy(typeof(NotificationsGroupComponent))] UserNode user)
        {
            base.NewEvent<ScreenPartShownEvent>().Attach(user).Schedule();
        }

        [OnEventFire]
        public void NewLeagueNotif(NodeAddedEvent e, SingleNode<LeagueFirstEntranceRewardNotificationComponent> notification)
        {
            IgnoreBattleResultScreenNotificationComponent component = new IgnoreBattleResultScreenNotificationComponent {
                ScreenPartIndex = 1
            };
            notification.Entity.AddComponent(component);
        }

        [OnEventFire]
        public void SetScreenPartIndex(NodeAddedEvent evt, [Combine] NewItemCardNotificationNode notification, BattleResultsScreenNode screen)
        {
            notification.Entity.RemoveComponent<IgnoreBattleResultScreenNotificationComponent>();
        }

        [OnEventFire]
        public void SetScreenPartIndex(NodeAddedEvent evt, [Combine] NewItemNotificationNode notification, BattleResultsScreenNode screen)
        {
            IgnoreBattleResultScreenNotificationComponent component = new IgnoreBattleResultScreenNotificationComponent {
                ScreenPartIndex = 1
            };
            notification.Entity.AddComponent(component);
        }

        [OnEventFire]
        public void SetScreenPartIndex(NodeAddedEvent evt, [Combine] UserRankRewardNotificationNode notification, BattleResultsScreenNode screen)
        {
            notification.ignoreBattleResultScreenNotification.ScreenPartIndex = 1;
        }

        [OnEventFire]
        public void ShowIgnoredNotification(ShowBattleResultsScreenNotificationEvent e, Node any, [JoinAll] UserNode user, [JoinBy(typeof(NotificationsGroupComponent))] ICollection<SingleNode<IgnoreBattleResultScreenNotificationComponent>> notifications)
        {
            <ShowIgnoredNotification>c__AnonStorey0 storey = new <ShowIgnoredNotification>c__AnonStorey0 {
                e = e
            };
            if ((notifications.Count != 0) && !notifications.All<SingleNode<IgnoreBattleResultScreenNotificationComponent>>(new Func<SingleNode<IgnoreBattleResultScreenNotificationComponent>, bool>(storey.<>m__0)))
            {
                SingleNode<IgnoreBattleResultScreenNotificationComponent> node = notifications.First<SingleNode<IgnoreBattleResultScreenNotificationComponent>>(new Func<SingleNode<IgnoreBattleResultScreenNotificationComponent>, bool>(storey.<>m__1));
                if (node != null)
                {
                    storey.e.NotificationExist = true;
                    node.Entity.RemoveComponent<IgnoreBattleResultScreenNotificationComponent>();
                    base.NewEvent<TryToShowNotificationEvent>().Attach(user).ScheduleDelayed(0.3f);
                }
            }
        }

        [OnEventFire]
        public void SkipNotificationOnBattleScreen(ShowNotificationEvent evt, SingleNode<IgnoreBattleResultScreenNotificationComponent> notification, [JoinAll] BattleResultsScreenNode screen)
        {
            evt.CanShowNotification = false;
        }

        [CompilerGenerated]
        private sealed class <ShowIgnoredNotification>c__AnonStorey0
        {
            internal ShowBattleResultsScreenNotificationEvent e;

            internal bool <>m__0(SingleNode<IgnoreBattleResultScreenNotificationComponent> n) => 
                n.component.ScreenPartIndex != this.e.Index;

            internal bool <>m__1(SingleNode<IgnoreBattleResultScreenNotificationComponent> n) => 
                n.component.ScreenPartIndex == this.e.Index;
        }

        public class ActiveNotificationNode : Node
        {
            public ActiveNotificationComponent activeNotification;
            public IgnoreBattleResultScreenNotificationComponent ignoreBattleResultScreenNotification;
        }

        public class ActiveReadyNotificationWithGroupNode : Node
        {
            public NotificationComponent notification;
            public ActiveNotificationComponent activeNotification;
            public NotificationsGroupComponent notificationsGroup;
        }

        public class BattleResultsScreenNode : Node
        {
            public ScreenComponent screen;
            public BattleResultScreenComponent battleResultScreen;
            public ActiveScreenComponent activeScreen;
        }

        public class NewItemCardNotificationNode : Node
        {
            public NewItemNotificationComponent newItemNotification;
            public NewCardItemNotificationComponent newCardItemNotification;
            public IgnoreBattleResultScreenNotificationComponent ignoreBattleResultScreenNotification;
        }

        public class NewItemNotificationNode : Node
        {
            public NewItemNotificationComponent newItemNotification;
        }

        public class UserNode : Node
        {
            public NotificationsGroupComponent notificationsGroup;
            public SelfUserComponent selfUser;
        }

        public class UserRankRewardNotificationNode : Node
        {
            public UserRankRewardNotificationInfoComponent userRankRewardNotificationInfo;
            public IgnoreBattleResultScreenNotificationComponent ignoreBattleResultScreenNotification;
        }
    }
}

