namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using Lobby.ClientPayment.API;
    using Lobby.ClientPayment.Impl;
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientEntrance.Impl;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientNavigation.API;
    using Tanks.Lobby.ClientPayment.Impl;

    public class GoodsSelectionScreenSystem : ECSSystem
    {
        [CompilerGenerated]
        private static Comparison<PersonalSpecialOfferPropertyNode> <>f__am$cache0;
        [CompilerGenerated]
        private static Comparison<PackNode> <>f__am$cache1;

        [OnEventFire]
        public void AddOffer(CollectOfferEvent e, PersonalSpecialOfferPropertyNode personalOfferProperty, [JoinBy(typeof(SpecialOfferGroupComponent))] SpecialOfferNode offer, [JoinAll] GoodsSelectionScreenNode screen)
        {
            StringBuilder builder = new StringBuilder();
            if (offer.itemsPackFromConfig.Pack.Count > 0)
            {
                int num = 0;
                builder.Append("* —");
                bool flag = true;
                foreach (long num2 in offer.itemsPackFromConfig.Pack)
                {
                    ItemInMarketRequestEvent eventInstance = new ItemInMarketRequestEvent();
                    base.ScheduleEvent(eventInstance, offer);
                    if (eventInstance.marketItems.ContainsKey(num2))
                    {
                        if (!flag)
                        {
                            builder.Append(", ");
                        }
                        flag = false;
                        builder.Append(eventInstance.marketItems[num2]);
                        num++;
                    }
                }
                if (num == 0)
                {
                    builder.Append(screen.goodsSelectionScreen.SpecialOfferEmptyRewardMessage);
                }
            }
            if (offer.specialOfferDuration.OneShot && personalOfferProperty.Entity.HasComponent(typeof(PaymentIntentComponent)))
            {
                if (builder.Length > 0)
                {
                    builder.Append("\n");
                }
                builder.Append(screen.goodsSelectionScreen.SpecialOfferOneShotMessage);
            }
            screen.goodsSelectionScreen.SpecialOfferDataProvider.AddItem(offer.Entity, builder.ToString());
        }

        [OnEventFire]
        public void ClearOffers(PaymentSectionLoadedEvent e, Node node, [JoinAll] GoodsSelectionScreenNode screen)
        {
            screen.goodsSelectionScreen.SpecialOfferDataProvider.ClearItems();
            screen.goodsSelectionScreen.XCrystalsDataProvider.ClearItems();
        }

        private void CreatePacks(ICollection<PackNode> packs, IUIList list)
        {
            List<PackNode> list2 = packs.ToList<PackNode>();
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = (a, b) => a.goodsPrice.Price.CompareTo(b.goodsPrice.Price);
            }
            list2.Sort(<>f__am$cache1);
            foreach (PackNode node in list2)
            {
                list.AddItem(node.Entity);
            }
        }

        [OnEventComplete]
        public void FilterOffers(UpdateListEvent e, SingleNode<SelfUserComponent> user, [JoinByUser] ICollection<PersonalSpecialOfferPropertyNode> personalOfferProperties)
        {
            List<PersonalSpecialOfferPropertyNode> list = personalOfferProperties.ToList<PersonalSpecialOfferPropertyNode>();
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = (a, b) => a.orderItem.Index.CompareTo(b.orderItem.Index);
            }
            list.Sort(<>f__am$cache0);
            foreach (PersonalSpecialOfferPropertyNode node in list)
            {
                base.ScheduleEvent<CollectOfferEvent>(node);
            }
        }

        [OnEventFire]
        public void InitConfirmation(NodeAddedEvent e, SingleNode<FirstPurchaseConfirmScreenComponent> screen, SelectedNode selectedGoods, UserNode user)
        {
            Node[] nodes = new Node[] { selectedGoods, user };
            base.NewEvent<CalculateCompensationRequestEvent>().AttachAll(nodes).Schedule();
        }

        [OnEventFire]
        public void InitConfirmation(CalculateCompensationResponseEvent e, SelectedNode selectedGoods, UserNode user, [JoinAll] SingleNode<FirstPurchaseConfirmScreenComponent> screen)
        {
            screen.component.Compensation = e.Amount;
        }

        [OnEventFire]
        public void InitiateUpdateList(PaymentSectionLoadedEvent e, SingleNode<SelfUserComponent> user, Optional<SingleNode<SteamComponent>> steamOptional)
        {
            if (!steamOptional.IsPresent() || !string.IsNullOrEmpty(SteamComponent.Ticket))
            {
                base.ScheduleEvent<UpdateListEvent>(user.Entity);
            }
        }

        [OnEventFire]
        public void InitScreen(UpdateListEvent e, Node node, [JoinAll] GoodsSelectionScreenNode screen, [JoinAll] ICollection<PackNode> packs)
        {
            this.CreatePacks(packs, screen.goodsSelectionScreen.XCrystalsDataProvider);
        }

        [OnEventFire]
        public void OpenSystemsSelectionScreen(ButtonClickEvent e, SingleNode<ContinueButtonComponent> button, [JoinByScreen] SingleNode<FirstPurchaseConfirmScreenComponent> screen, [JoinAll] SelectedNode selectedGoods, [JoinAll] SingleNode<ClientSessionComponent> session)
        {
            Entity context = selectedGoods.Entity;
            ShowScreenEvent eventInstance = new ShowScreenLeftEvent<MethodSelectionScreenComponent>();
            eventInstance.SetContext(context, false);
            base.ScheduleEvent(eventInstance, context);
            PaymentStatisticsEvent event3 = new PaymentStatisticsEvent {
                Screen = screen.component.gameObject.name,
                Action = PaymentStatisticsAction.CONFIRMED_ONE_TIME_OFFER,
                Item = context.Id
            };
            base.ScheduleEvent(event3, session);
        }

        [OnEventFire]
        public void OpenSystemsSelectionScreen(ListItemSelectedEvent e, SelectedNode selectedGoods, [JoinAll] GoodsSelectionScreenNode screen, [JoinAll] SingleNode<ClientSessionComponent> session, [JoinAll] Optional<SingleNode<SteamComponent>> steam)
        {
            Entity context = selectedGoods.Entity;
            if (!steam.IsPresent())
            {
                ShowScreenEvent event3 = new ShowScreenLeftEvent<MethodSelectionScreenComponent>();
                event3.SetContext(context, false);
                base.ScheduleEvent(event3, context);
            }
            else if (string.IsNullOrEmpty(SteamComponent.Ticket))
            {
                new ShowScreenLeftEvent<MethodSelectionScreenComponent>().SetContext(context, false);
            }
            else
            {
                base.ScheduleEvent<SteamBuyGoodsEvent>(context);
            }
            PaymentStatisticsEvent eventInstance = new PaymentStatisticsEvent {
                Screen = screen.goodsSelectionScreen.gameObject.name,
                Action = PaymentStatisticsAction.ITEM_SELECT,
                Item = context.Id
            };
            base.ScheduleEvent(eventInstance, session);
        }

        [OnEventFire]
        public void RegistratePaymentIntentComponent(NodeAddedEvent e, SingleNode<PaymentIntentComponent> paymentIntent)
        {
        }

        [OnEventFire]
        public void SetOfferTime(NodeAddedEvent e, SingleNode<SpecialOfferRemainingTimeComponent> remainNode)
        {
            remainNode.component.EndDate = Date.Now.AddSeconds((float) remainNode.component.Remain);
            SpecialOfferEndTimeComponent component = new SpecialOfferEndTimeComponent {
                EndDate = remainNode.component.EndDate
            };
            remainNode.Entity.AddComponent(component);
        }

        [OnEventFire]
        public void SetOfferTime(NodeAddedEvent e, ClientOfferNode specialOffer, [JoinBy(typeof(SpecialOfferGroupComponent)), Context] ClientPersonalOfferProperties personalProperty)
        {
            SpecialOfferEndTimeComponent component = new SpecialOfferEndTimeComponent {
                EndDate = personalProperty.specialOfferEndTime.EndDate
            };
            specialOffer.Entity.AddComponent(component);
        }

        [OnEventFire]
        public void SetOrderIndexToOffer(UpdateListEvent e, SingleNode<SelfUserComponent> user, [JoinByUser, Combine] PersonalSpecialOfferPropertyNode personalOfferProperty, [JoinBy(typeof(SpecialOfferGroupComponent))] SpecialOfferNode offer)
        {
            personalOfferProperty.orderItem.Index = offer.orderItem.Index;
        }

        [OnEventFire]
        public void UnsetOfferTime(NodeRemoveEvent e, SingleNode<SpecialOfferRemainingTimeComponent> remainNode)
        {
            if (remainNode.Entity.HasComponent<SpecialOfferEndTimeComponent>())
            {
                remainNode.Entity.RemoveComponent<SpecialOfferEndTimeComponent>();
            }
        }

        [Not(typeof(SpecialOfferEndTimeComponent))]
        public class ClientOfferNode : Node
        {
            public SpecialOfferGroupComponent specialOfferGroup;
            public SpecialOfferComponent specialOffer;
        }

        public class ClientPersonalOfferProperties : Node
        {
            public SpecialOfferRemainingTimeComponent specialOfferRemainingTime;
            public SpecialOfferGroupComponent specialOfferGroup;
            public SpecialOfferEndTimeComponent specialOfferEndTime;
        }

        public class CollectOfferEvent : Event
        {
        }

        public class GoodsSelectionScreenNode : Node
        {
            public GoodsSelectionScreenComponent goodsSelectionScreen;
            public XCrystalsSaleEndTimerComponent xCrystalsSaleEndTimer;
            public ActiveScreenComponent activeScreen;
        }

        [Not(typeof(SpecialOfferComponent))]
        public class PackNode : Node
        {
            public XCrystalsPackComponent xCrystalsPack;
            public GoodsPriceComponent goodsPrice;
        }

        public class PersonalSpecialOfferPropertyNode : Node
        {
            public PersonalSpecialOfferPropertiesComponent personalSpecialOfferProperties;
            public UserGroupComponent userGroup;
            public SpecialOfferGroupComponent specialOfferGroup;
            public SpecialOfferVisibleComponent specialOfferVisible;
            public OrderItemComponent orderItem;
        }

        public class SelectedNode : Node
        {
            public SelectedListItemComponent selectedListItem;
            public GoodsPriceComponent goodsPrice;
        }

        public class SpecialOfferNode : Node
        {
            public SpecialOfferComponent specialOffer;
            public GoodsPriceComponent goodsPrice;
            public OrderItemComponent orderItem;
            public ItemsPackFromConfigComponent itemsPackFromConfig;
            public SpecialOfferDurationComponent specialOfferDuration;
        }

        public class UpdateListEvent : Event
        {
        }

        public class UserNode : Node
        {
            public SelfUserComponent selfUser;
            public UserCountryComponent userCountry;
        }
    }
}

