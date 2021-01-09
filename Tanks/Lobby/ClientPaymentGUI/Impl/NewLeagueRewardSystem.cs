namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using Lobby.ClientPayment.Impl;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientBattleSelect.Impl;
    using Tanks.Lobby.ClientBattleSelect.Impl;
    using Tanks.Lobby.ClientPayment.Impl;
    using UnityEngine;

    public class NewLeagueRewardSystem : ECSSystem
    {
        [OnEventFire]
        public void Cancel(PaymentIsCancelledEvent e, SingleNode<PaymentMethodComponent> node, [JoinAll] SingleNode<NewLeaguePurchaseItemComponent> deals)
        {
            base.Log.Error("Error making payment: " + e.ErrorCode);
            deals.component.HandleError();
        }

        [OnEventFire]
        public void ChangeButtonOnBuy(NodeAddedEvent e, SingleNode<SpecialOfferPaidComponent> personalOffer, [JoinBy(typeof(SpecialOfferGroupComponent))] SingleNode<LeagueFirstEntranceSpecialOfferComponent> specialOffer, [JoinAll] ScreenNode screen, [JoinBy(typeof(BattleRewardGroupComponent))] SingleNode<LeagueFirstEntrancePersonalBattleRewardComponent> reward)
        {
            <ChangeButtonOnBuy>c__AnonStorey0 storey = new <ChangeButtonOnBuy>c__AnonStorey0 {
                ui = screen.battleResultsAwardsScreen.specialOfferUI
            };
            KeyValuePair<long, int> pair = specialOffer.Entity.GetComponent<CountableItemsPackComponent>().Pack.FirstOrDefault<KeyValuePair<long, int>>();
            long key = pair.Key;
            int quantity = pair.Value;
            if (((key != 0L) && Flow.Current.EntityRegistry.ContainsEntity(key)) && Flow.Current.EntityRegistry.GetEntity(key).HasComponent<ContainerMarkerComponent>())
            {
                storey.ui.SetOpenButton(key, quantity, new Action(storey.<>m__0));
            }
        }

        [OnEventFire]
        public void Clean(NodeRemoveEvent e, ScreenNode screen)
        {
            NewLeaguePurchaseItemComponent component = screen.battleResultsAwardsScreen.specialOfferUI.gameObject.GetComponent<NewLeaguePurchaseItemComponent>();
            if (component != null)
            {
                Object.Destroy(component);
            }
        }

        [OnEventFire]
        public void GoToUrl(GoToUrlToPayEvent e, Node any, [JoinAll] ScreenNode screen)
        {
            NewLeaguePurchaseItemComponent component = screen.battleResultsAwardsScreen.specialOfferUI.GetComponent<NewLeaguePurchaseItemComponent>();
            if (component != null)
            {
                component.HandleGoToLink();
            }
        }

        [OnEventFire]
        public void OnPriceButtonClick(ButtonClickEvent e, SingleNode<SpecialOfferPriceButtonComponent> button, [JoinAll, Mandatory] ScreenNode screen, [JoinBy(typeof(BattleRewardGroupComponent))] SingleNode<LeagueFirstEntrancePersonalBattleRewardComponent> reward, [JoinAll, Mandatory] SingleNode<Dialogs60Component> dialogs)
        {
            Entity personalOffer = reward.component.PersonalOffer;
            ShopDialogs shopDialogs = dialogs.component.Get<ShopDialogs>();
            screen.battleResultsAwardsScreen.specialOfferUI.GetComponent<NewLeaguePurchaseItemComponent>().ShowPurchaseDialog(shopDialogs, Flow.Current.EntityRegistry.GetEntity(personalOffer.GetComponent<SpecialOfferGroupComponent>().Key), false);
        }

        [OnEventFire]
        public void QiwiError(InvalidQiwiAccountEvent e, Node node, [JoinAll] SingleNode<NewLeaguePurchaseItemComponent> deals)
        {
            base.Log.Error("QIWI ERROR");
            deals.component.HandleQiwiError();
        }

        [OnEventFire]
        public void ShowLeagueReward(ShowRewardEvent e, ScreenNode screen, LeaguePersonalRewardNode reward, [JoinAll] ICollection<SingleNode<PaymentMethodComponent>> methods)
        {
            Entity personalOffer = reward.leagueFirstEntrancePersonalBattleReward.PersonalOffer;
            Entity entity = Flow.Current.EntityRegistry.GetEntity(personalOffer.GetComponent<SpecialOfferGroupComponent>().Key);
            SpecialOfferContentLocalizationComponent component = entity.GetComponent<SpecialOfferContentLocalizationComponent>();
            string title = component.Title;
            string description = component.Description;
            List<SpecialOfferItem> items = new List<SpecialOfferItem>();
            foreach (KeyValuePair<long, int> pair in entity.GetComponent<CountableItemsPackComponent>().Pack)
            {
                long key = pair.Key;
                Entity entity3 = Flow.Current.EntityRegistry.GetEntity(key);
                int quantity = pair.Value;
                string spriteUid = entity3.GetComponent<ImageItemComponent>().SpriteUid;
                string name = entity3.GetComponent<DescriptionItemComponent>().Name;
                items.Add(new SpecialOfferItem(quantity, spriteUid, name));
            }
            double price = entity.GetComponent<GoodsPriceComponent>().Price;
            string currency = entity.GetComponent<GoodsPriceComponent>().Currency;
            int discount = 0;
            int labelPercentage = 0;
            if (entity.HasComponent<LeagueFirstEntranceSpecialOfferComponent>())
            {
                labelPercentage = entity.GetComponent<LeagueFirstEntranceSpecialOfferComponent>().WorthItPercent;
            }
            BattleResultSpecialOfferUiComponent specialOfferUI = screen.battleResultsAwardsScreen.specialOfferUI;
            specialOfferUI.ShowContent(title, description, items);
            specialOfferUI.SetPriceButton(discount, price, labelPercentage, currency);
            specialOfferUI.Appear();
            specialOfferUI.gameObject.AddComponent<NewLeaguePurchaseItemComponent>();
            foreach (SingleNode<PaymentMethodComponent> node in methods)
            {
                specialOfferUI.gameObject.GetComponent<NewLeaguePurchaseItemComponent>().AddMethod(node.Entity);
            }
        }

        [OnEventComplete]
        public void SteamComponentAdded(NodeAddedEvent e, SingleNode<SteamComponent> stemNode, [Context] SingleNode<NewLeaguePurchaseItemComponent> starterPack)
        {
            starterPack.component.SteamComponentIsPresent = true;
        }

        [OnEventFire]
        public void Success(SuccessPaymentEvent e, SingleNode<PaymentMethodComponent> node, [JoinAll] SingleNode<NewLeaguePurchaseItemComponent> deals)
        {
            deals.component.HandleSuccess();
        }

        [OnEventFire]
        public void SuccessMobile(SuccessMobilePaymentEvent e, SingleNode<PaymentMethodComponent> node, [JoinAll] SingleNode<NewLeaguePurchaseItemComponent> deals)
        {
            deals.component.HandleSuccessMobile(e.TransactionId);
        }

        [CompilerGenerated]
        private sealed class <ChangeButtonOnBuy>c__AnonStorey0
        {
            internal BattleResultSpecialOfferUiComponent ui;

            internal void <>m__0()
            {
                this.ui.DeactivateAllButtons();
            }
        }

        public class BattleResultsNode : Node
        {
            public BattleResultsComponent battleResults;
        }

        public class LeaguePersonalRewardNode : NewLeagueRewardSystem.PersonalRewardNode
        {
            public LeagueFirstEntrancePersonalBattleRewardComponent leagueFirstEntrancePersonalBattleReward;
        }

        public class PersonalRewardNode : Node
        {
            public PersonalBattleRewardComponent personalBattleReward;
            public BattleRewardGroupComponent battleRewardGroup;
        }

        public class ScreenNode : Node
        {
            public BattleResultsAwardsScreenComponent battleResultsAwardsScreen;
        }
    }
}

