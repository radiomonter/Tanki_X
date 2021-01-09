namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientNavigation.API;

    public class GarageItemsScreenSystem : ECSSystem
    {
        private void MarkItem(Entity itemEntity, List<Entity> itemsForView, ScreenNode screenNode, bool mark)
        {
            if (itemsForView.Contains(itemEntity))
            {
                screenNode.simpleHorizontalList.GetItem(itemEntity).GetComponentInChildrenIncludeInactive<TickMarkerComponent>().gameObject.SetActive(mark);
            }
        }

        [OnEventFire]
        public void MarkMountedItem(NodeAddedEvent e, MountedUserItemNode item, [JoinAll] ScreenNode screenNode, [JoinByScreen] ItemsListNode itemsList)
        {
            this.MarkItem(item.Entity, itemsList.itemsListForView.Items, screenNode, true);
        }

        [OnEventComplete]
        public void MarkMountedItem(ShowGarageItemsEvent e, [Combine] MountedUserItemNode item, [JoinAll] ScreenNode screenNode, [JoinByScreen] ItemsListNode itemsList)
        {
            this.MarkItem(item.Entity, itemsList.itemsListForView.Items, screenNode, true);
        }

        [OnEventComplete]
        public void MoveToMountedItem(ShowGarageItemsEvent e, [Combine] SelectedMountedUserItemNode item, [JoinAll] ScreenNode screenNode)
        {
            screenNode.simpleHorizontalList.MoveToItem(item.Entity);
        }

        [OnEventFire]
        public void ReplaceBoughtItem(NodeAddedEvent e, UserItemNode userItemNode, [JoinByMarketItem] MarketItemNode boughtItem, [JoinAll] ScreenNode screenNode, [JoinByScreen] SingleNode<SelectedItemComponent> selectedItemNode, [JoinByScreen] ItemsListNode itemsList)
        {
            Entity item = boughtItem.Entity;
            if (itemsList.itemsListForView.Items.Contains(item))
            {
                bool select = ReferenceEquals(selectedItemNode.component.SelectedItem, item);
                this.ReplaceItem(screenNode.simpleHorizontalList, itemsList.itemsListForView, item, userItemNode.Entity, select);
            }
        }

        private void ReplaceItem(SimpleHorizontalListComponent horizontalList, ItemsListForViewComponent itemsListForView, Entity origEntity, Entity newEntity, bool select)
        {
            int index = horizontalList.GetIndex(origEntity);
            horizontalList.RemoveItem(origEntity);
            itemsListForView.Items.Remove(origEntity);
            horizontalList.AddItem(newEntity);
            itemsListForView.Items.Add(newEntity);
            horizontalList.SetIndex(newEntity, index);
            if (select)
            {
                base.ScheduleEvent<SelectGarageItemEvent>(newEntity);
            }
        }

        [OnEventFire]
        public void ReplaceUserItemToMarketItem(NodeRemoveEvent e, UserItemNode userItem, [JoinByMarketItem] Optional<MarketItemNode> marketItem, [JoinAll] ScreenNode screenNode, [JoinByScreen] SingleNode<SelectedItemComponent> selectedItemNode, [JoinByScreen] ItemsListNode itemsList)
        {
            bool select = ReferenceEquals(selectedItemNode.component.SelectedItem, userItem.Entity);
            if (marketItem.IsPresent())
            {
                this.ReplaceItem(screenNode.simpleHorizontalList, itemsList.itemsListForView, userItem.Entity, marketItem.Get().Entity, select);
            }
            else
            {
                screenNode.simpleHorizontalList.RemoveItem(userItem.Entity);
                itemsList.itemsListForView.Items.Remove(userItem.Entity);
                if (select && (screenNode.simpleHorizontalList.Count != 0))
                {
                    base.ScheduleEvent<SelectGarageItemEvent>(screenNode.simpleHorizontalList.GetItems().First<Entity>());
                }
                else if (screenNode.simpleHorizontalList.Count == 0)
                {
                    base.ScheduleEvent<ItemsListEmptyEvent>(screenNode);
                }
            }
        }

        [OnEventFire]
        public void UnMarkMountedItem(NodeRemoveEvent e, MountedUserItemNode item, [JoinAll] ScreenNode screenNode, [JoinByScreen] ItemsListNode itemsList)
        {
            this.MarkItem(item.Entity, itemsList.itemsListForView.Items, screenNode, false);
        }

        public class ItemsListNode : Node
        {
            public ItemsListForViewComponent itemsListForView;
            public ScreenGroupComponent screenGroup;
        }

        public class MarketItemNode : Node
        {
            public GarageItemComponent garageItem;
            public DescriptionItemComponent descriptionItem;
            public MarketItemComponent marketItem;
            public MarketItemGroupComponent marketItemGroup;
        }

        public class MountedUserItemNode : Node
        {
            public MountedItemComponent mountedItem;
            public UserItemComponent userItem;
        }

        public class ScreenNode : Node
        {
            public ActiveScreenComponent activeScreen;
            public ScreenComponent screen;
            public SimpleHorizontalListComponent simpleHorizontalList;
        }

        public class SelectedMountedUserItemNode : Node
        {
            public MountedItemComponent mountedItem;
            public UserItemComponent userItem;
            public SelectedItemComponent selectedItem;
        }

        public class UserItemNode : Node
        {
            public GarageItemComponent garageItem;
            public DescriptionItemComponent descriptionItem;
            public UserItemComponent userItem;
            public MarketItemGroupComponent marketItemGroup;
        }
    }
}

