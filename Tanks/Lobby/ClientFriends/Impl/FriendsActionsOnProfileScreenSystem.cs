namespace Tanks.Lobby.ClientFriends.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientFriends.API;
    using Tanks.Lobby.ClientNavigation.API;
    using Tanks.Lobby.ClientUserProfile.API;

    public class FriendsActionsOnProfileScreenSystem : ECSSystem
    {
        [OnEventFire]
        public void AcceptFriend(ButtonClickEvent e, SingleNode<AcceptFriendButtonComponent> button, [JoinByScreen] ProfileScreenWithUserGroupNode profileScreen, [JoinByUser] IncommingFriendNode incommingFriend, [JoinAll] SingleNode<SelfUserComponent> selfUser)
        {
            base.ScheduleEvent(new AcceptFriendEvent(incommingFriend.Entity), selfUser);
        }

        [OnEventFire]
        public void HideIncommingFriendButtons(NodeRemoveEvent e, IncommingFriendNode incommingFriend, [JoinByUser] ProfileScreenWithUserGroupNode profileScreen)
        {
            profileScreen.profileScreen.FriendRequestRow.SetActive(false);
            profileScreen.profileScreen.AddToFriendRow.SetActive(true);
        }

        [OnEventFire]
        public void HideOutgoingFriendButtons(NodeRemoveEvent e, OutgoingFriendNode outgoingFriend, [JoinByUser] ProfileScreenWithUserGroupNode profileScreen)
        {
            profileScreen.profileScreen.RevokeFriendRow.SetActive(false);
            profileScreen.profileScreen.AddToFriendRow.SetActive(true);
        }

        [OnEventFire]
        public void HideRemoveFriendButton(NodeRemoveEvent e, FriendUserNode friendUser, [JoinByUser] ProfileScreenWithUserGroupNode profileScreen)
        {
            profileScreen.profileScreen.RemoveFriendRow.SetActive(false);
            profileScreen.profileScreen.AddToFriendRow.SetActive(true);
        }

        [OnEventFire]
        public void RejectFriend(ButtonClickEvent e, SingleNode<RejectFriendButtonComponent> button, [JoinByScreen] ProfileScreenWithUserGroupNode profileScreen, [JoinByUser] IncommingFriendNode incommingFriend, [JoinAll] SingleNode<SelfUserComponent> selfUser)
        {
            base.ScheduleEvent(new RejectFriendEvent(incommingFriend.Entity), selfUser);
        }

        [OnEventFire]
        public void RemoveFriend(ButtonClickEvent e, SingleNode<BreakOffFriendButtonComponent> button, [JoinByScreen] ProfileScreenWithUserGroupNode profileScreen, [JoinByUser] FriendUserNode friend, [JoinAll] SingleNode<SelfUserComponent> selfUser)
        {
            base.ScheduleEvent(new BreakOffFriendEvent(friend.Entity), selfUser);
        }

        [OnEventFire]
        public void RequestFriend(ButtonClickEvent e, SingleNode<RequestFriendButtonComponent> button, [JoinByScreen] ProfileScreenWithUserGroupNode profileScreen, [JoinByUser] NotFriendUserNode notFriend, [JoinAll] SingleNode<SelfUserComponent> selfUser)
        {
            base.ScheduleEvent(new RequestFriendEvent(notFriend.Entity), selfUser);
        }

        [OnEventFire]
        public void RevokeFriend(ButtonClickEvent e, SingleNode<RevokeFriendRequestButtonComponent> button, [JoinByScreen] ProfileScreenWithUserGroupNode profileScreen, [JoinByUser] OutgoingFriendNode outgoingFriend, [JoinAll] SingleNode<SelfUserComponent> selfUser)
        {
            base.ScheduleEvent(new RevokeFriendEvent(outgoingFriend.Entity), selfUser);
        }

        [OnEventFire]
        public void ShowIncommingFriendButtons(NodeAddedEvent e, ProfileScreenWithUserGroupNode profileScreen, [Context, JoinByUser] IncommingFriendNode incommingFriend)
        {
            profileScreen.profileScreen.FriendRequestRow.SetActive(true);
            profileScreen.profileScreen.AddToFriendRow.SetActive(false);
        }

        [OnEventFire]
        public void ShowOutgoingFriendButtons(NodeAddedEvent e, ProfileScreenWithUserGroupNode profileScreen, [Context, JoinByUser] OutgoingFriendNode outgoingFriend)
        {
            profileScreen.profileScreen.RevokeFriendRow.SetActive(true);
            profileScreen.profileScreen.AddToFriendRow.SetActive(false);
        }

        [OnEventFire]
        public void ShowRemoveFriendButton(NodeAddedEvent e, ProfileScreenWithUserGroupNode profileScreen, [Context, JoinByUser] FriendUserNode friendUser)
        {
            profileScreen.profileScreen.RemoveFriendRow.SetActive(true);
            profileScreen.profileScreen.AddToFriendRow.SetActive(false);
        }

        [OnEventFire]
        public void ShowRequestFriendButtonIfNeed(NodeAddedEvent e, ProfileScreenWithUserGroupNode profileScreen, [JoinByUser] PossibleFriendNode possibleFriend, [JoinAll] SelfUserNode selfUser)
        {
            bool flag = (!selfUser.Entity.Equals(possibleFriend.Entity) && (!selfUser.friends.AcceptedFriendsIds.Contains(possibleFriend.Entity.Id) && !selfUser.friends.IncommingFriendsIds.Contains(possibleFriend.Entity.Id))) && !selfUser.friends.OutgoingFriendsIds.Contains(possibleFriend.Entity.Id);
            profileScreen.profileScreen.AddToFriendRow.SetActive(flag);
        }

        public class FriendUserNode : Node
        {
            public AcceptedFriendComponent acceptedFriend;
            public UserGroupComponent userGroup;
        }

        public class IncommingFriendNode : Node
        {
            public IncommingFriendComponent incommingFriend;
            public UserGroupComponent userGroup;
        }

        [Not(typeof(AcceptedFriendComponent))]
        public class NotFriendUserNode : Node
        {
            public UserComponent user;
            public UserGroupComponent userGroup;
        }

        public class OutgoingFriendNode : Node
        {
            public OutgoingFriendComponent outgoingFriend;
            public UserGroupComponent userGroup;
        }

        public class PossibleFriendNode : Node
        {
            public UserComponent user;
            public UserGroupComponent userGroup;
        }

        public class ProfileScreenWithUserGroupNode : Node
        {
            public ProfileScreenComponent profileScreen;
            public UserGroupComponent userGroup;
        }

        public class SelfUserNode : Node
        {
            public SelfUserComponent selfUser;
            public FriendsComponent friends;
            public UserGroupComponent userGroup;
        }
    }
}

