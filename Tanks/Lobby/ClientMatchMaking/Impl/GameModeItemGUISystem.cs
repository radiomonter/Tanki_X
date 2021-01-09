namespace Tanks.Lobby.ClientMatchMaking.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Lobby.ClientBattleSelect.API;
    using Tanks.Lobby.ClientBattleSelect.Impl;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientGarage.Impl;
    using Tanks.Lobby.ClientUserProfile.API;

    public class GameModeItemGUISystem : ECSSystem
    {
        [OnEventFire]
        public void BlockMode(NodeAddedEvent e, MatchMakingModeNode gameModeItemNode)
        {
            if (!gameModeItemNode.matchMakingModeActivation.Active)
            {
                gameModeItemNode.gameModeSelectButton.SetInactive();
            }
        }

        [OnEventFire]
        public void DefaultModeInited(NodeAddedEvent e, MatchMakingTrainingModeNode matchMakingTrainingMode)
        {
            this.DelayUpdate(matchMakingTrainingMode);
        }

        private void DelayUpdate(Node modeItem)
        {
            base.NewEvent<UpdateMatchMakingModeEvent>().Attach(modeItem).ScheduleDelayed(0f);
        }

        [OnEventFire]
        public void InitDescription(NodeAddedEvent e, MatchMakingModeNode gameModeItemNode)
        {
            gameModeItemNode.gameModeSelectButton.ModeDescription = gameModeItemNode.descriptionItem.Description;
        }

        [OnEventFire]
        public void InitImage(NodeAddedEvent e, MatchMakingModeNode gameModeItemNode)
        {
            gameModeItemNode.gameModeSelectButton.SetImage(gameModeItemNode.imageItem.SpriteUid);
        }

        [OnEventFire]
        public void InitName(NodeAddedEvent e, MatchMakingModeNode gameModeItemNode)
        {
            gameModeItemNode.gameModeSelectButton.GameModeTitle = gameModeItemNode.descriptionItem.Name;
        }

        [OnEventFire]
        public void RestrictMode(NodeAddedEvent e, MatchMakingModeWithEnergyRestrictionNode gameModeItemNode)
        {
            this.DelayUpdate(gameModeItemNode);
        }

        [OnEventFire]
        public void RestrictMode(NodeAddedEvent e, MatchMakingModeWithRankRestrictionNode gameModeItemNode)
        {
            this.DelayUpdate(gameModeItemNode);
        }

        [OnEventFire]
        public void SelfUserInSuad(NodeAddedEvent e, SelfUserSquadNode selfUserSquad, [Combine] MatchMakingModeNode gameModeItemNode)
        {
            this.DelayUpdate(gameModeItemNode);
        }

        [OnEventFire]
        public void SelfUserIsNotSquadLeader(NodeRemoveEvent e, SelfUserLeaderSquadNode selfUserLeaderSquadNode, [JoinAll, Combine] MatchMakingModeNode gameModeItemNode)
        {
            this.DelayUpdate(gameModeItemNode);
        }

        [OnEventFire]
        public void SelfUserIsSquadLeader(NodeAddedEvent e, SelfUserLeaderSquadNode selfUserLeaderSquadNode, [Combine] MatchMakingModeNode gameModeItemNode)
        {
            this.DelayUpdate(gameModeItemNode);
        }

        [OnEventFire]
        public void SquadRemoved(NodeRemoveEvent e, SelfUserSquadNode selfUserSquad, [JoinAll, Combine] MatchMakingModeNode gameModeItemNode)
        {
            this.DelayUpdate(gameModeItemNode);
        }

        [OnEventFire]
        public void TeammateAdded(NodeAddedEvent e, [Combine] UserInSquadNode teammate, [Combine] MatchMakingModeNode gameModeItemNode)
        {
            this.DelayUpdate(gameModeItemNode);
        }

        [OnEventFire]
        public void TeammateRemoved(NodeRemoveEvent e, UserInSquadNode teammate, [JoinAll, Combine] MatchMakingModeNode gameModeItemNode)
        {
            this.DelayUpdate(gameModeItemNode);
        }

        [OnEventFire]
        public void TutorialComplete(NodeAddedEvent e, TutorialCompleteNode tutorialComplete, [JoinAll, Combine] MatchMakingModeNode gameModeItemNode)
        {
            this.DelayUpdate(gameModeItemNode);
        }

        [OnEventFire]
        public void UpdateGameModeItem(UpdateMatchMakingModeEvent e, MatchMakingModeNode matchMakingModeNode, [JoinAll] SelfUserNode user, [JoinAll] RanksNamesNode ranksNames, [JoinAll] Optional<SelfUserSquadNode> selfUserSquadNode, [JoinBySquad] ICollection<UserInSquadNode> allTeammates, [JoinAll] Optional<SelfUserLeaderSquadNode> selfUserLeaderNode)
        {
            bool userInSquadNow = selfUserSquadNode.IsPresent() && (allTeammates.Count > 0);
            bool userIsSquadLeader = selfUserLeaderNode.IsPresent();
            bool modeIsDefault = matchMakingModeNode.Entity.HasComponent<MatchMakingTrainingModeComponent>();
            if (matchMakingModeNode.Entity.HasComponent<RankRestrictionGUIComponent>() && (user.userRank.Rank < matchMakingModeNode.matchMakingModeRestrictions.MinimalRank))
            {
                RankRestrictionGUIComponent component = matchMakingModeNode.Entity.GetComponent<RankRestrictionGUIComponent>();
                matchMakingModeNode.gameModeSelectButton.SetRestricted(true);
                component.SetRank(matchMakingModeNode.matchMakingModeRestrictions.MinimalRank);
                component.RankName = ranksNames.ranksNames.Names[matchMakingModeNode.matchMakingModeRestrictions.MinimalRank];
            }
            matchMakingModeNode.gameModeSelectButton.SetAvailableForSquadMode(userInSquadNow, userIsSquadLeader, modeIsDefault);
        }

        public class GameModeNode : Node
        {
            public GameModeSelectButtonComponent gameModeSelectButton;
        }

        public class MatchMakingModeNode : GameModeItemGUISystem.GameModeNode
        {
            public MatchMakingModeComponent matchMakingMode;
            public DescriptionItemComponent descriptionItem;
            public ImageItemComponent imageItem;
            public MatchMakingModeRestrictionsComponent matchMakingModeRestrictions;
            public MatchMakingModeActivationComponent matchMakingModeActivation;
        }

        public class MatchMakingModeWithEnergyRestrictionNode : GameModeItemGUISystem.MatchMakingModeNode
        {
            public EnergyRestrictionGUIComponent energyRestrictionGUI;
        }

        public class MatchMakingModeWithRankRestrictionNode : GameModeItemGUISystem.MatchMakingModeNode
        {
            public RankRestrictionGUIComponent rankRestrictionGUI;
        }

        public class MatchMakingTrainingModeNode : GameModeItemGUISystem.MatchMakingModeNode
        {
            public MatchMakingTrainingModeComponent matchMakingTrainingMode;
        }

        [Not(typeof(UserNotificatorRankNamesComponent))]
        public class RanksNamesNode : Node
        {
            public RanksNamesComponent ranksNames;
        }

        public class SelfUserLeaderSquadNode : Node
        {
            public UserComponent user;
            public SelfUserComponent selfUser;
            public UserGroupComponent userGroup;
            public SquadGroupComponent squadGroup;
            public SquadLeaderComponent squadLeader;
        }

        public class SelfUserNode : Node
        {
            public SelfUserComponent selfUser;
            public UserRankComponent userRank;
            public LeagueGroupComponent leagueGroup;
        }

        public class SelfUserSquadNode : Node
        {
            public UserComponent user;
            public SelfUserComponent selfUser;
            public UserGroupComponent userGroup;
            public SquadGroupComponent squadGroup;
        }

        public class TutorialCompleteNode : Node
        {
            public TutorialDataComponent tutorialData;
            public TutorialCompleteComponent tutorialComplete;
        }

        public class UpdateMatchMakingModeEvent : Event
        {
        }

        [Not(typeof(SelfUserComponent))]
        public class UserInSquadNode : Node
        {
            public UserComponent user;
            public UserGroupComponent userGroup;
            public SquadGroupComponent squadGroup;
        }
    }
}

