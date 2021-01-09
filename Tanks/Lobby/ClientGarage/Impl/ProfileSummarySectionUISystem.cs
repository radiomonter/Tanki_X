namespace Tanks.Lobby.ClientGarage.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientUserProfile.API;
    using Tanks.Lobby.ClientUserProfile.Impl;

    public class ProfileSummarySectionUISystem : ECSSystem
    {
        [OnEventFire]
        public void GetUserTotalBattlesCount(GetChangeTurretTutorialValidationDataEvent e, Node any, [JoinAll] SingleNode<SelfUserComponent> selfUser, [JoinByUser] UserStatisticsNode statistics)
        {
            Dictionary<string, long> dictionary = statistics.userStatistics.Statistics;
            e.BattlesCount = StatsTool.GetParameterValue<string>(dictionary, "ALL_BATTLES_PARTICIPATED") - StatsTool.GetParameterValue<string>(dictionary, "ALL_CUSTOM_BATTLES_PARTICIPATED");
        }

        [OnEventFire]
        public void HideLeaguePlaceInfo(NodeRemoveEvent e, LeaguePlaceUINode leaguePlaceUI)
        {
            leaguePlaceUI.leaguePlaceUI.Hide();
        }

        [OnEventFire]
        public void SetEquipmentStatisticsInfo(NodeAddedEvent e, SingleNode<MostPlayedEquipmentUIComponent> ui, ProfileScreenWithUserGroupNode profileScreen, [JoinByUser] UserStatisticsNode statistics, [JoinAll] SingleNode<SelectedPresetComponent> selectedPreset)
        {
            Dictionary<long, long> hullStatistics = statistics.favoriteEquipmentStatistics.HullStatistics;
            Dictionary<long, long> turretStatistics = statistics.favoriteEquipmentStatistics.TurretStatistics;
            if (!hullStatistics.Any<KeyValuePair<long, long>>() && !turretStatistics.Any<KeyValuePair<long, long>>())
            {
                ui.component.SwitchState(false);
            }
            else
            {
                ui.component.SwitchState(true);
                Entity entityById = base.GetEntityById(StatsTool.GetItemWithLagestValue(hullStatistics));
                Entity entity2 = base.GetEntityById(StatsTool.GetItemWithLagestValue(turretStatistics));
                string turretUID = entity2.GetComponent<MarketItemGroupComponent>().Key.ToString();
                ui.component.SetMostPlayed(turretUID, entity2.GetComponent<DescriptionItemComponent>().Name, entityById.GetComponent<MarketItemGroupComponent>().Key.ToString(), entityById.GetComponent<DescriptionItemComponent>().Name);
            }
        }

        [OnEventFire]
        public void SetLeagueInfo(NodeAddedEvent e, ProfileSummarySectionUINode sectionUI, ProfileScreenWithUserGroupNode profileScreen, [Context] LeagueUINode uiNode, [JoinByUser] UserWithLeagueNode user, [JoinByLeague, Context] LeagueNode league)
        {
            uiNode.leagueUI.SetLeague(league.leagueName.Name, league.leagueIcon.SpriteUid, user.userReputation.Reputation);
            sectionUI.profileSummarySectionUI.showRewardsButton.SetActive(user.Entity.HasComponent<SelfUserComponent>());
        }

        [OnEventFire]
        public void SetLevelInfo(NodeAddedEvent e, ProfileSummarySectionUINode sectionUI, ProfileScreenWithUserGroupNode profileScreen, [JoinByUser] UserStatisticsNode statistics, [JoinAll] RanksNamesNode rankNames)
        {
            GetUserLevelInfoEvent eventInstance = new GetUserLevelInfoEvent();
            base.ScheduleEvent(eventInstance, statistics);
            sectionUI.profileSummarySectionUI.SetLevelInfo(eventInstance.Info, rankNames.ranksNames.Names[eventInstance.Info.Level + 1]);
        }

        [OnEventFire]
        public void SetStatisticsInfo(NodeAddedEvent e, ProfileSummarySectionUINode sectionUI, ProfileScreenWithUserGroupNode profileScreen, [JoinByUser] UserStatisticsNode statistics)
        {
            Dictionary<string, long> dictionary = statistics.userStatistics.Statistics;
            long parameterValue = StatsTool.GetParameterValue<string>(dictionary, "VICTORIES");
            sectionUI.profileSummarySectionUI.SetWinLossStatistics(parameterValue, StatsTool.GetParameterValue<string>(dictionary, "DEFEATS"), StatsTool.GetParameterValue<string>(dictionary, "ALL_BATTLES_PARTICIPATED") - StatsTool.GetParameterValue<string>(dictionary, "ALL_CUSTOM_BATTLES_PARTICIPATED"));
        }

        [OnEventFire]
        public void ShowLeaguePlaceInfo(UpdateTopLeagueInfoEvent e, UserWithLeagueNode selfUser, [JoinAll] LeaguePlaceUINode leaguePlaceUI, [JoinByUser] UserWithLeagueNode user)
        {
            if (e.UserId.Equals(user.Entity.Id))
            {
                leaguePlaceUI.leaguePlaceUI.SetPlace(e.Place);
            }
        }

        [OnEventFire]
        public void ShowLeagueReward(ButtonClickEvent e, PreviewLegaueRewardButtonNode previewLeagueRewardButton, [JoinAll] SingleNode<Dialogs60Component> dialogs, [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens)
        {
            LeagueRewardDialog dialog = dialogs.component.Get<LeagueRewardDialog>();
            List<Animator> animators = new List<Animator>();
            if (screens.IsPresent())
            {
                animators = screens.Get().component.Animators;
            }
            dialog.Show(animators);
        }

        public class LeagueNode : Node
        {
            public LeagueComponent league;
            public LeagueGroupComponent leagueGroup;
            public LeagueNameComponent leagueName;
            public LeagueIconComponent leagueIcon;
        }

        public class LeaguePlaceUINode : Node
        {
            public LeaguePlaceUIComponent leaguePlaceUI;
        }

        public class LeagueUINode : Node
        {
            public LeagueUIComponent leagueUI;
        }

        public class PreviewLegaueRewardButtonNode : Node
        {
            public PreviewLeagueRewardButtonComponent previewLeagueRewardButton;
        }

        public class ProfileScreenWithUserGroupNode : Node
        {
            public ProfileScreenComponent profileScreen;
            public UserGroupComponent userGroup;
            public ActiveScreenComponent activeScreen;
        }

        public class ProfileSummarySectionUINode : Node
        {
            public ProfileSummarySectionUIComponent profileSummarySectionUI;
        }

        [Not(typeof(UserNotificatorRankNamesComponent))]
        public class RanksNamesNode : Node
        {
            public RanksNamesComponent ranksNames;
        }

        public class UserStatisticsNode : Node
        {
            public UserStatisticsComponent userStatistics;
            public FavoriteEquipmentStatisticsComponent favoriteEquipmentStatistics;
            public KillsEquipmentStatisticsComponent killsEquipmentStatistics;
        }

        public class UserWithLeagueNode : Node
        {
            public UserGroupComponent userGroup;
            public LeagueGroupComponent leagueGroup;
            public UserReputationComponent userReputation;
        }
    }
}

