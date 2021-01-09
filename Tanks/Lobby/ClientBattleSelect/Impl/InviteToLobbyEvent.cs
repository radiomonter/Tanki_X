namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15c8c4b7381L)]
    public class InviteToLobbyEvent : Event
    {
        public long[] InvitedUserIds { get; set; }
    }
}

