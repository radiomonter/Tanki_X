namespace Tanks.Lobby.ClientMatchMaking.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15fdd87e907L)]
    public class SquadTryEnterToMatchMakingAfterEnergySharingEvent : Event
    {
        public long MatchMakingModeId { get; set; }
    }
}

