namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientNavigation.API;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class DisplayBuyButtonSystem : ECSSystem
    {
        private void HideBuyButton(ScreenNode screenNode)
        {
            screenNode.garageItemsScreen.BuyButton.SetActive(false);
        }

        [OnEventFire]
        public void HideBuyButton(ListItemSelectedEvent e, UserItemNode item, [JoinAll] ScreenNode screen)
        {
            this.HideBuyButton(screen);
            this.HideXBuyButton(screen);
        }

        private void HideXBuyButton(ScreenNode screenNode)
        {
            screenNode.garageItemsScreen.XBuyButton.SetActive(false);
        }

        [OnEventFire]
        public void LocalizeBuyButton(NodeAddedEvent e, BuyButtonNode node)
        {
            node.confirmButton.ButtonText = node.buyButtonConfirmWithPriceLocalizedText.BuyText + " " + node.buyButtonConfirmWithPriceLocalizedText.ForText;
            node.confirmButton.CancelText = node.buyButtonConfirmWithPriceLocalizedText.CancelText;
            node.confirmButton.ConfirmText = node.buyButtonConfirmWithPriceLocalizedText.ConfirmText;
        }

        [OnEventFire]
        public void PriceChanged(PriceChangedEvent e, BuyableMarketItemNode item)
        {
            item.priceItem.OldPrice = (int) e.OldPrice;
            item.priceItem.Price = (int) e.Price;
            item.xPriceItem.OldPrice = (int) e.OldXPrice;
            item.xPriceItem.Price = (int) e.XPrice;
        }

        [OnEventComplete]
        public void PriceChanged(PriceChangedEvent e, BuyableMarketItemNode item, [JoinAll] ScreenNode screen, [JoinByScreen] SingleNode<SelectedItemComponent> selectedItem)
        {
            bool flag = item.Entity.Equals(selectedItem.component.SelectedItem);
            base.Log.InfoFormat("PriceChanged {0} item={1} itemIsSelected={2}", e, item, flag);
            GameObject buyButton = screen.garageItemsScreen.BuyButton;
            GameObject xBuyButton = screen.garageItemsScreen.XBuyButton;
            buyButton.GetComponent<PriceButtonComponent>().Price = e.Price;
            xBuyButton.GetComponent<PriceButtonComponent>().Price = e.XPrice;
            if (flag)
            {
                SetPriceEvent event2;
                if (buyButton.activeSelf)
                {
                    event2 = new SetPriceEvent {
                        Price = e.Price,
                        OldPrice = e.OldPrice,
                        XPrice = e.XPrice,
                        OldXPrice = e.OldXPrice
                    };
                    base.ScheduleEvent(event2, buyButton.GetComponent<EntityBehaviour>().Entity);
                }
                if (xBuyButton.activeSelf)
                {
                    event2 = new SetPriceEvent {
                        Price = e.Price,
                        OldPrice = e.OldPrice,
                        XPrice = e.XPrice,
                        OldXPrice = e.OldXPrice
                    };
                    base.ScheduleEvent(event2, xBuyButton.GetComponent<EntityBehaviour>().Entity);
                }
            }
        }

        private void ShowBuyButton(PriceItemComponent priceItem, ScreenNode screenNode)
        {
            GameObject buyButton = screenNode.garageItemsScreen.BuyButton;
            buyButton.SetActive(true);
            EventSystem.current.SetSelectedGameObject(buyButton.gameObject);
            SetPriceEvent eventInstance = new SetPriceEvent {
                Price = priceItem.Price,
                OldPrice = priceItem.OldPrice
            };
            base.ScheduleEvent(eventInstance, buyButton.GetComponent<EntityBehaviour>().Entity);
            buyButton.GetComponent<PriceButtonComponent>().Price = priceItem.Price;
        }

        private void ShowXBuyButton(XPriceItemComponent priceItem, ScreenNode screenNode)
        {
            GameObject xBuyButton = screenNode.garageItemsScreen.XBuyButton;
            xBuyButton.SetActive(true);
            EventSystem.current.SetSelectedGameObject(xBuyButton.gameObject);
            SetPriceEvent eventInstance = new SetPriceEvent {
                XPrice = priceItem.Price,
                OldXPrice = priceItem.OldPrice
            };
            base.ScheduleEvent(eventInstance, xBuyButton.GetComponent<EntityBehaviour>().Entity);
            xBuyButton.GetComponent<PriceButtonComponent>().Price = priceItem.Price;
        }

        [OnEventFire]
        public void SwitchBuyButton(ListItemSelectedEvent e, BuyableMarketItemNode item, [JoinByParentGroup] ICollection<UserItemNode> parentUserItem, [JoinAll] ScreenNode screen)
        {
            CheckMarketItemRestrictionsEvent eventInstance = new CheckMarketItemRestrictionsEvent();
            base.ScheduleEvent(eventInstance, item);
            if (eventInstance.RestrictedByRank || eventInstance.RestrictedByUpgradeLevel)
            {
                this.HideBuyButton(screen);
                this.HideXBuyButton(screen);
            }
            else
            {
                if (item.priceItem.IsBuyable)
                {
                    this.ShowBuyButton(item.priceItem, screen);
                }
                else
                {
                    this.HideBuyButton(screen);
                }
                if (item.xPriceItem.IsBuyable)
                {
                    this.ShowXBuyButton(item.xPriceItem, screen);
                }
                else
                {
                    this.HideXBuyButton(screen);
                }
            }
        }

        public class BuyableMarketItemNode : Node
        {
            public MarketItemComponent marketItem;
            public PriceItemComponent priceItem;
            public XPriceItemComponent xPriceItem;
        }

        public class BuyButtonNode : Node
        {
            public BuyButtonComponent buyButton;
            public ConfirmButtonComponent confirmButton;
            public BuyButtonConfirmWithPriceLocalizedTextComponent buyButtonConfirmWithPriceLocalizedText;
        }

        public class ScreenNode : Node
        {
            public GarageItemsScreenComponent garageItemsScreen;
            public ActiveScreenComponent activeScreen;
            public ScreenComponent screen;
        }

        public class UserItemNode : Node
        {
            public UserItemComponent userItem;
            public MarketItemGroupComponent marketItemGroup;
        }
    }
}

