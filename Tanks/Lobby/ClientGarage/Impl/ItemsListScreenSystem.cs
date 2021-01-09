namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientNavigation.API;

    public class ItemsListScreenSystem : ECSSystem
    {
        [CompilerGenerated]
        private static Comparison<UserItem> <>f__am$cache0;
        [CompilerGenerated]
        private static Comparison<MarketItem> <>f__am$cache1;
        [CompilerGenerated]
        private static Func<ContainerContentItemNode, bool> <>f__am$cache2;
        [CompilerGenerated]
        private static Comparison<ContainerContentItemNode> <>f__am$cache3;

        [OnEventFire]
        public void AddItems(NodeAddedEvent e, ItemsListNode itemListNode, [Context, JoinByScreen] SingleNode<SimpleHorizontalListComponent> horizontalListNode)
        {
            base.NewEvent<ShowGarageItemsEvent>().AttachAll(itemListNode.itemsListForView.Items).Schedule();
        }

        [OnEventFire, Mandatory]
        public void AddItems(ShowGarageItemsEvent e, ICollection<MarketItem> marketItems, ICollection<UserItem> userItems, ICollection<SingleNode<MountedItemComponent>> mountedItems, ICollection<ContainerContentItemNode> containerContentItems, ICollection<SingleNode<SlotUserItemInfoComponent>> slotItems, [JoinAll] ScreenNode screen, [JoinByScreen] SingleNode<SimpleHorizontalListComponent> horizontalListNode, [JoinByScreen] Optional<SingleNode<SelectedItemComponent>> selectedItemNode)
        {
            <AddItems>c__AnonStorey0 storey = new <AddItems>c__AnonStorey0 {
                itemsList = horizontalListNode.component
            };
            slotItems.ToList<SingleNode<SlotUserItemInfoComponent>>().ForEach(new Action<SingleNode<SlotUserItemInfoComponent>>(storey.<>m__0));
            List<UserItem> source = userItems.ToList<UserItem>();
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = (a, b) => a.orderItem.Index.CompareTo(b.orderItem.Index);
            }
            source.Sort(<>f__am$cache0);
            source.ForEach(new Action<UserItem>(storey.<>m__1));
            List<MarketItem> buyableMarketItems = GetBuyableMarketItems(marketItems, userItems);
            <>f__am$cache1 ??= (a, b) => a.orderItem.Index.CompareTo(b.orderItem.Index);
            buyableMarketItems.Sort(<>f__am$cache1);
            buyableMarketItems.ForEach(new Action<MarketItem>(storey.<>m__2));
            <>f__am$cache2 ??= item => item.Entity.Alive;
            List<ContainerContentItemNode> list3 = containerContentItems.Where<ContainerContentItemNode>(<>f__am$cache2).ToList<ContainerContentItemNode>();
            <>f__am$cache3 ??= (a, b) => a.orderItem.Index.CompareTo(b.orderItem.Index);
            list3.Sort(<>f__am$cache3);
            list3.ForEach(new Action<ContainerContentItemNode>(storey.<>m__3));
            if (storey.itemsList.Count == 0)
            {
                base.ScheduleEvent<ItemsListEmptyEvent>(screen);
            }
            else if (!selectedItemNode.IsPresent())
            {
                base.ScheduleEvent<SelectGarageItemEvent>(storey.itemsList.GetItems().First<Entity>());
            }
            else
            {
                Entity selectedItem = selectedItemNode.Get().component.SelectedItem;
                if ((selectedItem == null) || !selectedItem.Alive)
                {
                    selectedItem = (mountedItems.Count <= 0) ? ((source.Count <= 0) ? ((list3.Count <= 0) ? buyableMarketItems.First<MarketItem>().Entity : list3.First<ContainerContentItemNode>().Entity) : source.First<UserItem>().Entity) : mountedItems.First<SingleNode<MountedItemComponent>>().Entity;
                }
                if (!storey.itemsList.Contains(selectedItem))
                {
                    selectedItem = ReplaceMarketItemToUserItem(selectedItem, source);
                }
                base.ScheduleEvent<SelectGarageItemEvent>(selectedItem);
            }
        }

        [OnEventFire]
        public void ClearItemsList(NodeRemoveEvent e, ScreenNode screenNode)
        {
            screenNode.screen.GetComponentInChildrenIncludeInactive<SimpleHorizontalListComponent>().ClearItems(true);
        }

        private static List<MarketItem> GetBuyableMarketItems(ICollection<MarketItem> marketItems, ICollection<UserItem> userItems)
        {
            <GetBuyableMarketItems>c__AnonStorey2 storey = new <GetBuyableMarketItems>c__AnonStorey2 {
                buyableMarketItems = new Dictionary<long, MarketItem>()
            };
            marketItems.ForEach<MarketItem>(new Action<MarketItem>(storey.<>m__0));
            userItems.ForEach<UserItem>(new Action<UserItem>(storey.<>m__1));
            return storey.buyableMarketItems.Values.ToList<MarketItem>();
        }

        private static Entity ReplaceMarketItemToUserItem(Entity itemToSelect, List<UserItem> userItems)
        {
            <ReplaceMarketItemToUserItem>c__AnonStorey1 storey = new <ReplaceMarketItemToUserItem>c__AnonStorey1 {
                itemToSelect = itemToSelect
            };
            UserItem item = userItems.FirstOrDefault<UserItem>(new Func<UserItem, bool>(storey.<>m__0));
            return ((item == null) ? storey.itemToSelect : item.Entity);
        }

        [OnEventFire]
        public void SelectItem(ListItemSelectedEvent e, GarageItemNode item, [JoinAll] ScreenNode screenNode, [JoinByScreen] SingleNode<SelectedItemComponent> selectedItemNode)
        {
            selectedItemNode.component.SelectedItem = item.Entity;
        }

        [OnEventFire]
        public void SelectItem(SelectGarageItemEvent e, SingleNode<GarageListItemComponent> itemNode, [JoinAll] ScreenNode screen, [JoinByScreen] SingleNode<SimpleHorizontalListComponent> listNode)
        {
            Entity entity = itemNode.Entity;
            SimpleHorizontalListComponent component = listNode.component;
            component.Select(entity);
            component.MoveToItem(entity);
        }

        [CompilerGenerated]
        private sealed class <AddItems>c__AnonStorey0
        {
            internal SimpleHorizontalListComponent itemsList;

            internal void <>m__0(SingleNode<SlotUserItemInfoComponent> item)
            {
                this.itemsList.AddItem(item.Entity);
            }

            internal void <>m__1(ItemsListScreenSystem.UserItem item)
            {
                this.itemsList.AddItem(item.Entity);
            }

            internal void <>m__2(ItemsListScreenSystem.MarketItem item)
            {
                this.itemsList.AddItem(item.Entity);
            }

            internal void <>m__3(ItemsListScreenSystem.ContainerContentItemNode item)
            {
                this.itemsList.AddItem(item.Entity);
            }
        }

        [CompilerGenerated]
        private sealed class <GetBuyableMarketItems>c__AnonStorey2
        {
            internal Dictionary<long, ItemsListScreenSystem.MarketItem> buyableMarketItems;

            internal void <>m__0(ItemsListScreenSystem.MarketItem marketItem)
            {
                this.buyableMarketItems[marketItem.marketItemGroup.Key] = marketItem;
            }

            internal void <>m__1(ItemsListScreenSystem.UserItem userItem)
            {
                this.buyableMarketItems.Remove(userItem.marketItemGroup.Key);
            }
        }

        [CompilerGenerated]
        private sealed class <ReplaceMarketItemToUserItem>c__AnonStorey1
        {
            internal Entity itemToSelect;

            internal bool <>m__0(ItemsListScreenSystem.UserItem n) => 
                n.marketItemGroup.Key == this.itemToSelect.Id;
        }

        public class ContainerContentItemNode : Node
        {
            public ContainerContentItemComponent containerContentItem;
            public OrderItemComponent orderItem;
        }

        public class GarageItemNode : Node
        {
            public GarageItemComponent garageItem;
            public DescriptionItemComponent descriptionItem;
        }

        [Not(typeof(UpgradeLevelItemComponent))]
        public class GarageListUserItemNotUpgradeNode : Node
        {
            public UserItemComponent userItem;
            public GarageListItemComponent garageListItem;
        }

        public class ItemsListNode : Node
        {
            public ItemsListForViewComponent itemsListForView;
            public ScreenGroupComponent screenGroup;
        }

        public class MarketItem : Node
        {
            public MarketItemComponent marketItem;
            public OrderItemComponent orderItem;
            public MarketItemGroupComponent marketItemGroup;
        }

        [Not(typeof(ShellItemComponent))]
        public class NotShellGarageItemNode : Node
        {
            public GarageItemComponent garageItem;
            public DescriptionItemComponent descriptionItem;
        }

        public class ScreenNode : Node
        {
            public ItemsListScreenComponent itemsListScreen;
            public ScreenComponent screen;
            public ActiveScreenComponent activeScreen;
            public DisplayDescriptionItemComponent displayDescriptionItem;
        }

        public class UserItem : Node
        {
            public UserItemComponent userItem;
            public OrderItemComponent orderItem;
            public MarketItemGroupComponent marketItemGroup;
        }
    }
}

