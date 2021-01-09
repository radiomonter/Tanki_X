namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15f0fe1dfbfL)]
    public class AcceptRequestToSquadEvent : Event
    {
        public long FromUserId { get; set; }

        public long SquadId { get; set; }

        public long SquadEngineId { get; set; }
    }
}

