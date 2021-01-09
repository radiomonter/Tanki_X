namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientUserProfile.API;

    public class TopLeagueInfoSystem : ECSSystem
    {
        [OnEventFire]
        public void GetLeagueInfo(NodeAddedEvent e, TopLeagueInfoUINode infoUI, [JoinByUser] UserNode user, [JoinAll] SelfUserNode selfUser)
        {
            GetLeagueInfoEvent eventInstance = new GetLeagueInfoEvent {
                UserId = user.Entity.Id
            };
            base.NewEvent(eventInstance).Attach(selfUser).Schedule();
        }

        public class SelfUserNode : TopLeagueInfoSystem.UserNode
        {
            public SelfUserComponent selfUser;
        }

        public class TopLeagueInfoUINode : Node
        {
            public TopLeagueInfoUIComponent topLeagueInfoUI;
            public UserGroupComponent userGroup;
        }

        public class UserNode : Node
        {
            public UserReputationComponent userReputation;
            public LeagueGroupComponent leagueGroup;
            public UserGroupComponent userGroup;
        }
    }
}

