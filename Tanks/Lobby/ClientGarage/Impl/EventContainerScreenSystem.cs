namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientNotifications.API;
    using Tanks.Lobby.ClientUserProfile.API;
    using Tanks.Lobby.ClientUserProfile.Impl;
    using UnityEngine;

    public class EventContainerScreenSystem : ECSSystem
    {
        [OnEventFire]
        public void EventContainerNotificationAdded(NodeAddedEvent e, ContainerNotificationNode node)
        {
        }

        [OnEventFire]
        public void Fill(NodeAddedEvent e, EventContainerNotificationNode notification, [JoinAll] UserNode user, [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens)
        {
            if (!notification.Entity.HasComponent<EventContainerNotificationComponent>())
            {
                base.ScheduleEvent<NotificationShownEvent>(notification);
            }
            else
            {
                EventContainerPopupComponent eventContainerPopup = notification.eventContainerPopup;
                eventContainerPopup.itemsContainer.DestroyChildren();
                long containerId = notification.Entity.GetComponent<EventContainerNotificationComponent>().ContainerId;
                EventContainerItemComponent component2 = Object.Instantiate<EventContainerItemComponent>(eventContainerPopup.itemPrefab, eventContainerPopup.itemsContainer, false);
                Entity entity = Flow.Current.EntityRegistry.GetEntity(containerId);
                component2.preview.SpriteUid = entity.GetComponent<ImageItemComponent>().SpriteUid;
                component2.text.text = entity.GetComponent<DescriptionItemComponent>().Name;
                component2.gameObject.SetActive(true);
                eventContainerPopup.Show(!screens.IsPresent() ? new List<Animator>() : screens.Get().component.Animators);
            }
        }

        [OnEventFire]
        public void HidePopup(ButtonClickEvent e, SingleNode<EventContainerPopupCloseButtonComponent> button, [JoinAll] ICollection<EventContainerNotificationNode> notifications, [JoinAll] ICollection<SingleNode<EventContainerPopupComponent>> popups)
        {
            foreach (SingleNode<EventContainerPopupComponent> node in popups)
            {
                node.component.Hide();
            }
            foreach (EventContainerNotificationNode node2 in notifications)
            {
                base.ScheduleEvent<NotificationShownEvent>(node2);
            }
        }

        [OnEventFire]
        public void SetRewardInfo(NodeAddedEvent e, EventContainerNotificationNode notification)
        {
            notification.Entity.AddComponent(new NotificationMessageComponent(string.Empty));
        }

        public class ContainerNotificationNode : Node
        {
            public EventContainerNotificationComponent eventContainerNotification;
        }

        public class EventContainerNotificationNode : Node
        {
            public ActiveNotificationComponent activeNotification;
            public EventContainerPopupComponent eventContainerPopup;
        }

        public class UserNode : Node
        {
            public SelfUserComponent selfUser;
        }
    }
}

