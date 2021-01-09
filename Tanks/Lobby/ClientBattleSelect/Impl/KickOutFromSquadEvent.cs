namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15eecc29c61L)]
    public class KickOutFromSquadEvent : Event
    {
        public long KickedOutUserId { get; set; }
    }
}

