namespace Tanks.Lobby.ClientNotifications.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x8d40c885f6f1945L)]
    public class UserRankRewardNotificationInfoComponent : Component
    {
        public long RedCrystals { get; set; }

        public long BlueCrystals { get; set; }

        public long Rank { get; set; }
    }
}

