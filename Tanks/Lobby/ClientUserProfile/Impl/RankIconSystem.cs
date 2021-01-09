namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientUserProfile.API;

    public class RankIconSystem : ECSSystem
    {
        [OnEventFire]
        public void SetIcon(NodeAddedEvent e, [Combine] RankIconNode rankIcon, [Context, JoinByUser] UserRankNode userRank)
        {
            rankIcon.rankIcon.SetRank(userRank.userRank.Rank);
        }

        [OnEventFire]
        public void UpdateRank(UpdateRankEvent e, [Combine] SelfUserRankNode userRank, [Combine, JoinAll] SingleNode<SelfUserAvatarComponent> selfUserAvatar)
        {
            selfUserAvatar.component.SetRank(userRank.userRank.Rank);
        }

        [OnEventFire]
        public void UpdateRank(UpdateRankEvent e, UserRankNode userRank, [JoinByUser, Combine] RankIconNode rankIcon)
        {
            rankIcon.rankIcon.SetRank(userRank.userRank.Rank);
        }

        public class RankIconNode : Node
        {
            public RankIconComponent rankIcon;
            public UserGroupComponent userGroup;
        }

        public class SelfUserRankNode : RankIconSystem.UserRankNode
        {
            public SelfUserComponent selfUser;
        }

        public class UserRankNode : Node
        {
            public UserComponent user;
            public UserRankComponent userRank;
            public UserGroupComponent userGroup;
        }
    }
}

