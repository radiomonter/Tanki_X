namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x1685aab1a62L)]
    public class EnterBattleLobbyFailedEvent : Event
    {
        public bool AlreadyInLobby { get; set; }

        public bool LobbyIsFull { get; set; }
    }
}

