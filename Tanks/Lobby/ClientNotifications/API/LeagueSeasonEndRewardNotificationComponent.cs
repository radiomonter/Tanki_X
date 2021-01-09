namespace Tanks.Lobby.ClientNotifications.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15f48b0c55eL)]
    public class LeagueSeasonEndRewardNotificationComponent : Component
    {
        public int SeasonNumber { get; set; }

        public long LeagueId { get; set; }

        public Dictionary<long, int> Reward { get; set; }
    }
}

