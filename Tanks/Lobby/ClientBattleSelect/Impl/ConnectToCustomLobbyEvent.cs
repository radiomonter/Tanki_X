namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x168552232a7L)]
    public class ConnectToCustomLobbyEvent : Event
    {
        public long LobbyId { get; set; }
    }
}

