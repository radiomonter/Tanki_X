namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.API;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    public class ItemSelectUI : MonoBehaviour
    {
        private Tanks.Lobby.ClientGarage.Impl.Carousel carousel;
        [SerializeField]
        private TextMeshProUGUI itemName;
        [SerializeField]
        private TextMeshProUGUI feature;
        [SerializeField]
        private TextMeshProUGUI description;
        [SerializeField]
        private MainVisualPropertyUI[] props;
        [SerializeField]
        private AnimatedNumber mastery;
        [SerializeField]
        private Animator buttonsAnimator;
        [SerializeField]
        private BuyItemButton buyButton;
        [SerializeField]
        private BuyItemButton xBuyButton;
        [SerializeField]
        private TextMeshProUGUI crystalsRestrictionMismatch;
        [SerializeField]
        private TextMeshProUGUI crystalsRestrictionMatch;
        [SerializeField]
        private LocalizedField crystalsRestrictionMismatchField;
        [SerializeField]
        private LocalizedField crystalsRestrictionMatchField;
        [SerializeField]
        private Button changeSkinButton;
        [SerializeField]
        private Button changePaintButton;
        [SerializeField]
        private Button changeAmmoButton;
        [SerializeField]
        private Button changeCoverButton;
        [SerializeField]
        private CustomizationUIComponent customizationUI;
        private Entity savedSelection;
        private GarageItemUI currentGarageItemUi;
        private Action onEnable;

        private void Awake()
        {
            this.changeSkinButton.onClick.AddListener(new UnityAction(this.ChangeSkin));
            this.changePaintButton.onClick.AddListener(new UnityAction(this.ChangePaint));
            this.changeCoverButton.onClick.AddListener(new UnityAction(this.ChangeCover));
            this.changeAmmoButton.onClick.AddListener(new UnityAction(this.ChangeAmmo));
        }

        private void ChangeAmmo()
        {
            this.customizationUI.TurretVisualNoSwitch(4);
        }

        private void ChangeCover()
        {
            this.customizationUI.TurretVisualNoSwitch(2);
        }

        private void ChangePaint()
        {
            this.customizationUI.HullVisualNoSwitch(1);
        }

        private void ChangeSkin()
        {
            if (this.SelectedItem.Type == TankPartItem.TankPartItemType.Turret)
            {
                this.customizationUI.TurretVisualNoSwitch(0);
            }
            else
            {
                this.customizationUI.HullVisualNoSwitch(0);
            }
        }

        private void OnAnyBuyCallback(TankPartItem item)
        {
            if (this.Carousel.gameObject.activeInHierarchy && ReferenceEquals(item, this.Carousel.Selected.Item))
            {
                this.buttonsAnimator.SetBool("Bought", true);
                item.WaitForBuy = false;
            }
        }

        public void OnBuy()
        {
            <OnBuy>c__AnonStorey1 storey = new <OnBuy>c__AnonStorey1 {
                $this = this,
                item = this.Carousel.Selected.Item as TankPartItem
            };
            FindObjectOfType<Dialogs60Component>().Get<BuyConfirmationDialog>().Show(storey.item, new Action(storey.<>m__0), null, null);
        }

        private void OnDisable()
        {
            this.currentGarageItemUi = null;
        }

        private void OnEnable()
        {
            if (this.onEnable != null)
            {
                this.onEnable();
            }
        }

        private void OnItemSelect(GarageItemUI item)
        {
            <OnItemSelect>c__AnonStorey0 storey = new <OnItemSelect>c__AnonStorey0 {
                $this = this
            };
            TankPartItem item2 = item.Item as TankPartItem;
            bool flag = item2.Type == TankPartItem.TankPartItemType.Turret;
            this.changeAmmoButton.gameObject.SetActive(flag);
            this.changeCoverButton.gameObject.SetActive(flag);
            this.changePaintButton.gameObject.SetActive(!flag);
            storey.marketItem = item.Item.MarketItem;
            base.GetComponentInParent<MainScreenComponent>().SetOnBackCallback(new Action(storey.<>m__0));
            bool flag2 = !ReferenceEquals(item.Item.UserItem, null);
            DescriptionItemComponent component = storey.marketItem.GetComponent<DescriptionItemComponent>();
            VisualPropertiesComponent component2 = storey.marketItem.GetComponent<VisualPropertiesComponent>();
            this.itemName.text = component.Name.ToUpper();
            this.feature.text = component2.Feature;
            this.description.text = component.Description;
            this.description.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            this.buttonsAnimator.SetBool("Bought", flag2);
            for (int i = 0; i < component2.MainProperties.Count; i++)
            {
                this.props[i].gameObject.SetActive(true);
                this.props[i].Set(component2.MainProperties[i].Name, component2.MainProperties[i].NormalizedValue);
            }
            for (int j = component2.MainProperties.Count; j < this.props.Length; j++)
            {
                this.props[j].gameObject.SetActive(false);
            }
            this.mastery.transform.parent.gameObject.SetActive(flag2);
            if (flag2)
            {
                this.mastery.Value = item2.UpgradeLevel;
            }
            else
            {
                int rank = SelfUserComponent.SelfUser.GetComponent<UserRankComponent>().Rank;
                bool flag3 = item.Item.MarketItem.HasComponent<CrystalsPurchaseUserRankRestrictionComponent>();
                int num4 = !flag3 ? 0 : item.Item.MarketItem.GetComponent<CrystalsPurchaseUserRankRestrictionComponent>().RestrictionValue;
                bool flag4 = num4 <= rank;
                int price = item.Item.Price;
                if (price <= 0)
                {
                    this.buyButton.gameObject.SetActive(false);
                }
                else
                {
                    this.buyButton.gameObject.SetActive(true);
                    this.buyButton.SetPrice(item.Item.OldPrice, price);
                    this.buyButton.Button.interactable = flag4;
                    this.crystalsRestrictionMatch.gameObject.SetActive(false);
                    this.crystalsRestrictionMismatch.gameObject.SetActive(flag3 && !flag4);
                    this.crystalsRestrictionMismatch.SetText(string.Format(this.crystalsRestrictionMismatchField.Value, num4));
                    this.crystalsRestrictionMatch.SetText(string.Format(this.crystalsRestrictionMatchField.Value, num4));
                }
                int xPrice = item.Item.XPrice;
                if (xPrice <= 0)
                {
                    this.xBuyButton.gameObject.SetActive(false);
                }
                else
                {
                    this.xBuyButton.gameObject.SetActive(flag3 && !flag4);
                    this.xBuyButton.SetPrice(item.Item.OldXPrice, xPrice);
                    this.xBuyButton.Button.interactable = !flag4;
                }
            }
        }

        public void OnXBuy()
        {
            <OnXBuy>c__AnonStorey2 storey = new <OnXBuy>c__AnonStorey2 {
                $this = this,
                item = this.Carousel.Selected.Item as TankPartItem
            };
            FindObjectOfType<Dialogs60Component>().Get<BuyConfirmationDialog>().XShow(storey.item, new Action(storey.<>m__0), storey.item.XPrice, 1, null, false, null);
        }

        public void RefreshSelection()
        {
            if (base.gameObject.activeSelf && this.currentGarageItemUi)
            {
                this.OnItemSelect(this.currentGarageItemUi);
            }
        }

        public void SetItems(ICollection<TankPartItem> items, TankPartItem mountedItem)
        {
            <SetItems>c__AnonStorey3 storey = new <SetItems>c__AnonStorey3 {
                items = items,
                mountedItem = mountedItem,
                $this = this
            };
            this.onEnable = new Action(storey.<>m__0);
        }

        public void SubmitSelection()
        {
            this.IsSelected = true;
            MainScreenComponent componentInParent = base.GetComponentInParent<MainScreenComponent>();
            componentInParent.DisableReset();
            if (this.SelectedItem.Type == TankPartItem.TankPartItemType.Hull)
            {
                componentInParent.MountedHull = this.SelectedItem;
            }
            else
            {
                componentInParent.MountedTurret = this.SelectedItem;
            }
            this.SelectedItem.Mount(null);
        }

        private Tanks.Lobby.ClientGarage.Impl.Carousel Carousel
        {
            get
            {
                if (this.carousel == null)
                {
                    this.carousel = base.GetComponentInChildren<Tanks.Lobby.ClientGarage.Impl.Carousel>();
                }
                return this.carousel;
            }
        }

        private TankPartItem SelectedItem =>
            this.Carousel.Selected.Item as TankPartItem;

        public bool IsSelected { get; private set; }

        [CompilerGenerated]
        private sealed class <OnBuy>c__AnonStorey1
        {
            internal TankPartItem item;
            internal ItemSelectUI $this;

            internal void <>m__0()
            {
                this.$this.OnAnyBuyCallback(this.item);
            }
        }

        [CompilerGenerated]
        private sealed class <OnItemSelect>c__AnonStorey0
        {
            internal Entity marketItem;
            internal ItemSelectUI $this;

            internal void <>m__0()
            {
                this.$this.savedSelection = this.marketItem;
            }
        }

        [CompilerGenerated]
        private sealed class <OnXBuy>c__AnonStorey2
        {
            internal TankPartItem item;
            internal ItemSelectUI $this;

            internal void <>m__0()
            {
                this.$this.OnAnyBuyCallback(this.item);
            }
        }

        [CompilerGenerated]
        private sealed class <SetItems>c__AnonStorey3
        {
            internal ICollection<TankPartItem> items;
            internal TankPartItem mountedItem;
            internal ItemSelectUI $this;

            internal void <>m__0()
            {
                List<TankPartItem> newItems = this.items.ToList<TankPartItem>();
                newItems.Sort();
                this.$this.IsSelected = false;
                this.$this.Carousel.AddItems<TankPartItem>(newItems);
                this.$this.Carousel.onItemSelected = new UnityAction<GarageItemUI>(this.$this.OnItemSelect);
                bool flag = false;
                foreach (TankPartItem item in newItems)
                {
                    if ((this.$this.savedSelection == null) && ReferenceEquals(item, this.mountedItem))
                    {
                        flag = true;
                        this.$this.Carousel.Select(item, true);
                    }
                    else
                    {
                        if ((this.$this.savedSelection == null) || !ReferenceEquals(item.MarketItem, this.$this.savedSelection))
                        {
                            continue;
                        }
                        flag = true;
                        this.$this.Carousel.Select(item, true);
                    }
                    break;
                }
                this.$this.savedSelection = null;
                if (!flag)
                {
                    this.$this.Carousel.Select(newItems.First<TankPartItem>(), true);
                }
            }
        }
    }
}

