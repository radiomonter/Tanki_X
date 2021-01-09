namespace Tanks.Lobby.ClientFriends.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientFriends.API;
    using Tanks.Lobby.ClientUserProfile.API;
    using UnityEngine;

    public class InviteFriendsPopupSystem : ECSSystem
    {
        [OnEventFire]
        public void CreateUIsForFriends(SortedFriendsIdsWithNicknamesLoaded e, SingleNode<ClientSessionComponent> session, [JoinAll] InviteFriendsUINode inviteFriendsUi)
        {
            inviteFriendsUi.inviteFriendsUi.AddFriends(e.FriendsIdsAndNicknames, FriendType.None);
        }

        [OnEventFire]
        public void OnItemClick(UserLabelClickEvent e, UserLabelNode userLabel, [JoinAll] SingleNode<InviteFriendsPopupComponent> friendsPopup)
        {
        }

        [OnEventFire]
        public void RequestSortedFriends(NodeAddedEvent e, InviteFriendsUINode inviteFriendsUi, [JoinAll] SingleNode<ClientSessionComponent> session)
        {
            base.ScheduleEvent<LoadSortedFriendsIdsWithNicknamesEvent>(session);
        }

        [OnEventFire]
        public void ShowInvitePopup(ButtonClickEvent e, SingleNode<InviteFriendsButtonComponent> button, [JoinAll] SingleNode<InviteFriendsPopupComponent> friendsPopup)
        {
            friendsPopup.component.ShowInvite(button.component.PopupPosition, new Vector2(0.5f, 0f), InviteMode.Lobby);
        }

        [Inject]
        public static EngineServiceInternal EngineService { get; set; }

        public class FriendsStorageNode : Node
        {
            public FriendsComponent friends;
        }

        public class InviteFriendsUINode : Node
        {
            public InviteFriendsUIComponent inviteFriendsUi;
        }

        public class UserLabelNode : Node
        {
            public UserLabelComponent userLabel;
        }
    }
}

