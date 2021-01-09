namespace Tanks.Lobby.ClientFriends.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientUserProfile.API;
    using UnityEngine;

    public class FriendsBuilderSystem : ECSSystem
    {
        [OnEventFire]
        public void CreateUIsForFriends(SortedFriendsIdsLoadedEvent e, SingleNode<ClientSessionComponent> session, [JoinByUser] SingleNode<FriendsComponent> friends, [JoinAll] SingleNode<FriendsListUIComponent> friendsScreen)
        {
            friendsScreen.component.AddFriends(e.friendsOutgoingIds, FriendType.Outgoing);
            friendsScreen.component.AddFriends(e.friendsAcceptedIds, FriendType.Accepted);
            friendsScreen.component.AddFriends(e.friendsIncomingIds, FriendType.Incoming);
        }

        [OnEventFire]
        public void RemoveFriendUI(NodeRemoveEvent e, SingleNode<FriendComponent> friend, [JoinByUser] SingleNode<FriendsListItemComponent> friendUI)
        {
            Object.Destroy(friendUI.component.gameObject);
        }

        [OnEventFire]
        public void RemoveOutdatedUI(NodeAddedEvent e, FriendUI newFriendUI, [Combine, JoinByUser] FriendUI oldFriendUI)
        {
            if (newFriendUI.Entity.Id != oldFriendUI.Entity.Id)
            {
                Object.Destroy(oldFriendUI.friendsListItem.gameObject);
            }
        }

        [OnEventFire]
        public void RequestSortedFriends(NodeAddedEvent e, SingleNode<FriendsListUIComponent> friendsScreen, [JoinAll] SingleNode<ClientSessionComponent> session)
        {
            base.ScheduleEvent<LoadSortedFriendsIdsEvent>(session);
        }

        public class FriendUI : Node
        {
            public FriendsListItemComponent friendsListItem;
            public UserGroupComponent userGroup;
        }

        public class FriendUIWithUserNode : FriendsBuilderSystem.FriendUI
        {
            public UserLoadedComponent userLoaded;
        }
    }
}

