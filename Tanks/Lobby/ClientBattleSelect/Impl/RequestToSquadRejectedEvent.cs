namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15f2e9cec15L)]
    public class RequestToSquadRejectedEvent : Event
    {
        public RejectRequestToSquadReason Reason { get; set; }

        public long RequestReceiverId { get; set; }
    }
}

