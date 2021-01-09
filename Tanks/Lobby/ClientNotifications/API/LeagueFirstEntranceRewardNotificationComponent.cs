namespace Tanks.Lobby.ClientNotifications.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15e9effb9baL)]
    public class LeagueFirstEntranceRewardNotificationComponent : Component
    {
        public Dictionary<long, int> Reward { get; set; }
    }
}

