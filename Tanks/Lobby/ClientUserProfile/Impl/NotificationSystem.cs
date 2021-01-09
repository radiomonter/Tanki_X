namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientResources.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientNavigation.API;
    using Tanks.Lobby.ClientUserProfile.API;
    using UnityEngine;

    public class NotificationSystem : ECSSystem
    {
        private const float TIMEOUT = 0.05f;
        [CompilerGenerated]
        private static Func<ActiveReadyNotificationWithGroupNode, bool> <>f__am$cache0;
        [CompilerGenerated]
        private static Comparison<NotActiveReadyNotificationWithGroupNode> <>f__am$cache1;
        [CompilerGenerated]
        private static Func<NotActiveReadyNotificationWithGroupNode, Entity> <>f__am$cache2;

        [OnEventFire]
        public void AddMessage(NodeAddedEvent e, SingleNode<ServerNotificationMessageComponent> notification)
        {
            notification.Entity.AddComponent(new NotificationMessageComponent(notification.component.Message));
        }

        [OnEventFire]
        public void AddNotificationToQueue(NodeAddedEvent e, ReadyNotificationNode notification, [JoinAll] UserNotificationsGroupNode user)
        {
            base.Log.InfoFormat("AddNotificationToQueue notification={0}", notification);
            user.notificationsGroup.Attach(notification.Entity);
        }

        [OnEventFire]
        public void CloseActiveNotificationEvent(CloseNotificationEvent evt, ActiveReadyNotificationMappingWithGroupNode notification)
        {
            notification.notifficationMapping.enabled = false;
        }

        [OnEventComplete]
        public void CloseActiveNotificationEvent(CloseNotificationEvent evt, ActiveReadyNotificationWithGroupNode notification)
        {
            notification.activeNotification.Hide();
        }

        private void CreateNotificationObject(NotActiveReadyNotificationWithGroupNode notification, NotificationsContainerComponent notificationContainer, int index, int notificationsCount)
        {
            <CreateNotificationObject>c__AnonStorey0 storey = new <CreateNotificationObject>c__AnonStorey0 {
                notification = notification
            };
            GameObject instance = Object.Instantiate(storey.notification.resourceData.Data) as GameObject;
            instance.transform.SetParent(!storey.notification.notificationConfig.IsFullScreen ? notificationContainer.GetParenTransform(index, notificationsCount) : notificationContainer.GetFullSceenNotificationContainer(), false);
            storey.notification.Entity.AddComponent(new NotificationInstanceComponent(instance));
            instance.GetComponentsInChildren<ActiveNotificationComponent>(true).ForEach<ActiveNotificationComponent>(new Action<ActiveNotificationComponent>(storey.<>m__0));
            instance.GetComponent<EntityBehaviour>().BuildEntity(storey.notification.Entity);
            float showDuration = storey.notification.notificationConfig.ShowDuration;
            float showDelay = storey.notification.notificationConfig.ShowDelay;
            base.NewEvent<SetNotificationVisibleEvent>().Attach(storey.notification.Entity).ScheduleDelayed(index * showDelay);
            if (showDuration > 0f)
            {
                base.NewEvent<HideNotificationEvent>().Attach(storey.notification.Entity).ScheduleDelayed(showDuration);
            }
        }

        [OnEventFire]
        public void DestroyNotification(NodeRemoveEvent e, SingleNode<NotificationInstanceComponent> notification)
        {
            Object.Destroy(notification.component.Instance);
        }

        [OnEventComplete]
        public void HideNotification(NotificationClickEvent e, ClickableReadyNotificationWithGroupNode notification)
        {
            base.ScheduleEvent<CloseNotificationEvent>(notification);
        }

        [OnEventFire]
        public void HideNotification(HideNotificationEvent e, ActiveReadyNotificationWithGroupNode notification)
        {
            base.ScheduleEvent<CloseNotificationEvent>(notification);
        }

        [OnEventFire]
        public void HideNotification(NodeAddedEvent e, ScreenNode screen, [Combine] ActiveReadyNotificationMappingWithGroupNode notification)
        {
            if (!screen.screen.ShowNotifications)
            {
                base.ScheduleEvent<CloseNotificationEvent>(notification);
            }
        }

        [OnEventFire]
        public void SetNotificationText(NodeAddedEvent e, NotificationWithMessageNode notification)
        {
            notification.activeNotification.Text.text = notification.notificationMessage.Message;
        }

        [OnEventFire]
        public void SetNotificationVisible(SetNotificationVisibleEvent e, ActiveReadyNotificationWithGroupNode notification)
        {
            notification.activeNotification.Show();
        }

        [OnEventComplete]
        public void ShowNextNotification(NodeRemoveEvent e, ActiveReadyNotificationWithGroupNode notification, [JoinBy(typeof(NotificationsGroupComponent))] UserNotificationsGroupNode user)
        {
            base.NewEvent<TryToShowNotificationEvent>().Attach(user).ScheduleDelayed(0.05f);
        }

        [OnEventComplete]
        public void ShowNotification(ShowNotificationEvent e, NotActiveReadyNotificationWithGroupNode notification, [JoinBy(typeof(NotificationGroupComponent))] ICollection<NotActiveReadyNotificationWithGroupNode> notifications, [JoinAll] SingleNode<NotificationsContainerComponent> notificationContainer, [JoinAll] ScreenNode screen)
        {
            if (base.Log.IsInfoEnabled)
            {
                base.Log.InfoFormat("ShowNotification {0} CanShowNotification={1}", notification, e.CanShowNotification);
            }
            if (!e.CanShowNotification)
            {
                List<Entity> sortedNotifications = e.SortedNotifications;
                sortedNotifications.Remove(notification.Entity);
                if (sortedNotifications.Count != 0)
                {
                    base.ScheduleEvent(new ShowNotificationEvent(sortedNotifications), sortedNotifications.First<Entity>());
                }
            }
            else
            {
                e.SortedNotifications.Clear();
                if (!notifications.Any<NotActiveReadyNotificationWithGroupNode>())
                {
                    this.CreateNotificationObject(notification, notificationContainer.component, 0, 1);
                }
                else
                {
                    int index = 0;
                    foreach (NotActiveReadyNotificationWithGroupNode node in notifications.Take<NotActiveReadyNotificationWithGroupNode>(notificationContainer.component.MaxItemsPerScreen))
                    {
                        this.CreateNotificationObject(node, notificationContainer.component, index, notifications.Count);
                        index++;
                    }
                }
            }
        }

        [OnEventFire]
        public void ShowOnChangeScreenOrLoadNotification(NodeAddedEvent e, ScreenNode screen, [Context, Combine] ReadyNotificationWithGroupNode notification, UserNotificationsGroupNode user)
        {
            if (screen.screen.ShowNotifications)
            {
                base.NewEvent<TryToShowNotificationEvent>().Attach(user).ScheduleDelayed(0.05f);
            }
        }

        private List<Entity> SortNonActiveNotifications(ICollection<NotActiveReadyNotificationWithGroupNode> notActiveNotifications)
        {
            List<NotActiveReadyNotificationWithGroupNode> source = notActiveNotifications.ToList<NotActiveReadyNotificationWithGroupNode>();
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = (a, b) => a.notificationConfig.Order.CompareTo(b.notificationConfig.Order);
            }
            source.Sort(<>f__am$cache1);
            <>f__am$cache2 ??= n => n.Entity;
            return source.Select<NotActiveReadyNotificationWithGroupNode, Entity>(<>f__am$cache2).ToList<Entity>();
        }

        [OnEventFire]
        public void TryToShowNotification(TryToShowNotificationEvent evt, UserNotificationsGroupNode user1, [JoinBy(typeof(NotificationsGroupComponent))] ICollection<NotActiveReadyNotificationWithGroupNode> notActiveNotifications, UserNotificationsGroupNode user2, [JoinBy(typeof(NotificationsGroupComponent))] ICollection<ActiveReadyNotificationWithGroupNode> activeNotifications, [JoinAll] NotLockedScreenNode screen)
        {
            if (base.Log.IsInfoEnabled)
            {
                base.Log.InfoFormat("TryToShowNotification activeNotifications={0} notActiveNotifications={1}", EcsToStringUtil.EnumerableToString(activeNotifications), EcsToStringUtil.EnumerableToString(notActiveNotifications));
            }
            if (activeNotifications.Count > 0)
            {
                if (<>f__am$cache0 == null)
                {
                    <>f__am$cache0 = n => n.activeNotification.Visible;
                }
                if (activeNotifications.Any<ActiveReadyNotificationWithGroupNode>(<>f__am$cache0))
                {
                    return;
                }
            }
            if (notActiveNotifications.Count > 0)
            {
                List<Entity> sortedNotifications = this.SortNonActiveNotifications(notActiveNotifications);
                base.ScheduleEvent(new ShowNotificationEvent(sortedNotifications), sortedNotifications.First<Entity>());
            }
        }

        [OnEventComplete]
        public void UpdateNotificationText(UpdateEvent e, UpdatedReadyNotificationWithMessageNode notification)
        {
            notification.activeNotification.Text.text = notification.notificationMessage.Message;
        }

        [CompilerGenerated]
        private sealed class <CreateNotificationObject>c__AnonStorey0
        {
            internal NotificationSystem.NotActiveReadyNotificationWithGroupNode notification;

            internal void <>m__0(ActiveNotificationComponent i)
            {
                i.Entity = this.notification.Entity;
            }
        }

        public class ActiveReadyNotificationMappingWithGroupNode : NotificationSystem.ActiveReadyNotificationWithGroupNode
        {
            public NotifficationMappingComponent notifficationMapping;
        }

        public class ActiveReadyNotificationWithGroupNode : NotificationSystem.ReadyNotificationWithGroupNode
        {
            public ActiveNotificationComponent activeNotification;
        }

        [Not(typeof(NotClickableNotificationComponent))]
        public class ClickableReadyNotificationWithGroupNode : NotificationSystem.ActiveReadyNotificationWithGroupNode
        {
        }

        [Not(typeof(ActiveNotificationComponent))]
        public class NotActiveReadyNotificationWithGroupNode : NotificationSystem.ReadyNotificationWithGroupNode
        {
        }

        public class NotificationWithMessageNode : NotificationSystem.ReadyNotificationNode
        {
            public ActiveNotificationComponent activeNotification;
            public NotificationMessageComponent notificationMessage;
        }

        [Not(typeof(LockedScreenComponent))]
        public class NotLockedScreenNode : NotificationSystem.ScreenNode
        {
        }

        public class ReadyNotificationNode : Node, IComparable<NotificationSystem.ReadyNotificationNode>
        {
            public NotificationComponent notification;
            public ResourceDataComponent resourceData;
            public NotificationConfigComponent notificationConfig;

            public int CompareTo(NotificationSystem.ReadyNotificationNode other) => 
                this.notification.CompareTo(other.notification);
        }

        public class ReadyNotificationWithGroupNode : NotificationSystem.ReadyNotificationNode
        {
            public NotificationsGroupComponent notificationsGroup;
        }

        public class ScreenNode : Node
        {
            public ScreenComponent screen;
            public ActiveScreenComponent activeScreen;
        }

        public class UpdatedReadyNotificationWithMessageNode : NotificationSystem.NotificationWithMessageNode
        {
            public UpdatedNotificationComponent updatedNotification;
        }

        public class UserNotificationsGroupNode : Node
        {
            public NotificationsGroupComponent notificationsGroup;
            public SelfUserComponent selfUser;
        }
    }
}

