namespace Tanks.Lobby.ClientFriends.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientFriends.API;
    using Tanks.Lobby.ClientGarage.Impl;
    using Tanks.Lobby.ClientNavigation.API;
    using Tanks.Lobby.ClientUserProfile.API;
    using UnityEngine;

    public class FriendsScreenSystem : ECSSystem
    {
        [OnEventFire]
        public void AcceptAll(ButtonClickEvent e, SingleNode<AcceptAllFriendsButtonComponent> button, [JoinAll] SingleNode<SelfUserComponent> selfUser, [JoinAll] SingleNode<FriendsScreenComponent> friendsScreen)
        {
            base.ScheduleEvent<AcceptAllFriendsEvent>(selfUser);
            friendsScreen.component.ClearIncoming(true);
            friendsScreen.component.ResetButtons();
        }

        [OnEventFire]
        public void AcceptFriend(ButtonClickEvent e, SingleNode<AcceptFriendRequestButtonComponent> button, [JoinByUser] IncomingFriendNode incomingFriend, [JoinAll] SingleNode<SelfUserComponent> selfUser, [JoinAll] SingleNode<FriendsScreenComponent> friendsScreen, [JoinAll] ICollection<IncomingFriendNode> incomingFriends)
        {
            base.ScheduleEvent(new AcceptFriendEvent(incomingFriend.Entity), selfUser);
            friendsScreen.component.RemoveUser(incomingFriend.Entity.Id, false);
            friendsScreen.component.AddItem(incomingFriend.Entity.Id, incomingFriend.userUid.Uid, FriendType.Accepted);
            if (incomingFriends.Count > 1)
            {
                friendsScreen.component.AcceptFriend();
            }
            else
            {
                friendsScreen.component.ResetButtons();
            }
        }

        [OnEventFire]
        public void HideFriendsScreen(ButtonClickEvent e, SingleNode<HideFriendsScreenButtonComponent> node, [JoinAll] SingleNode<FriendsScreenComponent> friendsScreen)
        {
            friendsScreen.component.Hide();
        }

        [OnEventFire]
        public void HideScreen(ShowProfileScreenEvent e, Node any, [JoinAll] SingleNode<FriendsScreenComponent> friendsScreen)
        {
            friendsScreen.component.Hide();
        }

        [OnEventFire]
        public void HideWithMainScreen(NodeRemoveEvent e, SingleNode<MainScreenComponent> mainScreen, [JoinAll] SingleNode<FriendsScreenComponent> friendsScreen)
        {
            friendsScreen.component.HideImmediate();
        }

        [OnEventFire]
        public void MoveFriendsScreenToCanvas(NodeAddedEvent e, SingleNode<FriendsScreenComponent> friendsScreen, SingleNode<ScreensLayerComponent> layerNode)
        {
            GameObject gameObject = friendsScreen.component.transform.parent.gameObject;
            friendsScreen.component.transform.SetParent(layerNode.component.screens60Layer, false);
            Object.Destroy(gameObject);
        }

        [OnEventFire]
        public void OpenAddFriendDialog(ButtonClickEvent e, SingleNode<OpenAddFriendDialogButtonComponent> button, [JoinAll] SingleNode<Dialogs60Component> dialogs, [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens, [JoinAll] SingleNode<FriendsScreenComponent> friendsScreen)
        {
            friendsScreen.component.Hide();
            AddFriendDialogComponent component = dialogs.component.Get<AddFriendDialogComponent>();
            List<Animator> animators = new List<Animator>();
            if (screens.IsPresent())
            {
                animators = screens.Get().component.Animators;
            }
            component.Show(animators);
        }

        [OnEventFire]
        public void RejectAll(ButtonClickEvent e, SingleNode<RejectAllFriendsButtonComponent> button, [JoinAll] SingleNode<SelfUserComponent> selfUser, [JoinAll] SingleNode<FriendsScreenComponent> friendsScreen)
        {
            base.ScheduleEvent<RejectAllFriendsEvent>(selfUser);
            friendsScreen.component.ClearIncoming(false);
            friendsScreen.component.ResetButtons();
        }

        [OnEventFire]
        public void RejectFriend(ButtonClickEvent e, SingleNode<DeclineFriendRequestButtonComponent> button, [JoinByUser] IncomingFriendNode incommingFriend, [JoinAll] SingleNode<SelfUserComponent> selfUser, [JoinAll] SingleNode<FriendsScreenComponent> friendsScreen, [JoinAll] ICollection<IncomingFriendNode> incomingFriends)
        {
            base.ScheduleEvent(new RejectFriendEvent(incommingFriend.Entity), selfUser);
            friendsScreen.component.RemoveUser(incommingFriend.Entity.Id, true);
            friendsScreen.component.RejectFriend();
            if (incomingFriends.Count > 1)
            {
                friendsScreen.component.RejectFriend();
            }
            else
            {
                friendsScreen.component.ResetButtons();
            }
        }

        [OnEventFire]
        public void RevokeFriend(ButtonClickEvent e, SingleNode<RevokeFriendRequestButtonComponent> button, [JoinByUser] OutgoingFriendNode outgoingFriend, [JoinAll] SingleNode<SelfUserComponent> selfUser, [JoinAll] SingleNode<FriendsScreenComponent> friendsScreen)
        {
            base.ScheduleEvent(new RevokeFriendEvent(outgoingFriend.Entity), selfUser);
            friendsScreen.component.RemoveUser(outgoingFriend.Entity.Id, true);
        }

        [OnEventFire]
        public void ShowFriendsScreen(ButtonClickEvent e, SingleNode<OpenFriendsScreenButtonComponent> node, [JoinAll] SingleNode<FriendsScreenComponent> friendsScreen)
        {
            friendsScreen.component.Show();
        }

        [Inject]
        public static EngineServiceInternal EngineService { get; set; }

        [Not(typeof(UserInBattleAsSpectatorComponent))]
        public class FriendInBattle : Node
        {
            public AcceptedFriendComponent acceptedFriend;
            public UserGroupComponent userGroup;
            public BattleGroupComponent battleGroup;
        }

        public class FriendLabelNode : Node
        {
            public UserLabelComponent userLabel;
            public UserGroupComponent userGroup;
            public IncomingFriendButtonsComponent incomingFriendButtons;
            public OutgoingFriendButtonsComponent outgoingFriendButtons;
        }

        public class IncomingFriendNode : Node
        {
            public UserComponent user;
            public IncommingFriendComponent incommingFriend;
            public UserGroupComponent userGroup;
            public UserUidComponent userUid;
        }

        public class OutgoingFriendNode : Node
        {
            public UserComponent user;
            public OutgoingFriendComponent outgoingFriend;
            public UserGroupComponent userGroup;
        }

        public class SelectedFriendUI : Node
        {
            public SelectedListItemComponent selectedListItem;
            public FriendsListItemComponent friendsListItem;
            public UserGroupComponent userGroup;
        }
    }
}

