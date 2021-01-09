namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x16271a3ee62L)]
    public class UpdateTopLeagueInfoEvent : Event
    {
        public long UserId { get; set; }

        public double LastPlaceReputation { get; set; }

        public int Place { get; set; }
    }
}

