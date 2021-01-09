namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientGarage.API;
    using UnityEngine;

    public class UserItemRestrictionBadgeSystem : ECSSystem
    {
        [OnEventFire]
        public void HideUpgradeLevelRestrictionIndicator(NodeRemoveEvent e, SlotUserItemNode slot)
        {
            slot.upgradeLevelRestrictionBadgeGUI.SendMessageUpwards("Unlock", SendMessageOptions.DontRequireReceiver);
            slot.upgradeLevelRestrictionBadgeGUI.SendMessageUpwards("OnItemEnabled", SendMessageOptions.DontRequireReceiver);
            slot.upgradeLevelRestrictionBadgeGUI.gameObject.SetActive(false);
            slot.upgradeLevelRestrictionBadgeGUI.SendMessageUpwards("MoveToItem", slot.upgradeLevelRestrictionBadgeGUI.gameObject, SendMessageOptions.DontRequireReceiver);
        }

        [OnEventFire]
        public void HideUpgradeLevelRestrictionIndicator(NodeRemoveEvent e, UserItemWithUpgradeLevelRestrictionNode userItem)
        {
            userItem.upgradeLevelRestrictionBadgeGUI.SendMessageUpwards("Unlock", SendMessageOptions.DontRequireReceiver);
            userItem.upgradeLevelRestrictionBadgeGUI.SendMessageUpwards("OnItemEnabled", SendMessageOptions.DontRequireReceiver);
            userItem.upgradeLevelRestrictionBadgeGUI.gameObject.SetActive(false);
            userItem.upgradeLevelRestrictionBadgeGUI.SendMessageUpwards("MoveToItem", userItem.upgradeLevelRestrictionBadgeGUI.gameObject, SendMessageOptions.DontRequireReceiver);
        }

        [OnEventFire]
        public void HideUserRankRestrictionIndicator(NodeRemoveEvent e, UserItemWithUserRankRestrictionNode userItem)
        {
            userItem.userRankRestrictionBadgeGUI.gameObject.SetActive(false);
            userItem.userRankRestrictionBadgeGUI.SendMessageUpwards("OnItemEnabled", SendMessageOptions.DontRequireReceiver);
        }

        [OnEventFire]
        public void ShowUpgradeLevelRestrictionIndicator(NodeAddedEvent e, SlotUserItemNode slot)
        {
            slot.upgradeLevelRestrictionBadgeGUI.RestrictionValue = slot.slotUserItemInfo.UpgradeLevel.ToString();
            slot.upgradeLevelRestrictionBadgeGUI.gameObject.SetActive(true);
            slot.upgradeLevelRestrictionBadgeGUI.SendMessageUpwards("OnItemDisabled", SendMessageOptions.RequireReceiver);
        }

        [OnEventFire]
        public void ShowUpgradeLevelRestrictionIndicator(NodeAddedEvent e, NotGraffitiUserItemWithUpgradeLevelRestrictionNode userItem, [Context, JoinByMarketItem] MarketItemWithUpgradeLevelRestrictionNode marketItem)
        {
            userItem.upgradeLevelRestrictionBadgeGUI.RestrictionValue = marketItem.mountUpgradeLevelRestriction.RestrictionValue.ToString();
            userItem.upgradeLevelRestrictionBadgeGUI.gameObject.SetActive(true);
            userItem.upgradeLevelRestrictionBadgeGUI.SendMessageUpwards("OnItemDisabled", SendMessageOptions.RequireReceiver);
        }

        [OnEventFire]
        public void ShowUpgradeLevelRestrictionIndicator(NodeAddedEvent e, GraffitiUserItemWithUpgradeLevelRestrictionNode userItem, [Context, JoinByMarketItem] MarketItemWithUpgradeLevelRestrictionNode marketItem, [JoinByParentGroup] ParentHullMarketItemNode parent)
        {
            userItem.upgradeLevelRestrictionBadgeGUI.RestrictionValue = $"{parent.descriptionItem.Name} {marketItem.mountUpgradeLevelRestriction.RestrictionValue}";
            userItem.upgradeLevelRestrictionBadgeGUI.gameObject.SetActive(true);
            userItem.upgradeLevelRestrictionBadgeGUI.SendMessageUpwards("OnItemDisabled", SendMessageOptions.RequireReceiver);
        }

        [OnEventFire]
        public void ShowUpgradeLevelRestrictionIndicator(NodeAddedEvent e, GraffitiUserItemWithUpgradeLevelRestrictionNode userItem, [Context, JoinByMarketItem] MarketItemWithUpgradeLevelRestrictionNode marketItem, [JoinByParentGroup] ParentWeaponMarketItemNode parent)
        {
            userItem.upgradeLevelRestrictionBadgeGUI.RestrictionValue = $"{parent.descriptionItem.Name} {marketItem.mountUpgradeLevelRestriction.RestrictionValue}";
            userItem.upgradeLevelRestrictionBadgeGUI.gameObject.SetActive(true);
            userItem.upgradeLevelRestrictionBadgeGUI.SendMessageUpwards("OnItemDisabled", SendMessageOptions.RequireReceiver);
        }

        [OnEventFire]
        public void ShowUserRankRestrictionIndicator(NodeAddedEvent e, UserItemWithUserRankRestrictionNode userItem, [Context, JoinByMarketItem] MarketItemWithUserRankRestrictionNode marketItem)
        {
            userItem.userRankRestrictionBadgeGUI.SetRank(marketItem.mountUserRankRestriction.RestrictionValue);
            userItem.userRankRestrictionBadgeGUI.gameObject.SetActive(true);
            userItem.userRankRestrictionBadgeGUI.SendMessageUpwards("OnItemDisabled", SendMessageOptions.RequireReceiver);
        }

        [OnEventFire]
        public void StubToRegisterComponent(NodeAddedEvent e, SingleNode<RestrictionByUserFractionComponent> node)
        {
        }

        public class GraffitiUserItemWithUpgradeLevelRestrictionNode : UserItemRestrictionBadgeSystem.UserItemWithUpgradeLevelRestrictionNode
        {
            public GraffitiItemComponent graffitiItem;
        }

        public class MarketItemNode : Node
        {
            public MarketItemComponent marketItem;
        }

        public class MarketItemWithUpgradeLevelRestrictionNode : UserItemRestrictionBadgeSystem.MarketItemNode
        {
            public MountUpgradeLevelRestrictionComponent mountUpgradeLevelRestriction;
        }

        public class MarketItemWithUserRankRestrictionNode : UserItemRestrictionBadgeSystem.MarketItemNode
        {
            public MountUserRankRestrictionComponent mountUserRankRestriction;
        }

        [Not(typeof(GraffitiItemComponent))]
        public class NotGraffitiUserItemWithUpgradeLevelRestrictionNode : UserItemRestrictionBadgeSystem.UserItemWithUpgradeLevelRestrictionNode
        {
        }

        public class ParentHullMarketItemNode : Node
        {
            public TankItemComponent tankItem;
            public MarketItemComponent marketItem;
            public ParentGroupComponent parentGroup;
            public DescriptionItemComponent descriptionItem;
        }

        public class ParentWeaponMarketItemNode : Node
        {
            public WeaponItemComponent weaponItem;
            public MarketItemComponent marketItem;
            public ParentGroupComponent parentGroup;
            public DescriptionItemComponent descriptionItem;
        }

        public class SlotUserItemNode : Node
        {
            public SlotUserItemInfoComponent slotUserItemInfo;
            public SlotLockedComponent slotLocked;
            public UpgradeLevelRestrictionBadgeGUIComponent upgradeLevelRestrictionBadgeGUI;
        }

        public class UserItemNode : Node
        {
            public UserItemComponent userItem;
        }

        public class UserItemWithUpgradeLevelRestrictionNode : UserItemRestrictionBadgeSystem.UserItemNode
        {
            public RestrictedByUpgradeLevelComponent restrictedByUpgradeLevel;
            public UpgradeLevelRestrictionBadgeGUIComponent upgradeLevelRestrictionBadgeGUI;
        }

        public class UserItemWithUserRankRestrictionNode : UserItemRestrictionBadgeSystem.UserItemNode
        {
            public RestrictedByUserRankComponent restrictedByUserRank;
            public UserRankRestrictionBadgeGUIComponent userRankRestrictionBadgeGUI;
        }
    }
}

