namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using Lobby.ClientPayment.API;
    using Lobby.ClientPayment.Impl;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientNavigation.API;
    using Tanks.Lobby.ClientPayment.Impl;

    public class MethodSelectionScreenSystem : ECSSystem
    {
        [CompilerGenerated]
        private static Predicate<SingleNode<PaymentMethodComponent>> <>f__am$cache0;
        [CompilerGenerated]
        private static Comparison<SingleNode<PaymentMethodComponent>> <>f__am$cache1;

        [OnEventFire]
        public void AddMethodsButtons(NodeAddedEvent e, ScreenNode screen, [JoinAll] SelectedGoodsNode goods, [JoinAll] ICollection<SingleNode<PaymentMethodComponent>> methods)
        {
            List<SingleNode<PaymentMethodComponent>> collection = methods.ToList<SingleNode<PaymentMethodComponent>>();
            if (goods.Entity.HasComponent<SpecialOfferComponent>())
            {
                if (<>f__am$cache0 == null)
                {
                    <>f__am$cache0 = x => x.component.IsTerminal;
                }
                collection.RemoveAll(<>f__am$cache0);
            }
            List<SingleNode<PaymentMethodComponent>> list2 = new List<SingleNode<PaymentMethodComponent>>(collection);
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = (p1, p2) => string.CompareOrdinal(p1.component.ProviderName + p1.component.MethodName, p2.component.ProviderName + p2.component.MethodName);
            }
            list2.Sort(<>f__am$cache1);
            foreach (SingleNode<PaymentMethodComponent> node in list2)
            {
                screen.methodSelectionScreen.List.AddItem(node.Entity);
            }
        }

        [OnEventComplete]
        public void BuyGoods(ListItemSelectedEvent e, SelectedMethodNode method, [JoinAll] SelectedGoodsNode goods, [JoinAll] ScreenNode screen, [JoinAll] SingleNode<ClientSessionComponent> session, [JoinAll] ICollection<SelectedMethodNode> methods)
        {
            if (methods.Count > 1)
            {
                foreach (SelectedMethodNode node in methods)
                {
                    if (!ReferenceEquals(node.Entity, method.Entity))
                    {
                        node.Entity.RemoveComponent<SelectedListItemComponent>();
                    }
                }
            }
            PaymentStatisticsEvent eventInstance = new PaymentStatisticsEvent {
                Action = PaymentStatisticsAction.MODE_SELECT,
                Item = goods.Entity.Id,
                Screen = screen.methodSelectionScreen.gameObject.name,
                Method = method.Entity.Id
            };
            base.ScheduleEvent(eventInstance, session);
            if ((method.paymentMethod.MethodName == PaymentMethodNames.CREDIT_CARD) && (method.paymentMethod.ProviderName == "adyen"))
            {
                base.ScheduleEvent<ShowScreenLeftEvent<BankCardPaymentScreenComponent>>(screen);
            }
            else if (method.paymentMethod.MethodName == PaymentMethodNames.MOBILE)
            {
                base.ScheduleEvent<ShowScreenLeftEvent<MobilePaymentScreenComponent>>(screen);
            }
            else if ((method.paymentMethod.MethodName == PaymentMethodNames.QIWI_WALLET) && (method.paymentMethod.ProviderName == "qiwi"))
            {
                base.ScheduleEvent<ShowScreenLeftEvent<QiwiWalletScreenComponent>>(screen);
            }
            else
            {
                base.ScheduleEvent<ShowScreenLeftEvent<PaymentProcessingScreenComponent>>(method);
                Node[] nodes = new Node[] { method, goods };
                base.NewEvent<ProceedToExternalPaymentEvent>().AttachAll(nodes).Schedule();
                eventInstance = new PaymentStatisticsEvent {
                    Action = PaymentStatisticsAction.PROCEED,
                    Item = goods.Entity.Id,
                    Screen = screen.methodSelectionScreen.gameObject.name,
                    Method = method.Entity.Id
                };
                base.ScheduleEvent(eventInstance, session);
            }
        }

        public class ScreenNode : Node
        {
            public MethodSelectionScreenComponent methodSelectionScreen;
            public ScreenGroupComponent screenGroup;
        }

        public class SelectedGoodsNode : Node
        {
            public SelectedListItemComponent selectedListItem;
            public GoodsComponent goods;
            public GoodsPriceComponent goodsPrice;
        }

        public class SelectedMethodNode : Node
        {
            public PaymentMethodComponent paymentMethod;
            public SelectedListItemComponent selectedListItem;
        }

        public class SelectedXCrystalsPackNode : MethodSelectionScreenSystem.SelectedGoodsNode
        {
            public XCrystalsPackComponent xcrystalsPack;
        }
    }
}

