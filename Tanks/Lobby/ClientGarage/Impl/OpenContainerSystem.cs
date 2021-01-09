namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientGarage.API;

    public class OpenContainerSystem : ECSSystem
    {
        [OnEventFire]
        public void OpenContainer(OpenSelectedContainerEvent e, GamePlayChestItemNode containerNode)
        {
            OpenContainerEvent eventInstance = new OpenContainerEvent {
                Amount = containerNode.userItemCounter.Count
            };
            base.ScheduleEvent(eventInstance, containerNode);
        }

        [OnEventFire]
        public void OpenContainer(OpenSelectedContainerEvent e, ItemsContainerItemNode containerNode)
        {
            base.ScheduleEvent(new OpenContainerEvent(), containerNode);
        }

        public class GamePlayChestItemNode : Node
        {
            public GameplayChestItemComponent gameplayChestItem;
            public ContainerMarkerComponent containerMarker;
            public UserItemComponent userItem;
            public UserItemCounterComponent userItemCounter;
        }

        public class ItemsContainerItemNode : Node
        {
            public ItemsContainerItemComponent itemsContainerItem;
            public ContainerMarkerComponent containerMarker;
            public UserItemComponent userItem;
        }
    }
}

