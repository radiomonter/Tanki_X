namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15eeccf7c02L)]
    public class InviteToSquadEvent : Event
    {
        public long[] InvitedUsersIds { get; set; }
    }
}

