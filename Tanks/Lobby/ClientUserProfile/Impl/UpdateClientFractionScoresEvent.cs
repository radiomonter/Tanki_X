namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x167cf58f137L)]
    public class UpdateClientFractionScoresEvent : Event
    {
        public long TotalCryFund { get; set; }

        public Dictionary<long, long> Scores { get; set; }
    }
}

