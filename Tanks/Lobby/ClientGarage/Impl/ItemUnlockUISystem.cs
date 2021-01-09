namespace Tanks.Lobby.ClientGarage.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.API;
    using UnityEngine;

    public class ItemUnlockUISystem : ECSSystem
    {
        [CompilerGenerated]
        private static Comparison<VisualItem> <>f__am$cache0;

        private void CleanSkinUnlocks(ItemUnlockUIComponent itemUnlockUI)
        {
            foreach (GameObject obj2 in itemUnlockUI.rewardPreviews)
            {
                Object.Destroy(obj2);
            }
        }

        [OnEventFire]
        public void CleanSkinUnlocks(NodeAddedEvent e, HullItemUnlockUINode node)
        {
            this.CleanSkinUnlocks(node.itemUnlockUI);
        }

        [OnEventFire]
        public void CleanSkinUnlocks(NodeAddedEvent e, TurretItemUnlockUINode node)
        {
            this.CleanSkinUnlocks(node.itemUnlockUI);
        }

        [OnEventFire]
        public void InitHull(NodeAddedEvent e, SelfUserNode user, [Context] HullItemUnlockUINode node)
        {
            this.InitializeSkinUnlocks(node.itemUnlockUI, node.selectedHullUI, node.Entity);
            this.SetExperienceProgress(node.selectedHullUI, node.itemUnlockUI);
        }

        private void InitializeSkinUnlocks(ItemUnlockUIComponent itemUnlockUI, SelectedItemUI selectedItemUI, Entity entity)
        {
            TankPartItem tankPartItem = selectedItemUI.TankPartItem;
            List<VisualItem> list = new List<VisualItem>();
            foreach (VisualItem item2 in tankPartItem.Skins)
            {
                if (item2.RestrictionLevel > 0)
                {
                    list.Add(item2);
                }
            }
            GetAllGraffitiesEvent eventInstance = new GetAllGraffitiesEvent();
            base.ScheduleEvent(eventInstance, entity);
            foreach (VisualItem item3 in eventInstance.Items)
            {
                if (ReferenceEquals(item3.ParentItem, tankPartItem) && (item3.RestrictionLevel > 0))
                {
                    list.Add(item3);
                }
            }
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = (item1, item2) => item1.RestrictionLevel.CompareTo(item2.RestrictionLevel);
            }
            list.Sort(<>f__am$cache0);
            foreach (VisualItem item4 in list)
            {
                GameObject rewardContainer = itemUnlockUI.rewardContainer;
                GameObject item = Object.Instantiate<GameObject>(itemUnlockUI.rewardPrefab);
                item.transform.SetParent(rewardContainer.transform, false);
                RewardElement element = item.GetComponent<RewardElement>();
                element.titleText.text = MarketItemNameLocalization.Instance.GetCategoryName(item4.MarketItem) + " " + MarketItemNameLocalization.Instance.GetGarageItemName(item4);
                element.descriptionText.text = !item4.IsRestricted ? ((string) itemUnlockUI.recievedText) : (itemUnlockUI.levelText + " " + item4.RestrictionLevel);
                ImageItemComponent component = item4.MarketItem.GetComponent<ImageItemComponent>();
                element.imageSkin.SpriteUid = component.SpriteUid;
                itemUnlockUI.rewardPreviews.Add(item);
            }
        }

        [OnEventFire]
        public void InitTurret(NodeAddedEvent e, SelfUserNode user, [Context] TurretItemUnlockUINode node)
        {
            this.InitializeSkinUnlocks(node.itemUnlockUI, node.selectedTurretUI, node.Entity);
            this.SetExperienceProgress(node.selectedTurretUI, node.itemUnlockUI);
        }

        private void SetExperienceProgress(SelectedItemUI selectedItemUI, ItemUnlockUIComponent itemUnlockUI)
        {
            if ((selectedItemUI.TankPartItem.UserItem != null) && selectedItemUI.TankPartItem.UserItem.HasComponent<UpgradeLevelItemComponent>())
            {
                itemUnlockUI.gameObject.SetActive(true);
                if (selectedItemUI.TankPartItem.UpgradeLevel == UpgradablePropertiesUtils.MAX_LEVEL)
                {
                    itemUnlockUI.experienceProgressBar.InitialNormalizedValue = 0f;
                    itemUnlockUI.experienceProgressBar.NormalizedValue = 1f;
                    itemUnlockUI.text.text = (string) itemUnlockUI.maxText;
                }
                else
                {
                    float absExperience = selectedItemUI.TankPartItem.AbsExperience;
                    float finalLevelExperience = selectedItemUI.TankPartItem.Experience.FinalLevelExperience;
                    float num3 = absExperience / finalLevelExperience;
                    itemUnlockUI.experienceProgressBar.NormalizedValue = num3;
                    itemUnlockUI.text.text = absExperience + "/" + finalLevelExperience;
                }
            }
        }

        [OnEventComplete]
        public void UpdateHull(NodeAddedEvent e, MountedHullNode turret, [JoinByUser] SelfUserNode self, [JoinAll] HullItemUnlockUINode uiNode)
        {
            this.CleanSkinUnlocks(uiNode.itemUnlockUI);
            this.InitializeSkinUnlocks(uiNode.itemUnlockUI, uiNode.selectedHullUI, uiNode.Entity);
            this.SetExperienceProgress(uiNode.selectedHullUI, uiNode.itemUnlockUI);
        }

        [OnEventComplete]
        public void UpdateTurret(NodeAddedEvent e, MountedTurretNode turret, [JoinByUser] SelfUserNode self, [JoinAll] TurretItemUnlockUINode uiNode)
        {
            this.CleanSkinUnlocks(uiNode.itemUnlockUI);
            this.InitializeSkinUnlocks(uiNode.itemUnlockUI, uiNode.selectedTurretUI, uiNode.Entity);
            this.SetExperienceProgress(uiNode.selectedTurretUI, uiNode.itemUnlockUI);
        }

        public class GarageUserItemNode : Node
        {
            public MarketItemGroupComponent marketItemGroup;
            public DescriptionItemComponent descriptionItem;
            public UserGroupComponent userGroup;
            public VisualPropertiesComponent visualProperties;
            public ExperienceItemComponent experienceItem;
            public UpgradeLevelItemComponent upgradeLevelItem;
        }

        public class HullItemUnlockUINode : Node
        {
            public ItemUnlockUIComponent itemUnlockUI;
            public SelectedHullUIComponent selectedHullUI;
        }

        public class MountedHullNode : ItemUnlockUISystem.GarageUserItemNode
        {
            public MountedItemComponent mountedItem;
            public TankItemComponent tankItem;
        }

        public class MountedTurretNode : ItemUnlockUISystem.GarageUserItemNode
        {
            public MountedItemComponent mountedItem;
            public WeaponItemComponent weaponItem;
        }

        public class SelfUserNode : Node
        {
            public SelfUserComponent selfUser;
            public UserGroupComponent userGroup;
        }

        public class TurretItemUnlockUINode : Node
        {
            public ItemUnlockUIComponent itemUnlockUI;
            public SelectedTurretUIComponent selectedTurretUI;
        }
    }
}

