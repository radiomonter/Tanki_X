namespace Tanks.Lobby.ClientMatchMaking.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientEntrance.Impl;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientMatchMaking.API;
    using Tanks.Lobby.ClientUserProfile.API;

    public class MatchMakingDefaultModeSystem : ECSSystem
    {
        [CompilerGenerated]
        private static Func<MatchMakingDefaultModeNode, int> <>f__am$cache0;

        [OnEventComplete]
        public void EnterToDefaultMode(SelectDefaultMatchMakingModeEvent e, Node any)
        {
            if (e.DefaultMode.IsPresent())
            {
                base.ScheduleEvent<SaveBattleModeEvent>(e.DefaultMode.Get());
                base.ScheduleEvent(new UserEnterToMatchMakingEvent(), e.DefaultMode.Get());
            }
        }

        [OnEventFire]
        public void SelectDefaultMode(SelectDefaultMatchMakingModeEvent e, Node any, [JoinAll] SelfUserNode selfUser, [JoinAll] ICollection<MatchMakingDefaultModeNode> modes)
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = mode => mode.orderItem.Index;
            }
            List<MatchMakingDefaultModeNode> list = modes.OrderBy<MatchMakingDefaultModeNode, int>(<>f__am$cache0).ToList<MatchMakingDefaultModeNode>();
            Optional<Entity> optional = Optional<Entity>.empty();
            if (selfUser.registrationDate.RegistrationDate.UnityTime != 0f)
            {
                foreach (MatchMakingDefaultModeNode node in list)
                {
                    long num;
                    selfUser.userStatistics.Statistics.TryGetValue("ALL_BATTLES_PARTICIPATED", out num);
                    if (num < node.matchMakingDefaultMode.MinimalBattles)
                    {
                        optional = Optional<Entity>.of(node.Entity);
                        break;
                    }
                }
            }
            if (optional.IsPresent())
            {
                e.DefaultMode = optional;
            }
            else
            {
                foreach (MatchMakingDefaultModeNode node2 in list)
                {
                    if ((selfUser.userRank.Rank >= node2.matchMakingModeRestrictions.MinimalRank) && (selfUser.userRank.Rank <= node2.matchMakingModeRestrictions.MaximalRank))
                    {
                        optional = Optional<Entity>.of(node2.Entity);
                        break;
                    }
                }
                e.DefaultMode = optional;
            }
        }

        public class MatchMakingDefaultModeNode : Node
        {
            public MatchMakingModeComponent matchMakingMode;
            public MatchMakingModeRestrictionsComponent matchMakingModeRestrictions;
            public MatchMakingDefaultModeComponent matchMakingDefaultMode;
            public MatchMakingModeActivationComponent matchMakingModeActivation;
            public OrderItemComponent orderItem;
        }

        public class SelfUserNode : Node
        {
            public UserComponent user;
            public SelfUserComponent selfUser;
            public UserRankComponent userRank;
            public UserStatisticsComponent userStatistics;
            public RegistrationDateComponent registrationDate;
        }
    }
}

