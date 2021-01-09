namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class WaitingLobbyExitComponent : Component
    {
        public Tanks.Lobby.ClientBattleSelect.Impl.AcceptInviteEvent AcceptInviteEvent { get; set; }
    }
}

