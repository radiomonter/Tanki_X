namespace Tanks.Lobby.ClientMatchMaking.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientBattleSelect.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientGarage.Impl;
    using Tanks.Lobby.ClientUserProfile.API;
    using UnityEngine;

    public class PlayScreenSystem : ECSSystem
    {
        private void CreateActiveModeInstance(Entity mode, GameObject prefab, GameObject container)
        {
            this.CreateModeInstance(mode, prefab, container).transform.SetAsFirstSibling();
        }

        private GameObject CreateModeInstance(Entity mode, GameObject prefab, GameObject container)
        {
            GameObject obj2 = Object.Instantiate<GameObject>(prefab);
            obj2.transform.SetParent(container.transform, false);
            obj2.GetComponent<EntityBehaviour>().BuildEntity(mode);
            return obj2;
        }

        [OnEventFire]
        public void InitMode(NodeAddedEvent e, PlayScreenNode screen, RatingModeNode mode)
        {
            GameObject ratingModeItemPrefab = screen.playScreen.RatingModeItemPrefab;
            this.CreateActiveModeInstance(mode.Entity, ratingModeItemPrefab, screen.playScreen.RatingModeContainer);
        }

        [OnEventFire]
        public void RemoveModes(NodeRemoveEvent e, PlayScreenNode screen, [JoinAll] RatingModeGUINode gameMode)
        {
            Object.Destroy(gameMode.gameModeSelectButton.gameObject);
        }

        [OnEventFire]
        public void SetChestProgression(NodeAddedEvent e, PlayScreenNode screen, SingleNode<ChestProgressBarComponent> energygui, [JoinAll] SelfUserNode user, [JoinSelf] SingleNode<GameplayChestScoreComponent> chestScore)
        {
            energygui.component.SetProgress(chestScore.component.Current, chestScore.component.Limit);
        }

        [OnEventFire]
        public void SetChestProgression(NodeAddedEvent e, PlayScreenNode screen, SingleNode<ChestProgressBarComponent> energygui, [JoinAll] SelfUserNode user, [JoinSelf] SingleNode<GameplayChestScoreComponent> chestScore, [JoinByLeague] LeaguemNode league)
        {
            long chestId = league.chestBattleReward.ChestId;
            Entity entityById = base.GetEntityById(chestId);
            DescriptionItemComponent component = entityById.GetComponent<DescriptionItemComponent>();
            ImageItemComponent component2 = entityById.GetComponent<ImageItemComponent>();
            energygui.component.SetChest(component.Name, component2.SpriteUid);
            energygui.component.SetChestTooltip(chestScore.component.Limit, league.leagueConfig.LeagueIndex > 2);
        }

        [OnEventFire]
        public void SetLeagueInfo(NodeAddedEvent e, SingleNode<RankedBattleGUIComponent> uiNode, [JoinAll] SelfUserNode user, [JoinByLeague, Context] LeagueNode league)
        {
            uiNode.component.SetLeague(league.leagueName.Name, league.leagueIcon.SpriteUid);
        }

        [OnEventFire]
        public void SetLeagueInfo(NodeAddedEvent e, PlayScreenNode screen, SingleNode<PlayScreenSeasonGUIComponent> seasonGUI, [JoinAll] LeaguesConfigNode leaguesConfig)
        {
            if (string.IsNullOrEmpty(leaguesConfig.currentSeasonName.SeasonName))
            {
                seasonGUI.component.SetSeasonNameFromNumber((long) (leaguesConfig.currentSeasonNumber.SeasonNumber - 1));
            }
            else
            {
                seasonGUI.component.SetSeasonName(leaguesConfig.currentSeasonName.SeasonName);
            }
            seasonGUI.component.EndDate = leaguesConfig.seasonEndDate.EndDate;
        }

        public class LeaguemNode : Node
        {
            public LeagueConfigComponent leagueConfig;
            public ChestBattleRewardComponent chestBattleReward;
        }

        public class LeagueNode : Node
        {
            public LeagueComponent league;
            public LeagueGroupComponent leagueGroup;
            public LeagueNameComponent leagueName;
            public LeagueIconComponent leagueIcon;
        }

        public class LeaguesConfigNode : Node
        {
            public SeasonEndDateComponent seasonEndDate;
            public CurrentSeasonNumberComponent currentSeasonNumber;
            public CurrentSeasonNameComponent currentSeasonName;
        }

        public class PlayScreenNode : Node
        {
            public PlayScreenComponent playScreen;
        }

        public class QuickModeNode : Node
        {
            public MatchMakingEnergyModeComponent matchMakingEnergyMode;
        }

        public class RatingModeGUINode : PlayScreenSystem.RatingModeNode
        {
            public GameModeSelectButtonComponent gameModeSelectButton;
        }

        public class RatingModeNode : Node
        {
            public MatchMakingRatingModeComponent matchMakingRatingMode;
        }

        public class SelfUserNode : Node
        {
            public SelfUserComponent selfUser;
            public UserRankComponent userRank;
            public LeagueGroupComponent leagueGroup;
        }
    }
}

