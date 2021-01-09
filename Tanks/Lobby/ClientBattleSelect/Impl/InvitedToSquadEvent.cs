namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15f009356c2L)]
    public class InvitedToSquadEvent : Event
    {
        public string UserUid { get; set; }

        public long FromUserId { get; set; }

        public long EngineId { get; set; }
    }
}

