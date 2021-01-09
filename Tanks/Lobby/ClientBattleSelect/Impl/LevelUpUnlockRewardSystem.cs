namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientGarage.Impl;

    public class LevelUpUnlockRewardSystem : ECSSystem
    {
        [OnEventFire]
        public void OnTakeReward(ButtonClickEvent e, SingleNode<SpecialOfferTakeRewardButtonComponent> button, [JoinAll] ScreenNode screen)
        {
            screen.battleResultsAwardsScreen.specialOfferUI.DeactivateAllButtons();
        }

        [OnEventFire]
        public void ShowLevelUpUnlockReward(ShowRewardEvent e, ScreenNode screen, PersonalRewardNode personalReward, [JoinBy(typeof(BattleRewardGroupComponent))] LevelUpRewardNode reward)
        {
            <ShowLevelUpUnlockReward>c__AnonStorey0 storey = new <ShowLevelUpUnlockReward>c__AnonStorey0 {
                reward = reward
            };
            base.Log.DebugFormat("ShowLevelUpUnlockReward: reward={0}", personalReward.Entity.Id);
            storey.items = new List<SpecialOfferItem>();
            personalReward.levelUpUnlockPersonalReward.Unlocked.ForEach(new Action<Entity>(storey.<>m__0));
            BattleResultSpecialOfferUiComponent specialOfferUI = screen.battleResultsAwardsScreen.specialOfferUI;
            specialOfferUI.ShowContent(storey.reward.descriptionItem.Name, storey.reward.descriptionItem.Description, storey.items);
            specialOfferUI.SetTakeRewardButton();
            specialOfferUI.Appear();
        }

        [Inject]
        public static Tanks.Lobby.ClientGarage.API.GarageItemsRegistry GarageItemsRegistry { get; set; }

        [CompilerGenerated]
        private sealed class <ShowLevelUpUnlockReward>c__AnonStorey0
        {
            internal LevelUpUnlockRewardSystem.LevelUpRewardNode reward;
            internal List<SpecialOfferItem> items;

            internal void <>m__0(Entity unlockedItem)
            {
                GarageItem item = LevelUpUnlockRewardSystem.GarageItemsRegistry.GetItem<GarageItem>(unlockedItem.Id);
                string preview = item.Preview;
                string name = item.Name;
                if (unlockedItem.HasComponent<SlotItemComponent>())
                {
                    TankPartModuleType tankPart = unlockedItem.GetComponent<SlotTankPartComponent>().TankPart;
                    if (unlockedItem.GetComponent<SlotUserItemInfoComponent>().ModuleBehaviourType == ModuleBehaviourType.ACTIVE)
                    {
                        preview = this.reward.levelUpUnlockRewardConfig.ActiveSlotSpriteUid;
                        name = (tankPart != TankPartModuleType.TANK) ? this.reward.levelUpUnlockRewardConfig.ActiveSlotWeaponText : this.reward.levelUpUnlockRewardConfig.ActiveSlotHullText;
                    }
                    else
                    {
                        preview = this.reward.levelUpUnlockRewardConfig.PassiveSlotSpriteUid;
                        name = (tankPart != TankPartModuleType.TANK) ? this.reward.levelUpUnlockRewardConfig.PassiveSlotWeaponText : this.reward.levelUpUnlockRewardConfig.PassiveSlotHullText;
                    }
                }
                SpecialOfferItem item2 = new SpecialOfferItem(0, preview, name);
                this.items.Add(item2);
            }
        }

        public class LevelUpRewardNode : Node
        {
            public DescriptionItemComponent descriptionItem;
            public LevelUpUnlockRewardConfigComponent levelUpUnlockRewardConfig;
        }

        public class PersonalRewardNode : Node
        {
            public PersonalBattleRewardComponent personalBattleReward;
            public BattleRewardGroupComponent battleRewardGroup;
            public LevelUpUnlockPersonalRewardComponent levelUpUnlockPersonalReward;
        }

        public class ScreenNode : Node
        {
            public BattleResultsAwardsScreenComponent battleResultsAwardsScreen;
        }
    }
}

