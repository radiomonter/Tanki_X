namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.API;
    using TMPro;
    using UnityEngine;

    public class ModulesScreenUIComponent : BehaviourComponent
    {
        [SerializeField]
        private SlotUIComponent[] slots;
        [SerializeField]
        private CanvasGroup turretSlots;
        [SerializeField]
        private CanvasGroup hullSlots;
        [SerializeField]
        private ModuleCardItemUIComponent moduleCardItemUiComponentPrefab;
        [SerializeField]
        private ModuleCardsTierUI[] tiersUi;
        [SerializeField]
        private TextMeshProUGUI moduleName;
        [SerializeField]
        private TextMeshProUGUI moduleDesc;
        [SerializeField]
        private TextMeshProUGUI moduleFlavorText;
        [SerializeField]
        private ImageSkin moduleIcon;
        [SerializeField]
        private TextMeshProUGUI tankPartItemName;
        [SerializeField]
        private TextMeshProUGUI moduleTypeName;
        [SerializeField]
        private TextMeshProUGUI currentUpgradeLevel;
        [SerializeField]
        private TextMeshProUGUI nextUpgradeLevel;
        [SerializeField]
        private TextMeshProUGUI upgradeTitle;
        [SerializeField]
        private LocalizedField activeType;
        [SerializeField]
        private LocalizedField passiveType;
        [SerializeField]
        private LocalizedField upgradeLevel;
        [SerializeField]
        private LocalizedField hullHealth;
        [SerializeField]
        private LocalizedField turretDamage;
        [SerializeField]
        private ModulesPropertiesUIComponent modulesProperties;
        [SerializeField]
        private TankPartItemPropertiesUIComponent tankPartItemPropertiesUI;
        [SerializeField]
        private TutorialShowTriggerComponent upgradeModuleTrigger;
        private TankPartItem currentTankPartItem;
        public Action onEanble;

        public void AddCard(Entity entity)
        {
            int index = Mathf.Min(entity.GetComponent<ModuleTierComponent>().TierNumber, this.tiersUi.Length - 1);
            ModuleCardItemUIComponent moduleCardItem = Instantiate<ModuleCardItemUIComponent>(this.moduleCardItemUiComponentPrefab);
            this.tiersUi[index].AddCard(moduleCardItem);
            if (entity.HasComponent<ModuleCardItemUIComponent>())
            {
                entity.RemoveComponent<ModuleCardItemUIComponent>();
            }
            if (entity.HasComponent<ToggleListItemComponent>())
            {
                entity.RemoveComponent<ToggleListItemComponent>();
            }
            entity.AddComponent(moduleCardItem);
            entity.AddComponent(moduleCardItem.GetComponent<ToggleListItemComponent>());
        }

        public void Clear()
        {
            foreach (ModuleCardsTierUI rui in this.tiersUi)
            {
                rui.Clear();
            }
        }

        public void FilterCards(Entity mountedModule, ModuleBehaviourType slotType)
        {
            if (this.currentTankPartItem != null)
            {
                ModuleCardItemUIComponent[] componentsInChildren = base.GetComponentsInChildren<ModuleCardItemUIComponent>(true);
                bool flag = false;
                ModuleCardItemUIComponent[] componentArray2 = componentsInChildren;
                int index = 0;
                while (true)
                {
                    if (index < componentArray2.Length)
                    {
                        ModuleCardItemUIComponent component = componentArray2[index];
                        if (!component.gameObject.activeSelf || !component.GetComponent<ToggleListItemComponent>().Toggle.isOn)
                        {
                            index++;
                            continue;
                        }
                        flag = (component.Type == slotType) && (((component.TankPart != TankPartModuleType.WEAPON) || (this.currentTankPartItem.Type != TankPartItem.TankPartItemType.Turret)) ? ((component.TankPart == TankPartModuleType.TANK) && (this.currentTankPartItem.Type == TankPartItem.TankPartItemType.Hull)) : true);
                    }
                    foreach (ModuleCardItemUIComponent component2 in componentsInChildren)
                    {
                        bool flag2 = ((component2.TankPart != TankPartModuleType.WEAPON) || (this.currentTankPartItem.Type != TankPartItem.TankPartItemType.Turret)) ? ((component2.TankPart == TankPartModuleType.TANK) && (this.currentTankPartItem.Type == TankPartItem.TankPartItemType.Hull)) : true;
                        component2.gameObject.SetActive(flag2);
                        if (flag2 && (((mountedModule != null) && ReferenceEquals(component2.ModuleEntity, mountedModule)) || ((mountedModule == null) && (!flag && (component2.Type == slotType)))))
                        {
                            component2.GetComponent<ToggleListItemComponent>().Toggle.isOn = true;
                            flag = true;
                        }
                    }
                    this.upgradeModuleTrigger.Triggered();
                    return;
                }
            }
        }

        public void FilterSlots()
        {
            this.SwitchSlots(this.turretSlots, this.currentTankPartItem.Type == TankPartItem.TankPartItemType.Turret);
            this.SwitchSlots(this.hullSlots, this.currentTankPartItem.Type == TankPartItem.TankPartItemType.Hull);
            int index = (this.currentTankPartItem.Type != TankPartItem.TankPartItemType.Turret) ? 3 : 0;
            this.slots[index].GetComponent<ToggleListItemComponent>().Toggle.isOn = true;
        }

        public ModuleCardItemUIComponent GetCard(long marketItemGroupId)
        {
            foreach (ModuleCardsTierUI rui in this.tiersUi)
            {
                ModuleCardItemUIComponent card = rui.GetCard(marketItemGroupId);
                if (card != null)
                {
                    return card;
                }
            }
            return null;
        }

        public SlotUIComponent GetSlot(Slot slot) => 
            this.slots[(int) slot];

        private void OnEnable()
        {
            if (this.onEanble != null)
            {
                this.onEanble();
            }
        }

        private void Reset()
        {
            this.tiersUi = base.GetComponentsInChildren<ModuleCardsTierUI>();
        }

        public void SetItem(TankPartItem item)
        {
            this.currentTankPartItem = item;
            this.tankPartItemName.text = item.Name;
            this.FilterSlots();
        }

        public void ShowTankItemStat(float tankCoef, float weaponCoef, float tankCoefWithSelected, float weaponCoefWithSelected)
        {
            if (this.currentTankPartItem.Type == TankPartItem.TankPartItemType.Hull)
            {
                this.tankPartItemPropertiesUI.Show(this.currentTankPartItem, tankCoef, tankCoefWithSelected);
            }
            else
            {
                this.tankPartItemPropertiesUI.Show(this.currentTankPartItem, weaponCoef, weaponCoefWithSelected);
            }
        }

        private void SwitchSlots(CanvasGroup slots, bool isOn)
        {
            slots.interactable = isOn;
            slots.alpha = !isOn ? 0f : 1f;
            slots.blocksRaycasts = isOn;
        }

        public string HullHealth =>
            this.hullHealth.Value;

        public string TurretDamage =>
            this.turretDamage.Value;

        public TankPartItem CurrentTankPartItem =>
            this.currentTankPartItem;

        public ModulesPropertiesUIComponent ModulesProperties =>
            this.modulesProperties;

        public string ModuleName
        {
            set => 
                this.moduleName.text = value;
        }

        public string ModuleDesc
        {
            set => 
                this.moduleDesc.text = value;
        }

        public string ModuleFlavorText
        {
            set => 
                this.moduleFlavorText.text = value;
        }

        public bool ModuleActive
        {
            set => 
                this.moduleTypeName.text = !value ? this.passiveType.Value : this.activeType.Value;
        }

        public string ModuleIconUID
        {
            set => 
                this.moduleIcon.SpriteUid = value;
        }

        public int CurrentUpgradeLevel
        {
            set
            {
                this.currentUpgradeLevel.text = this.upgradeLevel.Value + " " + value;
                this.currentUpgradeLevel.gameObject.SetActive(value >= 0);
                this.upgradeTitle.text = (value >= 0) ? (this.upgradeLevel.Value + " " + value) : string.Empty;
            }
        }

        public int NextUpgradeLevel
        {
            set
            {
                this.nextUpgradeLevel.text = this.upgradeLevel.Value + " " + value;
                this.nextUpgradeLevel.gameObject.SetActive(value >= 0);
            }
        }
    }
}

