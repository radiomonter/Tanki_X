namespace Tanks.Lobby.ClientGarage.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.API;

    public class MarketInfoSystem : ECSSystem
    {
        [OnEventFire]
        public void AddItem(NodeAddedEvent e, UserItemNode userItem, [JoinByUser] SelfUserNode selfUserNode)
        {
            selfUserNode.purchasedItemList.AddPurchasedItem(userItem.marketItemGroup.Key);
        }

        [OnEventFire]
        public void CheckItem(ItemInMarketRequestEvent e, Node any, [JoinAll] SelfUserNode selfUserNode, [JoinAll, Combine] MarketItemNode item)
        {
            if (!selfUserNode.purchasedItemList.Contains(item.Entity.Id))
            {
                e.marketItems.Add(item.Entity.Id, item.descriptionItem.Name);
            }
        }

        [OnEventFire]
        public void CreateSelf(NodeAddedEvent e, SingleNode<SelfUserComponent> selfUser)
        {
            selfUser.Entity.AddComponent(new PurchasedItemListComponent());
        }

        public class MarketItemNode : Node
        {
            public DescriptionItemComponent descriptionItem;
            public MarketItemComponent marketItem;
        }

        public class SelfUserNode : Node
        {
            public SelfUserComponent selfUser;
            public UserGroupComponent userGroup;
            public PurchasedItemListComponent purchasedItemList;
        }

        public class UserItemNode : Node
        {
            public UserGroupComponent userGroup;
            public UserItemComponent userItem;
            public MarketItemGroupComponent marketItemGroup;
        }
    }
}

