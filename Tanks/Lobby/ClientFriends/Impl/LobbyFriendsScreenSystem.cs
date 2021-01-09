namespace Tanks.Lobby.ClientFriends.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientFriends.API;
    using Tanks.Lobby.ClientNavigation.API;
    using Tanks.Lobby.ClientUserProfile.API;
    using Tanks.Lobby.ClientUserProfile.Impl;

    public class LobbyFriendsScreenSystem : ECSSystem
    {
        [OnEventFire]
        public void AcceptFriend(ButtonClickEvent e, SingleNode<AcceptFriendButtonComponent> button, [JoinAll] SelectedFriendUINode friendUI, [JoinByUser] SingleNode<IncommingFriendComponent> friend, [JoinAll] SingleNode<SelfUserComponent> user)
        {
            base.ScheduleEvent(new AcceptFriendEvent(friend.Entity), user);
        }

        [OnEventFire]
        public void AttachProfileScreenToUserGroup(UserProfileLoadedEvent e, RemoteUserNode remoteUser, [JoinAll] SingleNode<AddFriendDialogComponent> dialog, [JoinByScreen] SearchUserInputFieldNode searchField, [JoinByScreen] SingleNode<SearchUserButtonComponent> button, [JoinAll] SingleNode<SelfUserComponent> selfUser)
        {
            base.ScheduleEvent(new RequestFriendEvent(remoteUser.Entity), selfUser);
            base.ScheduleEvent<FriendRequestSentEvent>(selfUser);
            dialog.component.Hide();
        }

        [OnEventFire]
        public void PrepareNotificationText(NodeAddedEvent e, SingleNode<FriendSentNotificationComponent> notification, [JoinAll] SingleNode<SelfUserComponent> selfUser)
        {
            notification.Entity.AddComponent(new NotificationMessageComponent(notification.component.Message));
        }

        [OnEventFire]
        public void RejectFriend(ButtonClickEvent e, SingleNode<RejectFriendButtonComponent> button, [JoinAll] SelectedFriendUINode friendUI, [JoinByUser] SingleNode<IncommingFriendComponent> friend, [JoinAll] SingleNode<SelfUserComponent> user)
        {
            base.ScheduleEvent(new RejectFriendEvent(friend.Entity), user);
        }

        [OnEventFire]
        public void RemoveFriend(ConfirmButtonClickYesEvent e, SingleNode<BreakOffFriendButtonComponent> button, [JoinAll] SelectedFriendUINode friendUI, [JoinByUser] SingleNode<AcceptedFriendComponent> friend, [JoinAll] SingleNode<SelfUserComponent> user)
        {
            base.ScheduleEvent(new BreakOffFriendEvent(friend.Entity), user);
        }

        [OnEventFire]
        public void RequestUserId(ButtonClickEvent e, SingleNode<SearchUserButtonComponent> button, [JoinByScreen] SearchUserInputFieldNode searchField, [JoinAll] SingleNode<ClientSessionComponent> clientSession)
        {
            SearchUserIdByUidEvent eventInstance = new SearchUserIdByUidEvent {
                Uid = searchField.searchUserInputField.SearchString
            };
            base.ScheduleEvent(eventInstance, clientSession);
            button.component.gameObject.SetInteractable(false);
            searchField.esm.Esm.ChangeState<InputFieldStates.AwaitState>();
        }

        [OnEventFire]
        public void ResponseUserId(SerachUserIdByUidResultEvent e, SingleNode<ClientSessionComponent> clientSession, [JoinAll] SingleNode<AddFriendDialogComponent> dialog, [JoinByScreen] SearchUserInputFieldNode searchField, [JoinByScreen] SingleNode<SearchUserButtonComponent> button, [JoinAll] SingleNode<SelfUserComponent> selfUser)
        {
            button.component.gameObject.SetInteractable(true);
            if (e.Found)
            {
                base.ScheduleEvent(new RequestLoadUserProfileEvent(e.UserId), selfUser);
            }
            else
            {
                searchField.esm.Esm.ChangeState<InputFieldStates.InvalidState>();
            }
        }

        [OnEventFire]
        public void RevokeFriend(ButtonClickEvent e, SingleNode<RevokeFriendRequestButtonComponent> button, [JoinAll] SelectedFriendUINode friendUI, [JoinByUser] SingleNode<OutgoingFriendComponent> friend, [JoinAll] SingleNode<SelfUserComponent> user)
        {
            base.ScheduleEvent(new RevokeFriendEvent(friend.Entity), user);
        }

        [OnEventFire]
        public void ShowProfile(ButtonClickEvent e, SingleNode<FriendProfileButtonComponent> button, [JoinAll] SelectedFriendUINode friendUI, [JoinByUser] SingleNode<UserComponent> friend)
        {
            base.ScheduleEvent(new ShowProfileScreenEvent(friend.Entity.Id), friend.Entity);
        }

        [OnEventFire]
        public void ValidateSearchUserField(InputFieldValueChangedEvent e, SearchUserInputFieldNode searchField)
        {
            searchField.esm.Esm.ChangeState<InputFieldStates.NormalState>();
        }

        [Inject]
        public static EngineServiceInternal EngineService { get; set; }

        public class FriendUINode : Node
        {
            public UserGroupComponent userGroup;
            public FriendsListItemComponent friendsListItem;
            public UserLoadedComponent userLoaded;
        }

        [Not(typeof(SelfUserComponent))]
        public class RemoteUserNode : Node
        {
            public UserComponent user;
            public UserGroupComponent userGroup;
        }

        public class SearchUserInputFieldNode : Node
        {
            public InputFieldComponent inputField;
            public SearchUserInputFieldComponent searchUserInputField;
            public ESMComponent esm;
        }

        public class SelectedFriendUINode : LobbyFriendsScreenSystem.FriendUINode
        {
            public ToggleListSelectedItemComponent toggleListSelectedItem;
        }
    }
}

