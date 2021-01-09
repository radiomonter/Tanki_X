namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientProfile.Impl;
    using tanks.modules.lobby.ClientGarage.Scripts.API.UI.Items;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    public class NewModulesScreenUIComponent : BehaviourComponent
    {
        public static float OVER_SCREEN_Z_OFFSET = -0.054f;
        [SerializeField]
        private LocalizedField hullHealth;
        [SerializeField]
        private LocalizedField turretDamage;
        [SerializeField]
        private LocalizedField level;
        [SerializeField]
        private PresetsDropDownList presetsDropDownList;
        public List<List<int>> level2PowerByTier;
        public XCrystalsIndicatorComponent xCrystalButton;
        public CrystalsIndicatorComponent crystalButton;
        public TankPartCollectionView turretCollectionView;
        public TankPartCollectionView hullCollectionView;
        public CollectionView collectionView;
        public Button backButton;
        public SelectedModuleView selectedModuleView;
        public GameObject background;
        public DragAndDropController dragAndDropController;
        public GameObject slotItemPrefab;
        public static Dictionary<ModuleItem, SlotItemView> slotItems = new Dictionary<ModuleItem, SlotItemView>();
        public static ModuleScreenSelection selection;
        public TankPartModeController tankPartModeController;
        public Action OnShowAnimationFinishedAction;
        public bool showAnimationFinished;
        private ModuleItem selectedModule;
        private bool needUpdate;
        [CompilerGenerated]
        private static Func<ModuleItem, bool> <>f__am$cache0;

        private void AddItemToTankCollection(ModuleItem moduleItem, SlotItemView slotItemView)
        {
            TankPartCollectionView tankPartCollection = this.GetTankPartCollection(moduleItem);
            if (moduleItem.ModuleBehaviourType == ModuleBehaviourType.PASSIVE)
            {
                tankPartCollection.passiveSlot.SetItem(slotItemView);
            }
            else if (tankPartCollection.activeSlot.SlotNode.Entity.Equals(moduleItem.Slot))
            {
                tankPartCollection.activeSlot.SetItem(slotItemView);
            }
            else
            {
                tankPartCollection.activeSlot2.SetItem(slotItemView);
            }
        }

        public void Awake()
        {
            ModuleScreenSelection selection = new ModuleScreenSelection {
                onSelectionChange = new Action<ModuleItem>(this.OnSelectionChange)
            };
            NewModulesScreenUIComponent.selection = selection;
            this.tankPartModeController = new TankPartModeController(this.turretCollectionView, this.hullCollectionView, this.collectionView);
            this.tankPartModeController.onModeChange = new Action(this.OnTankPartModeChange);
            if (CollectionView.slots != null)
            {
                CollectionView.slots.Clear();
                CollectionView.slots = null;
            }
            if (slotItems != null)
            {
                slotItems.Clear();
            }
            this.collectionView.UpdateView();
            foreach (CollectionSlotView view in CollectionView.slots.Values)
            {
                view.onDoubleClick += new Action<CollectionSlotView>(this.OnCollectionSlotDoubleClick);
            }
            this.backButton.onClick.AddListener(new UnityAction(this.Hide));
            this.selectedModule = null;
        }

        private void CreateSlotItems()
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = mi => mi.IsMutable();
            }
            foreach (ModuleItem item in GarageItemsRegistry.Modules.Where<ModuleItem>(<>f__am$cache0).ToList<ModuleItem>())
            {
                if ((item.UserItem != null) && !slotItems.ContainsKey(item))
                {
                    GameObject obj2 = Instantiate<GameObject>(this.slotItemPrefab);
                    obj2.SetActive(false);
                    SlotItemView component = obj2.GetComponent<SlotItemView>();
                    component.UpdateView(item);
                    component.onDoubleClick = new Action<SlotItemView>(this.OnSlotItemDoubleClick);
                    slotItems.Add(item, component);
                }
            }
        }

        private TankPartCollectionView GetTankPartCollection(ModuleItem moduleItem) => 
            (moduleItem.TankPartModuleType != TankPartModuleType.TANK) ? this.turretCollectionView : this.hullCollectionView;

        public void Hide()
        {
            MainScreenComponent.Instance.ClearOnBackOverride();
            base.GetComponent<Animator>().SetBool("hide", true);
        }

        public void InitMoney(NewModulesScreenSystem.SelfUserMoneyNode money)
        {
            this.selectedModuleView.InitMoney(money);
            this.crystalButton.SetValueWithoutAnimation(money.userMoney.Money);
            this.xCrystalButton.SetValueWithoutAnimation(money.userXCrystals.Money);
        }

        public void InitPresetsDropDown(List<PresetItem> items)
        {
            this.presetsDropDownList.UpdateList(items);
        }

        public void InitSlots(ICollection<NewModulesScreenSystem.SlotNode> slotNodes)
        {
            List<TankSlotView> list = new List<TankSlotView> {
                this.turretCollectionView.activeSlot,
                this.turretCollectionView.activeSlot2,
                this.turretCollectionView.passiveSlot,
                this.hullCollectionView.activeSlot,
                this.hullCollectionView.activeSlot2,
                this.hullCollectionView.passiveSlot
            };
            if (slotNodes.Count != list.Count)
            {
                throw new ArgumentException("Incorrect module slot entity count " + slotNodes.Count);
            }
            foreach (NewModulesScreenSystem.SlotNode node in slotNodes)
            {
                TankSlotView view = list[(int) node.slotUserItemInfo.Slot];
                view.SlotNode = node;
            }
        }

        private void Mount(Entity entity)
        {
            ECSBehaviour.EngineService.Engine.ScheduleEvent<MountItemEvent>(entity);
        }

        private void OnCollectionSlotDoubleClick(CollectionSlotView collectionSlotView)
        {
            ModuleItem moduleItem = collectionSlotView.ModuleItem;
            if (moduleItem.Slot != null)
            {
                TankSlotView slotBySlotEntity = this.GetTankPartCollection(moduleItem).GetSlotBySlotEntity(moduleItem.Slot);
                if (slotBySlotEntity == null)
                {
                    throw new Exception("Modules screen: couldn't find tank slot");
                }
                DragAndDropItem component = slotBySlotEntity.GetItem().GetComponent<DragAndDropItem>();
                this.dragAndDropController.OnDrop(slotBySlotEntity.GetComponent<DragAndDropCell>(), collectionSlotView.GetComponent<DragAndDropCell>(), component);
            }
        }

        private void OnDisable()
        {
            this.presetsDropDownList.onDropDownListItemSelected -= new OnDropDownListItemSelected(this.OnPresetSelected);
        }

        private void OnEnable()
        {
            this.presetsDropDownList.onDropDownListItemSelected += new OnDropDownListItemSelected(this.OnPresetSelected);
        }

        public void OnHideAnimationFinished()
        {
            base.gameObject.SetActive(false);
        }

        public void OnPresetSelected(ListItem item)
        {
            PresetItem data = (PresetItem) item.Data;
            this.Mount(data.presetEntity);
        }

        private void OnSelectionChange(ModuleItem item)
        {
            this.selectedModule = item;
            this.selectedModuleView.UpdateView(item, this.level2PowerByTier, this.Tank, this.Weapon);
        }

        public void OnShowAnimationFinished()
        {
            this.showAnimationFinished = true;
            if (this.OnShowAnimationFinishedAction != null)
            {
                this.OnShowAnimationFinishedAction();
            }
        }

        private void OnSlotItemDoubleClick(SlotItemView view)
        {
            ModuleItem moduleItem = view.ModuleItem;
            DragAndDropItem component = view.GetComponent<DragAndDropItem>();
            TankPartCollectionView tankPartCollection = this.GetTankPartCollection(moduleItem);
            DragAndDropCell cellFrom = CollectionView.slots[moduleItem].GetComponent<DragAndDropCell>();
            if (!moduleItem.IsMounted)
            {
                TankSlotView slotForDrop = tankPartCollection.GetSlotForDrop(moduleItem.ModuleBehaviourType);
                if (slotForDrop != null)
                {
                    this.dragAndDropController.OnDrop(cellFrom, slotForDrop.GetComponent<DragAndDropCell>(), component);
                }
            }
            else
            {
                TankSlotView slotBySlotEntity = tankPartCollection.GetSlotBySlotEntity(moduleItem.Slot);
                if (slotBySlotEntity == null)
                {
                    throw new Exception("Modules screen: couln't find tank slot for moduleItem slot entity " + moduleItem.Slot.Id);
                }
                this.dragAndDropController.OnDrop(slotBySlotEntity.GetComponent<DragAndDropCell>(), cellFrom, component);
            }
        }

        private void OnTankPartModeChange()
        {
            selection.Clear();
        }

        public void OnVisualItemSelected(ListItem item)
        {
            VisualItem data = (VisualItem) item.Data;
            this.Mount(data.UserItem);
        }

        private void PlaceSlotItems()
        {
            List<ModuleItem> list = slotItems.Keys.ToList<ModuleItem>();
            list.Sort();
            for (int i = 0; i < list.Count; i++)
            {
                ModuleItem moduleItem = list[i];
                SlotItemView slotItemView = slotItems[moduleItem];
                if (moduleItem.Slot != null)
                {
                    this.AddItemToTankCollection(moduleItem, slotItemView);
                }
                else
                {
                    this.collectionView.AddSlotItem(moduleItem, slotItemView);
                }
                slotItemView.gameObject.SetActive(true);
            }
        }

        public void Show(TankPartModuleType tankPartModuleType)
        {
            MainScreenComponent.Instance.OverrideOnBack(new Action(this.Hide));
            this.showAnimationFinished = false;
            base.gameObject.SetActive(true);
            this.tankPartModeController.SetMode(tankPartModuleType);
        }

        private void Update()
        {
            if (this.needUpdate)
            {
                this.UpdateView();
                this.needUpdate = false;
            }
        }

        public void UpdateLineCollectionView()
        {
            this.hullCollectionView.lineCollectionView.slot1.SetActive(this.hullCollectionView.activeSlot.HasItem());
            this.hullCollectionView.lineCollectionView.slot2.SetActive(this.hullCollectionView.activeSlot2.HasItem());
            this.hullCollectionView.lineCollectionView.slot3.SetActive(this.hullCollectionView.passiveSlot.HasItem());
            this.turretCollectionView.lineCollectionView.slot1.SetActive(this.turretCollectionView.activeSlot.HasItem());
            this.turretCollectionView.lineCollectionView.slot2.SetActive(this.turretCollectionView.activeSlot2.HasItem());
            this.turretCollectionView.lineCollectionView.slot3.SetActive(this.turretCollectionView.passiveSlot.HasItem());
        }

        private void UpdateSlotItems()
        {
            foreach (KeyValuePair<ModuleItem, SlotItemView> pair in slotItems)
            {
                pair.Value.UpdateView(pair.Key);
            }
        }

        public void UpdateView()
        {
            this.hullCollectionView.UpdateView(this.Tank);
            this.turretCollectionView.UpdateView(this.Weapon);
            this.collectionView.UpdateView();
            this.CreateSlotItems();
            this.PlaceSlotItems();
            this.UpdateSlotItems();
            this.tankPartModeController.UpdateView();
            this.UpdateLineCollectionView();
            selection.Update(CollectionView.slots, slotItems);
            this.selectedModule = selection.GetSelectedModuleItem();
            if (this.selectedModule != null)
            {
                this.selectedModuleView.UpdateView(this.selectedModule, this.level2PowerByTier, this.Tank, this.Weapon);
            }
        }

        public void UpdateViewInNextFrame()
        {
            this.needUpdate = true;
        }

        [Inject]
        public static Tanks.Lobby.ClientGarage.API.GarageItemsRegistry GarageItemsRegistry { get; set; }

        public TankPartItem Weapon { get; set; }

        public TankPartItem Tank { get; set; }

        public string HullHealth =>
            this.hullHealth.Value;

        public string TurretDamage =>
            this.turretDamage.Value;

        public string Level =>
            this.level.Value;

        public ModuleItem SelectedModule =>
            this.selectedModule;
    }
}

