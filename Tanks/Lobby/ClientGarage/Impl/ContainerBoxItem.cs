namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientGarage.API;

    public class ContainerBoxItem : GarageItem
    {
        private readonly List<GarageItem> content = new List<GarageItem>();
        private readonly Dictionary<long, string> marketItemToName = new Dictionary<long, string>();
        private readonly Dictionary<long, string> entityToDescription = new Dictionary<long, string>();
        private Action onOpen;

        public string GetLocalizedContentItemName(long marketItemId) => 
            this.marketItemToName.ContainsKey(marketItemId) ? this.marketItemToName[marketItemId] : string.Empty;

        public string GetLocalizedDescription(long entityId) => 
            !this.entityToDescription.ContainsKey(entityId) ? string.Empty : this.entityToDescription[entityId];

        public void Open(Action onOpen)
        {
            this.onOpen = onOpen;
            GarageItem.EngineService.Engine.ScheduleEvent<OpenVisualContainerEvent>(base.UserItem);
        }

        public void Opend()
        {
            if (this.onOpen != null)
            {
                this.onOpen();
                this.onOpen = null;
            }
        }

        [Inject]
        public static Tanks.Lobby.ClientGarage.API.GarageItemsRegistry GarageItemsRegistry { get; set; }

        public ICollection<GarageItem> Content =>
            this.content;

        public int Count =>
            (base.UserItem != null) ? (base.UserItem.HasComponent<UserItemCounterComponent>() ? ((int) base.UserItem.GetComponent<UserItemCounterComponent>().Count) : 0) : 0;

        public Dictionary<int, int> PackXPrices =>
            !this.MarketItem.HasComponent<PackPriceComponent>() ? null : this.MarketItem.GetComponent<PackPriceComponent>().PackXPrice;

        public override Entity MarketItem
        {
            get => 
                base.MarketItem;
            set
            {
                base.MarketItem = value;
                this.IsBlueprint = value.HasComponent<GameplayChestItemComponent>();
                base.Preview = value.GetComponent<ImageItemComponent>().SpriteUid;
                if (value.HasComponent<DescriptionItemComponent>())
                {
                    DescriptionItemComponent component = value.GetComponent<DescriptionItemComponent>();
                    this.entityToDescription.Add(value.Id, component.Description);
                }
                if (!this.IsBlueprint)
                {
                    ItemsContainerItemComponent component = value.GetComponent<ItemsContainerItemComponent>();
                    DescriptionBundleItemComponent component3 = value.GetComponent<DescriptionBundleItemComponent>();
                    foreach (ContainerItem item in component.Items)
                    {
                        foreach (MarketItemBundle bundle in item.ItemBundles)
                        {
                            if ((item.NameLocalizationKey != null) && ((component3.Names != null) && component3.Names.ContainsKey(item.NameLocalizationKey)))
                            {
                                this.marketItemToName.Add(bundle.MarketItem, component3.Names[item.NameLocalizationKey]);
                            }
                            this.content.Add(GarageItemsRegistry.GetItem<VisualItem>(bundle.MarketItem));
                        }
                    }
                    if (component.RareItems != null)
                    {
                        foreach (ContainerItem item2 in component.RareItems)
                        {
                            foreach (MarketItemBundle bundle2 in item2.ItemBundles)
                            {
                                if ((item2.NameLocalizationKey != null) && ((component3.Names != null) && component3.Names.ContainsKey(item2.NameLocalizationKey)))
                                {
                                    this.marketItemToName.Add(bundle2.MarketItem, component3.Names[item2.NameLocalizationKey]);
                                }
                                this.content.Add(GarageItemsRegistry.GetItem<VisualItem>(bundle2.MarketItem));
                            }
                        }
                    }
                }
            }
        }

        public bool IsBlueprint { get; set; }
    }
}

