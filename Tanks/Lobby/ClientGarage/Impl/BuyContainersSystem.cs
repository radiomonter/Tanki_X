namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientNavigation.API;
    using Tanks.Lobby.ClientProfile.API;

    public class BuyContainersSystem : ECSSystem
    {
        [OnEventFire]
        public void BuyContainers(ConfirmButtonClickYesEvent e, ButtonNode buyButton, [JoinByScreen] SingleNode<SelectedItemComponent> selectedItem)
        {
            Entity[] entities = new Entity[] { selectedItem.component.SelectedItem, buyButton.Entity };
            base.NewEvent<BuySelectedContainerItemEvent>().AttachAll(entities).Schedule();
        }

        [OnEventFire]
        public void BuyContainers(BuySelectedContainerItemEvent evt, ButtonNode buyButton, ContainerUserItemNode containerUserItem, [JoinByMarketItem] ContainerMarketItemNode containerMarketItem, [JoinAll] SelfUserNode userNode)
        {
            if (buyButton.universalPriceButton.XPriceActivity)
            {
                XBuyMarketItemEvent eventInstance = new XBuyMarketItemEvent {
                    Amount = buyButton.itemPackButton.Count,
                    Price = (int) buyButton.priceButton.Price
                };
                Entity[] entities = new Entity[] { containerMarketItem.Entity, userNode.Entity };
                base.NewEvent(eventInstance).AttachAll(entities).Schedule();
            }
            else if (buyButton.universalPriceButton.PriceActivity)
            {
                BuyMarketItemEvent eventInstance = new BuyMarketItemEvent {
                    Amount = buyButton.itemPackButton.Count,
                    Price = (int) buyButton.priceButton.Price
                };
                Entity[] entities = new Entity[] { containerMarketItem.Entity, userNode.Entity };
                base.NewEvent(eventInstance).AttachAll(entities).Schedule();
            }
        }

        public class ButtonNode : Node
        {
            public PriceButtonComponent priceButton;
            public UniversalPriceButtonComponent universalPriceButton;
            public BuyContainerButtonComponent buyContainerButton;
            public ItemPackButtonComponent itemPackButton;
        }

        public class BuySelectedContainerItemEvent : Event
        {
        }

        public class ContainerItemNode : Node
        {
            public ContainerMarkerComponent containerMarker;
            public MarketItemGroupComponent marketItemGroup;
        }

        public class ContainerMarketItemNode : BuyContainersSystem.ContainerItemNode
        {
            public MarketItemComponent marketItem;
        }

        public class ContainerUserItemNode : BuyContainersSystem.ContainerItemNode
        {
            public UserItemComponent userItem;
        }

        public class SelfUserNode : Node
        {
            public UserXCrystalsComponent userXCrystals;
            public UserMoneyComponent userMoney;
            public SelfUserComponent selfUser;
        }
    }
}

