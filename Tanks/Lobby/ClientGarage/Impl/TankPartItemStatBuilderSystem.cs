namespace Tanks.Lobby.ClientGarage.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.API;
    using tanks.modules.lobby.ClientGarage.Scripts.Impl.NewModules.System;

    public class TankPartItemStatBuilderSystem : ECSSystem
    {
        [OnEventFire]
        public void CalculateHullUpgradeCoeff(CalculateTankPartUpgradeCoeffEvent e, SingleNode<TankItemComponent> tankItemComponent, [JoinAll] SingleNode<SelfUserComponent> selfUser, [JoinByUser] ICollection<MountedModuleItemNode> mountedModules, [JoinAll] ICollection<SlotNode> slots, [JoinAll] ModuleUpgradeConfigNode moduleUpgradeConfig)
        {
            e.UpgradeCoeff = TankUpgradeUtils.CalculateUpgradeCoeff(this.GetTierAndLevelsDictionary(this.GetMountedModules(mountedModules, TankPartModuleType.TANK)), this.GetSlotsCount(slots, TankPartModuleType.TANK), moduleUpgradeConfig.moduleUpgradablePowerConfig);
        }

        [OnEventFire]
        public void CalculateWeaponUpgradeCoeff(CalculateTankPartUpgradeCoeffEvent e, SingleNode<WeaponItemComponent> weaponItemComponent, [JoinAll] SingleNode<SelfUserComponent> selfUser, [JoinByUser] ICollection<MountedModuleItemNode> mountedModules, [JoinAll] ICollection<SlotNode> slots, [JoinAll] ModuleUpgradeConfigNode moduleUpgradeConfig)
        {
            e.UpgradeCoeff = TankUpgradeUtils.CalculateUpgradeCoeff(this.GetTierAndLevelsDictionary(this.GetMountedModules(mountedModules, TankPartModuleType.WEAPON)), this.GetSlotsCount(slots, TankPartModuleType.WEAPON), moduleUpgradeConfig.moduleUpgradablePowerConfig);
        }

        private void FilterModules(ICollection<MountedModuleItemNode> mountedModules, ModuleItemNode selectedModule, ModuleItemNode mountedToSlotModule, List<ModuleItemNode> tankModules, List<ModuleItemNode> weaponModules, List<ModuleItemNode> tankModulesWithSelected, List<ModuleItemNode> weaponModulesWithSelected)
        {
            foreach (MountedModuleItemNode node in mountedModules)
            {
                if ((mountedToSlotModule == null) || ((selectedModule == null) || (!mountedToSlotModule.Entity.Equals(node.Entity) || mountedToSlotModule.Entity.Equals(selectedModule.Entity))))
                {
                    if (node.moduleTankPart.TankPart == TankPartModuleType.TANK)
                    {
                        tankModulesWithSelected.Add(node);
                        if ((selectedModule != null) && node.Entity.Equals(selectedModule.Entity))
                        {
                            continue;
                        }
                        tankModules.Add(node);
                        continue;
                    }
                    weaponModulesWithSelected.Add(node);
                    if ((selectedModule == null) || !node.Entity.Equals(selectedModule.Entity))
                    {
                        weaponModules.Add(node);
                    }
                }
            }
            if (selectedModule != null)
            {
                if ((selectedModule.moduleTankPart.TankPart == TankPartModuleType.TANK) && !this.ListContainsNodeWithEntity(tankModulesWithSelected, selectedModule.Entity))
                {
                    tankModulesWithSelected.Add(selectedModule);
                }
                else if ((selectedModule.moduleTankPart.TankPart == TankPartModuleType.WEAPON) && !this.ListContainsNodeWithEntity(weaponModulesWithSelected, selectedModule.Entity))
                {
                    weaponModulesWithSelected.Add(selectedModule);
                }
            }
        }

        public UpgradeCoefs GetCoefs(ICollection<MountedModuleItemNode> mountedModules, SelectedModuleNode selectedModule, SelectedSlotNode selectedSlot, ModuleItemNode mountedToSlotModule, ICollection<SlotNode> slots, ModuleUpgradeConfigNode moduleUpgradeConfig)
        {
            List<ModuleItemNode> tankModules = new List<ModuleItemNode>();
            List<ModuleItemNode> weaponModules = new List<ModuleItemNode>();
            List<ModuleItemNode> tankModulesWithSelected = new List<ModuleItemNode>();
            List<ModuleItemNode> weaponModulesWithSelected = new List<ModuleItemNode>();
            this.FilterModules(mountedModules, selectedModule, mountedToSlotModule, tankModules, weaponModules, tankModulesWithSelected, weaponModulesWithSelected);
            int slotsCount = this.GetSlotsCount(slots, TankPartModuleType.TANK);
            int slotCount = this.GetSlotsCount(slots, TankPartModuleType.WEAPON);
            return new UpgradeCoefs { 
                tankCoeff = TankUpgradeUtils.CalculateUpgradeCoeff(this.GetTierAndLevelsDictionary(tankModules), slotsCount, moduleUpgradeConfig.moduleUpgradablePowerConfig),
                weaponCoeff = TankUpgradeUtils.CalculateUpgradeCoeff(this.GetTierAndLevelsDictionary(weaponModules), slotCount, moduleUpgradeConfig.moduleUpgradablePowerConfig),
                tankCoeffWithSelected = TankUpgradeUtils.CalculateUpgradeCoeff(this.GetTierAndLevelsDictionary(tankModulesWithSelected), slotsCount, moduleUpgradeConfig.moduleUpgradablePowerConfig),
                weaponCoeffWithSelected = TankUpgradeUtils.CalculateUpgradeCoeff(this.GetTierAndLevelsDictionary(weaponModulesWithSelected), slotCount, moduleUpgradeConfig.moduleUpgradablePowerConfig)
            };
        }

        [OnEventFire]
        public void GetItemStat(GetItemStatEvent e, ModuleItemNode module, [JoinAll] SingleNode<SelectedPresetComponent> selectedPreset, [JoinByUser] MountedHullNode mountedHull, [JoinByUser] MountedWeaponNode mountedWeapon, [JoinByUser] ICollection<MountedModuleItemNode> mountedModules, [JoinByModule] Optional<ModuleItemNode> mountedToSelectedSlotModule, [JoinAll] ICollection<SlotNode> slots, [JoinAll] ModuleUpgradeConfigNode moduleUpgradeConfig, [JoinAll] ScreenNode screen)
        {
            List<ModuleItemNode> tankModules = new List<ModuleItemNode>();
            List<ModuleItemNode> weaponModules = new List<ModuleItemNode>();
            List<ModuleItemNode> tankModulesWithSelected = new List<ModuleItemNode>();
            List<ModuleItemNode> weaponModulesWithSelected = new List<ModuleItemNode>();
            this.FilterModules(mountedModules, module, !mountedToSelectedSlotModule.IsPresent() ? null : mountedToSelectedSlotModule.Get(), tankModules, weaponModules, tankModulesWithSelected, weaponModulesWithSelected);
            int slotsCount = this.GetSlotsCount(slots, TankPartModuleType.TANK);
            int slotCount = this.GetSlotsCount(slots, TankPartModuleType.WEAPON);
            UpgradeCoefs coefs2 = new UpgradeCoefs {
                tankCoeffWithSelected = TankUpgradeUtils.CalculateUpgradeCoeff(this.GetTierAndLevelsDictionary(tankModulesWithSelected), slotsCount, moduleUpgradeConfig.moduleUpgradablePowerConfig),
                weaponCoeffWithSelected = TankUpgradeUtils.CalculateUpgradeCoeff(this.GetTierAndLevelsDictionary(weaponModulesWithSelected), slotCount, moduleUpgradeConfig.moduleUpgradablePowerConfig)
            };
            UpgradeCoefs coefs = coefs2;
            coefs2 = new UpgradeCoefs {
                tankCoeffWithSelected = TankUpgradeUtils.CalculateUpgradeCoeff(this.GetTierAndLevelsDictionaryForNextLevelModule(tankModulesWithSelected, module), slotsCount, moduleUpgradeConfig.moduleUpgradablePowerConfig),
                weaponCoeffWithSelected = TankUpgradeUtils.CalculateUpgradeCoeff(this.GetTierAndLevelsDictionaryForNextLevelModule(weaponModulesWithSelected, module), slotCount, moduleUpgradeConfig.moduleUpgradablePowerConfig)
            };
            UpgradeCoefs coefs3 = coefs2;
            float coef = (module.moduleTankPart.TankPart != TankPartModuleType.TANK) ? coefs.weaponCoeffWithSelected : coefs.tankCoeffWithSelected;
            float num4 = (module.moduleTankPart.TankPart != TankPartModuleType.TANK) ? coefs3.weaponCoeffWithSelected : coefs3.tankCoeffWithSelected;
            long marketId = (module.moduleTankPart.TankPart != TankPartModuleType.TANK) ? mountedWeapon.marketItemGroup.Key : mountedHull.marketItemGroup.Key;
            VisualProperty property = GarageItemsRegistry.GetItem<TankPartItem>(marketId).Properties[0];
            ModuleUpgradeCharacteristic characteristic = new ModuleUpgradeCharacteristic {
                Min = property.GetValue(0f) - (property.GetValue(0f) / 10f),
                Max = property.GetValue(1f),
                Current = property.GetValue(coef),
                Next = property.GetValue(num4),
                CurrentStr = property.GetFormatedValue(coef),
                NextStr = property.GetFormatedValue(num4),
                Unit = property.Unit,
                Name = (module.moduleTankPart.TankPart != TankPartModuleType.TANK) ? screen.newModulesScreenUi.TurretDamage : screen.newModulesScreenUi.HullHealth
            };
            e.moduleUpgradeCharacteristic = characteristic;
        }

        private int GetModuleLevel(ModuleItemNode module) => 
            !module.Entity.HasComponent<ModuleUpgradeLevelComponent>() ? 0 : ((int) module.Entity.GetComponent<ModuleUpgradeLevelComponent>().Level);

        private List<ModuleItemNode> GetMountedModules(ICollection<MountedModuleItemNode> modules, TankPartModuleType tankPartModuleType)
        {
            List<ModuleItemNode> list = new List<ModuleItemNode>();
            foreach (MountedModuleItemNode node in modules)
            {
                if (node.moduleTankPart.TankPart == tankPartModuleType)
                {
                    list.Add(node);
                }
            }
            return list;
        }

        private int GetSlotsCount(ICollection<SlotNode> slots, TankPartModuleType tankPartType)
        {
            int num = 0;
            foreach (SlotNode node in slots)
            {
                if (node.slotTankPart.TankPart == tankPartType)
                {
                    num++;
                }
            }
            return num;
        }

        private List<int[]> GetTierAndLevelsDictionary(List<ModuleItemNode> modules)
        {
            List<int[]> list = new List<int[]>();
            foreach (ModuleItemNode node in modules)
            {
                int[] item = new int[] { node.moduleTier.TierNumber, this.GetModuleLevel(node) };
                list.Add(item);
            }
            return list;
        }

        private List<int[]> GetTierAndLevelsDictionaryForNextLevelModule(List<ModuleItemNode> modules, ModuleItemNode nextLevelModule)
        {
            List<int[]> list = new List<int[]>();
            foreach (ModuleItemNode node in modules)
            {
                int moduleLevel = this.GetModuleLevel(node);
                if (nextLevelModule.Entity.Equals(node.Entity))
                {
                    moduleLevel++;
                }
                int[] item = new int[] { node.moduleTier.TierNumber, moduleLevel };
                list.Add(item);
            }
            return list;
        }

        private bool ListContainsNodeWithEntity(List<ModuleItemNode> nodes, Entity entity)
        {
            bool flag;
            using (List<ModuleItemNode>.Enumerator enumerator = nodes.GetEnumerator())
            {
                while (true)
                {
                    if (enumerator.MoveNext())
                    {
                        ModuleItemNode current = enumerator.Current;
                        if (!current.Entity.Equals(entity))
                        {
                            continue;
                        }
                        flag = true;
                    }
                    else
                    {
                        return false;
                    }
                    break;
                }
            }
            return flag;
        }

        [OnEventFire]
        public void ModuleChanged(ModuleChangedEvent e, Node node, [JoinAll] SingleNode<SelectedHullUIComponent> selectedHullUI, [JoinAll] SingleNode<SelectedTurretUIComponent> selectedTurretUI, [JoinAll] SingleNode<SelectedPresetComponent> selectedPreset, [JoinByUser] ICollection<MountedModuleItemNode> mountedModules, [JoinAll] ICollection<SlotNode> slots, [JoinAll] ModuleUpgradeConfigNode moduleUpgradeConfig)
        {
            float coef = TankUpgradeUtils.CalculateUpgradeCoeff(this.GetTierAndLevelsDictionary(this.GetMountedModules(mountedModules, TankPartModuleType.TANK)), this.GetSlotsCount(slots, TankPartModuleType.TANK), moduleUpgradeConfig.moduleUpgradablePowerConfig);
            selectedHullUI.component.SetStars(coef);
            float num2 = TankUpgradeUtils.CalculateUpgradeCoeff(this.GetTierAndLevelsDictionary(this.GetMountedModules(mountedModules, TankPartModuleType.WEAPON)), this.GetSlotsCount(slots, TankPartModuleType.WEAPON), moduleUpgradeConfig.moduleUpgradablePowerConfig);
            selectedTurretUI.component.SetStars(num2);
        }

        [OnEventFire]
        public void SetHullStars(NodeAddedEvent e, SingleNode<SelectedHullUIComponent> selectedHullUI, [Context] SingleNode<SelectedPresetComponent> selectedPreset, [JoinByUser] ICollection<MountedModuleItemNode> mountedModules, [JoinAll] ICollection<SlotNode> slots, [JoinAll] ModuleUpgradeConfigNode moduleUpgradeConfig)
        {
            float coef = TankUpgradeUtils.CalculateUpgradeCoeff(this.GetTierAndLevelsDictionary(this.GetMountedModules(mountedModules, TankPartModuleType.TANK)), this.GetSlotsCount(slots, TankPartModuleType.TANK), moduleUpgradeConfig.moduleUpgradablePowerConfig);
            selectedHullUI.component.SetStars(coef);
        }

        [OnEventFire]
        public void SetTurretStars(NodeAddedEvent e, SingleNode<SelectedTurretUIComponent> selectedTurretUI, [Context] SingleNode<SelectedPresetComponent> selectedPreset, [JoinByUser] ICollection<MountedModuleItemNode> mountedModules, [JoinAll] ICollection<SlotNode> slots, [JoinAll] ModuleUpgradeConfigNode moduleUpgradeConfig)
        {
            float coef = TankUpgradeUtils.CalculateUpgradeCoeff(this.GetTierAndLevelsDictionary(this.GetMountedModules(mountedModules, TankPartModuleType.WEAPON)), this.GetSlotsCount(slots, TankPartModuleType.WEAPON), moduleUpgradeConfig.moduleUpgradablePowerConfig);
            selectedTurretUI.component.SetStars(coef);
        }

        [OnEventFire]
        public void ShowTankPartStatGarage(ButtonClickEvent e, SingleNode<TankPartItemPropertiesButtonComponent> tankPartItemPropertiesButton, [JoinAll] SingleNode<SelectedPresetComponent> selectedPreset, [JoinByUser] ICollection<MountedModuleItemNode> mountedModules, [JoinAll] ICollection<SlotNode> slots, [JoinAll] ModuleUpgradeConfigNode moduleUpgradeConfig, [JoinAll] SingleNode<SelectedHullUIComponent> garageScreen)
        {
            List<ModuleItemNode> modules = new List<ModuleItemNode>();
            TankPartModuleType tankPartModuleType = tankPartItemPropertiesButton.component.tankPartModuleType;
            foreach (MountedModuleItemNode node in mountedModules)
            {
                if (node.moduleTankPart.TankPart == tankPartModuleType)
                {
                    modules.Add(node);
                }
            }
            int slotsCount = this.GetSlotsCount(slots, tankPartModuleType);
            float coef = TankUpgradeUtils.CalculateUpgradeCoeff(this.GetTierAndLevelsDictionary(modules), slotsCount, moduleUpgradeConfig.moduleUpgradablePowerConfig);
            tankPartItemPropertiesButton.component.itemPropertiesUiComponent.Show(null, coef, coef);
        }

        [Inject]
        public static Tanks.Lobby.ClientGarage.API.GarageItemsRegistry GarageItemsRegistry { get; set; }

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

        public class MountedHullNode : Node
        {
            public TankItemComponent tankItem;
            public MountedItemComponent mountedItem;
            public MarketItemGroupComponent marketItemGroup;
        }

        public class MountedModuleItemNode : TankPartItemStatBuilderSystem.ModuleItemNode
        {
            public MountedItemComponent mountedItem;
            public UserItemComponent userItem;
            public ModuleUpgradeLevelComponent moduleUpgradeLevel;
        }

        public class MountedWeaponNode : Node
        {
            public WeaponItemComponent weaponItem;
            public MountedItemComponent mountedItem;
            public MarketItemGroupComponent marketItemGroup;
        }

        public class ScreenNode : Node
        {
            public NewModulesScreenUIComponent newModulesScreenUi;
        }

        public class SelectedModuleNode : TankPartItemStatBuilderSystem.ModuleItemNode
        {
            public ToggleListSelectedItemComponent toggleListSelectedItem;
        }

        public class SelectedSlotNode : TankPartItemStatBuilderSystem.SlotNode
        {
            public ToggleListSelectedItemComponent toggleListSelectedItem;
        }

        public class SlotNode : Node
        {
            public SlotUserItemInfoComponent slotUserItemInfo;
            public SlotTankPartComponent slotTankPart;
        }
    }
}

