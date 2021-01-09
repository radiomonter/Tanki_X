namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientHangar.Impl;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    public class AvatarUIComponent : BehaviourComponent
    {
        [SerializeField]
        private AvatarButton avatarButtonPrefab;
        [SerializeField]
        private Transform grid;
        [SerializeField]
        private GaragePrice xPrice;
        [SerializeField]
        private GaragePrice price;
        [SerializeField]
        private Button xBuyButton;
        [SerializeField]
        private Button buyButton;
        [SerializeField]
        private Button equipButton;
        [SerializeField]
        private Button cancelButton;
        [SerializeField]
        private Button closeButton;
        [SerializeField]
        private Button toContainerButton;
        [SerializeField]
        private TMP_Text restriction;
        [SerializeField]
        private LocalizedField restrictionLocalization;
        [SerializeField]
        private LocalizedField avatarTypeLocalization;
        [SerializeField]
        private LocalizedField _commonString;
        [SerializeField]
        private LocalizedField _rareString;
        [SerializeField]
        private LocalizedField _epicString;
        [SerializeField]
        private LocalizedField _legendaryString;
        private readonly Dictionary<long, Avatar> _avatars = new Dictionary<long, Avatar>();
        private readonly Dictionary<long, AvatarButton> _avatarButtons = new Dictionary<long, AvatarButton>();
        [CompilerGenerated]
        private static Comparison<Avatar> <>f__am$cache0;
        [CompilerGenerated]
        private static Comparison<AvatarButton> <>f__am$cache1;
        [CompilerGenerated]
        private static Func<KeyValuePair<long, Avatar>, bool> <>f__am$cache2;
        [CompilerGenerated]
        private static Func<KeyValuePair<long, Avatar>, long> <>f__am$cache3;

        private AvatarButton AddAvatar(string iconUid, string rarity, IAvatarStateChanger changer)
        {
            AvatarButton button = Instantiate<AvatarButton>(this.avatarButtonPrefab);
            button.Init(iconUid, rarity, changer);
            button.transform.SetParent(this.grid, false);
            return button;
        }

        public void AddMarketItem(IEnumerable<long> avatarIds, int currentUserRank)
        {
            foreach (long num in avatarIds)
            {
                if (!this._avatars.ContainsKey(num))
                {
                    Avatar item = GarageItemsRegistry.GetItem<Avatar>(num);
                    if ((item.UserItem != null) || (item.IsBuyable || item.IsContainerItem))
                    {
                        this.CreateAvatarButton(item, currentUserRank);
                    }
                    this._avatars.Add(item.MarketItem.Id, item);
                }
            }
            this.UpdateButtons();
        }

        public void AddUserItem(long key, int currentUserRank)
        {
            if (this._avatarButtons.ContainsKey(key))
            {
                this.UpdateButtons();
            }
            else
            {
                this.CreateAvatarButton(this._avatars[key], currentUserRank);
            }
            this._avatars[key].OnBought();
        }

        private void Awake()
        {
            this.toContainerButton.onClick.AddListener(() => this.GoToContainer(this.Selected));
            this.equipButton.onClick.AddListener(() => ECSBehaviour.EngineService.Engine.ScheduleEvent<MountItemEvent>(this._avatars[this.Selected].UserItem));
            this.cancelButton.onClick.AddListener(new UnityAction(this.Cancel));
            this.closeButton.onClick.AddListener(new UnityAction(this.Close));
            this.xBuyButton.onClick.AddListener(() => this.XBuy(this.Selected));
            this.buyButton.onClick.AddListener(() => this.Buy(this.Selected));
        }

        private void Buy(long key)
        {
            GarageItem item = GarageItemsRegistry.GetItem<GarageItem>(key);
            FindObjectOfType<Dialogs60Component>().Get<BuyConfirmationDialog>().Show(item, new Action(this.SendOpenAvatarsEvent), this.GetName(item.Name), new Action(this.SendOpenAvatarsEvent));
            base.GetComponent<AvatarDialogComponent>().Hide();
        }

        private void Cancel()
        {
        }

        private void Close()
        {
        }

        private void CreateAvatarButton(Avatar avatar, int currentUserRank)
        {
            <CreateAvatarButton>c__AnonStorey0 storey = new <CreateAvatarButton>c__AnonStorey0 {
                avatar = avatar,
                $this = this
            };
            storey.id = storey.avatar.MarketItem.Id;
            storey.button = this.AddAvatar(storey.avatar.IconUid, storey.avatar.RarityName, storey.avatar);
            storey.button.GetIndex = new Func<int>(storey.<>m__0);
            storey.button.OnPress = new Action(storey.<>m__1);
            storey.button.OnDoubleClick = new Action(storey.<>m__2);
            storey.avatar.Unlocked = (storey.avatar.UserItem != null) || (storey.avatar.MinRank <= currentUserRank);
            storey.avatar.Remove = new Action(storey.<>m__3);
            storey.button.GetComponent<TooltipShowBehaviour>().TipText = MarketItemNameLocalization.GetFullItemDescription(storey.avatar, false, (string) this._commonString, (string) this._rareString, (string) this._epicString, (string) this._legendaryString);
            this._avatarButtons.Add(storey.id, storey.button);
        }

        private string GetName(string itemName) => 
            string.Format((string) this.avatarTypeLocalization, itemName);

        private void GoToContainer(long key)
        {
            Entity entity = null;
            try
            {
                entity = base.Select<SingleNode<ContainerMarkerComponent>>(base.Select<SingleNode<ContainerContentItemComponent>>(this._avatars[key].MarketItem, typeof(ContainerContentItemGroupComponent)).First<SingleNode<ContainerContentItemComponent>>().Entity, typeof(ContainerGroupComponent)).First<SingleNode<ContainerMarkerComponent>>().Entity;
            }
            catch (Exception)
            {
                Debug.LogError("No such container");
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
            this.OnSelect(this.Equipped);
            this.grid.DestroyChildren();
            this._avatars.Clear();
            this._avatarButtons.Clear();
        }

        public void OnDoubleClick(long key)
        {
            if (this._avatars[key].Unlocked)
            {
                if (this._avatars[key].UserItem != null)
                {
                    ECSBehaviour.EngineService.Engine.ScheduleEvent<MountItemEvent>(this._avatars[key].UserItem);
                }
                else if (this._avatars[key].XPrice > 0)
                {
                    this.XBuy(key);
                }
                else if (this._avatars[key].Price > 0)
                {
                    this.Buy(key);
                }
                else if (this._avatars[key].IsContainerItem)
                {
                    this.GoToContainer(key);
                }
            }
        }

        private void OnEnable()
        {
            this.SortAvatars();
            if (this._avatars.ContainsKey(this.Equipped))
            {
                this._avatars[this.Equipped].SetEquipped(true);
            }
        }

        public void OnEquip(long avatarKey)
        {
            if (this.Equipped != avatarKey)
            {
                if (this.Equipped != 0L)
                {
                    this._avatars[this.Equipped].SetEquipped(false);
                }
                this.Equipped = avatarKey;
                this._avatars[this.Equipped].SetEquipped(true);
                if (this.Selected == 0L)
                {
                    this.Selected = this.Equipped;
                    this._avatars[this.Equipped].SetSelected(true);
                }
                this.UpdateButtons();
            }
        }

        public void OnSelect(long key)
        {
            if (this.Selected != key)
            {
                if (this.Selected != 0L)
                {
                    this._avatars[this.Selected].SetSelected(false);
                }
                this.Selected = key;
                this._avatars[this.Selected].SetSelected(true);
                Entity entity = this._avatars[this.Selected].UserItem ?? this._avatars[this.Selected].MarketItem;
                ECSBehaviour.EngineService.Engine.NewEvent(new ItemPreviewBaseSystem.PrewievEvent()).Attach(entity).Schedule();
                this.UpdatePrice(key);
                this.UpdateButtons();
            }
        }

        public void Remove(long key)
        {
            if (this._avatars.ContainsKey(key))
            {
                Avatar avatar = this._avatars[key];
                if (avatar.UserItem == null)
                {
                    if ((this.Selected == key) && this._avatars.ContainsKey(this.Equipped))
                    {
                        this.Selected = this.Equipped;
                        this._avatars[this.Equipped].SetSelected(true);
                        this.UpdatePrice(key);
                        this.UpdateButtons();
                    }
                    if (avatar.Remove != null)
                    {
                        avatar.Remove();
                    }
                    this._avatars.Remove(key);
                }
            }
        }

        private void SendOpenAvatarsEvent()
        {
            ECSBehaviour.EngineService.Engine.NewEvent(new AvatarMenuSystem.ShowMenuEvent()).Attach(new EntityStub()).Schedule();
        }

        public void SortAvatars()
        {
            List<Avatar> list = this._avatars.Values.ToList<Avatar>();
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = (a1, a2) => a1.CompareTo(a2);
            }
            list.Sort(<>f__am$cache0);
            for (int i = 0; i < list.Count; i++)
            {
                list[i].Index = i;
            }
            List<AvatarButton> list2 = this.grid.GetComponentsInChildren<AvatarButton>().ToList<AvatarButton>();
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = (x, y) => x.GetIndex() - y.GetIndex();
            }
            list2.Sort(<>f__am$cache1);
            foreach (AvatarButton button in list2)
            {
                button.transform.SetAsLastSibling();
            }
        }

        public void UpdateAvatars()
        {
            if (<>f__am$cache2 == null)
            {
                <>f__am$cache2 = x => ReferenceEquals(x.Value.MarketItem, null);
            }
            <>f__am$cache3 ??= x => x.Key;
            this._avatars.Where<KeyValuePair<long, Avatar>>(<>f__am$cache2).Select<KeyValuePair<long, Avatar>, long>(<>f__am$cache3).ToList<long>().ForEach(new Action<long>(this.Remove));
        }

        private void UpdateButtons()
        {
            this.restriction.gameObject.SetActive(false);
            this.xBuyButton.gameObject.SetActive(false);
            this.buyButton.gameObject.SetActive(false);
            this.equipButton.gameObject.SetActive(false);
            this.toContainerButton.gameObject.SetActive(false);
            if (this.Selected == this.Equipped)
            {
                this.cancelButton.gameObject.SetActive(false);
                this.closeButton.gameObject.SetActive(true);
            }
            else
            {
                this.cancelButton.gameObject.SetActive(true);
                this.closeButton.gameObject.SetActive(false);
                Avatar avatar = this._avatars[this.Selected];
                if (avatar.UserItem != null)
                {
                    this.equipButton.gameObject.SetActive(true);
                    avatar.Unlocked = true;
                }
                else if (!avatar.Unlocked)
                {
                    this.restriction.text = string.Format(this.restrictionLocalization.Value, this._avatars[this.Selected].MinRank);
                    this.restriction.gameObject.SetActive(true);
                }
                else if (avatar.XPrice > 0)
                {
                    this.xBuyButton.gameObject.SetActive(true);
                }
                else if (avatar.Price > 0)
                {
                    this.buyButton.gameObject.SetActive(true);
                }
                else
                {
                    this.toContainerButton.gameObject.SetActive(true);
                }
            }
        }

        public void UpdatePrice(long key)
        {
            if ((key == this.Selected) && (this._avatars[this.Selected].UserItem == null))
            {
                this.xPrice.SetPrice(this._avatars[this.Selected].OldXPrice, this._avatars[this.Selected].XPrice);
                this.price.SetPrice(this._avatars[this.Selected].OldPrice, this._avatars[this.Selected].Price);
            }
        }

        public void UpdateRank(int rank)
        {
            foreach (Avatar avatar in this._avatars.Values)
            {
                avatar.Unlocked = (avatar.UserItem != null) || (avatar.MinRank <= rank);
            }
            this.UpdateButtons();
        }

        private void XBuy(long key)
        {
            GarageItem item = GarageItemsRegistry.GetItem<GarageItem>(key);
            int xPrice = this._avatars[key].XPrice;
            FindObjectOfType<Dialogs60Component>().Get<BuyConfirmationDialog>().XShow(item, new Action(this.SendOpenAvatarsEvent), xPrice, 1, this.GetName(item.Name), true, new Action(this.SendOpenAvatarsEvent));
            base.GetComponent<AvatarDialogComponent>().Hide();
        }

        [Inject]
        public static Tanks.Lobby.ClientGarage.API.GarageItemsRegistry GarageItemsRegistry { get; set; }

        private long Equipped { get; set; }

        private long Selected { get; set; }

        [CompilerGenerated]
        private sealed class <CreateAvatarButton>c__AnonStorey0
        {
            internal Avatar avatar;
            internal long id;
            internal AvatarButton button;
            internal AvatarUIComponent $this;

            internal int <>m__0() => 
                this.avatar.Index;

            internal void <>m__1()
            {
                this.$this.OnSelect(this.id);
            }

            internal void <>m__2()
            {
                this.$this.OnDoubleClick(this.id);
            }

            internal void <>m__3()
            {
                Object.Destroy(this.button);
            }
        }
    }
}

