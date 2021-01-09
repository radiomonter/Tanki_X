namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class GetUserStatisticsInfoEvent : Event
    {
        public Dictionary<string, long> Statistics { get; set; }

        public Dictionary<long, long> FavoriteHullStatistics { get; set; }

        public Dictionary<long, long> FavoriteTurretStatistics { get; set; }

        public Dictionary<long, long> KillHullStatistics { get; set; }

        public Dictionary<long, long> KillTurretStatistics { get; set; }
    }
}

