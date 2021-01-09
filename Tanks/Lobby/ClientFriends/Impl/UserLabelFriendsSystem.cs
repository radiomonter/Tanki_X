namespace Tanks.Lobby.ClientFriends.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Lobby.ClientFriends.API;
    using Tanks.Lobby.ClientUserProfile.API;

    public class UserLabelFriendsSystem : ECSSystem
    {
        [OnEventFire]
        public void DisableInBattleIcon(NodeRemoveEvent e, UserInBattleNode user, [JoinByUser, Combine] UserLabelNode userLabel)
        {
            if (userLabel.userLabel.AllowInBattleIcon && (userLabel.userLabel.inBattleIcon != null))
            {
                userLabel.userLabel.inBattleIcon.SetActive(false);
            }
        }

        [OnEventFire]
        public void DisableInBattleState(NodeRemoveEvent e, UserInBattleNode user, [JoinByUser, Context, Combine] UserLabelStateNode userLabel)
        {
            userLabel.userLabelState.UserOutBattle(user.Entity.HasComponent<UserOnlineComponent>());
        }

        [OnEventFire]
        public void EnableInBattleIcon(NodeAddedEvent e, UserInBattleNode user, [JoinByUser, Context, Combine] UserLabelNode userLabel)
        {
            if (userLabel.userLabel.AllowInBattleIcon && (userLabel.userLabel.inBattleIcon != null))
            {
                userLabel.userLabel.inBattleIcon.SetActive(true);
            }
        }

        [OnEventFire]
        public void EnableInBattleState(NodeAddedEvent e, UserInBattleNode user, [JoinByUser, Context, Combine] UserLabelStateNode userLabel)
        {
            userLabel.userLabelState.UserInBattle();
            if (userLabel.Entity.HasComponent<BattleGroupComponent>())
            {
                userLabel.Entity.RemoveComponent<BattleGroupComponent>();
            }
            user.battleGroup.Attach(userLabel.Entity);
        }

        [OnEventFire]
        public void HideIncomingFriendButtons(NodeAddedEvent e, AcceptedFriendNode friend, [JoinByUser, Context, Combine] FriendLabelNode userLabel)
        {
            userLabel.incomingFriendButtons.IsIncoming = false;
            userLabel.outgoingFriendButtons.IsOutgoing = false;
        }

        [OnEventFire]
        public void HiglightFriend(NodeAddedEvent e, AcceptedFriendNode user, [JoinByUser, Context, Combine] UserLabelWithHighlightningNode userLabel)
        {
            userLabel.uidIndicator.FontStyle = userLabel.uidHighlighting.friend;
        }

        [OnEventFire]
        public void RemoveHiglightning(NodeRemoveEvent e, UserFriendNode user, [JoinByUser, Context, Combine] UserLabelWithHighlightningNode userLabel)
        {
            userLabel.uidIndicator.FontStyle = userLabel.uidHighlighting.normal;
        }

        [OnEventFire]
        public void SetFriendColor(NodeAddedEvent e, AcceptedFriendNode user, [JoinByUser, Context, Combine] UserLabelNode userLabel)
        {
            userLabel.uidIndicator.Color = userLabel.uidColor.FriendColor;
        }

        [OnEventFire]
        public void SetNormalColor(NodeRemoveEvent e, AcceptedFriendNode user, [JoinByUser, Combine] UserLabelNode userLabel)
        {
            userLabel.uidIndicator.Color = userLabel.uidColor.Color;
        }

        [OnEventFire]
        public void ShowIncomingFriendButtons(NodeAddedEvent e, IncomingFriendNode friend, [JoinByUser, Context, Combine] FriendLabelNode userLabel)
        {
            userLabel.incomingFriendButtons.IsIncoming = true;
        }

        [OnEventFire]
        public void ShowOutgoingFriendButton(NodeAddedEvent e, OutgoingFriendNode friend, [JoinByUser, Context, Combine] FriendLabelNode userLabel)
        {
            userLabel.outgoingFriendButtons.IsOutgoing = true;
        }

        public class AcceptedFriendNode : UserLabelFriendsSystem.UserFriendNode
        {
            public AcceptedFriendComponent acceptedFriend;
        }

        public class FriendLabelNode : Node
        {
            public UserLabelComponent userLabel;
            public UserGroupComponent userGroup;
            public IncomingFriendButtonsComponent incomingFriendButtons;
            public OutgoingFriendButtonsComponent outgoingFriendButtons;
        }

        public class IncomingFriendNode : UserLabelFriendsSystem.UserFriendNode
        {
            public IncommingFriendComponent incommingFriend;
        }

        public class OutgoingFriendNode : UserLabelFriendsSystem.UserFriendNode
        {
            public OutgoingFriendComponent outgoingFriend;
        }

        public class UserFriendNode : Node
        {
            public UserComponent user;
            public UserGroupComponent userGroup;
        }

        public class UserInBattleNode : UserLabelFriendsSystem.UserFriendNode
        {
            public BattleGroupComponent battleGroup;
        }

        public class UserLabelNode : Node
        {
            public UserLabelComponent userLabel;
            public UidColorComponent uidColor;
            public UidIndicatorComponent uidIndicator;
            public UserGroupComponent userGroup;
        }

        public class UserLabelStateNode : Node
        {
            public UserLabelComponent userLabel;
            public UserLabelStateComponent userLabelState;
            public UserGroupComponent userGroup;
        }

        public class UserLabelWithHighlightningNode : UserLabelFriendsSystem.UserLabelNode
        {
            public UidHighlightingComponent uidHighlighting;
        }
    }
}

