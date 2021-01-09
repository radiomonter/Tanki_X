namespace Tanks.Lobby.ClientGarage.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientNavigation.API;
    using Tanks.Lobby.ClientPayment.API;
    using Tanks.Lobby.ClientProfile.API;
    using tanks.modules.lobby.ClientGarage.Scripts.API.UI.Items;
    using tanks.modules.lobby.ClientGarage.Scripts.Impl.NewModules.UI.New.DragAndDrop;
    using UnityEngine.Events;

    public class NewModulesScreenSystem : ECSSystem
    {
        [CompilerGenerated]
        private static Func<SlotNode, bool> <>f__am$cache0;
        [CompilerGenerated]
        private static Func<ChestItem, int> <>f__am$cache1;
        [CompilerGenerated]
        private static Func<ChestItem, int> <>f__am$cache2;
        [CompilerGenerated]
        private static Func<ChestItem, int> <>f__am$cache3;

        [OnEventFire]
        public void AddUpgradeConfig(NodeAddedEvent e, SingleNode<NewModulesScreenUIComponent> screen, [JoinAll] ModuleUpgradeConfigNode config)
        {
            screen.component.level2PowerByTier = config.moduleUpgradablePowerConfig.Level2PowerByTier;
        }

        [OnEventFire]
        public void BuyBlueprints(ButtonClickEvent e, SingleNode<BuyBlueprintsButtonComponent> buyButton, [JoinAll] SingleNode<NewModulesScreenUIComponent> screen, [JoinAll] ICollection<ChestItem> chests)
        {
            screen.component.Hide();
            this.GoToChest(screen.component.SelectedModule.TierNumber, screen.component.SelectedModule.MarketCardItem.Id, chests);
        }

        private void GoToChest(int tier, long id, ICollection<ChestItem> chests)
        {
            <GoToChest>c__AnonStorey0 storey = new <GoToChest>c__AnonStorey0 {
                tier = tier,
                id = id
            };
            IEnumerable<ChestItem> source = chests.Where<ChestItem>(new Func<ChestItem, bool>(storey.<>m__0));
            ChestItem node = null;
            if (source.Any<ChestItem>())
            {
                if (<>f__am$cache1 == null)
                {
                    <>f__am$cache1 = x => x.orderItem.Index;
                }
                <>f__am$cache2 ??= x => x.targetTier.TargetTier;
                node = source.OrderByDescending<ChestItem, int>(<>f__am$cache1).ThenByDescending<ChestItem, int>(<>f__am$cache2).FirstOrDefault<ChestItem>();
            }
            if (node == null)
            {
                if (<>f__am$cache3 == null)
                {
                    <>f__am$cache3 = x => x.targetTier.TargetTier;
                }
                node = chests.Where<ChestItem>(new Func<ChestItem, bool>(storey.<>m__1)).OrderByDescending<ChestItem, int>(<>f__am$cache3).FirstOrDefault<ChestItem>();
            }
            ShowGarageCategoryEvent eventInstance = new ShowGarageCategoryEvent {
                Category = GarageCategory.BLUEPRINTS,
                SelectedItem = node.Entity
            };
            base.ScheduleEvent(eventInstance, node);
        }

        [OnEventFire]
        public void GoToModulesScreen(GoToModulesScreenEvent e, Node node, [JoinAll] SingleNode<Dialogs60Component> dialogs)
        {
            dialogs.component.Get<NewModulesScreenUIComponent>().Show(e.TankPartModuleType);
        }

        [OnEventFire]
        public void InitHull(NodeAddedEvent e, SingleNode<NewModulesScreenUIComponent> screen, SelfUserNode self, [JoinByUser, Context] MountedHullNode hull, [JoinByMarketItem, Context] ParentGarageMarketItemNode marketItem, [JoinByParentGroup, Context] MountedSkin mountedSkin, [JoinByUser, Context] SelfUserNode self2)
        {
            screen.component.UpdateViewInNextFrame();
            screen.component.hullCollectionView.preview.SpriteUid = mountedSkin.imageItem.SpriteUid;
            screen.component.hullCollectionView.partName.text = hull.descriptionItem.Name;
            object[] objArray1 = new object[] { "(", screen.component.Level, " ", hull.upgradeLevelItem.Level, ")" };
            screen.component.hullCollectionView.PartLevel = $"{string.Concat(objArray1)}";
            screen.component.Tank = GarageItemsRegistry.GetItem<TankPartItem>(marketItem.Entity);
        }

        [OnEventFire]
        public void InitScreen(NodeAddedEvent e, SingleNode<NewModulesScreenUIComponent> screenNode, [JoinAll] SingleNode<SelfUserComponent> user, [JoinByUser] ICollection<SlotNode> slotNodes, [JoinAll] SelfUserMoneyNode money)
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = s => s.slotTankPart.TankPart.Equals(TankPartModuleType.TANK) || s.slotTankPart.TankPart.Equals(TankPartModuleType.WEAPON);
            }
            slotNodes = slotNodes.Where<SlotNode>(<>f__am$cache0).ToList<SlotNode>();
            screenNode.component.InitSlots(slotNodes);
            screenNode.component.InitMoney(money);
            screenNode.component.UpdateViewInNextFrame();
            screenNode.component.dragAndDropController.onDrop += new Action<DropDescriptor, DropDescriptor>(this.OnDrop);
            screenNode.component.crystalButton.GetComponent<Button>().onClick.AddListener(new UnityAction(screenNode.component.Hide));
            screenNode.component.xCrystalButton.GetComponent<Button>().onClick.AddListener(new UnityAction(screenNode.component.Hide));
        }

        [OnEventFire]
        public void InitTurret(NodeAddedEvent e, SingleNode<NewModulesScreenUIComponent> screen, SelfUserNode self, [JoinByUser, Context] MountedTurretNode turret, [JoinByMarketItem, Context] ParentGarageMarketItemNode marketItem, [JoinByParentGroup, Context] MountedSkin mountedSkin, [JoinByUser, Context] SelfUserNode self2)
        {
            screen.component.UpdateViewInNextFrame();
            screen.component.turretCollectionView.preview.SpriteUid = mountedSkin.imageItem.SpriteUid;
            screen.component.turretCollectionView.partName.text = turret.descriptionItem.Name;
            object[] objArray1 = new object[] { "(", screen.component.Level, " ", turret.upgradeLevelItem.Level, ")" };
            screen.component.turretCollectionView.PartLevel = $"{string.Concat(objArray1)}";
            screen.component.Weapon = GarageItemsRegistry.GetItem<TankPartItem>(marketItem.Entity);
        }

        private bool IsOwnModule(Entity item) => 
            base.Select<SingleNode<SelfUserComponent>>(item, typeof(UserGroupComponent)).Count > 0;

        private bool IsTankSlot(DragAndDropCell slot) => 
            slot.GetComponent<TankSlotView>() != null;

        [OnEventFire]
        public void ModuleChanged(ModuleChangedEvent e, Node node, [JoinAll] SingleNode<NewModulesScreenUIComponent> screen)
        {
            base.NewEvent<UpdateScreenEvent>().Attach(screen).Schedule();
        }

        private void OnDrop(DropDescriptor dropDescriptor, DropDescriptor backDescriptor)
        {
            Entity entity;
            ModuleItem moduleItem;
            if (this.IsTankSlot(dropDescriptor.sourceCell))
            {
                entity = dropDescriptor.sourceCell.GetComponent<TankSlotView>().SlotNode.Entity;
                moduleItem = dropDescriptor.item.GetComponent<SlotItemView>().ModuleItem;
                Entity[] entities = new Entity[] { moduleItem.UserItem, entity };
                EngineImpl.EngineService.Engine.NewEvent<UnmountModuleFromSlotEvent>().AttachAll(entities).Schedule();
            }
            if ((backDescriptor.item != null) && this.IsTankSlot(backDescriptor.sourceCell))
            {
                entity = backDescriptor.sourceCell.GetComponent<TankSlotView>().SlotNode.Entity;
                moduleItem = backDescriptor.item.GetComponent<SlotItemView>().ModuleItem;
                Entity[] entities = new Entity[] { moduleItem.UserItem, entity };
                EngineImpl.EngineService.Engine.NewEvent<UnmountModuleFromSlotEvent>().AttachAll(entities).Schedule();
            }
            if (this.IsTankSlot(dropDescriptor.destinationCell))
            {
                entity = dropDescriptor.destinationCell.GetComponent<TankSlotView>().SlotNode.Entity;
                moduleItem = dropDescriptor.item.GetComponent<SlotItemView>().ModuleItem;
                if (this.IsOwnModule(moduleItem.UserItem))
                {
                    Entity[] entities = new Entity[] { moduleItem.UserItem, entity };
                    EngineImpl.EngineService.Engine.NewEvent<ModuleMountEvent>().AttachAll(entities).Schedule();
                }
            }
            if ((backDescriptor.item != null) && this.IsTankSlot(backDescriptor.destinationCell))
            {
                entity = backDescriptor.destinationCell.GetComponent<TankSlotView>().SlotNode.Entity;
                moduleItem = backDescriptor.item.GetComponent<SlotItemView>().ModuleItem;
                if (this.IsOwnModule(moduleItem.UserItem))
                {
                    Entity[] entities = new Entity[] { moduleItem.UserItem, entity };
                    EngineImpl.EngineService.Engine.NewEvent<ModuleMountEvent>().AttachAll(entities).Schedule();
                }
            }
        }

        [OnEventFire]
        public void OnRemoveScreen(NodeRemoveEvent e, SingleNode<NewModulesScreenUIComponent> screenNode)
        {
            screenNode.component.dragAndDropController.onDrop -= new Action<DropDescriptor, DropDescriptor>(this.OnDrop);
            screenNode.component.crystalButton.GetComponent<Button>().onClick.RemoveListener(new UnityAction(screenNode.component.Hide));
            screenNode.component.xCrystalButton.GetComponent<Button>().onClick.RemoveListener(new UnityAction(screenNode.component.Hide));
        }

        [OnEventFire]
        public void Register(NodeAddedEvent e, SingleNode<ImmutableModuleItemComponent> module)
        {
        }

        [OnEventFire]
        public void ResearchModule(ButtonClickEvent e, SingleNode<ResearchModuleButtonComponent> mountButton, [JoinAll] SingleNode<NewModulesScreenUIComponent> screen, [JoinAll] SingleNode<SelfUserComponent> user)
        {
            Entity marketItem = screen.component.SelectedModule.MarketItem;
            base.NewEvent<ModuleAssembleEvent>().Attach(marketItem).Attach(user).Schedule();
        }

        [OnEventFire]
        public void SendUpgradeXCry(DialogConfirmEvent e, SingleNode<ModuleUpgradeXCryConfirmWindowComponent> upgradeButton, [JoinAll] SingleNode<NewModulesScreenUIComponent> screen, [JoinAll] SelfUserMoneyNode selfUserMoneyNode, [JoinAll] SingleNode<Dialogs60Component> dialogs, [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> windows)
        {
            Entity userItem = screen.component.SelectedModule.UserItem;
            base.NewEvent<UpgradeModuleWithXCrystalsEvent>().Attach(userItem).Schedule();
        }

        private void ShowDialog(SingleNode<Dialogs60Component> dialogs, Optional<SingleNode<WindowsSpaceComponent>> screens, SingleNode<NewModulesScreenUIComponent> screen, bool useXCry = true)
        {
            CraftModuleConfirmWindowComponent component = dialogs.component.Get<CraftModuleConfirmWindowComponent>();
            ModuleItem selectedModule = screen.component.SelectedModule;
            component.Setup(selectedModule.Name, selectedModule.Description(), selectedModule.CardSpriteUid, (double) selectedModule.UpgradePriceXCRY, false, !useXCry ? "8" : "9", false);
            List<Animator> animators = new List<Animator>();
            if (screens.IsPresent())
            {
                animators = screens.Get().component.Animators;
            }
            component.Show(animators);
            component.GetComponent<EntityBehaviour>().Entity.AddComponent<ModuleUpgradeXCryConfirmWindowComponent>();
        }

        [OnEventFire]
        public void ShowPresetList(NodeAddedEvent e, SingleNode<NewModulesScreenUIComponent> screen, [JoinAll] SingleNode<SelfUserComponent> selfUser, [JoinByUser] ICollection<PresetNode> presetsList)
        {
            List<PresetItem> items = new List<PresetItem>();
            foreach (PresetNode node in presetsList)
            {
                if ((node.presetEquipment.HullId != 0L) && (node.presetEquipment.WeaponId != 0L))
                {
                    Entity entityById = base.GetEntityById(node.presetEquipment.HullId);
                    Entity entity2 = base.GetEntityById(node.presetEquipment.WeaponId);
                    string name = entityById.GetComponent<DescriptionItemComponent>().Name;
                    items.Add(new PresetItem(node.presetName.Name, 1, name, entity2.GetComponent<DescriptionItemComponent>().Name, entityById.GetComponent<MarketItemGroupComponent>().Key, entity2.GetComponent<MarketItemGroupComponent>().Key, node.Entity));
                }
            }
            screen.component.InitPresetsDropDown(items);
        }

        private void ShowUpgradeXCryConfirmDialog(SingleNode<Dialogs60Component> dialogs, Optional<SingleNode<WindowsSpaceComponent>> window, SingleNode<NewModulesScreenUIComponent> screen)
        {
            this.ShowDialog(dialogs, window, screen, true);
        }

        [OnEventFire]
        public void UpdateScreenView(UpdateScreenEvent e, SingleNode<NewModulesScreenUIComponent> screen)
        {
            screen.component.UpdateView();
        }

        [OnEventFire]
        public void UpgradeModuleWithCRY(ButtonClickEvent e, SingleNode<UpgradeModuleButtonComponent> upgradeButton, [JoinAll] SingleNode<NewModulesScreenUIComponent> screen, [JoinAll] SelfUserMoneyNode selfUserMoneyNode)
        {
            Entity userItem = screen.component.SelectedModule.UserItem;
            if (screen.component.SelectedModule.UpgradePriceCRY <= selfUserMoneyNode.userMoney.Money)
            {
                base.NewEvent<UpgradeModuleWithCrystalsEvent>().Attach(userItem).Schedule();
            }
            else
            {
                screen.component.Hide();
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

        [OnEventFire]
        public void UpgradeModuleWithXCRY(ButtonClickEvent e, SingleNode<UpgradeXCryModuleButtonComponent> upgradeButton, [JoinAll] SingleNode<NewModulesScreenUIComponent> screen, [JoinAll] SelfUserMoneyNode selfUserMoneyNode, [JoinAll] SingleNode<Dialogs60Component> dialogs, [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> windows)
        {
            if (screen.component.SelectedModule.UpgradePriceXCRY <= selfUserMoneyNode.userXCrystals.Money)
            {
                this.ShowUpgradeXCryConfirmDialog(dialogs, windows, screen);
            }
            else
            {
                screen.component.Hide();
                base.ScheduleEvent(new GoToXCryShopScreen(), selfUserMoneyNode);
            }
        }

        [Inject]
        public static Tanks.Lobby.ClientGarage.API.GarageItemsRegistry GarageItemsRegistry { get; set; }

        [CompilerGenerated]
        private sealed class <GoToChest>c__AnonStorey0
        {
            internal int tier;
            internal long id;

            internal bool <>m__0(NewModulesScreenSystem.ChestItem chest) => 
                chest.Entity.HasComponent<UserItemComponent>() && ((chest.Entity.GetComponent<UserItemCounterComponent>().Count > 0L) && ((chest.targetTier.MaxExistTier >= this.tier) && (chest.targetTier.ContainsAllTierItem || chest.targetTier.ItemList.Contains(this.id))));

            internal bool <>m__1(NewModulesScreenSystem.ChestItem chest) => 
                ((chest.targetTier.TargetTier == this.tier) && (chest.targetTier.ContainsAllTierItem || chest.targetTier.ItemList.Contains(this.id))) && chest.Entity.HasComponent<PackPriceComponent>();
        }

        public class ChestItem : Node
        {
            public GameplayChestItemComponent gameplayChestItem;
            public TargetTierComponent targetTier;
            public OrderItemComponent orderItem;
            public MarketItemGroupComponent marketItemGroup;
        }

        public class GarageMarketItemNode : Node
        {
            public DescriptionItemComponent descriptionItem;
            public MarketItemGroupComponent marketItemGroup;
            public MarketItemComponent marketItem;
            public VisualPropertiesComponent visualProperties;
            public GarageMarketItemRegisteredComponent garageMarketItemRegistered;
        }

        public class GarageUserItemNode : Node
        {
            public MarketItemGroupComponent marketItemGroup;
            public DescriptionItemComponent descriptionItem;
            public UserGroupComponent userGroup;
            public ExperienceItemComponent experienceItem;
            public VisualPropertiesComponent visualProperties;
            public UpgradeLevelItemComponent upgradeLevelItem;
        }

        public class MarketChestItem : NewModulesScreenSystem.ChestItem
        {
            public MarketItemComponent marketItem;
            public XPriceItemComponent xPriceItem;
        }

        public class ModuleItemNode : Node
        {
            public ModuleTierComponent moduleTier;
            public ModuleItemComponent moduleItem;
            public ModuleTankPartComponent moduleTankPart;
        }

        public class ModuleUpgradeConfigNode : Node
        {
            public ModuleUpgradablePowerConfigComponent moduleUpgradablePowerConfig;
        }

        public class MountedHullNode : NewModulesScreenSystem.GarageUserItemNode
        {
            public MountedItemComponent mountedItem;
            public TankItemComponent tankItem;
        }

        public class MountedModuleNode : NewModulesScreenSystem.ModuleItemNode
        {
            public MountedItemComponent mountedItem;
        }

        public class MountedSkin : Node
        {
            public SkinItemComponent skinItem;
            public MountedItemComponent mountedItem;
            public ParentGroupComponent parentGroup;
            public DescriptionItemComponent descriptionItem;
            public ImageItemComponent imageItem;
        }

        public class MountedTurretNode : NewModulesScreenSystem.GarageUserItemNode
        {
            public MountedItemComponent mountedItem;
            public WeaponItemComponent weaponItem;
        }

        public class ParentGarageMarketItemNode : NewModulesScreenSystem.GarageMarketItemNode
        {
            public ParentGroupComponent parentGroup;
        }

        public class PresetNode : Node
        {
            public PresetItemComponent presetItem;
            public PresetNameComponent presetName;
            public UserItemComponent userItem;
            public PresetEquipmentComponent presetEquipment;
        }

        public class SelfUserMoneyNode : Node
        {
            public SelfUserComponent selfUser;
            public UserGroupComponent userGroup;
            public UserMoneyComponent userMoney;
            public UserXCrystalsComponent userXCrystals;
        }

        public class SelfUserNode : Node
        {
            public SelfUserComponent selfUser;
            public UserGroupComponent userGroup;
        }

        public class SlotNode : Node
        {
            public UserItemComponent userItem;
            public SlotUserItemInfoComponent slotUserItemInfo;
            public SlotTankPartComponent slotTankPart;
        }
    }
}

