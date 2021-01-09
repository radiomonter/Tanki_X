namespace Tanks.Lobby.ClientGarage.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.API;
    using UnityEngine;

    public class MarketItemRestrictionBadgeSystem : ECSSystem
    {
        [OnEventFire]
        public void ShowUpgradeLevelRestrictionIndicator(NodeAddedEvent e, MarketItemWithUpgradeLevelRestrictionNode marketItem)
        {
            CheckMarketItemRestrictionsEvent eventInstance = new CheckMarketItemRestrictionsEvent();
            base.ScheduleEvent(eventInstance, marketItem);
            if (eventInstance.RestrictedByUpgradeLevel)
            {
                marketItem.upgradeLevelRestrictionBadgeGUI.RestrictionValue = (((marketItem.purchaseUpgradeLevelRestriction.RestrictionValue != 0) || !eventInstance.MountWillBeRestrictedByUpgradeLevel) ? marketItem.purchaseUpgradeLevelRestriction.RestrictionValue : marketItem.mountUpgradeLevelRestriction.RestrictionValue).ToString();
                marketItem.upgradeLevelRestrictionBadgeGUI.gameObject.SetActive(true);
                marketItem.upgradeLevelRestrictionBadgeGUI.SendMessageUpwards("OnItemDisabled", SendMessageOptions.RequireReceiver);
            }
        }

        [OnEventFire]
        public void ShowUpgradeLevelRestrictionIndicator(ItemUpgradeUpdatedEvent e, UpgradableItemNode parentItem, [JoinByParentGroup, Combine] MarketItemWithUpgradeLevelRestrictionNode marketItem)
        {
            CheckMarketItemRestrictionsEvent eventInstance = new CheckMarketItemRestrictionsEvent();
            base.ScheduleEvent(eventInstance, marketItem);
            if (!eventInstance.RestrictedByUpgradeLevel && (!eventInstance.RestrictedByRank && marketItem.upgradeLevelRestrictionBadgeGUI.gameObject.activeSelf))
            {
                marketItem.upgradeLevelRestrictionBadgeGUI.SendMessageUpwards("OnItemEnabled", SendMessageOptions.DontRequireReceiver);
                marketItem.upgradeLevelRestrictionBadgeGUI.SendMessageUpwards("Unlock", SendMessageOptions.DontRequireReceiver);
                marketItem.upgradeLevelRestrictionBadgeGUI.gameObject.SetActive(false);
                marketItem.upgradeLevelRestrictionBadgeGUI.SendMessageUpwards("MoveToItem", marketItem.upgradeLevelRestrictionBadgeGUI.gameObject, SendMessageOptions.DontRequireReceiver);
            }
        }

        [OnEventFire]
        public void ShowUserRankRestrictionIndicator(NodeAddedEvent e, [Combine] MarketItemWithUserRankRestrictionNode item, SelfUserNode selfUser)
        {
            CheckMarketItemRestrictionsEvent eventInstance = new CheckMarketItemRestrictionsEvent();
            base.ScheduleEvent(eventInstance, item);
            if (eventInstance.RestrictedByRank)
            {
                item.userRankRestrictionBadgeGUI.SetRank(item.purchaseUserRankRestriction.RestrictionValue);
                item.userRankRestrictionBadgeGUI.gameObject.SetActive(true);
                item.userRankRestrictionBadgeGUI.SendMessageUpwards("OnItemDisabled", SendMessageOptions.RequireReceiver);
            }
        }

        [OnEventFire]
        public void ShowUserRankRestrictionIndicator(UpdateRankEvent e, SelfUserNode selfUser, [JoinAll, Combine] MarketItemWithUserRankRestrictionNode item)
        {
            CheckMarketItemRestrictionsEvent eventInstance = new CheckMarketItemRestrictionsEvent();
            base.ScheduleEvent(eventInstance, item);
            if (!eventInstance.RestrictedByRank)
            {
                item.userRankRestrictionBadgeGUI.SendMessageUpwards("OnItemEnabled", SendMessageOptions.DontRequireReceiver);
                item.userRankRestrictionBadgeGUI.gameObject.SetActive(false);
            }
        }

        public class MarketItemNode : Node
        {
            public MarketItemComponent marketItem;
        }

        public class MarketItemWithUpgradeLevelRestrictionNode : MarketItemRestrictionBadgeSystem.MarketItemNode
        {
            public PurchaseUpgradeLevelRestrictionComponent purchaseUpgradeLevelRestriction;
            public MountUpgradeLevelRestrictionComponent mountUpgradeLevelRestriction;
            public UpgradeLevelRestrictionBadgeGUIComponent upgradeLevelRestrictionBadgeGUI;
            public ParentGroupComponent parentGroup;
        }

        public class MarketItemWithUserRankRestrictionNode : MarketItemRestrictionBadgeSystem.MarketItemNode
        {
            public PurchaseUserRankRestrictionComponent purchaseUserRankRestriction;
            public UserRankRestrictionBadgeGUIComponent userRankRestrictionBadgeGUI;
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
        }
    }
}

