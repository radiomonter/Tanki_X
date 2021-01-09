namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15f0fdb81f4L)]
    public class RequestedToSquadEvent : Event
    {
        public string UserUid { get; set; }

        public long FromUserId { get; set; }

        public long SquadId { get; set; }

        public long EngineId { get; set; }
    }
}

