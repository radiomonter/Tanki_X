namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Battle.ClientBattleSelect.Impl;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientPayment.Impl;

    public class XCrystalBonusRewardsSystem : ECSSystem
    {
        private static void AddRewardGroup(ScreenNode screen, Entity reward)
        {
            long key = reward.GetComponent<BattleRewardGroupComponent>().Key;
            screen.Entity.RemoveComponentIfPresent<BattleRewardGroupComponent>();
            screen.Entity.AddComponent(new BattleRewardGroupComponent(key));
        }

        [OnEventFire]
        public void disabeleGoBackWhenXCrystalsDialogActive(NodeAddedEvent e, SingleNode<BuyXCrystalsDialogComponent> dialog, [JoinAll] SingleNode<BackButtonComponent> back)
        {
            back.component.Disabled = true;
        }

        [OnEventFire]
        public void enableGoBackWhenXCrystalsDialogHide(NodeRemoveEvent e, SingleNode<BuyXCrystalsDialogComponent> dialog, [JoinAll] SingleNode<BackButtonComponent> back)
        {
            back.component.Disabled = false;
        }

        [OnEventFire]
        public void OnBonusRenew(NodeAddedEvent e, ActivePaymentSaleNode sale, ScreenNode screen, XCrystalPersonalRewardNode personalReward, [JoinBy(typeof(BattleRewardGroupComponent))] XCrystalRewardNode reward)
        {
            if (sale.activePaymentSale.PersonalXCrystalBonus)
            {
                screen.battleResultsAwardsScreen.specialOfferUI.ShowDiscountButtonIfXBonus();
            }
        }

        [OnEventFire]
        public void OnBonusUse(NodeRemoveEvent e, ActivePaymentSaleNode sale, [Context] ScreenNode screen)
        {
            screen.battleResultsAwardsScreen.specialOfferUI.HideDiscountButton();
        }

        [OnEventFire]
        public void OnBonusUsed(FinishPersonalXCrystalBonusEvent e, SingleNode<SelfUserComponent> user, [JoinAll] ResultsNode results)
        {
            Entity reward = results.battleResults.ResultForClient.PersonalResult.Reward;
            base.ScheduleEvent<ChangeRewardUiOnSuccessPaymentEvent>(reward);
        }

        [OnEventFire]
        public void ShowReward(NodeAddedEvent e, ScreenNode screen, [JoinAll] ResultsNode results)
        {
            Entity reward = results.battleResults.ResultForClient.PersonalResult.Reward;
            if (reward != null)
            {
                AddRewardGroup(screen, reward);
                base.Log.DebugFormat("ShowReward: reward={0}", reward.Id);
                base.NewEvent<ShowRewardEvent>().Attach(reward).Attach(screen).Schedule();
            }
        }

        [OnEventFire]
        public void ShowThankYou(ChangeRewardUiOnSuccessPaymentEvent e, XCrystalPersonalRewardNode personalReward, [JoinBy(typeof(BattleRewardGroupComponent))] XCrystalRewardNode reward, [JoinAll] ScreenNode screen, [JoinAll] SingleNode<Dialogs60Component> dialogs)
        {
            dialogs.component.GetComponentInChildren<BuyXCrystalsDialogComponent>(true).Hide();
            screen.battleResultsAwardsScreen.specialOfferUI.ShowSmile(reward.xCrystalRewardTextConfig.PurchaseText);
        }

        [OnEventFire]
        public void ShowXCrystalReward(ShowRewardEvent e, ScreenNode screen, XCrystalPersonalRewardNode personalReward, [JoinBy(typeof(BattleRewardGroupComponent))] XCrystalRewardNode reward)
        {
            base.Log.DebugFormat("ShowXCrystalReward: reward={0}", personalReward.Entity.Id);
            XCrystalBonusActivationReason activationReason = personalReward.xCrystalBonusPersonalReward.ActivationReason;
            string ribbonLabel = "x" + personalReward.xCrystalBonusPersonalReward.Multiplier.ToString();
            List<SpecialOfferItem> items = new List<SpecialOfferItem> {
                new SpecialOfferItem(0, reward.xCrystalRewardItemsConfig.SpriteUid, reward.xCrystalRewardItemsConfig.Title, ribbonLabel)
            };
            BattleResultSpecialOfferUiComponent specialOfferUI = screen.battleResultsAwardsScreen.specialOfferUI;
            specialOfferUI.ShowContent(reward.xCrystalRewardTextConfig.Title[activationReason], reward.xCrystalRewardTextConfig.Description[activationReason], items);
            specialOfferUI.SetUseDiscountButton();
            specialOfferUI.Appear();
        }

        [OnEventFire]
        public void ShowXCrystalsDialog(ShowXCrystalsDialogEvent e, Node any, [JoinAll] SingleNode<Dialogs60Component> dialogs)
        {
            dialogs.component.GetComponentInChildren<BuyXCrystalsDialogComponent>(true).Show(e.ShowTitle);
        }

        public class ActivePaymentSaleNode : Node
        {
            public ActivePaymentSaleComponent activePaymentSale;
            public SelfUserComponent selfUser;
        }

        public class PersonalRewardNode : Node
        {
            public PersonalBattleRewardComponent personalBattleReward;
            public BattleRewardGroupComponent battleRewardGroup;
        }

        public class ResultsNode : Node
        {
            public BattleResultsComponent battleResults;
        }

        public class ScreenNode : Node
        {
            public BattleResultsAwardsScreenComponent battleResultsAwardsScreen;
        }

        public class XCrystalPersonalRewardNode : XCrystalBonusRewardsSystem.PersonalRewardNode
        {
            public XCrystalBonusPersonalRewardComponent xCrystalBonusPersonalReward;
        }

        public class XCrystalRewardNode : Node
        {
            public XCrystalRewardTextConfigComponent xCrystalRewardTextConfig;
            public XCrystalRewardItemsConfigComponent xCrystalRewardItemsConfig;
        }
    }
}

