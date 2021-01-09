namespace Tanks.Lobby.ClientGarage.Impl
{
    using Lobby.ClientPayment.Impl;
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientPayment.API;
    using Tanks.Lobby.ClientPayment.Impl;
    using Tanks.Lobby.ClientProfile.API;
    using TMPro;
    using UnityEngine;

    public class PremiumUiShopSystem : ECSSystem
    {
        [OnEventFire]
        public void ActivatePremiumMainScreenActiveIcon(NodeAddedEvent e, SelfPremiumUserNode user, SingleNode<PremiumMainScreenButtonComponent> button)
        {
            button.component.ActivatePremium();
        }

        [OnEventFire]
        public void ActivatePremiumQuestLine(NodeAddedEvent e, SelfPremiumQuestUserNode user, SingleNode<PremiumToolbarUiComponent> toolbar)
        {
            toolbar.component.ActivatePremiumTasks();
        }

        private List<PremiumGoodsNode> BuildList(ICollection<PremiumGoodsNode> goods, BaseUserNode userNode)
        {
            List<PremiumGoodsNode> list = new List<PremiumGoodsNode>();
            int rank = userNode.userRank.Rank;
            foreach (PremiumGoodsNode node in goods)
            {
                int minRank = node.premiumOffer.MinRank;
                int maxRank = node.premiumOffer.MaxRank;
                if ((rank >= minRank) && (rank < maxRank))
                {
                    list.Add(node);
                }
            }
            return list;
        }

        [OnEventFire]
        public void CreatePacks(NodeAddedEvent e, SingleNode<PremiumShopTabComponent> shopNode, [JoinAll] ICollection<PremiumGoodsNode> goods, [JoinAll] BaseUserNode userNode)
        {
            List<PremiumGoodsNode> list = this.BuildList(goods, userNode);
            list.Sort(new PremiumGoodsNodeComparer());
            for (int i = 0; i < list.Count; i++)
            {
                PremiumPackComponent pack = Object.Instantiate<GameObject>(shopNode.component.PackPrefab, shopNode.component.PackContainer).GetComponent<PremiumPackComponent>();
                this.FillPack(pack, list[i], i);
            }
        }

        [OnEventFire]
        public void DeactivatePremiumMainScreenActiveIcon(NodeRemoveEvent e, SelfPremiumUserNode user, SingleNode<PremiumMainScreenButtonComponent> button)
        {
            button.component.DeactivatePremium();
        }

        [OnEventFire]
        public void DeactivatePremiumQuestLine(NodeRemoveEvent e, SelfPremiumQuestUserNode user, SingleNode<PremiumToolbarUiComponent> toolbar)
        {
            toolbar.component.DeactivatePremiumTasks();
        }

        [OnEventFire]
        public void DestroyPacks(NodeRemoveEvent e, SingleNode<PremiumShopTabComponent> shopNode)
        {
            IEnumerator enumerator = shopNode.component.PackContainer.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Transform current = (Transform) enumerator.Current;
                    Object.Destroy(current.gameObject);
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }

        [OnEventFire]
        public void DiscountActivated(NodeAddedEvent e, SingleNode<PremiumAccountDiscountActivatedComponent> user, SingleNode<MainScreenComponent> homeScreen, [JoinAll] SingleNode<Dialogs60Component> dialogs, [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens)
        {
            List<Animator> animators = new List<Animator>();
            if (screens.IsPresent())
            {
                animators = screens.Get().component.Animators;
            }
            dialogs.component.Get<PremiumExpiredUiComponent>().Show(animators);
            user.Entity.RemoveComponent<PremiumAccountDiscountActivatedComponent>();
        }

        [OnEventFire]
        public void FillDaysLeft(UpdateEvent e, SelfPremiumUserNode user, [JoinAll] SingleNode<PremiumToolbarUiComponent> toolbar)
        {
            if (toolbar.component.visible)
            {
                TextMeshProUGUI activeText = toolbar.component.activeText;
                Date endDate = user.premiumAccountBoost.EndDate;
                float num = endDate.UnityTime / 86400f;
                float num2 = endDate.UnityTime / 3600f;
                activeText.text = (num <= 1f) ? string.Format(toolbar.component.hoursTextLocalizedField.Value, num2.ToString("####")) : string.Format(toolbar.component.daysTextLocalizedField.Value, num.ToString("####"));
            }
        }

        private void FillPack(PremiumPackComponent pack, PremiumGoodsNode packNode, int count)
        {
            GetDiscountForOfferEvent eventInstance = new GetDiscountForOfferEvent();
            base.ScheduleEvent(eventInstance, packNode);
            pack.DaysText = packNode.countableItemsPack.Pack.First<KeyValuePair<long, int>>().Value.ToString();
            pack.DaysDescription = packNode.specialOfferContentLocalization.Description;
            pack.Price = $"{(1f - eventInstance.Discount) * packNode.goodsPrice.Price:0.00} {packNode.goodsPrice.Currency}";
            pack.Discount = eventInstance.Discount;
            pack.HasXCrystals = this.IsGoodsWithCrystals(packNode);
            pack.LearnMoreIndex = count;
            pack.GoodsEntity = packNode.Entity;
        }

        [OnEventFire]
        public void HideActivateNotification(NodeAddedEvent e, SingleNode<ActiveNotificationComponent> notif, [JoinAll] SingleNode<Dialogs60Component> dialogs)
        {
            dialogs.component.Get<PremiumActivatedUIComponent>().HideImmediate();
        }

        private bool IsGoodsWithCrystals(PremiumGoodsNode sortedGood) => 
            sortedGood.countableItemsPack.Pack.ContainsKey(-180272377L);

        [OnEventFire]
        public void OnDiscountAdded(NodeAddedEvent e, PremiumGoodsNode good, [Context, JoinBy(typeof(SpecialOfferGroupComponent))] SingleNode<DiscountComponent> personalOffer)
        {
            if (good.Entity.HasComponent<CustomOfferPriceForUIComponent>())
            {
                good.Entity.RemoveComponent<CustomOfferPriceForUIComponent>();
            }
            double num = good.goodsPrice.Price * (1f - personalOffer.component.DiscountCoeff);
            num = good.goodsPrice.Round(num);
            good.Entity.AddComponent(new CustomOfferPriceForUIComponent(num));
        }

        [OnEventComplete]
        public void PremiumMainScreenButtonClick(ButtonClickEvent e, SingleNode<PremiumMainScreenButtonComponent> button, [JoinAll] SingleNode<Dialogs60Component> dialogs, [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens, [JoinAll] BaseUserNode selfUser, [JoinAll] SingleNode<PremiumToolbarUiComponent> premiumToolbar)
        {
            if (selfUser.Entity.HasComponent<PremiumAccountBoostComponent>())
            {
                premiumToolbar.component.Toggle();
                dialogs.component.Get<PremiumLearnMoreComponent>().HideImmediate();
            }
        }

        [OnEventFire]
        public void SetDiscount(GetDiscountForOfferEvent e, PremiumGoodsNode good, [JoinBy(typeof(SpecialOfferGroupComponent))] Optional<SingleNode<DiscountComponent>> personalOffer)
        {
            e.Discount = 0f;
            if (personalOffer.IsPresent())
            {
                e.Discount = personalOffer.Get().component.DiscountCoeff;
            }
        }

        [OnEventFire]
        public void ShowPremiumActivatedDialog(NodeAddedEvent e, SingleNode<MainScreenComponent> homeScreen, PremiumBoostItemDurationChangedNode boostItem, [JoinByUser] SelfPremiumUserNode user, [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens, [JoinAll] SingleNode<Dialogs60Component> dialogs)
        {
            Entity[] entities = new Entity[] { user.Entity, boostItem.Entity };
            base.NewEvent<ShowPremiumActivatedDialogEvent>().AttachAll(entities).ScheduleDelayed(0f);
        }

        [OnEventFire]
        public void ShowPremiumActivatedDialog(ShowPremiumActivatedDialogEvent e, SelfPremiumUserNode user, PremiumBoostItemDurationChangedNode boostItem, [JoinByUser] Optional<PremiumQuestItemDurationChangedNode> questItem, [JoinAll] SingleNode<MainScreenComponent> homeScreen, [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens, [JoinAll] SingleNode<Dialogs60Component> dialogs)
        {
            <ShowPremiumActivatedDialog>c__AnonStorey0 storey = new <ShowPremiumActivatedDialog>c__AnonStorey0();
            Action action = null;
            storey.animators = new List<Animator>();
            if (screens.IsPresent())
            {
                storey.animators = screens.Get().component.Animators;
            }
            storey.premiumActivatedUiComponent = dialogs.component.Get<PremiumActivatedUIComponent>();
            Date endDate = user.premiumAccountBoost.EndDate;
            storey.days = Convert.ToInt32((float) (endDate.UnityTime / 86400f));
            if (!boostItem.Entity.HasComponent<PremiumPromoComponent>())
            {
                if (questItem.IsPresent())
                {
                    action = new Action(storey.<>m__2);
                    questItem.Get().Entity.RemoveComponent<PremiumDurationChangedComponent>();
                    boostItem.Entity.RemoveComponent<PremiumDurationChangedComponent>();
                    return;
                }
                action = new Action(storey.<>m__3);
                boostItem.Entity.RemoveComponent<PremiumDurationChangedComponent>();
            }
            else
            {
                if (questItem.IsPresent())
                {
                    action = new Action(storey.<>m__0);
                    boostItem.Entity.RemoveComponent<PremiumPromoComponent>();
                    questItem.Get().Entity.RemoveComponent<PremiumDurationChangedComponent>();
                    boostItem.Entity.RemoveComponent<PremiumDurationChangedComponent>();
                    return;
                }
                action = new Action(storey.<>m__1);
                boostItem.Entity.RemoveComponent<PremiumPromoComponent>();
                boostItem.Entity.RemoveComponent<PremiumDurationChangedComponent>();
            }
            if ((base.SelectAll<SingleNode<ActiveNotificationComponent>>().FirstOrDefault<SingleNode<ActiveNotificationComponent>>() == null) && (action != null))
            {
                action();
            }
        }

        [CompilerGenerated]
        private sealed class <ShowPremiumActivatedDialog>c__AnonStorey0
        {
            internal PremiumActivatedUIComponent premiumActivatedUiComponent;
            internal List<Animator> animators;
            internal int days;

            internal void <>m__0()
            {
                this.premiumActivatedUiComponent.ShowPrem(this.animators, true, this.days, true);
            }

            internal void <>m__1()
            {
                this.premiumActivatedUiComponent.ShowPrem(this.animators, false, this.days, true);
            }

            internal void <>m__2()
            {
                this.premiumActivatedUiComponent.ShowPrem(this.animators, true, this.days, false);
            }

            internal void <>m__3()
            {
                this.premiumActivatedUiComponent.ShowPrem(this.animators, false, this.days, false);
            }
        }

        public class BaseUserNode : Node
        {
            public SelfUserComponent selfUser;
            public UserRankComponent userRank;
        }

        public class GetDiscountForOfferEvent : Event
        {
            public float Discount { get; set; }
        }

        public class PremiumBoostItemDurationChangedNode : Node
        {
            public PremiumBoostItemComponent premiumBoostItem;
            public DurationUserItemComponent durationUserItem;
            public PremiumDurationChangedComponent premiumDurationChanged;
        }

        public class PremiumGoodsNode : Node
        {
            public PremiumOfferComponent premiumOffer;
            public CountableItemsPackComponent countableItemsPack;
            public SpecialOfferContentLocalizationComponent specialOfferContentLocalization;
            public OrderItemComponent orderItem;
            public GoodsPriceComponent goodsPrice;
        }

        private class PremiumGoodsNodeComparer : IComparer<PremiumUiShopSystem.PremiumGoodsNode>
        {
            public int Compare(PremiumUiShopSystem.PremiumGoodsNode a, PremiumUiShopSystem.PremiumGoodsNode b) => 
                a.orderItem.Index.CompareTo(b.orderItem.Index);
        }

        public class PremiumQuestItemDurationChangedNode : Node
        {
            public PremiumQuestItemComponent premiumQuestItem;
            public DurationUserItemComponent durationUserItem;
            public PremiumDurationChangedComponent premiumDurationChanged;
        }

        public class SelfPremiumQuestUserNode : PremiumUiShopSystem.BaseUserNode
        {
            public PremiumAccountQuestComponent premiumAccountQuest;
        }

        public class SelfPremiumUserNode : PremiumUiShopSystem.BaseUserNode
        {
            public PremiumAccountBoostComponent premiumAccountBoost;
        }

        public class ShowPremiumActivatedDialogEvent : Event
        {
        }
    }
}

