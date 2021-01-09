namespace Tanks.Lobby.ClientFriends.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientFriends.API;
    using Tanks.Lobby.ClientUserProfile.API;
    using UnityEngine;

    public class FriendInteractionSystem : ECSSystem
    {
        [OnEventFire]
        public void RemoveFriend(RemoveFriendButtonEvent e, AcceptedFriendNode friend, [JoinAll] SingleNode<FriendsScreenComponent> friendsScreen, [JoinAll] SingleNode<SelfUserComponent> selfUser)
        {
            base.ScheduleEvent(new BreakOffFriendEvent(friend.Entity), selfUser);
            friendsScreen.component.RemoveUser(friend.Entity.Id, true);
        }

        [OnEventFire]
        public void ShowTooltipInLobby(RightMouseButtonClickEvent e, FriendLabelNode userButton, [JoinByUser] FriendNode friend, [JoinByUser] Optional<UserInBattleNode> userInBattle, [JoinAll] SingleNode<SelfUserComponent> selfUser)
        {
            bool flag = friend.Entity.HasComponent<AcceptedFriendComponent>();
            bool flag2 = selfUser.Entity.HasComponent<UserAdminComponent>();
            CheckForSpectatorButtonShowEvent eventInstance = new CheckForSpectatorButtonShowEvent();
            base.ScheduleEvent(eventInstance, friend);
            CheckForShowInviteToSquadEvent event3 = new CheckForShowInviteToSquadEvent();
            base.ScheduleEvent(event3, friend);
            FriendInteractionTooltipData data = new FriendInteractionTooltipData {
                FriendEntity = friend.Entity,
                ShowRemoveButton = flag,
                ShowEnterAsSpectatorButton = (userInBattle.IsPresent() && (flag || flag2)) && eventInstance.CanGoToSpectatorMode,
                ShowInviteToSquadButton = event3.ShowInviteToSquadButton,
                ActiveShowInviteToSquadButton = event3.ActiveInviteToSquadButton,
                ShowRequestToSquadButton = event3.ShowRequestToInviteToSquadButton,
                ShowChatButton = friend.Entity.HasComponent<UserOnlineComponent>()
            };
            TooltipController.Instance.ShowTooltip(Input.mousePosition, data, userButton.friendInteractionButton.tooltipPrefab, false);
        }

        public class AcceptedFriendNode : FriendInteractionSystem.FriendNode
        {
            public AcceptedFriendComponent acceptedFriend;
        }

        public class FriendLabelNode : Node
        {
            public UserLabelComponent userLabel;
            public UserGroupComponent userGroup;
            public IncomingFriendButtonsComponent incomingFriendButtons;
            public OutgoingFriendButtonsComponent outgoingFriendButtons;
            public FriendInteractionButtonComponent friendInteractionButton;
        }

        [Not(typeof(SelfUserComponent))]
        public class FriendNode : Node
        {
            public UserComponent user;
            public UserGroupComponent userGroup;
        }

        public class UserInBattleNode : FriendInteractionSystem.FriendNode
        {
            public BattleGroupComponent battleGroup;
        }
    }
}

