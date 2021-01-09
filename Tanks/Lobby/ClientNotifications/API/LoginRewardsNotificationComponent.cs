namespace Tanks.Lobby.ClientNotifications.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x162b318fcc7L)]
    public class LoginRewardsNotificationComponent : Component
    {
        public Dictionary<long, int> Reward { get; set; }

        public List<LoginRewardItem> AllReward { get; set; }

        public int CurrentDay { get; set; }
    }
}

