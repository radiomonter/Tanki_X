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
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientNavigation.API;
    using Tanks.Lobby.ClientPayment.Impl;
    using Tanks.Lobby.ClientUserProfile.Impl;
    using UnityEngine;

    public class PaymentSectionSystem : ECSSystem
    {
        [OnEventFire]
        public void Changed(GoodsChangedEvent e, SelectedPackNode selectedPack, [JoinAll] CurrentScreenNode screen)
        {
            this.ShowNotification(screen, selectedPack);
        }

        [OnEventFire]
        public void Changed(NodeRemoveEvent e, PersonalSpecialNode personal, [JoinBy(typeof(SpecialOfferGroupComponent))] SelectedSpecialPackNode selectedPack, [JoinAll] CurrentScreenNode screen)
        {
            this.ShowNotification(screen, selectedPack);
        }

        [OnEventFire]
        public void Click(ButtonClickEvent e, SingleNode<XCrystalsIndicatorComponent> indicator, [JoinAll] SelfUserNode user)
        {
            base.ScheduleEvent<GoToXCryShopScreen>(user);
        }

        [OnEventFire]
        public void ClosePaymentSection(NodeRemoveEvent e, SingleNode<SectionComponent> section, [JoinAll] SingleNode<SelfUserComponent> user)
        {
            Debug.Log("PaymentSectionSystem.ClosePaymentSection");
            base.ScheduleEvent<ClosePaymentSectionEvent>(user);
        }

        [OnEventFire]
        public void Destroy(NotificationShownEvent e, SingleNode<SaleEndNotificationTextComponent> node)
        {
            base.DeleteEntity(node.Entity);
        }

        [OnEventFire]
        public void LeavePayment(NodeAddedEvent e, SingleNode<ScreenComponent> screen, [JoinAll] SingleNode<UserInPaymentSectionComponent> session)
        {
            if (screen.component.GetComponent<PaymentScreen>() == null)
            {
                session.Entity.RemoveComponent<UserInPaymentSectionComponent>();
            }
        }

        [OnEventFire]
        public void MarkXCrystals(NodeAddedEvent e, SelfUserNode user, [Combine] SingleNode<ShopBadgeComponent> indicator, SingleNode<PaymentSpecialIconMinimalRankComponent> config)
        {
            indicator.component.NotificationAvailable = user.userRank.Rank >= int.Parse(config.component.MinimalRank);
        }

        [OnEventFire]
        public void Process(ProcessPaymentEvent e, PackNode pack, [JoinAll] Optional<SelectedMethodNode> methodNodeOptional)
        {
            long amount = pack.xCrystalsPack.Amount;
            if (!pack.Entity.HasComponent<SpecialOfferComponent>())
            {
                amount = (long) Math.Round((double) (pack.goods.SaleState.AmountMultiplier * amount));
            }
            e.TotalAmount = amount + pack.xCrystalsPack.Bonus;
        }

        [OnEventFire]
        public void SetBadge(NodeAddedEvent e, [Combine] SingleNode<SpecialOfferVisibleComponent> special, SingleNode<ShopBadgeComponent> indicator)
        {
            indicator.component.SpecialOfferAvailable = true;
        }

        [OnEventComplete]
        public void SetBadge(NodeRemoveEvent e, [Combine] SingleNode<SpecialOfferVisibleComponent> special, ICollection<SingleNode<SpecialOfferVisibleComponent>> specials, [JoinAll] SingleNode<ShopBadgeComponent> indicator)
        {
            <SetBadge>c__AnonStorey0 storey = new <SetBadge>c__AnonStorey0 {
                special = special
            };
            if (!specials.ToList<SingleNode<SpecialOfferVisibleComponent>>().Any<SingleNode<SpecialOfferVisibleComponent>>(new Func<SingleNode<SpecialOfferVisibleComponent>, bool>(storey.<>m__0)))
            {
                indicator.component.SpecialOfferAvailable = false;
            }
        }

        private void ShowNotification(CurrentScreenNode screen, SelectedPackNode selectedPack)
        {
            PaymentScreen component = screen.screen.GetComponent<PaymentScreen>();
            if ((component != null) && !ReferenceEquals(component.GetType(), typeof(GoodsSelectionScreenComponent)))
            {
                base.ScheduleEvent<ShowScreenRightEvent<GoodsSelectionScreenComponent>>(selectedPack);
                base.CreateEntity<SaleEndNotificationTemplate>("notification/saleend").AddComponent<NotificationComponent>();
            }
        }

        [CompilerGenerated]
        private sealed class <SetBadge>c__AnonStorey0
        {
            internal SingleNode<SpecialOfferVisibleComponent> special;

            internal bool <>m__0(SingleNode<SpecialOfferVisibleComponent> x) => 
                !ReferenceEquals(x.Entity, this.special.Entity);
        }

        public class CurrentScreenNode : Node
        {
            public ActiveScreenComponent activeScreen;
            public ScreenComponent screen;
        }

        public class DestroyNotificationEvent : Event
        {
        }

        public class PackNode : Node
        {
            public GoodsComponent goods;
            public XCrystalsPackComponent xCrystalsPack;
        }

        public class PersonalSpecialNode : Node
        {
            public SpecialOfferVisibleComponent specialOfferVisible;
            public SpecialOfferGroupComponent specialOfferGroup;
        }

        public class SelectedMethodNode : Node
        {
            public PaymentMethodComponent paymentMethod;
            public SelectedListItemComponent selectedListItem;
        }

        public class SelectedPackNode : Node
        {
            public GoodsComponent goods;
            public SelectedListItemComponent selectedListItem;
        }

        public class SelectedSpecialPackNode : PaymentSectionSystem.SelectedPackNode
        {
            public GoodsComponent goods;
            public SelectedListItemComponent selectedListItem;
            public SpecialOfferComponent specialOffer;
            public SpecialOfferGroupComponent specialOfferGroup;
        }

        public class SelfUserNode : Node
        {
            public UserRankComponent userRank;
            public SelfUserComponent selfUser;
        }
    }
}

