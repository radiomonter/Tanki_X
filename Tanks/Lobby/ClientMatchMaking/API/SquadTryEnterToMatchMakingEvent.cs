namespace Tanks.Lobby.ClientMatchMaking.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15f9ba664ebL)]
    public class SquadTryEnterToMatchMakingEvent : Event
    {
        public long MatchMakingModeId { get; set; }

        public bool RatingMatchMakingMode { get; set; }
    }
}

