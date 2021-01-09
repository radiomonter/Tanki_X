namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using Lobby.ClientPayment.Impl;
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Lobby.ClientBattleSelect.Impl;
    using Tanks.Lobby.ClientPayment.API;
    using Tanks.Lobby.ClientPayment.Impl;
    using UnityEngine;

    public class DealsUISystem : ECSSystem
    {
        [OnEventFire]
        public void AddmarketItemSale(NodeAddedEvent e, [Combine] MarketItemSaleNode marketItemSaleNode, [Context] SingleNode<DealsUIComponent> deals)
        {
            deals.component.AddMarketItem(marketItemSaleNode.Entity);
        }

        [OnEventFire]
        public void AddMethod(NodeAddedEvent e, [Combine] SingleNode<PaymentMethodComponent> method, SingleNode<DealsUIComponent> deals)
        {
            deals.component.AddMethod(method.Entity);
        }

        [OnEventFire]
        public void AddPromo(NodeAddedEvent e, SingleNode<GiftPromoUIDataComponent> promo, [JoinAll] SingleNode<DealsUIComponent> deals)
        {
            deals.component.AddPromo(promo.component.PromoKey);
        }

        [OnEventFire]
        public void AddPromo(NodeAddedEvent e, SingleNode<DealsUIComponent> deals, [JoinAll] Optional<SingleNode<GiftPromoUIDataComponent>> promo, [JoinAll] SingleNode<Dialogs60Component> dialogs)
        {
            deals.component.shopDialogs = dialogs.component.Get<ShopDialogs>();
            if (promo.IsPresent())
            {
                deals.component.AddPromo(promo.Get().component.PromoKey);
            }
            else
            {
                deals.component.RemovePromo();
            }
        }

        [OnEventFire]
        public void AddSpecialOffer(NodeAddedEvent e, SingleNode<DealsUIComponent> deals, [Combine] SpecialOfferNode offer, [JoinBy(typeof(SpecialOfferGroupComponent))] PersonalSpecialOfferPropertyNode personalOfferProperty)
        {
            if (!offer.Entity.HasComponent<LeagueFirstEntranceSpecialOfferComponent>())
            {
                deals.component.AddSpecialOffer(offer.Entity, null);
            }
            else
            {
                GameObject gameObject = deals.component.leagueSpecialOfferPrefab.gameObject;
                SpecialOfferContent content = deals.component.AddSpecialOffer(offer.Entity, gameObject);
                List<SpecialOfferItem> items = new List<SpecialOfferItem>();
                foreach (KeyValuePair<long, int> pair in offer.Entity.GetComponent<CountableItemsPackComponent>().Pack)
                {
                    long key = pair.Key;
                    Entity entity = Flow.Current.EntityRegistry.GetEntity(key);
                    int quantity = pair.Value;
                    string spriteUid = entity.GetComponent<ImageItemComponent>().SpriteUid;
                    string name = entity.GetComponent<DescriptionItemComponent>().Name;
                    items.Add(new SpecialOfferItem(quantity, spriteUid, name));
                }
                content.GetComponent<LeagueSpecialOfferComponent>().ShowOfferItems(items, offer.Entity.GetComponent<LeagueFirstEntranceSpecialOfferComponent>().WorthItPercent);
            }
        }

        [OnEventFire]
        public void Cancel(PaymentIsCancelledEvent e, SingleNode<PaymentMethodComponent> node, [JoinAll] SingleNode<DealsUIComponent> deals)
        {
            base.Log.Error("Error making payment: " + e.ErrorCode);
            deals.component.HandleError();
        }

        [OnEventFire]
        public void Clear(NodeRemoveEvent e, SingleNode<DealsUIComponent> deals)
        {
            deals.component.Clear();
        }

        [OnEventFire]
        public void GoToUrl(GoToUrlToPayEvent e, Node any, [JoinAll] SingleNode<DealsUIComponent> deals)
        {
            base.Log.Debug("GoToUrl");
            deals.component.HandleGoToLink();
        }

        [OnEventFire]
        public void QiwiError(InvalidQiwiAccountEvent e, Node node, [JoinAll] SingleNode<DealsUIComponent> deals)
        {
            base.Log.Error("QIWI ERROR");
            deals.component.HandleQiwiError();
        }

        [OnEventFire]
        public void RemovePromo(NodeRemoveEvent e, SingleNode<GiftPromoUIDataComponent> promo, [JoinAll] SingleNode<DealsUIComponent> deals, [JoinAll] SingleNode<GiftsPromoComponent> promoObj)
        {
            deals.component.RemovePromo();
        }

        [OnEventFire]
        public void RemoveSpecialOfferNode(NodeRemoveEvent e, PersonalSpecialOfferPropertyNode node, [JoinBy(typeof(SpecialOfferGroupComponent))] SpecialOfferNode offer, [JoinAll] SingleNode<DealsUIComponent> deals)
        {
            deals.component.RemoveSpecialOffer(offer.Entity);
        }

        [OnEventFire]
        public void Success(SuccessPaymentEvent e, SingleNode<PaymentMethodComponent> node, [JoinAll] SingleNode<DealsUIComponent> deals)
        {
            base.Log.Debug("Success");
            deals.component.HandleSuccess();
        }

        [OnEventFire]
        public void SuccessMobile(SuccessMobilePaymentEvent e, SingleNode<PaymentMethodComponent> node, [JoinAll] SingleNode<DealsUIComponent> deals)
        {
            base.Log.Debug("SuccessMobile");
            deals.component.HandleSuccessMobile(e.TransactionId);
        }

        public class MarketItemSaleNode : Node
        {
            public MarketItemSaleComponent marketItemSale;
        }

        public class PersonalSpecialOfferPropertyNode : Node
        {
            public PersonalSpecialOfferPropertiesComponent personalSpecialOfferProperties;
            public UserGroupComponent userGroup;
            public SpecialOfferGroupComponent specialOfferGroup;
            public SpecialOfferVisibleComponent specialOfferVisible;
            public OrderItemComponent orderItem;
        }

        [Not(typeof(LegendaryTankSpecialOfferComponent))]
        public class SpecialOfferNode : Node
        {
            public SpecialOfferComponent specialOffer;
            public GoodsPriceComponent goodsPrice;
            public OrderItemComponent orderItem;
            public ItemsPackFromConfigComponent itemsPackFromConfig;
            public SpecialOfferDurationComponent specialOfferDuration;
            public SpecialOfferEndTimeComponent specialOfferEndTime;
        }
    }
}

