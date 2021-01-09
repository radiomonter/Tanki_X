namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15d0dc75527L)]
    public class UserStatisticsComponent : Component
    {
        public Dictionary<string, long> Statistics { get; set; }
    }
}

