namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Lobby.ClientBattleSelect.API;

    public class ScoreTablePingIndicatorSystem : ECSSystem
    {
        private void SetPing(PingIndicatorNode pingIndicator, UserNode user)
        {
        }

        [OnEventFire]
        public void SetPing(NodeAddedEvent e, [Combine] PingIndicatorNode pingIndicator, [Context, JoinByUser] UserNode user)
        {
            this.SetPing(pingIndicator, user);
        }

        [OnEventFire]
        public void SetPing(RoundUserStatisticsUpdatedEvent e, UserNode user, [Combine, JoinByUser] PingIndicatorNode pingIndicator)
        {
            this.SetPing(pingIndicator, user);
        }

        public class PingIndicatorNode : Node
        {
            public ScoreTablePingIndicatorComponent scoreTablePingIndicator;
            public UserGroupComponent userGroup;
        }

        public class UserNode : Node
        {
            public UserGroupComponent userGroup;
            public RoundUserStatisticsComponent roundUserStatistics;
        }
    }
}

