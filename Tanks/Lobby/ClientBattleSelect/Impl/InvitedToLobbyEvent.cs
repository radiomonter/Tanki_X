namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15d114234adL)]
    public class InvitedToLobbyEvent : Event
    {
        public string userUid { get; set; }

        public long lobbyId { get; set; }

        public long engineId { get; set; }
    }
}

