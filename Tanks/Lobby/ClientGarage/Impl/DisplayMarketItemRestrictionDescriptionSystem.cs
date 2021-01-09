namespace Tanks.Lobby.ClientGarage.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientNavigation.API;

    public class DisplayMarketItemRestrictionDescriptionSystem : ECSSystem
    {
        public static readonly string RANK = "%RANK%";
        public static readonly string ITEM_upgLEVEL = "%ITEM_upgLEVEL%";

        [OnEventFire]
        public void HideDescriptions(ListItemSelectedEvent e, Node any, [JoinAll] ScreenNode screen)
        {
            screen.garageItemsScreen.UserRankRestrictionDescription.gameObject.SetActive(false);
            screen.garageItemsScreen.UpgradeLevelRestrictionDescription.gameObject.SetActive(false);
        }

        private void ShowUpgradeLevelRestrictionDescription(ScreenNode screen, int restrictionValue)
        {
            screen.garageItemsScreen.UpgradeLevelRestrictionDescription.Description = screen.garageItemsScreenText.UpgradeLevelRestrictionDescription.Replace(ITEM_upgLEVEL, restrictionValue.ToString());
            screen.garageItemsScreen.UpgradeLevelRestrictionDescription.gameObject.SetActive(true);
        }

        [OnEventComplete]
        public void ShowUpgradeLevelRestrictionDescription(ListItemSelectedEvent e, UpgradeLevelRestrictionNode upgradeLevelRestriction, [JoinAll] ScreenNode screen)
        {
            CheckMarketItemRestrictionsEvent eventInstance = new CheckMarketItemRestrictionsEvent();
            base.ScheduleEvent(eventInstance, upgradeLevelRestriction);
            if (eventInstance.RestrictedByUpgradeLevel)
            {
                if ((upgradeLevelRestriction.purchaseUpgradeLevelRestriction.RestrictionValue == 0) && eventInstance.MountWillBeRestrictedByUpgradeLevel)
                {
                    this.ShowUpgradeLevelRestrictionDescription(screen, upgradeLevelRestriction.mountUpgradeLevelRestriction.RestrictionValue);
                }
                else
                {
                    this.ShowUpgradeLevelRestrictionDescription(screen, upgradeLevelRestriction.purchaseUpgradeLevelRestriction.RestrictionValue);
                }
            }
        }

        private void ShowUserRankRestrictionDescription(ScreenNode screen, PurchaseUserRankRestrictionComponent restriction, RanksNamesComponent ranksNames)
        {
            screen.garageItemsScreen.UserRankRestrictionDescription.Description = screen.garageItemsScreenText.UserRankRestrictionDescription.Replace(RANK, ranksNames.Names[restriction.RestrictionValue]);
            screen.garageItemsScreen.UserRankRestrictionDescription.gameObject.SetActive(true);
        }

        [OnEventComplete]
        public void ShowUserRankRestrictionDescription(ListItemSelectedEvent e, UserRankRestrictionNode userRankRestriction, [JoinAll] SelfUserNode user, [JoinAll] ScreenNode screen, [JoinAll] SingleNode<RanksNamesComponent> ranksNames)
        {
            CheckMarketItemRestrictionsEvent eventInstance = new CheckMarketItemRestrictionsEvent();
            base.ScheduleEvent(eventInstance, userRankRestriction);
            if (eventInstance.RestrictedByRank)
            {
                this.ShowUserRankRestrictionDescription(screen, userRankRestriction.purchaseUserRankRestriction, ranksNames.component);
            }
        }

        public class MarketItemNode : Node
        {
            public MarketItemComponent marketItem;
        }

        public class ScreenNode : Node
        {
            public GarageItemsScreenComponent garageItemsScreen;
            public ScreenComponent screen;
            public ActiveScreenComponent activeScreen;
            public GarageItemsScreenTextComponent garageItemsScreenText;
        }

        public class SelfUserNode : Node
        {
            public SelfUserComponent selfUser;
            public UserRankComponent userRank;
        }

        public class UpgradeLevelRestrictionNode : DisplayMarketItemRestrictionDescriptionSystem.MarketItemNode
        {
            public PurchaseUpgradeLevelRestrictionComponent purchaseUpgradeLevelRestriction;
            public MountUpgradeLevelRestrictionComponent mountUpgradeLevelRestriction;
            public ParentGroupComponent parentGroup;
        }

        public class UserRankRestrictionNode : DisplayMarketItemRestrictionDescriptionSystem.MarketItemNode
        {
            public PurchaseUserRankRestrictionComponent purchaseUserRankRestriction;
        }
    }
}

