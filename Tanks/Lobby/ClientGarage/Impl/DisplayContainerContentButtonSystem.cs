﻿namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientNavigation.API;

    public class DisplayContainerContentButtonSystem : ECSSystem
    {
        [OnEventFire]
        public void HideButton(ListItemSelectedEvent e, GameplayChestNode container, [JoinAll] ScreenNode screenNode)
        {
            screenNode.containersScreen.ContentButtonActivity = false;
        }

        [OnEventFire]
        public void ShowButton(ListItemSelectedEvent e, ItemsContainerNode container, [JoinAll] ScreenNode screenNode)
        {
            screenNode.containersScreen.ContentButtonActivity = true;
        }

        public class GameplayChestNode : Node
        {
            public ContainerMarkerComponent containerMarker;
            public GameplayChestItemComponent gameplayChestItem;
        }

        public class ItemsContainerNode : Node
        {
            public ContainerMarkerComponent containerMarker;
            public ItemsContainerItemComponent itemsContainerItem;
        }

        public class ScreenNode : Node
        {
            public ContainersScreenComponent containersScreen;
            public ScreenComponent screen;
            public ActiveScreenComponent activeScreen;
        }
    }
}

