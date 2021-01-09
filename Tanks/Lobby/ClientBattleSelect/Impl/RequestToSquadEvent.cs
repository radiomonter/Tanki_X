namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15f0f75550aL)]
    public class RequestToSquadEvent : Event
    {
        public long ToUserId { get; set; }

        public long SquadId { get; set; }
    }
}

