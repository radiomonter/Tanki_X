namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15ca0fde322L)]
    public class AcceptInviteEvent : Event
    {
        public long lobbyId { get; set; }

        public long engineId { get; set; }
    }
}

