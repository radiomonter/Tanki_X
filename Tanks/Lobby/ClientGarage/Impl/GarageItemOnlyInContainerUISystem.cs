namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientNavigation.API;

    public class GarageItemOnlyInContainerUISystem : ECSSystem
    {
        [OnEventFire]
        public void HideOnlyInContainerUI(ListItemSelectedEvent e, SingleNode<UserItemComponent> item, [JoinAll] ScreenNode screenNode)
        {
            screenNode.garageItemsScreen.OnlyInContainerUIVisibility = false;
        }

        [OnEventFire]
        public void HideOnlyInContainerUI(ListItemSelectedEvent e, NotInContainerMarketItemNode item, [JoinAll] ScreenNode screenNode)
        {
            screenNode.garageItemsScreen.OnlyInContainerUIVisibility = false;
        }

        [OnEventFire]
        public void SetMountText(NodeAddedEvent e, ScreenNode screenNode)
        {
            screenNode.garageItemsScreen.OnlyInContainerLabel = screenNode.garageItemsScreenText.OnlyInContainerText;
        }

        [OnEventFire]
        public void ShowOnlyInContainerUI(ListItemSelectedEvent e, InContainerMarketItemNode item, [JoinAll] ScreenNode screenNode)
        {
            screenNode.garageItemsScreen.OnlyInContainerLabelVisibility = (item.priceItem.Price == 0) && (item.xPriceItem.Price == 0);
            screenNode.garageItemsScreen.InContainerButtonVisibility = true;
        }

        public class InContainerMarketItemNode : Node
        {
            public MarketItemComponent marketItem;
            public PriceItemComponent priceItem;
            public XPriceItemComponent xPriceItem;
            public ContainerContentItemGroupComponent containerContentItemGroup;
        }

        [Not(typeof(ContainerContentItemGroupComponent))]
        public class NotInContainerMarketItemNode : Node
        {
            public MarketItemComponent marketItem;
        }

        public class ScreenNode : Node
        {
            public GarageItemsScreenComponent garageItemsScreen;
            public ScreenComponent screen;
            public ActiveScreenComponent activeScreen;
            public GarageItemsScreenTextComponent garageItemsScreenText;
        }
    }
}

