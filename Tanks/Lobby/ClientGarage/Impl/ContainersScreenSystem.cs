namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientUserProfile.API;
    using Tanks.Lobby.ClientUserProfile.Impl;

    public class ContainersScreenSystem : ECSSystem
    {
        [OnEventFire]
        public void CloseContainer(CloseNotificationEvent e, SingleNode<NotificationGroupComponent> notification, [JoinAll] SingleNode<ContainerComponent> containerNode)
        {
            containerNode.component.CloseContainer();
        }

        [OnEventFire]
        public void OpenContainer(OpenContainerAnimationShownEvent e, Node any, [JoinAll] ContainerNode selectedItem)
        {
            base.ScheduleEvent(new OpenSelectedContainerEvent(), GarageItemsRegistry.GetItem<ContainerBoxItem>(selectedItem.marketItemGroup.Key).UserItem);
        }

        [OnEventFire]
        public void ShowOpenContainerAnimation(OpenVisualContainerEvent e, Node any, [JoinAll] SingleNode<ContainerComponent> containerNode, [JoinAll] ICollection<NotificationNode> notifications)
        {
            containerNode.component.ShowOpenContainerAnimation();
            foreach (NotificationNode node in notifications)
            {
                base.ScheduleEvent<CloseNotificationEvent>(node);
            }
        }

        [OnEventFire]
        public void UnshareContainer(NodeRemoveEvent e, UserItemNode containerUserItem, [JoinAll] SingleNode<ContainersUI> containerUi)
        {
            containerUi.component.DeleteContainerItem(containerUserItem.marketItemGroup.Key);
        }

        [Inject]
        public static Tanks.Lobby.ClientGarage.API.GarageItemsRegistry GarageItemsRegistry { get; set; }

        public class ContainerNode : Node
        {
            public ContainerMarkerComponent containerMarker;
            public MarketItemGroupComponent marketItemGroup;
            public HangarItemPreviewComponent hangarItemPreview;
        }

        public class NotificationNode : Node
        {
            public ActiveNotificationComponent activeNotification;
        }

        public class UserItemNode : Node
        {
            public UserItemComponent userItem;
            public MarketItemGroupComponent marketItemGroup;
        }
    }
}

