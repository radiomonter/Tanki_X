namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15f004e3c0dL)]
    public class AcceptInviteToSquadEvent : Event
    {
        public long FromUserId { get; set; }

        public long EngineId { get; set; }
    }
}

