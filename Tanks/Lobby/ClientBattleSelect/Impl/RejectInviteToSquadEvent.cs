namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15fb92f6ddfL)]
    public class RejectInviteToSquadEvent : Event
    {
        public long FromUserId { get; set; }

        public long EngineId { get; set; }
    }
}

