namespace Tanks.Lobby.ClientGarage.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.API;
    using UnityEngine;

    public class ModulesBadgesSystem : ECSSystem
    {
        private string GetPlayerPrefsKey(long moduleId) => 
            "moduleLastWatchedLevel-" + moduleId;

        private bool ModuleLevelWasWatched(long moduleId, long currentLevel)
        {
            string playerPrefsKey = this.GetPlayerPrefsKey(moduleId);
            return (PlayerPrefs.HasKey(playerPrefsKey) && (PlayerPrefs.GetInt(playerPrefsKey) >= currentLevel));
        }

        [OnEventFire]
        public void ModuleUIAdded(NodeAddedEvent e, SelectedModuleUI module)
        {
            long currentLevel = !module.Entity.HasComponent<ModuleUpgradeLevelComponent>() ? -1L : module.Entity.GetComponent<ModuleUpgradeLevelComponent>().Level;
            this.ModuleWasWatched(module.marketItemGroup.Key, currentLevel);
        }

        private void ModuleWasWatched(long moduleId, long currentLevel)
        {
            PlayerPrefs.SetInt(this.GetPlayerPrefsKey(moduleId), (int) currentLevel);
        }

        [OnEventFire]
        public void NotificationBadgeInit(NodeAddedEvent e, [Combine] SingleNode<ModulesNotificationBadgeComponent> modulesNotificationBadge, SelfUserNode selfUser, SelectedPresetNode selectedPreset, [JoinAll] ICollection<MarketModuleNode> marketModules)
        {
            <NotificationBadgeInit>c__AnonStorey0 storey = new <NotificationBadgeInit>c__AnonStorey0 {
                selfUser = selfUser
            };
            if (selectedPreset.userGroup.Key != storey.selfUser.userGroup.Key)
            {
                modulesNotificationBadge.component.CurrentState = State.None;
            }
            else
            {
                modulesNotificationBadge.component.CurrentState = State.None;
                foreach (MarketModuleNode node in marketModules)
                {
                    if (modulesNotificationBadge.component.TankPart == node.moduleTankPart.TankPart)
                    {
                        long count = 0L;
                        IList<ModuleCardNode> source = base.Select<ModuleCardNode>(node.Entity, typeof(ParentGroupComponent));
                        if (source.Count > 0)
                        {
                            count = source.Single<ModuleCardNode>().userItemCounter.Count;
                        }
                        if (count != 0L)
                        {
                            List<UserModuleNode> list2 = base.Select<UserModuleNode>(node.Entity, typeof(ParentGroupComponent)).Where<UserModuleNode>(new Func<UserModuleNode, bool>(storey.<>m__0)).ToList<UserModuleNode>();
                            if (list2.Count <= 0)
                            {
                                if ((count >= node.moduleCardsComposition.CraftPrice.Cards) && !this.ModuleLevelWasWatched(node.marketItemGroup.Key, -1L))
                                {
                                    modulesNotificationBadge.component.CurrentState = State.NewModuleAvailable;
                                    break;
                                }
                            }
                            else if (modulesNotificationBadge.component.CurrentState != State.ModuleUpgradeAvailable)
                            {
                                long level = list2.Single<UserModuleNode>().moduleUpgradeLevel.Level;
                                if ((level < node.moduleCardsComposition.UpgradePrices.Count) && ((count >= node.moduleCardsComposition.UpgradePrices[(int) level].Cards) && !this.ModuleLevelWasWatched(node.marketItemGroup.Key, level)))
                                {
                                    modulesNotificationBadge.component.CurrentState = State.ModuleUpgradeAvailable;
                                }
                            }
                        }
                    }
                }
            }
        }

        [CompilerGenerated]
        private sealed class <NotificationBadgeInit>c__AnonStorey0
        {
            internal ModulesBadgesSystem.SelfUserNode selfUser;

            internal bool <>m__0(ModulesBadgesSystem.UserModuleNode m) => 
                m.userGroup.Key == this.selfUser.userGroup.Key;
        }

        public class MarketModuleNode : ModulesBadgesSystem.ModuleNode
        {
            public MarketItemComponent marketItem;
        }

        public class ModuleCardNode : Node
        {
            public ModuleCardItemComponent moduleCardItem;
            public UserItemComponent userItem;
            public UserItemCounterComponent userItemCounter;
        }

        public class ModuleNode : Node
        {
            public ModuleItemComponent moduleItem;
            public MarketItemGroupComponent marketItemGroup;
            public ModuleBehaviourTypeComponent moduleBehaviourType;
            public ModuleTierComponent moduleTier;
            public ModuleTankPartComponent moduleTankPart;
            public DescriptionItemComponent descriptionItem;
            public ItemIconComponent itemIcon;
            public ItemBigIconComponent itemBigIcon;
            public OrderItemComponent orderItem;
            public ModuleCardsCompositionComponent moduleCardsComposition;
        }

        public class SelectedModuleUI : ModulesBadgesSystem.ModuleNode
        {
            public ModuleCardItemUIComponent moduleCardItemUI;
            public ToggleListSelectedItemComponent toggleListSelectedItem;
        }

        public class SelectedPresetNode : Node
        {
            public SelectedPresetComponent selectedPreset;
            public UserGroupComponent userGroup;
        }

        public class SelfUserNode : Node
        {
            public SelfUserComponent selfUser;
            public UserGroupComponent userGroup;
            public UserComponent user;
        }

        public class UserModuleNode : ModulesBadgesSystem.ModuleNode
        {
            public UserItemComponent userItem;
            public ModuleUpgradeLevelComponent moduleUpgradeLevel;
            public UserGroupComponent userGroup;
            public ModuleGroupComponent moduleGroup;
        }
    }
}

