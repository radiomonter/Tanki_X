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
    using Tanks.Lobby.ClientUserProfile.API;
    using Tanks.Lobby.ClientUserProfile.Impl;

    public class LeagueRewardScreenSystem : ECSSystem
    {
        [CompilerGenerated]
        private static Comparison<LeagueNode> <>f__mg$cache0;

        [OnEventFire]
        public void AttachLeagueRewardDialogToUserGroup(NodeAddedEvent e, SingleNode<LeagueRewardDialog> leagueRewardDialog, [JoinAll] UserWithLeagueNode userWithLeagueNode)
        {
            userWithLeagueNode.userGroup.Attach(leagueRewardDialog.Entity);
        }

        [OnEventFire]
        public void GetLeagueByIndex(GetLeagueByIndexEvent e, Node any, [JoinAll] ICollection<LeagueNode> leagues)
        {
            foreach (LeagueNode node in SortedLeagues(leagues))
            {
                if (node.leagueConfig.LeagueIndex == e.index)
                {
                    e.leagueEntity = node.Entity;
                    break;
                }
            }
        }

        [OnEventFire]
        public void InitLeagueScreen(NodeAddedEvent e, SingleNode<LeagueRewardUIComponent> screen, [JoinAll] ICollection<LeagueNode> leagueNodes, [JoinAll] UserWithLeagueNode userWithLeagueNode, [JoinAll] SingleNode<SeasonEndDateComponent> leaguesConfig)
        {
            Entity entity = null;
            foreach (LeagueNode node in SortedLeagues(leagueNodes))
            {
                screen.component.AddLeagueItem(node.Entity).Init(node.Entity);
                if (node.leagueGroup.Key == userWithLeagueNode.leagueGroup.Key)
                {
                    entity = node.Entity;
                }
            }
            screen.component.SetChestScoreLimit(userWithLeagueNode.gameplayChestScore.Limit);
            screen.component.SetLeaguesCount(leagueNodes.Count);
            screen.component.SelectUserLeague(entity, userWithLeagueNode.userReputation.Reputation);
            screen.component.SetSeasonEndDate(leaguesConfig.component.EndDate);
            screen.component.SelectBar(0);
        }

        private static int LeagueCompare(LeagueNode a, LeagueNode b) => 
            a.leagueConfig.LeagueIndex - b.leagueConfig.LeagueIndex;

        [OnEventFire]
        public void PutReputationToEnter(UpdateTopLeagueInfoEvent e, UserWithLeagueNode userWithLeagueNode, [JoinAll] SingleNode<LeagueRewardUIComponent> screen, [JoinAll] TopLeagueNode topLeague)
        {
            screen.component.PlaceInTopLeague = e.Place;
            screen.component.PutReputationToEnter(topLeague.Entity.Id, e.LastPlaceReputation);
            screen.component.UpdateLeagueRewardUI();
        }

        private static List<LeagueNode> SortedLeagues(ICollection<LeagueNode> leagueNodes)
        {
            List<LeagueNode> list = leagueNodes.ToList<LeagueNode>();
            if (<>f__mg$cache0 == null)
            {
                <>f__mg$cache0 = new Comparison<LeagueNode>(LeagueRewardScreenSystem.LeagueCompare);
            }
            list.Sort(<>f__mg$cache0);
            return list;
        }

        public class LeagueNode : Node
        {
            public LeagueComponent league;
            public LeagueGroupComponent leagueGroup;
            public LeagueNameComponent leagueName;
            public LeagueIconComponent leagueIcon;
            public LeagueConfigComponent leagueConfig;
            public ChestBattleRewardComponent chestBattleReward;
            public CurrentSeasonRewardForClientComponent currentSeasonRewardForClient;
        }

        public class TopLeagueNode : Node
        {
            public TopLeagueComponent topLeague;
        }

        public class UserWithLeagueNode : Node
        {
            public SelfUserComponent selfUser;
            public UserReputationComponent userReputation;
            public LeagueGroupComponent leagueGroup;
            public GameplayChestScoreComponent gameplayChestScore;
            public UserGroupComponent userGroup;
        }
    }
}

