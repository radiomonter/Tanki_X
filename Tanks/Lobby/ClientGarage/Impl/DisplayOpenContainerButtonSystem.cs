namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientNavigation.API;

    public class DisplayOpenContainerButtonSystem : ECSSystem
    {
        [OnEventFire]
        public void HideButton(ListItemSelectedEvent e, SingleNode<MarketItemComponent> container, [JoinAll] ScreenNode screenNode)
        {
            screenNode.containersScreen.SetOpenButtonsActive(false, false);
        }

        [OnEventFire]
        public void ShowButton(ListItemSelectedEvent e, ContainerNode container, [JoinAll] ScreenNode screenNode)
        {
            screenNode.containersScreen.SetOpenButtonsActive(container.userItemCounter.Count > 0L, false);
        }

        [OnEventFire]
        public void UpdateContainersCount(ItemsCountChangedEvent e, ContainerNode container, [JoinAll] ScreenNode screenNode, [JoinByScreen] SingleNode<SelectedItemComponent> selectedItem)
        {
            if (selectedItem.component.SelectedItem.Id.Equals(container.Entity.Id))
            {
                screenNode.containersScreen.SetOpenButtonsActive(container.userItemCounter.Count > 0L, false);
            }
        }

        public class ContainerNode : Node
        {
            public ContainerMarkerComponent containerMarker;
            public UserItemCounterComponent userItemCounter;
        }

        public class ScreenNode : Node
        {
            public ContainersScreenComponent containersScreen;
            public ContainersScreenTextComponent containersScreenText;
            public ScreenComponent screen;
            public ActiveScreenComponent activeScreen;
        }
    }
}

