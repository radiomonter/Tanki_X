namespace Tanks.Lobby.ClientGarage.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.API;

    public class ModulesListSystem : ECSSystem
    {
        [OnEventFire]
        public void ClearModules(NodeRemoveEvent e, SingleNode<ModulesScreenUIComponent> screen)
        {
            screen.component.Clear();
        }

        [OnEventFire]
        public void ModuleInited(NodeAddedEvent e, ModuleWithUINode moduleWithUI, [JoinByParentGroup] Optional<ModuleCardNode> moduleCard)
        {
            moduleWithUI.moduleCardItemUi.Type = moduleWithUI.moduleBehaviourType.Type;
            moduleWithUI.moduleCardItemUi.TankPart = moduleWithUI.moduleTankPart.TankPart;
            moduleWithUI.moduleCardItemUi.Icon = moduleWithUI.itemBigIcon.SpriteUid;
            moduleWithUI.moduleCardItemUi.Level = -1;
            moduleWithUI.moduleCardItemUi.ModuleName = moduleWithUI.descriptionItem.Name;
            moduleWithUI.moduleCardItemUi.MaxCardsCount = moduleWithUI.moduleCardsComposition.CraftPrice.Cards;
            moduleWithUI.moduleCardItemUi.CardsCount = !moduleCard.IsPresent() ? 0 : ((int) moduleCard.Get().userItemCounter.Count);
            moduleWithUI.moduleCardItemUi.Active = moduleWithUI.moduleBehaviourType.Type == ModuleBehaviourType.ACTIVE;
        }

        [OnEventFire]
        public void ModulesMarketItemsInit(NodeAddedEvent e, SingleNode<ModulesScreenUIComponent> screen, [Combine] MarketModuleNode module)
        {
            screen.component.AddCard(module.Entity);
        }

        [OnEventFire]
        public void ModuleUserInited(NodeAddedEvent e, [Combine] UserModuleNode userModule, [JoinByMarketItem, Context] ModuleWithUINode moduleWithUI, [JoinByParentGroup] Optional<ModuleCardNode> moduleCards, [JoinAll] Optional<SelectedSlotWithModuleNode> selectedSlot, [JoinAll] SelfUserNode selfUser)
        {
            if (userModule.userGroup.Key == selfUser.userGroup.Key)
            {
                moduleWithUI.Entity.RemoveComponent<ModuleCardItemUIComponent>();
                moduleWithUI.Entity.RemoveComponent<ToggleListItemComponent>();
                if (moduleWithUI.Entity.HasComponent<ToggleListSelectedItemComponent>())
                {
                    moduleWithUI.Entity.RemoveComponent<ToggleListSelectedItemComponent>();
                }
                if (userModule.Entity.HasComponent<ModuleCardItemUIComponent>())
                {
                    userModule.Entity.RemoveComponent<ModuleCardItemUIComponent>();
                }
                if (userModule.Entity.HasComponent<ToggleListItemComponent>())
                {
                    userModule.Entity.RemoveComponent<ToggleListItemComponent>();
                }
                userModule.Entity.AddComponent(moduleWithUI.moduleCardItemUi);
                userModule.Entity.AddComponent(moduleWithUI.moduleCardItemUi.GetComponent<ToggleListItemComponent>());
                this.SetModuleLevel(moduleWithUI.moduleCardItemUi, userModule.moduleUpgradeLevel, moduleWithUI.moduleCardsComposition, moduleCards);
                this.SelectMountedModuleCard(moduleWithUI.moduleCardItemUi, userModule.moduleGroup, selectedSlot);
            }
        }

        [OnEventFire]
        public void ModuleWasUpgrade(ModuleUpgradedEvent e, UserModuleWithUINode userModuleWithUI, [JoinByParentGroup] Optional<ModuleCardNode> moduleCards, [JoinAll] Optional<SelectedSlotWithModuleNode> selectedSlot)
        {
            this.SetModuleLevel(userModuleWithUI.moduleCardItemUi, userModuleWithUI.moduleUpgradeLevel, userModuleWithUI.moduleCardsComposition, moduleCards);
            this.SelectMountedModuleCard(userModuleWithUI.moduleCardItemUi, userModuleWithUI.moduleGroup, selectedSlot);
        }

        private void SelectMountedModuleCard(ModuleCardItemUIComponent moduleCardItemUi, ModuleGroupComponent userModuleGroup, Optional<SelectedSlotWithModuleNode> selectedSlot)
        {
            ToggleListItemComponent component = moduleCardItemUi.GetComponent<ToggleListItemComponent>();
            if (selectedSlot.IsPresent() && (selectedSlot.Get().moduleGroup.Key == userModuleGroup.Key))
            {
                component.Toggle.isOn = true;
            }
            else if (component.Toggle.isOn)
            {
                component.OnValueChangedListener();
            }
        }

        private void SetModuleLevel(ModuleCardItemUIComponent moduleCardItemUi, ModuleUpgradeLevelComponent moduleUpgradeLevel, ModuleCardsCompositionComponent moduleCardsComposition, Optional<ModuleCardNode> moduleCards)
        {
            int level = (int) moduleUpgradeLevel.Level;
            moduleCardItemUi.Level = level + 1;
            List<ModulePrice> upgradePrices = moduleCardsComposition.UpgradePrices;
            moduleCardItemUi.MaxCardsCount = (level >= upgradePrices.Count) ? 0 : upgradePrices[level].Cards;
            moduleCardItemUi.CardsCount = !moduleCards.IsPresent() ? 0 : ((int) moduleCards.Get().userItemCounter.Count);
        }

        public class MarketModuleNode : ModulesListSystem.ModuleNode
        {
            public MarketItemComponent marketItem;
        }

        public class ModuleCardNode : Node
        {
            public ModuleCardItemComponent moduleCardItem;
            public UserItemComponent userItem;
            public UserItemCounterComponent userItemCounter;
        }

        [Not(typeof(ImmutableModuleItemComponent))]
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

        public class ModuleWithUINode : ModulesListSystem.MarketModuleNode
        {
            public ModuleCardItemUIComponent moduleCardItemUi;
            public ToggleListItemComponent toggleListItem;
        }

        public class SelectedSlot : Node
        {
            public SlotUserItemInfoComponent slotUserItemInfo;
            public ToggleListSelectedItemComponent toggleListSelectedItem;
        }

        public class SelectedSlotWithModuleNode : Node
        {
            public SlotUIComponent slotUI;
            public SlotUserItemInfoComponent slotUserItemInfo;
            public ToggleListSelectedItemComponent toggleListSelectedItem;
            public ModuleGroupComponent moduleGroup;
        }

        public class SelfUserNode : Node
        {
            public SelfUserComponent selfUser;
            public UserGroupComponent userGroup;
        }

        public class UserModuleNode : ModulesListSystem.ModuleNode
        {
            public UserItemComponent userItem;
            public ModuleUpgradeLevelComponent moduleUpgradeLevel;
            public ModuleGroupComponent moduleGroup;
            public UserGroupComponent userGroup;
        }

        public class UserModuleWithUINode : ModulesListSystem.UserModuleNode
        {
            public ModuleCardItemUIComponent moduleCardItemUi;
            public ToggleListItemComponent toggleListItem;
        }
    }
}

