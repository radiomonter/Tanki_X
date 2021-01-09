namespace Tanks.Lobby.ClientGarage.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientUserProfile.Impl;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Events;

    public class ContainersUI : BehaviourComponent
    {
        [SerializeField]
        private Carousel carousel;
        [SerializeField]
        private TextMeshProUGUI title;
        [SerializeField]
        private TextMeshProUGUI openText;
        [SerializeField]
        private TextMeshProUGUI amount;
        [SerializeField]
        private LocalizedField containersAmountSingularText;
        [SerializeField]
        private LocalizedField containersAmountPlural1Text;
        [SerializeField]
        private LocalizedField containersAmountPlural2Text;
        [SerializeField]
        private LocalizedField openContainer;
        [SerializeField]
        private LocalizedField openAllContainers;
        [SerializeField]
        private GameObject openContainerBlock;
        [SerializeField]
        private TextMeshProUGUI openButtonText;
        [SerializeField]
        private BuyContainerButton buyButton;
        [SerializeField]
        private BuyContainerButton[] xBuyButtons;
        [SerializeField]
        private ContainerContentUI containerContent;
        [SerializeField]
        private Animator contentAnimator;
        [SerializeField]
        private GameObject previewButton;
        [SerializeField]
        private float contentCameraOffset;
        [SerializeField]
        private ContainerDescriptionUI containerDescription;
        [SerializeField]
        private bool blueprints;
        private ContainerBoxItem selected;
        public bool previewMode;
        private List<ContainerBoxItem> containers;
        [CompilerGenerated]
        private static Comparison<BuyContainerButton> <>f__am$cache0;

        private void ContainerSelected(GarageItemUI item)
        {
            ContainerBoxItem item2 = item.Item as ContainerBoxItem;
            this.openText.text = this.openContainer.Value;
            this.selected = item2;
            this.amount.text = string.Format(this.Pluralize(item2.Count), item2.Count);
            this.title.text = item2.Name;
            bool flag = item.Item.MarketItem.HasComponent<GameplayChestItemComponent>();
            this.openButtonText.text = (!flag || (item2.Count <= 1)) ? this.openContainer.Value : this.openAllContainers.Value;
            this.openContainerBlock.GetComponent<Animator>().SetBool("Visible", item2.Count > 0);
            if (item2.Price <= 0)
            {
                this.buyButton.gameObject.SetActive(false);
            }
            else
            {
                this.buyButton.gameObject.SetActive(true);
                this.buyButton.Init(1, item2.OldPrice, item2.Price);
            }
            int index = 0;
            Dictionary<int, int> packXPrices = item2.PackXPrices;
            if (packXPrices != null)
            {
                List<BuyContainerButton> list = new List<BuyContainerButton>();
                foreach (KeyValuePair<int, int> pair in packXPrices)
                {
                    BuyContainerButton button = this.xBuyButtons[index];
                    button.gameObject.SetActive(true);
                    button.Init(pair.Key, pair.Key * item2.XPrice, pair.Value);
                    list.Add(button);
                    index++;
                }
                if (<>f__am$cache0 == null)
                {
                    <>f__am$cache0 = (a, b) => a.Price.CompareTo(b.Price);
                }
                list.Sort(<>f__am$cache0);
                foreach (BuyContainerButton button2 in list)
                {
                    button2.transform.SetAsLastSibling();
                }
            }
            while (index < this.xBuyButtons.Length)
            {
                this.xBuyButtons[index].gameObject.SetActive(false);
                index++;
            }
            bool flag2 = item2.Content.Count > 0;
            this.containerContent.gameObject.SetActive(flag2);
            if (flag2)
            {
                this.containerContent.Set(this.carousel.Selected.Item as ContainerBoxItem, false);
                this.containerDescription.gameObject.SetActive(false);
            }
            else
            {
                this.containerDescription.gameObject.SetActive(true);
                this.containerDescription.Title.text = this.carousel.Selected.Item.MarketItem.GetComponent<DescriptionItemComponent>().Name;
                this.containerDescription.Description.text = this.carousel.Selected.Item.MarketItem.GetComponent<DescriptionItemComponent>().Description;
            }
        }

        public void DeleteContainerItem(long marketItem)
        {
            this.carousel.RemoveItem(marketItem);
        }

        public List<ContainerBoxItem> GetContainers() => 
            this.containers;

        public void OnBuy()
        {
            <OnBuy>c__AnonStorey1 storey = new <OnBuy>c__AnonStorey1 {
                $this = this,
                item = this.carousel.Selected.Item
            };
            FindObjectOfType<Dialogs60Component>().Get<BuyConfirmationDialog>().Show(storey.item, new Action(storey.<>m__0), null, null);
        }

        public void OnContentPreview()
        {
            this.ShowPreview();
            this.containerContent.Set(this.carousel.Selected.Item as ContainerBoxItem, true);
        }

        public void OnEnable()
        {
            this.UpdateContainerUI();
        }

        public void OnHide()
        {
            this.containerContent.GraffitiRoot.SetActive(false);
        }

        public void OnItemSelect(ListItem item)
        {
            this.ShowPreview();
        }

        public void OnOpen()
        {
            <OnOpen>c__AnonStorey3 storey = new <OnOpen>c__AnonStorey3 {
                $this = this,
                item = this.carousel.Selected.Item as ContainerBoxItem
            };
            this.openContainerBlock.GetComponent<Animator>().SetBool("Visible", false);
            storey.item.Open(new Action(storey.<>m__0));
        }

        public void OnXBuy(int buttonIndex)
        {
            <OnXBuy>c__AnonStorey2 storey = new <OnXBuy>c__AnonStorey2 {
                $this = this
            };
            BuyContainerButton button = this.xBuyButtons[buttonIndex];
            storey.item = this.carousel.Selected.Item;
            FindObjectOfType<Dialogs60Component>().Get<BuyConfirmationDialog>().XShow(storey.item, new Action(storey.<>m__0), button.Price, button.Amount, this.Pluralize(button.Amount) + " " + storey.item.Name, false, null);
        }

        private string Pluralize(int amount)
        {
            CaseType @case = CasesUtil.GetCase(amount);
            if (@case == CaseType.DEFAULT)
            {
                return string.Format(this.containersAmountPlural1Text.Value, amount);
            }
            if (@case == CaseType.ONE)
            {
                return string.Format(this.containersAmountSingularText.Value, amount);
            }
            if (@case != CaseType.TWO)
            {
                throw new Exception("ivnalid case");
            }
            return string.Format(this.containersAmountPlural2Text.Value, amount);
        }

        private void ShowPreview()
        {
            <ShowPreview>c__AnonStorey4 storey = new <ShowPreview>c__AnonStorey4 {
                $this = this
            };
            if (!this.previewMode)
            {
                this.previewMode = true;
                this.previewButton.SetActive(false);
                this.contentAnimator.SetBool("ShowContent", true);
                this.containerContent.CheckGraffityVisibility();
                storey.onBack = MainScreenComponent.Instance.OnBack;
                MainScreenComponent.Instance.OverrideOnBack(new Action(storey.<>m__0));
            }
        }

        public void UpdateContainerUI()
        {
            <UpdateContainerUI>c__AnonStorey0 storey = new <UpdateContainerUI>c__AnonStorey0 {
                $this = this
            };
            this.previewMode = false;
            this.containerContent.GraffitiRoot.SetActive(false);
            this.previewButton.SetActive(true);
            storey.selfUser = ECSBehaviour.EngineService.Engine.SelectAll<SelfUserNode>().FirstOrDefault<SelfUserNode>();
            storey.fractionCompetition = ECSBehaviour.EngineService.Engine.SelectAll<FractionCompetitionNode>().FirstOrDefault<FractionCompetitionNode>();
            storey.lockByFraction = new Func<ContainerBoxItem, bool>(storey.<>m__0);
            this.containers = GarageItemsRegistry.Containers.Where<ContainerBoxItem>(new Func<ContainerBoxItem, bool>(storey.<>m__1)).ToList<ContainerBoxItem>();
            this.containers.Sort();
            this.carousel.AddItems<ContainerBoxItem>(this.containers);
            this.carousel.onItemSelected = new UnityAction<GarageItemUI>(this.ContainerSelected);
            foreach (ContainerBoxItem item in this.containers)
            {
                if (item.IsSelected)
                {
                    this.selected = item;
                    break;
                }
            }
            if (!this.containers.Contains(this.selected))
            {
                this.selected = this.containers.First<ContainerBoxItem>();
            }
            if (this.carousel.IsAnySelected)
            {
                this.carousel.Selected.Deselect();
            }
            this.carousel.Select(this.selected, true);
        }

        [Inject]
        public static Tanks.Lobby.ClientGarage.API.GarageItemsRegistry GarageItemsRegistry { get; set; }

        public bool Blueprints =>
            this.blueprints;

        [CompilerGenerated]
        private sealed class <OnBuy>c__AnonStorey1
        {
            internal GarageItem item;
            internal ContainersUI $this;

            internal void <>m__0()
            {
                if (this.$this.gameObject.activeInHierarchy)
                {
                    if (ReferenceEquals(this.$this.carousel.Selected.Item, this.item))
                    {
                        this.$this.ContainerSelected(this.$this.carousel.Selected);
                    }
                    else
                    {
                        this.$this.carousel.Select(this.item, false);
                    }
                }
            }
        }

        [CompilerGenerated]
        private sealed class <OnOpen>c__AnonStorey3
        {
            internal ContainerBoxItem item;
            internal ContainersUI $this;

            internal void <>m__0()
            {
                this.$this.openContainerBlock.GetComponent<Animator>().SetBool("Visible", this.item.Count > 0);
                if (this.$this.gameObject.activeInHierarchy)
                {
                    if (!ReferenceEquals(this.$this.carousel.Selected.Item, this.item))
                    {
                        this.$this.carousel.Select(this.item, false);
                    }
                    else
                    {
                        this.$this.ContainerSelected(this.$this.carousel.Selected);
                        if (this.$this.containerContent.gameObject.activeInHierarchy)
                        {
                            foreach (ContainerContentItemUIContent content in this.$this.containerContent.GetComponentsInChildren<ContainerContentItemUIContent>())
                            {
                                content.UpdateOwn();
                            }
                        }
                    }
                }
            }
        }

        [CompilerGenerated]
        private sealed class <OnXBuy>c__AnonStorey2
        {
            internal GarageItem item;
            internal ContainersUI $this;

            internal void <>m__0()
            {
                if (this.$this.gameObject.activeInHierarchy)
                {
                    if (ReferenceEquals(this.$this.carousel.Selected.Item, this.item))
                    {
                        this.$this.ContainerSelected(this.$this.carousel.Selected);
                    }
                    else
                    {
                        this.$this.carousel.Select(this.item, false);
                    }
                }
            }
        }

        [CompilerGenerated]
        private sealed class <ShowPreview>c__AnonStorey4
        {
            internal Action onBack;
            internal ContainersUI $this;

            internal void <>m__0()
            {
                this.$this.contentAnimator.SetBool("ShowContent", false);
                this.$this.OnEnable();
                MainScreenComponent.Instance.OverrideOnBack(this.onBack);
            }
        }

        [CompilerGenerated]
        private sealed class <UpdateContainerUI>c__AnonStorey0
        {
            internal ContainersUI.FractionCompetitionNode fractionCompetition;
            internal ContainersUI.SelfUserNode selfUser;
            internal Func<ContainerBoxItem, bool> lockByFraction;
            internal ContainersUI $this;

            internal bool <>m__0(ContainerBoxItem container) => 
                (this.fractionCompetition != null) ? (container.MarketItem.HasComponent<RestrictionByUserFractionComponent>() ? ((container.MarketItem.GetComponent<RestrictionByUserFractionComponent>().FractionId != 0L) ? (this.selfUser.Entity.HasComponent<FractionGroupComponent>() ? (container.MarketItem.GetComponent<RestrictionByUserFractionComponent>().FractionId != this.selfUser.Entity.GetComponent<FractionGroupComponent>().Key) : true) : false) : false) : false;

            internal bool <>m__1(ContainerBoxItem x)
            {
                bool flag1;
                if (((x.Count <= 0) && (!x.IsBuyable && (x.PackXPrices == null))) || (this.$this.blueprints != x.IsBlueprint))
                {
                    flag1 = false;
                }
                else
                {
                    flag1 = !this.lockByFraction(x);
                }
                return flag1;
            }
        }

        public class FractionCompetitionNode : Node
        {
            public FractionsCompetitionInfoComponent fractionsCompetitionInfo;
        }

        public class SelfUserNode : Node
        {
            public SelfUserComponent selfUser;
            public UserUidComponent userUid;
        }
    }
}

