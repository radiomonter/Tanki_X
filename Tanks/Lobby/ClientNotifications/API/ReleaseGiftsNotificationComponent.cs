namespace Tanks.Lobby.ClientNotifications.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x8d5881652575a1cL)]
    public class ReleaseGiftsNotificationComponent : Component
    {
        public Dictionary<long, int> Reward { get; set; }
    }
}

