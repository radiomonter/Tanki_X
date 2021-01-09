namespace Tanks.Lobby.ClientPayment.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class LeagueFirstEntranceSpecialOfferComponent : Component
    {
        public long LeagueId { get; set; }

        public int WorthItPercent { get; set; }
    }
}

