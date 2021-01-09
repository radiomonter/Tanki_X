namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Linq;
    using Tanks.Lobby.ClientBattleSelect.API;
    using Tanks.Lobby.ClientUserProfile.API;
    using Tanks.Lobby.ClientUserProfile.Impl;
    using UnityEngine;

    public class ScoreTableUserLabelIndicatorSystem : ECSSystem
    {
        [OnEventFire]
        public void SetUserLabel(NodeAddedEvent e, UserNode user, [Context, JoinByUser] UserLabelIndicatorNode userLabelIndicator, [JoinByScoreTable] ScoreTableNode scoreTable)
        {
            GameObject userLabel = userLabelIndicator.scoreTableUserLabelIndicator.userLabel;
            bool premium = user.Entity.HasComponent<PremiumAccountBoostComponent>();
            UserLabelBuilder builder = new UserLabelBuilder(user.Entity.Id, userLabel, user.userAvatar.Id, premium);
            LeagueNode node = base.Select<LeagueNode>(user.Entity, typeof(LeagueGroupComponent)).FirstOrDefault<LeagueNode>();
            if (node != null)
            {
                builder.SetLeague(node.leagueConfig.LeagueIndex);
            }
            builder.SkipLoadUserFromServer();
            if (scoreTable.scoreTableUserAvatar.EnableShowUserProfileOnAvatarClick)
            {
                builder.SubscribeAvatarClick();
            }
            builder.Build();
        }

        public class LeagueNode : Node
        {
            public LeagueConfigComponent leagueConfig;
            public LeagueGroupComponent leagueGroup;
        }

        public class ScoreTableNode : Node
        {
            public ScoreTableComponent scoreTable;
            public ScoreTableGroupComponent scoreTableGroup;
            public ScoreTableUserAvatarComponent scoreTableUserAvatar;
        }

        public class UserLabelIndicatorNode : Node
        {
            public ScoreTableUserLabelIndicatorComponent scoreTableUserLabelIndicator;
            public UserGroupComponent userGroup;
        }

        public class UserNode : Node
        {
            public UserGroupComponent userGroup;
            public UserUidComponent userUid;
            public UserAvatarComponent userAvatar;
        }
    }
}

