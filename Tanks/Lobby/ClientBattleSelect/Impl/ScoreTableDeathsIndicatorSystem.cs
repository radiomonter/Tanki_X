namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Lobby.ClientBattleSelect.API;

    public class ScoreTableDeathsIndicatorSystem : ECSSystem
    {
        [OnEventFire]
        public void SetDeaths(NodeAddedEvent e, DeathsNode deaths, [Context, JoinByUser] UserNode user)
        {
            deaths.scoreTableDeathsIndicator.Deaths = user.roundUserStatistics.Deaths;
        }

        [OnEventFire]
        public void SetDeaths(RoundUserStatisticsUpdatedEvent e, UserNode user, [JoinByUser] DeathsNode deaths)
        {
            deaths.scoreTableDeathsIndicator.Deaths = user.roundUserStatistics.Deaths;
        }

        public class DeathsNode : Node
        {
            public UserGroupComponent userGroup;
            public ScoreTableDeathsIndicatorComponent scoreTableDeathsIndicator;
        }

        public class UserNode : Node
        {
            public UserGroupComponent userGroup;
            public RoundUserStatisticsComponent roundUserStatistics;
        }
    }
}

