namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientGarage.Impl;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class DealsUIComponent : PurchaseItemComponent
    {
        [SerializeField]
        private RectTransform bigRowsContainer;
        [SerializeField]
        private RectTransform[] availableRows;
        [SerializeField]
        private SpecialOfferContent specialOfferPrefab;
        [SerializeField]
        private MarketItemSaleContent marketItemSalePrefab;
        [SerializeField]
        private FirstPurchaseDiscountContent firstPurchaseDiscountPrefab;
        [SerializeField]
        private GameObject bonusPrefab;
        [SerializeField]
        private GameObject quantumPrefab;
        [SerializeField]
        public LeagueSpecialOfferComponent leagueSpecialOfferPrefab;
        [SerializeField]
        private List<GiftPromo> promo;
        private GameObject promoGameObject;
        [SerializeField]
        private ScrollRect scrollRect;
        [SerializeField]
        private RectTransform scrollContent;
        [SerializeField]
        private float autoScrollSpeed = 1f;
        [SerializeField]
        private float pageWidth = 250f;
        [SerializeField]
        private int pageCount;
        [SerializeField]
        private int currentPage = 1;
        [SerializeField]
        private bool interactWithScrollView;
        [SerializeField]
        private bool scrollMode;
        [SerializeField]
        private Button leftScrollButton;
        [SerializeField]
        private Button rightScrollButton;
        [SerializeField]
        private GameObject noDealsText;
        private int contentRowsCount;
        private bool secondBigRowFilled;
        private List<long> marketItems;
        [CompilerGenerated]
        private static Action <>f__am$cache0;

        public void AddBonus(Entity entity, Entity user)
        {
            <AddBonus>c__AnonStorey2 storey = new <AddBonus>c__AnonStorey2 {
                entity = entity,
                $this = this
            };
            this.contentRowsCount++;
            EnergyBonusContent component = this.AddItem(this.bonusPrefab).GetComponent<EnergyBonusContent>();
            component.Premium = user.HasComponent<PremiumAccountBoostComponent>();
            component.SetDataProvider(storey.entity);
            component.GetComponentInChildren<Button>().onClick.AddListener(new UnityAction(storey.<>m__0));
            this.UpdateOrders();
        }

        public void AddFirstPurchaseDiscount(double discount)
        {
            this.contentRowsCount++;
            FirstPurchaseDiscountContent component = this.AddItem(this.firstPurchaseDiscountPrefab.gameObject).GetComponent<FirstPurchaseDiscountContent>();
            component.Discount = discount;
            component.GetComponentInChildren<Button>().onClick.AddListener(() => this.OnFirstPurchaseDiscountClick());
            this.UpdateOrders();
        }

        private GameObject AddItem(GameObject prefab)
        {
            GameObject obj2 = Instantiate<GameObject>(prefab);
            obj2.GetComponentInChildren<DealItemContent>().SetParent(base.transform);
            return obj2;
        }

        public void AddMarketItem(Entity entity)
        {
            <AddMarketItem>c__AnonStorey4 storey = new <AddMarketItem>c__AnonStorey4 {
                entity = entity,
                $this = this
            };
            if (!this.marketItems.Contains(storey.entity.Id))
            {
                this.marketItems.Add(storey.entity.Id);
                this.contentRowsCount++;
                MarketItemSaleContent component = this.AddItem(this.marketItemSalePrefab.gameObject).GetComponent<MarketItemSaleContent>();
                component.SetDataProvider(storey.entity);
                component.GetComponentInChildren<Button>().onClick.AddListener(new UnityAction(storey.<>m__0));
                this.UpdateOrders();
            }
        }

        public void AddPromo(string key)
        {
            <AddPromo>c__AnonStorey1 storey = new <AddPromo>c__AnonStorey1 {
                key = key
            };
            if (this.promoGameObject == null)
            {
                GiftPromo promo = this.promo.FirstOrDefault<GiftPromo>(new Func<GiftPromo, bool>(storey.<>m__0));
                if (promo != null)
                {
                    GameObject prefab = promo.Prefab;
                    if (prefab != null)
                    {
                        this.promoGameObject = Instantiate<GameObject>(prefab);
                        this.promoGameObject.transform.SetParent(base.transform, false);
                        this.contentRowsCount++;
                        this.UpdateOrders();
                    }
                }
            }
        }

        public void AddQuantum(Entity entity)
        {
            <AddQuantum>c__AnonStorey3 storey = new <AddQuantum>c__AnonStorey3 {
                entity = entity,
                $this = this
            };
            this.contentRowsCount++;
            QuantumShopContent component = this.AddItem(this.quantumPrefab).GetComponent<QuantumShopContent>();
            component.SetDataProvider(storey.entity);
            component.GetComponentInChildren<Button>().onClick.AddListener(new UnityAction(storey.<>m__0));
            this.UpdateOrders();
        }

        public SpecialOfferContent AddSpecialOffer(Entity specialOffer, GameObject customPrefab = null)
        {
            <AddSpecialOffer>c__AnonStorey0 storey = new <AddSpecialOffer>c__AnonStorey0 {
                specialOffer = specialOffer,
                $this = this
            };
            this.contentRowsCount++;
            GameObject obj2 = this.PresentSpecilaOffer(storey.specialOffer);
            if (obj2 != null)
            {
                obj2.transform.parent = null;
                Destroy(obj2);
                this.contentRowsCount--;
            }
            GameObject prefab = customPrefab;
            if (customPrefab == null)
            {
                GameObject local1 = customPrefab;
                prefab = this.specialOfferPrefab.gameObject;
            }
            SpecialOfferContent component = this.AddItem(prefab).GetComponent<SpecialOfferContent>();
            component.SetDataProvider(storey.specialOffer);
            component.GetComponentInChildren<Button>().onClick.AddListener(new UnityAction(storey.<>m__0));
            this.UpdateOrders();
            return component;
        }

        private bool CheckUserItem(object obj)
        {
            DealItemContent content = obj as DealItemContent;
            if (content == null)
            {
                return false;
            }
            if ((content.Entity == null) || (!content.Entity.HasComponent<GarageItemComponent>() || content.Entity.HasComponent<ContainerMarkerComponent>()))
            {
                return false;
            }
            if (GarageItemsRegistry.GetItem<GarageItem>(content.Entity).UserItem == null)
            {
                return (content.Entity.HasComponent<XPriceItemComponent>() && !content.Entity.GetComponent<XPriceItemComponent>().IsBuyable);
            }
            if (content.gameObject.activeSelf)
            {
                content.gameObject.SetActive(false);
                this.contentRowsCount--;
            }
            return true;
        }

        public void Clear()
        {
            foreach (DealItemContent content in base.GetComponentsInChildren<DealItemContent>(true))
            {
                Destroy(content.gameObject);
            }
            base.methods.Clear();
            this.contentRowsCount = 0;
        }

        private int Compare(ContentWithOrder a, ContentWithOrder b) => 
            (a.Order <= b.Order) ? ((a.Order >= b.Order) ? 0 : -1) : 1;

        private void Layout()
        {
            if (!this.scrollMode)
            {
                Canvas.ForceUpdateCanvases();
                this.scrollContent.anchoredPosition = new Vector2((this.scrollRect.GetComponent<RectTransform>().rect.width - this.scrollContent.rect.width) / 2f, 0f);
            }
        }

        private void OnBonusClick(Entity entity)
        {
            ECSBehaviour.EngineService.Engine.ScheduleEvent<TryUseBonusEvent>(entity);
        }

        private void OnEnable()
        {
            this.interactWithScrollView = false;
            this.marketItems = new List<long>();
        }

        private void OnFirstPurchaseDiscountClick()
        {
            ShopTabManager.Instance.Show(3);
        }

        private void OnItemBuyCallback(GarageItem item)
        {
            if (!item.MarketItem.HasComponent<ContainerMarkerComponent>())
            {
                this.UpdateOrders();
            }
            else
            {
                ShowGarageCategoryEvent eventInstance = new ShowGarageCategoryEvent {
                    Category = GarageCategory.CONTAINERS,
                    SelectedItem = item.MarketItem
                };
                ECSBehaviour.EngineService.Engine.ScheduleEvent(eventInstance, item.MarketItem);
            }
        }

        private void OnMarketItemClick(Entity entity)
        {
            <OnMarketItemClick>c__AnonStorey5 storey = new <OnMarketItemClick>c__AnonStorey5 {
                $this = this,
                item = GarageItemsRegistry.GetItem<GarageItem>(entity)
            };
            FindObjectOfType<Dialogs60Component>().Get<BuyConfirmationDialog>().XShow(storey.item, new Action(storey.<>m__0), storey.item.XPrice, 1, null, false, null);
        }

        public void OnPointerDown()
        {
            if (!this.interactWithScrollView && !this.PointerOnScrollButtons())
            {
                this.interactWithScrollView = true;
            }
        }

        public void OnPointerUp()
        {
            if (this.scrollMode && this.interactWithScrollView)
            {
                this.interactWithScrollView = false;
                this.currentPage = Mathf.Min(Mathf.Max(0, Mathf.RoundToInt(-this.scrollContent.anchoredPosition.x / this.pageWidth)) + 1, this.pageCount);
                this.SetScrollButtons();
            }
        }

        private void OnQuantumClick(Entity entity)
        {
            GarageItem item = GarageItemsRegistry.GetItem<GarageItem>(entity);
            item.MarketItem = entity;
            KeyValuePair<int, int> pair = entity.GetComponent<PackPriceComponent>().PackXPrice.First<KeyValuePair<int, int>>();
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = delegate {
                };
            }
            FindObjectOfType<Dialogs60Component>().Get<BuyConfirmationDialog>().XShow(item, <>f__am$cache0, pair.Value, pair.Key, null, false, null);
        }

        private bool PointerOnScrollButtons()
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current) {
                position = Input.mousePosition
            };
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, raycastResults);
            if (raycastResults.Count > 0)
            {
                using (List<RaycastResult>.Enumerator enumerator = raycastResults.GetEnumerator())
                {
                    while (true)
                    {
                        if (!enumerator.MoveNext())
                        {
                            break;
                        }
                        RaycastResult current = enumerator.Current;
                        if ((current.gameObject == this.leftScrollButton.gameObject) || (current.gameObject == this.rightScrollButton))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private GameObject PresentSpecilaOffer(Entity specialOffer)
        {
            if (specialOffer != null)
            {
                SpecialOfferContent[] componentsInChildren = base.GetComponentsInChildren<SpecialOfferContent>();
                for (int i = 0; i < componentsInChildren.Length; i++)
                {
                    if (ReferenceEquals(componentsInChildren[i].Entity, specialOffer))
                    {
                        return componentsInChildren[i].gameObject;
                    }
                }
            }
            return null;
        }

        public void RemovePromo()
        {
            if (this.promoGameObject != null)
            {
                this.promoGameObject.transform.SetParent(null);
                this.promoGameObject.SetActive(false);
                Destroy(this.promoGameObject);
                this.promoGameObject = null;
                this.contentRowsCount--;
                this.UpdateOrders();
            }
        }

        public void RemoveSpecialOffer(Entity specialOffer)
        {
            GameObject obj2 = this.PresentSpecilaOffer(specialOffer);
            if (obj2 != null)
            {
                obj2.transform.parent = null;
                Destroy(obj2);
                this.contentRowsCount--;
            }
            this.UpdateOrders();
        }

        private void ScrollLeft()
        {
            this.currentPage = Mathf.Max(1, this.currentPage - 1);
            this.interactWithScrollView = false;
            this.SetScrollButtons();
        }

        private void ScrollOff()
        {
            this.scrollRect.enabled = false;
            this.scrollRect.horizontalScrollbar.gameObject.SetActive(false);
            this.scrollMode = false;
            this.SetScrollButtons();
            this.Layout();
        }

        private void ScrollOn()
        {
            this.scrollRect.enabled = true;
            this.scrollMode = true;
            this.SetScrollButtons();
        }

        private void ScrollRight()
        {
            this.currentPage = Mathf.Min(this.pageCount, this.currentPage + 1);
            this.interactWithScrollView = false;
            this.SetScrollButtons();
        }

        private void SetScrollButtons()
        {
            this.leftScrollButton.GetComponent<CanvasGroup>().alpha = ((this.currentPage <= 1) || !this.scrollMode) ? 0f : 1f;
            this.rightScrollButton.GetComponent<CanvasGroup>().alpha = ((this.currentPage >= this.pageCount) || !this.scrollMode) ? 0f : 1f;
        }

        private void Start()
        {
            this.leftScrollButton.onClick.AddListener(new UnityAction(this.ScrollLeft));
            this.rightScrollButton.onClick.AddListener(new UnityAction(this.ScrollRight));
        }

        private void Update()
        {
            this.UpdateScroll();
            if (Input.GetMouseButton(0))
            {
                this.OnPointerDown();
            }
            else
            {
                this.OnPointerUp();
            }
            float axis = Input.GetAxis("Mouse ScrollWheel");
            if (axis > 0f)
            {
                this.ScrollLeft();
            }
            else if (axis < 0f)
            {
                this.ScrollRight();
            }
        }

        private void UpdateOrders()
        {
            ContentWithOrder[] componentsInChildren = base.GetComponentsInChildren<ContentWithOrder>(true);
            Array.Sort<ContentWithOrder>(componentsInChildren, new Comparison<ContentWithOrder>(this.Compare));
            bool flag = false;
            this.secondBigRowFilled = false;
            int a = 0;
            this.availableRows[2].gameObject.SetActive(true);
            for (int i = 0; i < componentsInChildren.Length; i++)
            {
                bool flag2 = false;
                if (!this.CheckUserItem(componentsInChildren[i]))
                {
                    if ((a == 0) && (i == 0))
                    {
                        if (componentsInChildren[i].CanFillBigRow)
                        {
                            flag = true;
                        }
                        else
                        {
                            a++;
                        }
                    }
                    if (flag && ((a == 1) && ((i == 1) && !componentsInChildren[i].CanFillSmallRow)))
                    {
                        this.availableRows[2].gameObject.SetActive(false);
                        flag2 = true;
                        this.secondBigRowFilled = true;
                    }
                    componentsInChildren[i].SetParent(this.availableRows[Mathf.Min(a, this.availableRows.Length - 1)]);
                    if (flag2)
                    {
                        a++;
                    }
                    a++;
                }
            }
            if (flag)
            {
                DealItemContent[] array = this.availableRows[this.availableRows.Length - 1].GetComponentsInChildren<DealItemContent>(true);
                Array.Sort<DealItemContent>(array, new Comparison<DealItemContent>(this.Compare));
                int index = 0;
                while (true)
                {
                    if (index >= array.Length)
                    {
                        this.bigRowsContainer.gameObject.SetActive(true);
                        break;
                    }
                    array[index].transform.SetSiblingIndex(index);
                    index++;
                }
            }
            else
            {
                RectTransform parent = this.availableRows[this.availableRows.Length - 1];
                int index = 0;
                while (true)
                {
                    if (index >= (this.availableRows.Length - 1))
                    {
                        this.bigRowsContainer.gameObject.SetActive(false);
                        break;
                    }
                    DealItemContent[] contentArray2 = this.availableRows[index].GetComponentsInChildren<DealItemContent>(true);
                    int num4 = 0;
                    while (true)
                    {
                        if (num4 >= contentArray2.Length)
                        {
                            index++;
                            break;
                        }
                        DealItemContent content = contentArray2[num4];
                        content.SetParent(parent);
                        content.transform.SetSiblingIndex(index - 1);
                        num4++;
                    }
                }
            }
            this.UpdateScroll();
            this.Layout();
            if ((this.contentRowsCount == 0) && !this.noDealsText.activeSelf)
            {
                this.noDealsText.SetActive(true);
            }
            else if ((this.contentRowsCount > 0) && this.noDealsText.activeSelf)
            {
                this.noDealsText.SetActive(false);
            }
        }

        private void UpdateScroll()
        {
            int num = !this.secondBigRowFilled ? 7 : 6;
            if (this.contentRowsCount <= num)
            {
                if (this.scrollMode)
                {
                    this.ScrollOff();
                }
            }
            else
            {
                if (!this.interactWithScrollView)
                {
                    this.pageCount = ((this.contentRowsCount - (num - 1)) / 2) + 1;
                    Vector2 b = new Vector2(-(this.currentPage - 1) * this.pageWidth, 0f);
                    this.scrollContent.anchoredPosition = Vector2.Lerp(this.scrollContent.anchoredPosition, b, this.autoScrollSpeed * Time.deltaTime);
                }
                if (!this.scrollMode)
                {
                    this.ScrollOn();
                }
            }
        }

        [Inject]
        public static Tanks.Lobby.ClientGarage.API.GarageItemsRegistry GarageItemsRegistry { get; set; }

        [CompilerGenerated]
        private sealed class <AddBonus>c__AnonStorey2
        {
            internal Entity entity;
            internal DealsUIComponent $this;

            internal void <>m__0()
            {
                this.$this.OnBonusClick(this.entity);
            }
        }

        [CompilerGenerated]
        private sealed class <AddMarketItem>c__AnonStorey4
        {
            internal Entity entity;
            internal DealsUIComponent $this;

            internal void <>m__0()
            {
                this.$this.OnMarketItemClick(this.entity);
            }
        }

        [CompilerGenerated]
        private sealed class <AddPromo>c__AnonStorey1
        {
            internal string key;

            internal bool <>m__0(DealsUIComponent.GiftPromo x) => 
                x.Key == this.key;
        }

        [CompilerGenerated]
        private sealed class <AddQuantum>c__AnonStorey3
        {
            internal Entity entity;
            internal DealsUIComponent $this;

            internal void <>m__0()
            {
                this.$this.OnQuantumClick(this.entity);
            }
        }

        [CompilerGenerated]
        private sealed class <AddSpecialOffer>c__AnonStorey0
        {
            internal Entity specialOffer;
            internal DealsUIComponent $this;

            internal void <>m__0()
            {
                this.$this.OnPackClick(this.specialOffer, false);
            }
        }

        [CompilerGenerated]
        private sealed class <OnMarketItemClick>c__AnonStorey5
        {
            internal GarageItem item;
            internal DealsUIComponent $this;

            internal void <>m__0()
            {
                this.$this.OnItemBuyCallback(this.item);
            }
        }

        [Serializable]
        public class GiftPromo
        {
            public string Key;
            public GameObject Prefab;
        }
    }
}

