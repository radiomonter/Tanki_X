namespace Tanks.Lobby.ClientGarage.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.API;
    using UnityEngine;

    public class HiddenInGarageItemSystem : ECSSystem
    {
        [OnEventFire]
        public void HideExclusiveItem(NodeAddedEvent e, [Combine] NotHiddenExclusiveMarketItemNode marketItem, SelfUserNode selfUser)
        {
            if (marketItem.exclusiveItem.ForbiddenForPublishers.Contains(selfUser.userPublisher.Publisher))
            {
                marketItem.Entity.AddComponent<HiddenInGarageItemComponent>();
            }
        }

        [OnEventFire]
        public void ShowNotExclusiveItem(MarketItemAvailabilityUpdatedEvent e, HiddenExclusiveMarketItemNode marketItem, [JoinAll] SelfUserNode selfUser)
        {
            object[] objArray1 = new object[] { "HiddenInGarageItemSystem.ShowNotExclusiveItem ", marketItem.Entity, " ", !marketItem.exclusiveItem.ForbiddenForPublishers.Contains(selfUser.userPublisher.Publisher) };
            Debug.Log(string.Concat(objArray1));
            if (!marketItem.exclusiveItem.ForbiddenForPublishers.Contains(selfUser.userPublisher.Publisher))
            {
                marketItem.Entity.RemoveComponent<HiddenInGarageItemComponent>();
            }
        }

        public class ExclusiveMarketItemNode : HiddenInGarageItemSystem.MarketItemNode
        {
            public ExclusiveItemComponent exclusiveItem;
        }

        public class HiddenExclusiveMarketItemNode : HiddenInGarageItemSystem.ExclusiveMarketItemNode
        {
            public HiddenInGarageItemComponent hiddenInGarageItem;
        }

        public class MarketItemNode : Node
        {
            public MarketItemComponent marketItem;
            public MarketItemGroupComponent marketItemGroup;
        }

        [Not(typeof(HiddenInGarageItemComponent))]
        public class NotHiddenExclusiveMarketItemNode : HiddenInGarageItemSystem.ExclusiveMarketItemNode
        {
        }

        public class SelfUserNode : Node
        {
            public UserComponent user;
            public SelfUserComponent selfUser;
            public UserPublisherComponent userPublisher;
        }
    }
}

