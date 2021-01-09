namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientUnityIntegration.API;
    using Platform.System.Data.Exchange.ClientNetwork.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Lobby.ClientBattleSelect.API;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientFriends.API;
    using Tanks.Lobby.ClientFriends.Impl;
    using Tanks.Lobby.ClientUserProfile.API;

    public class EnterAsSpectatorSystem : ECSSystem
    {
        [OnEventFire]
        public void EnterAsSpectator(EnterAsSpectatorToFriendBattleEvent e, FriendInBattle friend, [JoinAll] ClientSessionNode session, [JoinAll] Optional<SelfBattleLobbyUser> selfBattleLobbyUser)
        {
            if (!selfBattleLobbyUser.IsPresent())
            {
                base.ScheduleEvent(new EnterBattleAsSpectatorFromLobbyRequestEvent(friend.battleGroup.Key), session);
            }
        }

        [OnEventFire]
        public void EnterAsSpectator(ButtonClickEvent e, SingleNode<EnterBattleAsSpectatorButtonComponent> button, [JoinAll] ProfileScreenWithUserGroupNode profileScreen, [JoinByUser] UserInBattleNode userInBattle, [JoinAll] ClientSessionNode session, [JoinAll] Optional<SelfBattleLobbyUser> selfBattleLobbyUser)
        {
            if (!selfBattleLobbyUser.IsPresent())
            {
                base.ScheduleEvent(new EnterBattleAsSpectatorFromLobbyRequestEvent(userInBattle.battleGroup.Key), session);
            }
        }

        [OnEventFire]
        public void EnterAsSpectator(ButtonClickEvent e, SingleNode<EnterBattleAsSpectatorButtonComponent> button, [JoinAll] SelectedFriendUI friendUI, [JoinByUser] FriendInBattle friend, [JoinAll] ClientSessionNode session, [JoinAll] Optional<SelfBattleLobbyUser> selfBattleLobbyUser)
        {
            if (!selfBattleLobbyUser.IsPresent())
            {
                base.ScheduleEvent(new EnterBattleAsSpectatorFromLobbyRequestEvent(friend.battleGroup.Key), session);
            }
        }

        private void HideSpectatorButton(ProfileScreenWithUserGroupNode profileScreen)
        {
            profileScreen.profileScreen.EnterBattleAsSpectatorRow.SetActive(false);
        }

        [OnEventFire]
        public void ProfileScreenLoadedWithUserInBattle(NodeAddedEvent e, ProfileScreenWithUserGroupNode profileScreen, [JoinByUser] UserInBattleNode userInBattle, [JoinAll] Optional<SelfBattleLobbyUser> selfBattleLobbyUser)
        {
            if (!selfBattleLobbyUser.IsPresent())
            {
                this.ShowSpectatorButton(profileScreen);
            }
        }

        [OnEventFire]
        public void SelfUserInLobby(NodeAddedEvent e, SelfBattleLobbyUser selfBattleLobbyUser, [JoinAll] ProfileScreenWithUserGroupNode profileScreen)
        {
            this.HideSpectatorButton(profileScreen);
        }

        private void ShowSpectatorButton(ProfileScreenWithUserGroupNode profileScreen)
        {
            profileScreen.profileScreen.EnterBattleAsSpectatorRow.SetActive(true);
        }

        [OnEventFire]
        public void UserInBattle(NodeAddedEvent e, UserInBattleNode userInBattle, [JoinByUser] ProfileScreenWithUserGroupNode profileScreen, [JoinAll] Optional<SelfBattleLobbyUser> selfBattleLobbyUser)
        {
            if (!selfBattleLobbyUser.IsPresent())
            {
                this.ShowSpectatorButton(profileScreen);
            }
        }

        [OnEventFire]
        public void UserOutBattle(NodeRemoveEvent e, UserInBattleNode userInBattle, [JoinByUser] ProfileScreenWithUserGroupNode profileScreen)
        {
            this.HideSpectatorButton(profileScreen);
        }

        public class ClientSessionNode : Node
        {
            public ClientSessionComponent clientSession;
            public UserGroupComponent userGroup;
        }

        [Not(typeof(UserInBattleAsSpectatorComponent))]
        public class FriendInBattle : Node
        {
            public UserGroupComponent userGroup;
            public BattleGroupComponent battleGroup;
        }

        public class ProfileScreenWithUserGroupNode : Node
        {
            public ProfileScreenComponent profileScreen;
            public UserGroupComponent userGroup;
            public ActiveScreenComponent activeScreen;
        }

        public class SelectedFriendUI : Node
        {
            public SelectedListItemComponent selectedListItem;
            public FriendsListItemComponent friendsListItem;
            public UserGroupComponent userGroup;
        }

        public class SelfBattleLobbyUser : Node
        {
            public UserComponent user;
            public SelfUserComponent selfUser;
            public BattleLobbyGroupComponent battleLobbyGroup;
        }

        public class UserInBattleNode : Node
        {
            public AcceptedFriendComponent acceptedFriend;
            public UserGroupComponent userGroup;
            public BattleGroupComponent battleGroup;
        }
    }
}

