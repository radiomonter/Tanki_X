namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15f0b8f14a1L)]
    public class ChangeSquadLeaderEvent : Event
    {
        public long NewLeaderUserId { get; set; }
    }
}

