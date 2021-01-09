namespace Tanks.Lobby.ClientGarage.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientNavigation.API;
    using Tanks.Lobby.ClientPayment.API;
    using Tanks.Lobby.ClientProfile.API;

    public class ModulesScreenSystem : ECSSystem
    {
        [CompilerGenerated]
        private static Func<ChestItem, int> <>f__am$cache0;
        [CompilerGenerated]
        private static Func<ChestItem, int> <>f__am$cache1;

        private ModuleTooltipData GetModuleTooltipData(ModuleNode moduleNode, [JoinByMarketItem] ModuleEffectUpgradablePropertyNode moduleEffectUpgradableProperties)
        {
            string name = moduleNode.descriptionItem.Name;
            return new ModuleTooltipData(name, moduleNode.descriptionItem.Description, !moduleNode.Entity.HasComponent<ModuleUpgradeLevelComponent>() ? -1 : ((int) moduleNode.Entity.GetComponent<ModuleUpgradeLevelComponent>().Level), moduleNode.moduleCardsComposition.UpgradePrices.Count, moduleEffectUpgradableProperties.moduleVisualProperties.Properties);
        }

        private void GoToChest(int tier, ICollection<ChestItem> chests)
        {
            <GoToChest>c__AnonStorey0 storey = new <GoToChest>c__AnonStorey0 {
                tier = tier,
                $this = this
            };
            IEnumerable<ChestItem> source = chests.Where<ChestItem>(new Func<ChestItem, bool>(storey.<>m__0));
            ChestItem node = null;
            if (source.Any<ChestItem>())
            {
                <GoToChest>c__AnonStorey1 storey2 = new <GoToChest>c__AnonStorey1 {
                    <>f__ref$0 = storey
                };
                if (<>f__am$cache0 == null)
                {
                    <>f__am$cache0 = x => x.targetTier.MaxExistTier;
                }
                storey2.minTier = source.Min<ChestItem>(<>f__am$cache0);
                node = source.Where<ChestItem>(new Func<ChestItem, bool>(storey2.<>m__0)).OrderBy<ChestItem, int>(new Func<ChestItem, int>(storey2.<>m__1)).FirstOrDefault<ChestItem>();
            }
            if (node == null)
            {
                if (<>f__am$cache1 == null)
                {
                    <>f__am$cache1 = x => x.targetTier.MaxExistTier;
                }
                node = chests.Where<ChestItem>(new Func<ChestItem, bool>(storey.<>m__1)).OrderBy<ChestItem, int>(<>f__am$cache1).FirstOrDefault<ChestItem>();
            }
            ShowGarageCategoryEvent eventInstance = new ShowGarageCategoryEvent {
                Category = GarageCategory.BLUEPRINTS,
                SelectedItem = node.Entity
            };
            base.ScheduleEvent(eventInstance, node);
        }

        [OnEventFire]
        public void MarketModuleSelected(NodeAddedEvent e, SelectedMarketModuleNode selectedMarketModule, [JoinByMarketItem] ModuleEffectUpgradablePropertyNode moduleEffectUpgradableProperties, [JoinAll] ScreenNode screen, [JoinAll] ICollection<SlotNode> slots, [JoinAll] SelectedSlotNode selectedSlot)
        {
            this.ModuleSelected(selectedMarketModule, moduleEffectUpgradableProperties, screen, slots, selectedSlot);
        }

        [OnEventFire]
        public void ModuleMounted(NodeAddedEvent e, SelectedSlotWithModuleNode selectedSlotWithModule, [JoinAll] SelectedUserModuleNode selectedUserModuleNode, [JoinByMarketItem] ModuleEffectUpgradablePropertyNode moduleEffectUpgradableProperties, [JoinAll] ScreenNode screen, [JoinAll] ICollection<SlotNode> slots)
        {
            this.ModuleSelected(selectedUserModuleNode, moduleEffectUpgradableProperties, screen, slots, selectedSlotWithModule);
        }

        public void ModuleSelected(ModuleNode moduleNode, ModuleEffectUpgradablePropertyNode moduleEffectUpgradableProperties, ScreenNode screen, ICollection<SlotNode> slots, SelectedSlotNode selectedSlot)
        {
            if (selectedSlot.slotUserItemInfo.ModuleBehaviourType != moduleNode.moduleBehaviourType.Type)
            {
                foreach (SlotNode node in slots)
                {
                    if ((!node.slotUI.Locked && ((node.slotUserItemInfo.ModuleBehaviourType == moduleNode.moduleBehaviourType.Type) && (node.slotUI.TankPart == moduleNode.moduleTankPart.TankPart))) && (base.Select<UserModuleNode>(node.Entity, typeof(ModuleGroupComponent)).Count <= 0))
                    {
                        node.slotUI.GetComponent<ToggleListItemComponent>().Toggle.isOn = true;
                        break;
                    }
                }
            }
            screen.modulesScreenUi.ModuleName = moduleNode.descriptionItem.Name;
            screen.modulesScreenUi.ModuleDesc = moduleNode.descriptionItem.Description;
            screen.modulesScreenUi.ModuleFlavorText = moduleNode.descriptionItem.Flavor;
            screen.modulesScreenUi.ModuleIconUID = moduleNode.itemIcon.SpriteUid;
            int moduleUpgradeLevel = !moduleNode.Entity.HasComponent<ModuleUpgradeLevelComponent>() ? -1 : ((int) moduleNode.Entity.GetComponent<ModuleUpgradeLevelComponent>().Level);
            int count = moduleNode.moduleCardsComposition.UpgradePrices.Count;
            if ((moduleUpgradeLevel != -1) && (moduleUpgradeLevel != count))
            {
                screen.modulesScreenUi.CurrentUpgradeLevel = moduleUpgradeLevel + 1;
                screen.modulesScreenUi.NextUpgradeLevel = moduleUpgradeLevel + 2;
            }
            else
            {
                int num3 = -1;
                screen.modulesScreenUi.NextUpgradeLevel = num3;
                screen.modulesScreenUi.CurrentUpgradeLevel = num3;
            }
            screen.modulesScreenUi.ModulesProperties.Clear();
            GetItemStatEvent eventInstance = new GetItemStatEvent();
            base.ScheduleEvent(eventInstance, moduleNode);
            ModuleUpgradeCharacteristic moduleUpgradeCharacteristic = eventInstance.moduleUpgradeCharacteristic;
            screen.modulesScreenUi.ModulesProperties.AddProperty(moduleUpgradeCharacteristic.Name, moduleUpgradeCharacteristic.Unit, moduleUpgradeCharacteristic.CurrentStr, moduleUpgradeCharacteristic.NextStr, moduleUpgradeCharacteristic.Min, moduleUpgradeCharacteristic.Max, moduleUpgradeCharacteristic.Current, moduleUpgradeCharacteristic.Next, string.Empty);
            for (int i = 0; i < moduleEffectUpgradableProperties.moduleVisualProperties.Properties.Count; i++)
            {
                ModuleVisualProperty property = moduleEffectUpgradableProperties.moduleVisualProperties.Properties[i];
                if (property.Upgradable && ((moduleUpgradeLevel != count) && (moduleUpgradeLevel != -1)))
                {
                    float minValue = 0f;
                    float maxValue = property.CalculateModuleEffectPropertyValue(count, count);
                    screen.modulesScreenUi.ModulesProperties.AddProperty(property.Name, property.Unit, minValue, maxValue, (moduleUpgradeLevel == -1) ? 0f : property.CalculateModuleEffectPropertyValue(moduleUpgradeLevel, count), property.CalculateModuleEffectPropertyValue(moduleUpgradeLevel + 1, count), property.Format);
                }
                else if (moduleUpgradeLevel == -1)
                {
                    float currentValue = property.CalculateModuleEffectPropertyValue(0, count);
                    screen.modulesScreenUi.ModulesProperties.AddProperty(property.Name, property.Unit, currentValue, property.Format);
                }
                else
                {
                    float currentValue = property.CalculateModuleEffectPropertyValue(count, count);
                    screen.modulesScreenUi.ModulesProperties.AddProperty(property.Name, property.Unit, currentValue, property.Format);
                }
            }
        }

        [OnEventFire]
        public void ModuleUnmounted(NodeRemoveEvent e, SelectedSlotWithModuleNode selectedSlotWithModule, [JoinAll] SelectedUserModuleNode selectedUserModuleNode, [JoinByMarketItem] ModuleEffectUpgradablePropertyNode moduleEffectUpgradableProperties, [JoinAll] ScreenNode screen, [JoinAll] ICollection<SlotNode> slots)
        {
            this.ModuleSelected(selectedUserModuleNode, moduleEffectUpgradableProperties, screen, slots, selectedSlotWithModule);
        }

        [OnEventFire]
        public void MountModule(ButtonClickEvent e, SingleNode<MountModuleButtonComponent> mountButton, [JoinAll] SelectedSlotNode selectedSlotNode, [JoinAll] SelectedUserModuleNode selectedModule)
        {
            if (mountButton.component.mount)
            {
                Node[] nodes = new Node[] { selectedModule, selectedSlotNode };
                base.NewEvent<ModuleMountEvent>().AttachAll(nodes).Schedule();
            }
            else
            {
                Node[] nodes = new Node[] { selectedModule, selectedSlotNode };
                base.NewEvent<UnmountModuleFromSlotEvent>().AttachAll(nodes).Schedule();
            }
        }

        [OnEventFire]
        public void SendAssemble(DialogConfirmEvent e, SingleNode<ModuleAssembleNotEnouthCardWindowComponent> window, [JoinAll] ICollection<ChestItem> chests)
        {
            this.GoToChest(window.component.Tier, chests);
        }

        [OnEventFire]
        public void SendAssemble(DialogConfirmEvent e, SingleNode<ModuleAssembleConfirmWindowComponent> window, [JoinByMarketItem] MarketModuleNode module, [JoinAll] SingleNode<SelfUserComponent> user)
        {
            base.NewEvent<ModuleAssembleEvent>().Attach(module).Attach(user).Schedule();
        }

        [OnEventFire]
        public void SendUpgrade(DialogConfirmEvent e, SingleNode<ModuleUpgradeConfirmWindowComponent> window, [JoinByMarketItem] MarketModuleNode module, [JoinByMarketItem] UserModuleNode userModule, [JoinByUser] SingleNode<SelfUserComponent> user)
        {
            base.NewEvent<UpgradeModuleWithCrystalsEvent>().Attach(userModule).Schedule();
        }

        [OnEventFire]
        public void SendUpgradeXCry(DialogConfirmEvent e, SingleNode<ModuleUpgradeXCryConfirmWindowComponent> window, [JoinByMarketItem] MarketModuleNode module, [JoinByMarketItem] UserModuleNode userModule, [JoinByUser] SingleNode<SelfUserComponent> user)
        {
            base.NewEvent<UpgradeModuleWithXCrystalsEvent>().Attach(userModule).Schedule();
        }

        private void ShowAssemblyConfirmDialog(ModuleNode module, SingleNode<Dialogs60Component> dialogs, Optional<SingleNode<WindowsSpaceComponent>> screens)
        {
            this.ShowDialog(module, dialogs, screens, false);
        }

        private void ShowDialog(ModuleNode module, SingleNode<Dialogs60Component> dialogs, Optional<SingleNode<WindowsSpaceComponent>> screens, bool useXCry = false)
        {
            bool craft = module is MarketModuleNode;
            int level = 0;
            if (!craft && module.Entity.HasComponent<ModuleUpgradeLevelComponent>())
            {
                level = (int) module.Entity.GetComponent<ModuleUpgradeLevelComponent>().Level;
                if (level >= module.moduleCardsComposition.UpgradePrices.Count)
                {
                    return;
                }
            }
            bool dontenoughtcard = false;
            if (craft)
            {
                dontenoughtcard = !module.Entity.GetComponent<ModuleCardItemUIComponent>().ActivateAvailable;
            }
            double price = !craft ? (!useXCry ? ((double) module.moduleCardsComposition.UpgradePrices[level].Crystals) : ((double) module.moduleCardsComposition.UpgradePrices[level].XCrystals)) : ((double) module.moduleCardsComposition.CraftPrice.Crystals);
            CraftModuleConfirmWindowComponent component = dialogs.component.Get<CraftModuleConfirmWindowComponent>();
            component.Setup(module.descriptionItem.Name, module.descriptionItem.Description, module.itemIcon.SpriteUid, price, craft, !useXCry ? "8" : "9", dontenoughtcard);
            List<Animator> animators = new List<Animator>();
            if (screens.IsPresent())
            {
                animators = screens.Get().component.Animators;
            }
            component.Show(animators);
            Entity entity = component.GetComponent<EntityBehaviour>().Entity;
            if (craft)
            {
                if (module.Entity.GetComponent<ModuleCardItemUIComponent>().ActivateAvailable)
                {
                    entity.AddComponent<ModuleAssembleConfirmWindowComponent>();
                }
                else
                {
                    entity.AddComponent(new ModuleAssembleNotEnouthCardWindowComponent(module.moduleTier.TierNumber));
                }
            }
            else if (useXCry)
            {
                entity.AddComponent<ModuleUpgradeXCryConfirmWindowComponent>();
            }
            else
            {
                entity.AddComponent<ModuleUpgradeConfirmWindowComponent>();
            }
            module.Entity.GetComponent<MarketItemGroupComponent>().Attach(entity);
        }

        [OnEventFire]
        public void ShowModuleTooltip(ModuleTooltipShowEvent e, ModuleWithUI moduleNode, [JoinByMarketItem] ModuleEffectUpgradablePropertyNode moduleEffectUpgradableProperties)
        {
            ModuleTooltipData moduleTooltipData = this.GetModuleTooltipData(moduleNode, moduleEffectUpgradableProperties);
            moduleNode.moduleCardItemUI.GetComponent<ModuleTooltipShowComponent>().ShowTooltip(moduleTooltipData);
        }

        [OnEventFire]
        public void ShowSlotTooltip(ModuleTooltipShowEvent e, SingleNode<SlotUIComponent> slot, [JoinByModule] Optional<ModuleNode> module, [JoinByMarketItem] Optional<ModuleEffectUpgradablePropertyNode> moduleEffectUpgradableProperties)
        {
            if (!slot.Entity.HasComponent<ModuleGroupComponent>())
            {
                slot.component.GetComponent<SlotTooltipShowComponent>().ShowEmptySlotTooltip();
            }
            else if (module.IsPresent() && moduleEffectUpgradableProperties.IsPresent())
            {
                ModuleTooltipData moduleTooltipData = this.GetModuleTooltipData(module.Get(), moduleEffectUpgradableProperties.Get());
                slot.component.GetComponent<SlotTooltipShowComponent>().ShowModuleTooltip(moduleTooltipData);
            }
        }

        private void ShowUpgradeConfirmDialog(ModuleNode module, SingleNode<Dialogs60Component> dialogs, Optional<SingleNode<WindowsSpaceComponent>> screens)
        {
            this.ShowDialog(module, dialogs, screens, false);
        }

        private void ShowUpgradeXCryConfirmDialog(ModuleNode module, SingleNode<Dialogs60Component> dialogs, Optional<SingleNode<WindowsSpaceComponent>> screens)
        {
            this.ShowDialog(module, dialogs, screens, true);
        }

        [OnEventComplete]
        public void SlotSelected(NodeAddedEvent e, SelectedSlotNode selectedSlot, [JoinByModule] Optional<UserModuleNode> mountedUserModule, [JoinAll] SingleNode<ModulesScreenUIComponent> screen)
        {
            screen.component.FilterCards(!mountedUserModule.IsPresent() ? null : mountedUserModule.Get().Entity, selectedSlot.slotUserItemInfo.ModuleBehaviourType);
            screen.component.ModuleActive = selectedSlot.slotUserItemInfo.ModuleBehaviourType == ModuleBehaviourType.ACTIVE;
        }

        [OnEventFire]
        public void UpgradeModule(ButtonClickEvent e, SingleNode<UpgradeModuleButtonComponent> mountButton, [JoinAll] SelectedUserModuleWithUINode module, [JoinAll] SingleNode<Dialogs60Component> dialogs, [JoinAll] ScreenNode screen, [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens, [JoinAll] SelfUserMoneyNode selfUserMoneyNode)
        {
            if (module.moduleCardItemUI.UpgradeAvailable)
            {
                int level = module.moduleCardItemUI.Level;
                List<ModulePrice> upgradePrices = module.moduleCardsComposition.UpgradePrices;
                int crystals = 0;
                if ((level > -1) && ((level - 1) < upgradePrices.Count))
                {
                    crystals = upgradePrices[level - 1].Crystals;
                }
                if (crystals < selfUserMoneyNode.userMoney.Money)
                {
                    this.ShowUpgradeConfirmDialog(module, dialogs, screens);
                }
                else
                {
                    MainScreenComponent.Instance.ShowShopIfNotVisible();
                    if (ShopTabManager.Instance == null)
                    {
                        ShopTabManager.shopTabIndex = 4;
                    }
                    else
                    {
                        ShopTabManager.Instance.Show(4);
                    }
                }
            }
        }

        [OnEventFire]
        public void UpgradeXCryModule(ButtonClickEvent e, SingleNode<UpgradeXCryModuleButtonComponent> mountButton, [JoinAll] SelectedUserModuleWithUINode module, [JoinAll] SingleNode<Dialogs60Component> dialogs, [JoinAll] ScreenNode screen, [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens, [JoinAll] ICollection<ChestItem> chests, [JoinAll] SelfUserMoneyNode selfUserMoneyNode)
        {
            if (!module.moduleCardItemUI.UpgradeAvailable)
            {
                this.GoToChest(module.moduleTier.TierNumber, chests);
            }
            else
            {
                int level = module.moduleCardItemUI.Level;
                List<ModulePrice> upgradePrices = module.moduleCardsComposition.UpgradePrices;
                int xCrystals = 0;
                if ((level > -1) && ((level - 1) < upgradePrices.Count))
                {
                    xCrystals = upgradePrices[level - 1].XCrystals;
                }
                if (xCrystals < selfUserMoneyNode.userXCrystals.Money)
                {
                    this.ShowUpgradeXCryConfirmDialog(module, dialogs, screens);
                }
                else
                {
                    base.ScheduleEvent(new GoToXCryShopScreen(), selfUserMoneyNode);
                }
            }
        }

        [OnEventFire]
        public void UserModuleSelected(NodeAddedEvent e, SelectedUserModuleNode selectedUserModuleNode, [JoinByMarketItem] ModuleEffectUpgradablePropertyNode moduleEffectUpgradableProperties, [JoinAll] ScreenNode screen, [JoinAll] ICollection<SlotNode> slots, [JoinAll] SelectedSlotNode selectedSlot)
        {
            this.ModuleSelected(selectedUserModuleNode, moduleEffectUpgradableProperties, screen, slots, selectedSlot);
        }

        [OnEventFire]
        public void UserModuleUpgraded(ModuleUpgradedEvent e, SelectedUserModuleNode selectedUserModuleNode, [JoinByMarketItem] ModuleEffectUpgradablePropertyNode moduleEffectUpgradableProperties, [JoinAll] ScreenNode screen, [JoinAll] ICollection<SlotNode> slots, [JoinAll] SelectedSlotNode selectedSlot)
        {
            this.ModuleSelected(selectedUserModuleNode, moduleEffectUpgradableProperties, screen, slots, selectedSlot);
        }

        [CompilerGenerated]
        private sealed class <GoToChest>c__AnonStorey0
        {
            internal int tier;
            internal ModulesScreenSystem $this;

            internal bool <>m__0(ModulesScreenSystem.ChestItem chest) => 
                chest.Entity.HasComponent<UserItemComponent>() && ((chest.Entity.GetComponent<UserItemCounterComponent>().Count > 0L) && (chest.targetTier.MaxExistTier >= this.tier));

            internal bool <>m__1(ModulesScreenSystem.ChestItem chest) => 
                chest.Entity.HasComponent<PackPriceComponent>() && (chest.targetTier.MaxExistTier >= this.tier);
        }

        [CompilerGenerated]
        private sealed class <GoToChest>c__AnonStorey1
        {
            internal int minTier;
            internal ModulesScreenSystem.<GoToChest>c__AnonStorey0 <>f__ref$0;

            internal bool <>m__0(ModulesScreenSystem.ChestItem x) => 
                x.targetTier.MaxExistTier == this.minTier;

            internal int <>m__1(ModulesScreenSystem.ChestItem x) => 
                this.<>f__ref$0.$this.Select<ModulesScreenSystem.MarketChestItem>(x.Entity, typeof(MarketItemGroupComponent)).First<ModulesScreenSystem.MarketChestItem>().xPriceItem.Price;
        }

        public class ChestItem : Node
        {
            public GameplayChestItemComponent gameplayChestItem;
            public TargetTierComponent targetTier;
            public MarketItemGroupComponent marketItemGroup;
        }

        public class MarketChestItem : ModulesScreenSystem.ChestItem
        {
            public MarketItemComponent marketItem;
            public XPriceItemComponent xPriceItem;
        }

        public class MarketModuleNode : ModulesScreenSystem.ModuleNode
        {
            public MarketItemComponent marketItem;
        }

        public class ModuleEffectUpgradablePropertyNode : Node
        {
            public MarketItemGroupComponent marketItemGroup;
            public ModuleVisualPropertiesComponent moduleVisualProperties;
        }

        [Not(typeof(ImmutableModuleItemComponent))]
        public class ModuleNode : Node
        {
            public ModuleItemComponent moduleItem;
            public MarketItemGroupComponent marketItemGroup;
            public ModuleCardsCompositionComponent moduleCardsComposition;
            public ModuleBehaviourTypeComponent moduleBehaviourType;
            public ModuleTierComponent moduleTier;
            public DescriptionItemComponent descriptionItem;
            public ItemIconComponent itemIcon;
            public OrderItemComponent orderItem;
            public ModuleTankPartComponent moduleTankPart;
        }

        public class ModuleWithUI : ModulesScreenSystem.ModuleNode
        {
            public ModuleCardItemUIComponent moduleCardItemUI;
        }

        public class ScreenNode : Node
        {
            public ModulesScreenUIComponent modulesScreenUi;
        }

        public class SelectedMarketModuleNode : ModulesScreenSystem.MarketModuleNode
        {
            public ToggleListSelectedItemComponent toggleListSelectedItem;
        }

        public class SelectedSlotNode : Node
        {
            public SlotUserItemInfoComponent slotUserItemInfo;
            public ToggleListSelectedItemComponent toggleListSelectedItem;
        }

        public class SelectedSlotWithModuleNode : ModulesScreenSystem.SelectedSlotNode
        {
            public ModuleGroupComponent moduleGroup;
        }

        public class SelectedUserModuleNode : ModulesScreenSystem.UserModuleNode
        {
            public ToggleListSelectedItemComponent toggleListSelectedItem;
        }

        public class SelectedUserModuleWithUINode : ModulesScreenSystem.SelectedUserModuleNode
        {
            public ModuleCardItemUIComponent moduleCardItemUI;
        }

        public class SelfUserMoneyNode : Node
        {
            public SelfUserComponent selfUser;
            public UserGroupComponent userGroup;
            public UserMoneyComponent userMoney;
            public UserXCrystalsComponent userXCrystals;
        }

        public class SlotNode : Node
        {
            public SlotUIComponent slotUI;
            public SlotUserItemInfoComponent slotUserItemInfo;
        }

        public class SlotWithModule : Node
        {
            public ModuleGroupComponent moduleGroup;
            public SlotUIComponent slotUI;
        }

        public class UserModuleNode : ModulesScreenSystem.ModuleNode
        {
            public UserItemComponent userItem;
            public ModuleUpgradeLevelComponent moduleUpgradeLevel;
        }
    }
}

