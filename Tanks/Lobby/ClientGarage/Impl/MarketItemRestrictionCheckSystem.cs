namespace Tanks.Lobby.ClientGarage.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.API;

    public class MarketItemRestrictionCheckSystem : ECSSystem
    {
        [OnEventFire]
        public void RestrictMarketItemByUpgradeLevel(CheckMarketItemRestrictionsEvent e, MarketItemWithUpgradeLevelRestrictionNode marketItem, [JoinByParentGroup] ICollection<UpgradableItemNode> upgradableItems)
        {
            <RestrictMarketItemByUpgradeLevel>c__AnonStorey0 storey = new <RestrictMarketItemByUpgradeLevel>c__AnonStorey0 {
                marketParentItem = base.GetEntityById(marketItem.parentGroup.Key)
            };
            if (upgradableItems.Count<UpgradableItemNode>(new Func<UpgradableItemNode, bool>(storey.<>m__0)) == 0)
            {
                bool flag = marketItem.mountUpgradeLevelRestriction.RestrictionValue > 0;
                e.RestrictByUpgradeLevel(flag);
                e.MountRestrictByUpgradeLevel(flag);
            }
            else
            {
                foreach (UpgradableItemNode node in upgradableItems)
                {
                    if (node.marketItemGroup.Key == storey.marketParentItem.Id)
                    {
                        int level = node.upgradeLevelItem.Level;
                        e.RestrictByUpgradeLevel(level < marketItem.purchaseUpgradeLevelRestriction.RestrictionValue);
                    }
                }
            }
        }

        [OnEventFire]
        public void RestrictMarketItemByUserRank(CheckMarketItemRestrictionsEvent e, [Combine] MarketItemWithUserRankRestrictionNode item, [JoinAll] SelfUserNode selfUser)
        {
            e.RestrictByRank(item.purchaseUserRankRestriction.RestrictionValue > selfUser.userRank.Rank);
        }

        [CompilerGenerated]
        private sealed class <RestrictMarketItemByUpgradeLevel>c__AnonStorey0
        {
            internal Entity marketParentItem;

            internal bool <>m__0(MarketItemRestrictionCheckSystem.UpgradableItemNode upgradableItem) => 
                upgradableItem.marketItemGroup.Key == this.marketParentItem.Id;
        }

        public class MarketItemNode : Node
        {
            public MarketItemComponent marketItem;
        }

        public class MarketItemWithUpgradeLevelRestrictionNode : MarketItemRestrictionCheckSystem.MarketItemNode
        {
            public PurchaseUpgradeLevelRestrictionComponent purchaseUpgradeLevelRestriction;
            public MountUpgradeLevelRestrictionComponent mountUpgradeLevelRestriction;
            public ParentGroupComponent parentGroup;
        }

        public class MarketItemWithUserRankRestrictionNode : MarketItemRestrictionCheckSystem.MarketItemNode
        {
            public PurchaseUserRankRestrictionComponent purchaseUserRankRestriction;
        }

        public class SelfUserNode : Node
        {
            public SelfUserComponent selfUser;
            public UserRankComponent userRank;
        }

        public class UpgradableItemNode : Node
        {
            public UpgradeLevelItemComponent upgradeLevelItem;
            public UserItemComponent userItem;
            public ParentGroupComponent parentGroup;
            public MarketItemGroupComponent marketItemGroup;
        }
    }
}

