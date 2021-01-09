namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientResources.API;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.API;

    public class GarageItem : IComparable<GarageItem>, IComparable
    {
        private Entity userItem;
        private Action onBought;
        private Action onMount;

        public void Buy(Action onBought)
        {
            this.onBought = onBought;
            BuyMarketItemEvent eventInstance = new BuyMarketItemEvent {
                Price = this.Price,
                Amount = 1
            };
            EngineService.Engine.NewEvent(eventInstance).Attach(SelfUserComponent.SelfUser).Attach(this.MarketItem).Schedule();
        }

        private int CalcPrice(int price)
        {
            price += this.AdditionalPrice;
            return ((this.PersonalSalePercent <= 0) ? price : ((int) ((((float) price) / 100f) * (100 - this.PersonalSalePercent))));
        }

        public int CompareByType(GarageItem other) => 
            !ReferenceEquals(this, other) ? (!this.MarketItem.HasComponent<AvatarItemComponent>() ? (!other.MarketItem.HasComponent<AvatarItemComponent>() ? (!this.MarketItem.HasComponent<GraffitiItemComponent>() ? (!other.MarketItem.HasComponent<GraffitiItemComponent>() ? (!this.MarketItem.HasComponent<ShellItemComponent>() ? (!other.MarketItem.HasComponent<ShellItemComponent>() ? (!this.MarketItem.HasComponent<PaintItemComponent>() ? (!other.MarketItem.HasComponent<PaintItemComponent>() ? this.CompareTo(other) : 1) : -1) : 1) : -1) : 1) : -1) : 1) : -1) : 0;

        public int CompareTo(object obj) => 
            (obj is GarageItem) ? this.CompareTo((GarageItem) obj) : -1;

        public int CompareTo(GarageItem other) => 
            !ReferenceEquals(this, other) ? ((this.MarketItem.HasComponent<GameplayChestItemComponent>() || (!this.UserHasItem || other.UserHasItem)) ? ((this.MarketItem.HasComponent<GameplayChestItemComponent>() || (this.UserHasItem || !other.UserHasItem)) ? ((!this.UserHasItem || (!other.UserHasItem || (!this.IsSortItem || !other.IsSortItem))) ? ((!this.IsSortItem || !other.IsSortItem) ? (this.MarketItem.HasComponent<OrderItemComponent>() ? (other.MarketItem.HasComponent<OrderItemComponent>() ? this.MarketItem.GetComponent<OrderItemComponent>().Index.CompareTo(other.MarketItem.GetComponent<OrderItemComponent>().Index) : 1) : -1) : string.Compare(this.Name, other.Name, StringComparison.Ordinal)) : (!this.UserItem.GetComponent<DefaultItemComponent>().Default ? (!other.UserItem.GetComponent<DefaultItemComponent>().Default ? ((this.IsRestricted || !other.IsRestricted) ? ((!this.IsRestricted || other.IsRestricted) ? string.Compare(this.Name, other.Name, StringComparison.Ordinal) : 1) : -1) : 1) : -1)) : 1) : -1) : 0;

        public void Mount(Action onMount = null)
        {
            if (!this.IsMounted)
            {
                EngineService.Engine.ScheduleEvent<MountItemEvent>(this.UserItem);
            }
        }

        public void Mounted()
        {
            if (this.onMount != null)
            {
                this.onMount();
                this.onMount = null;
            }
        }

        public void Select()
        {
            Entity userItem = this.UserItem;
            Entity entity = userItem;
            if (userItem == null)
            {
                Entity local1 = userItem;
                entity = this.MarketItem;
            }
            EngineService.Engine.ScheduleEvent<ListItemSelectedEvent>(entity);
        }

        public void XBuy(Action onBought, int price, int amount)
        {
            this.onBought = onBought;
            XBuyMarketItemEvent eventInstance = new XBuyMarketItemEvent {
                Price = price,
                Amount = amount
            };
            EngineService.Engine.NewEvent(eventInstance).Attach(SelfUserComponent.SelfUser).Attach(this.MarketItem).Schedule();
        }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.EngineService EngineService { get; set; }

        public virtual Entity MarketItem { get; set; }

        public string ConfigPath { get; set; }

        public Entity UserItem
        {
            get => 
                this.userItem;
            set
            {
                this.userItem = value;
                if (this.onBought != null)
                {
                    this.onBought();
                    this.onBought = null;
                }
            }
        }

        public string Preview { get; set; }

        public bool WaitForBuy { get; set; }

        public int PersonalSalePercent { get; set; }

        public int AdditionalPrice { get; set; }

        public string Name =>
            (this.MarketItem != null) ? this.MarketItem.GetComponent<DescriptionItemComponent>().Name : string.Empty;

        public string Description =>
            (this.MarketItem != null) ? this.MarketItem.GetComponent<DescriptionItemComponent>().Description : string.Empty;

        public ItemRarityType Rarity =>
            (this.MarketItem != null) ? (!this.MarketItem.HasComponent<ItemRarityComponent>() ? ItemRarityType.COMMON : this.MarketItem.GetComponent<ItemRarityComponent>().RarityType) : ItemRarityType.COMMON;

        public bool IsRestricted =>
            ((this.UserItem == null) || !this.UserItem.HasComponent<RestrictedByUpgradeLevelComponent>()) ? ((this.UserItem == null) && (this.RestrictionLevel > 0)) : true;

        public int RestrictionLevel =>
            !this.MarketItem.HasComponent<MountUpgradeLevelRestrictionComponent>() ? 0 : this.MarketItem.GetComponent<MountUpgradeLevelRestrictionComponent>().RestrictionValue;

        public bool IsSelected =>
            this.MarketItem.HasComponent<HangarItemPreviewComponent>() || ((this.UserItem != null) && this.UserItem.HasComponent<HangarItemPreviewComponent>());

        public bool IsBuyable =>
            (this.Price > 0) || (this.XPrice > 0);

        public int Price =>
            this.CalcPrice(this.MarketItem.GetComponent<PriceItemComponent>().Price);

        public int XPrice =>
            this.CalcPrice(this.MarketItem.GetComponent<XPriceItemComponent>().Price);

        public int OldPrice
        {
            get
            {
                PriceItemComponent component = this.MarketItem.GetComponent<PriceItemComponent>();
                return ((component.OldPrice == 0) ? component.Price : component.OldPrice);
            }
        }

        public int OldXPrice
        {
            get
            {
                XPriceItemComponent component = this.MarketItem.GetComponent<XPriceItemComponent>();
                return ((component.OldPrice == 0) ? component.Price : component.OldPrice);
            }
        }

        public bool IsContainerItem =>
            this.MarketItem.HasComponent<ContainerContentItemGroupComponent>();

        public bool IsMounted =>
            (this.UserItem != null) && this.UserItem.HasComponent<MountedItemComponent>();

        public bool IsVisualItem =>
            (this.MarketItem != null) && (this.MarketItem.HasComponent<ShellItemComponent>() || (this.MarketItem.HasComponent<SkinItemComponent>() || (this.MarketItem.HasComponent<GraffitiItemComponent>() || (this.MarketItem.HasComponent<WeaponPaintItemComponent>() || (this.MarketItem.HasComponent<AvatarItemComponent>() || this.MarketItem.HasComponent<TankPaintItemComponent>())))));

        public string AssertGuid =>
            (this.MarketItem == null) ? AssetReferenceComponent.createFromConfig(this.ConfigPath).Reference.AssetGuid : this.MarketItem.GetComponent<AssetReferenceComponent>().Reference.AssetGuid;

        private bool UserHasItem =>
            (this.UserItem != null) && (!this.UserItem.HasComponent<UserItemCounterComponent>() || (this.UserItem.GetComponent<UserItemCounterComponent>().Count > 0L));

        private bool IsSortItem =>
            (this.MarketItem.HasComponent<GraffitiItemComponent>() || (this.MarketItem.HasComponent<PaintItemComponent>() || (this.MarketItem.HasComponent<SkinItemComponent>() || (this.MarketItem.HasComponent<ShellItemComponent>() || this.MarketItem.HasComponent<AvatarItemComponent>())))) || (this.MarketItem.HasComponent<ContainerMarkerComponent>() && !this.MarketItem.HasComponent<GameplayChestItemComponent>());
    }
}

