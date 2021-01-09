namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientBattleSelect.API;
    using Tanks.Lobby.ClientEntrance.API;

    public class MatchLobbySoundSystem : ECSSystem
    {
        [OnEventFire]
        public void PlayLobbySound(NodeAddedEvent e, SelfUserInMatchMakingLobby user, [Context, JoinByBattleLobby] BattleLobbyNode battleLobby, [JoinAll] SingleNode<MainScreenComponent> mainScreen, [JoinAll] SingleNode<HangarMatchLobbySoundComponent> hangar)
        {
            hangar.component.Play();
        }

        public class BattleLobbyNode : Node
        {
            public BattleLobbyGroupComponent battleLobbyGroup;
            public BattleLobbyComponent battleLobby;
        }

        public class SelfUserInMatchMakingLobby : Node
        {
            public MatchMakingUserComponent matchMakingUser;
            public UserComponent user;
            public SelfUserComponent selfUser;
            public BattleLobbyGroupComponent battleLobbyGroup;
        }
    }
}

