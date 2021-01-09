namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientNavigation.API;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class VisualUI : ECSBehaviour
    {
        [SerializeField]
        private RectTransform buttonsRoot;
        [SerializeField]
        private GameObject ammoButton;
        [SerializeField]
        private GameObject paintButton;
        [SerializeField]
        private GameObject coverButton;
        [SerializeField]
        private GameObject graffitiRoot;
        [SerializeField]
        private VisualUIListSwitch visualUIListSwitch;
        [SerializeField]
        private DefaultListDataProvider dataProvider;
        [SerializeField]
        private GameObject buyButton;
        [SerializeField]
        private GameObject xBuyButton;
        [SerializeField]
        private GameObject containersButton;
        [SerializeField]
        private GameObject equipButton;
        [SerializeField]
        private GaragePrice price;
        [SerializeField]
        private GaragePrice xPrice;
        [SerializeField]
        private TextMeshProUGUI itemName;
        [SerializeField]
        private float cameraOffset = -0.2f;
        private TankPartItem target;
        private List<VisualItem> skins;
        private List<VisualItem> paints;
        private List<VisualItem> graffities;
        private List<VisualItem> ammo;
        private List<VisualItem> nextList;
        private VisualItem selected;
        private int tab;
        public Action onEanble;
        [CompilerGenerated]
        private static Func<VisualItem, bool> <>f__am$cache0;
        [CompilerGenerated]
        private static Action <>f__am$cache1;
        [CompilerGenerated]
        private static Action <>f__am$cache2;

        public void Clear()
        {
            this.target = null;
            this.skins = null;
            this.paints = null;
            this.graffities = null;
            this.ammo = null;
            this.nextList = null;
            this.selected = null;
        }

        private List<VisualItem> GetActiveItemList(ICollection<VisualItem> list)
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = target => (target.IsBuyable || (target.UserItem != null)) || target.IsContainerItem;
            }
            return list.Where<VisualItem>(<>f__am$cache0).ToList<VisualItem>();
        }

        private List<VisualItem> GetItemsList<T>(Entity context = null) where T: GetAllItemsEvent<VisualItem>, new() => 
            this.SendEvent<T>(context).Items;

        public void OnBuy()
        {
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = delegate {
                };
            }
            FindObjectOfType<Dialogs60Component>().Get<BuyConfirmationDialog>().Show(this.selected, <>f__am$cache1, null, null);
        }

        public void OnContainer()
        {
            Entity entity = null;
            try
            {
                entity = base.Select<SingleNode<ContainerMarkerComponent>>(base.Select<SingleNode<ContainerContentItemComponent>>(this.selected.MarketItem, typeof(ContainerContentItemGroupComponent)).First<SingleNode<ContainerContentItemComponent>>().Entity, typeof(ContainerGroupComponent)).First<SingleNode<ContainerMarkerComponent>>().Entity;
            }
            catch (Exception)
            {
                Debug.Log("No such container");
            }
            if (entity != null)
            {
                ShowGarageCategoryEvent evt = new ShowGarageCategoryEvent {
                    Category = GarageCategory.CONTAINERS,
                    SelectedItem = entity
                };
                this.SendEvent<ShowGarageCategoryEvent>(evt, null);
            }
        }

        private void OnDisable()
        {
            this.Clear();
            this.graffitiRoot.SetActive(false);
        }

        private void OnDoubleClick(ListItem item)
        {
            this.OnItemSelect(item);
            VisualItem data = (VisualItem) item.Data;
            if (data.UserItem != null)
            {
                data.Mount(null);
            }
        }

        private void OnEnable()
        {
            if (this.onEanble != null)
            {
                this.onEanble();
            }
            FindObjectOfType<CameraOffsetBehaviour>().AnimateOffset(this.cameraOffset);
        }

        public void OnEquip()
        {
            this.SendEvent<MountItemEvent>(this.selected.UserItem);
        }

        private void OnItemChanged(VisualItem item)
        {
            if (item.UserItem != null)
            {
                this.buyButton.SetActive(false);
                this.xBuyButton.SetActive(false);
                this.containersButton.SetActive(false);
                this.equipButton.SetActive(!item.IsMounted && !item.IsRestricted);
            }
            else
            {
                int price = item.Price;
                int xPrice = item.XPrice;
                this.buyButton.SetActive(price > 0);
                this.xBuyButton.SetActive(xPrice > 0);
                this.equipButton.SetActive(false);
                this.price.SetPrice(item.OldPrice, price);
                this.xPrice.SetPrice(item.OldXPrice, xPrice);
                this.containersButton.SetActive((!this.buyButton.activeSelf && !this.xBuyButton.activeSelf) && item.MarketItem.HasComponent<ContainerContentItemGroupComponent>());
            }
        }

        private void OnItemSelect(ListItem item)
        {
            this.selected = (VisualItem) item.Data;
            this.OnItemChanged(this.selected);
        }

        public void OnUpgrade()
        {
            CustomizationUIComponent componentInParent = base.GetComponentInParent<CustomizationUIComponent>();
            this.SendEvent<ListItemSelectedEvent>(this.selected.ParentItem.UserItem);
            if (this.selected.ParentItem.Type == TankPartItem.TankPartItemType.Hull)
            {
                componentInParent.HullModules();
            }
            else
            {
                componentInParent.TurretModules();
            }
        }

        public void OnXBuy()
        {
            if (<>f__am$cache2 == null)
            {
                <>f__am$cache2 = delegate {
                };
            }
            FindObjectOfType<Dialogs60Component>().Get<BuyConfirmationDialog>().XShow(this.selected, <>f__am$cache2, this.selected.XPrice, 1, null, false, null);
        }

        public void ReturnCameraOffset()
        {
            CameraOffsetBehaviour behaviour = FindObjectOfType<CameraOffsetBehaviour>();
            if (behaviour != null)
            {
                behaviour.AnimateOffset(0f);
            }
        }

        public void Set(TankPartItem item, int tab)
        {
            if ((this.target == null) || !ReferenceEquals(this.target.MarketItem, item))
            {
                this.target = item;
                this.itemName.text = this.target.Name.ToUpper();
                this.skins = this.GetActiveItemList(this.target.Skins);
                this.skins.Sort();
                this.paints = (item.Type != TankPartItem.TankPartItemType.Hull) ? this.GetActiveItemList(GarageItemsRegistry.Coatings) : this.GetActiveItemList(GarageItemsRegistry.Paints);
                this.paints.Sort();
                this.graffities = this.GetActiveItemList(this.GetItemsList<GetAllGraffitiesEvent>(null));
                this.graffities.Sort();
                bool flag = this.target.Type == TankPartItem.TankPartItemType.Turret;
                this.ammoButton.SetActive(flag);
                this.coverButton.SetActive(flag);
                this.paintButton.SetActive(!flag);
                if (flag)
                {
                    this.ammo = this.GetActiveItemList(this.GetItemsList<GetAllShellsEvent>(this.target.MarketItem));
                    this.ammo.Sort();
                }
                this.selected = null;
                this.tab = tab;
                this.ToVisual(true);
            }
        }

        private void SetSelectionButton(GameObject button, bool selection)
        {
            Button component = button.GetComponent<Button>();
            component.interactable = !selection;
            if (component.interactable)
            {
                component.OnPointerExit(null);
            }
        }

        public void Switch()
        {
            this.UpdateList(this.nextList);
        }

        private void SwitchToList(List<VisualItem> items, bool immediate)
        {
            if (!ReferenceEquals(this.nextList, items))
            {
                if (immediate)
                {
                    this.UpdateList(items);
                }
                this.visualUIListSwitch.Animate();
                this.nextList = items;
            }
        }

        public void ToCover(bool immediate = false)
        {
            this.SwitchToList(this.paints, immediate);
            this.tab = 2;
            this.ValidateButtons();
            MainScreenComponent.Instance.SendShowScreenStat(LogScreen.TurretPaints);
        }

        public void ToGraffities(bool immediate = false)
        {
            this.SwitchToList(this.graffities, immediate);
            this.tab = 3;
            this.ValidateButtons();
            MainScreenComponent.Instance.SendShowScreenStat(LogScreen.Graffities);
        }

        public void ToPaints(bool immediate = false)
        {
            this.SwitchToList(this.paints, immediate);
            this.tab = 1;
            this.ValidateButtons();
            MainScreenComponent.Instance.SendShowScreenStat(LogScreen.HullPaints);
        }

        public void ToShells(bool immediate = false)
        {
            this.SwitchToList(this.ammo, immediate);
            this.tab = 4;
            this.ValidateButtons();
            MainScreenComponent.Instance.SendShowScreenStat(LogScreen.TurretShells);
        }

        public void ToSkins(bool immediate = false)
        {
            this.SwitchToList(this.skins, immediate);
            this.tab = 0;
            this.ValidateButtons();
            TankPartItem.TankPartItemType type = this.target.Type;
            if (type == TankPartItem.TankPartItemType.Hull)
            {
                MainScreenComponent.Instance.SendShowScreenStat(LogScreen.HullSkins);
            }
            else if (type == TankPartItem.TankPartItemType.Turret)
            {
                MainScreenComponent.Instance.SendShowScreenStat(LogScreen.TurretSkins);
            }
        }

        public void ToVisual(bool immediate = false)
        {
            switch (this.tab)
            {
                case 0:
                    this.ToSkins(immediate);
                    break;

                case 1:
                    if (this.target.Type == TankPartItem.TankPartItemType.Turret)
                    {
                        this.ToCover(immediate);
                    }
                    else
                    {
                        this.ToPaints(immediate);
                    }
                    break;

                case 2:
                    if (this.target.Type == TankPartItem.TankPartItemType.Turret)
                    {
                        this.ToCover(immediate);
                    }
                    else
                    {
                        this.ToPaints(immediate);
                    }
                    break;

                case 3:
                    this.ToGraffities(immediate);
                    break;

                case 4:
                    if (this.target.Type == TankPartItem.TankPartItemType.Turret)
                    {
                        this.ToShells(immediate);
                    }
                    else
                    {
                        this.ToSkins(immediate);
                    }
                    break;

                default:
                    break;
            }
        }

        private void UpdateList(ICollection<VisualItem> items)
        {
            this.graffitiRoot.SetActive(ReferenceEquals(items, this.graffities));
            this.dataProvider.ClearItems();
            this.selected = null;
            VisualItem item = null;
            foreach (VisualItem item2 in items)
            {
                if (item2.IsSelected)
                {
                    this.selected = item2;
                    this.OnItemChanged(item2);
                }
                if (item2.IsMounted)
                {
                    item = item2;
                }
            }
            if ((this.selected == null) && (item != null))
            {
                this.selected = item;
            }
            this.dataProvider.Init<VisualItem>(items, this.selected);
        }

        private void ValidateButtons()
        {
            for (int i = 0; i < this.buttonsRoot.childCount; i++)
            {
                this.SetSelectionButton(this.buttonsRoot.GetChild(i).gameObject, i == this.tab);
            }
            base.GetComponentInParent<CustomizationUIComponent>().SetVisualTab(this.tab);
        }

        [Inject]
        public static Tanks.Lobby.ClientGarage.API.GarageItemsRegistry GarageItemsRegistry { get; set; }
    }
}

