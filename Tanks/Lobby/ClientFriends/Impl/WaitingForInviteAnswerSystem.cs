namespace Tanks.Lobby.ClientFriends.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientUserProfile.API;

    public class WaitingForInviteAnswerSystem : ECSSystem
    {
        [OnEventFire]
        public void AttachInviteButtonToUserGroup(NodeAddedEvent e, SingleNode<SendInviteToLobbyButtonComponent> button)
        {
            button.component.AttachToUserGroup();
        }

        [OnEventFire]
        public void AttachInviteButtonToUserGroup(NodeAddedEvent e, SingleNode<SendInviteToSquadButtonComponent> button)
        {
            button.component.AttachToUserGroup();
        }

        [OnEventFire]
        public void InitLabel(NodeAddedEvent e, InviteToLobbyUserLabelNode label, [JoinAll] SingleNode<FriendsComponent> friends)
        {
            if (friends.component.InLobbyInvitations.ContainsKey(label.userGroup.Key))
            {
                label.waitingForInviteToLobbyAnswerUi.SetTimer(friends.component.InLobbyInvitations[label.userGroup.Key]);
            }
            else
            {
                label.waitingForInviteToLobbyAnswerUi.Waiting = false;
            }
        }

        [OnEventFire]
        public void InitLabel(NodeAddedEvent e, InviteToSquadUserLabelNode label, [JoinAll] SingleNode<FriendsComponent> friends)
        {
            if (friends.component.InSquadInvitations.ContainsKey(label.userGroup.Key))
            {
                label.waitingForInviteToSquadAnswerUi.SetTimer(friends.component.InSquadInvitations[label.userGroup.Key]);
            }
            else
            {
                label.waitingForInviteToSquadAnswerUi.Waiting = false;
            }
        }

        [OnEventFire]
        public void SetWaitingTimer(NodeAddedEvent e, InvitedToLobbyUserNode user, [JoinByUser, Combine] InviteToLobbyUserLabelNode label, [JoinAll] SingleNode<FriendsComponent> friends)
        {
            label.waitingForInviteToLobbyAnswerUi.SetTimer(friends.component.InLobbyInvitations[user.userGroup.Key]);
        }

        [OnEventFire]
        public void SetWaitingTimer(NodeAddedEvent e, InvitedToSquadUserNode user, [JoinByUser, Combine] InviteToSquadUserLabelNode label, [JoinAll] SingleNode<FriendsComponent> friends)
        {
            label.waitingForInviteToSquadAnswerUi.SetTimer(friends.component.InSquadInvitations[user.userGroup.Key]);
        }

        public class InvitedToLobbyUserNode : WaitingForInviteAnswerSystem.UserNode
        {
            public InvitedToLobbyUserComponent invitedToLobbyUser;
        }

        public class InvitedToSquadUserNode : WaitingForInviteAnswerSystem.UserNode
        {
            public InvitedToSquadUserComponent invitedToSquadUser;
        }

        public class InviteToLobbyUserLabelNode : Node
        {
            public UserLabelComponent userLabel;
            public UserGroupComponent userGroup;
            public WaitingForInviteToLobbyAnswerUIComponent waitingForInviteToLobbyAnswerUi;
        }

        public class InviteToSquadUserLabelNode : Node
        {
            public UserLabelComponent userLabel;
            public UserGroupComponent userGroup;
            public WaitingForInviteToSquadAnswerUIComponent waitingForInviteToSquadAnswerUi;
        }

        public class UserNode : Node
        {
            public UserComponent user;
            public UserGroupComponent userGroup;
        }
    }
}

