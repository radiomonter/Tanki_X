namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15d0dd2f9e7L)]
    public class KillsEquipmentStatisticsComponent : Component
    {
        public Dictionary<long, long> HullStatistics { get; set; }

        public Dictionary<long, long> TurretStatistics { get; set; }
    }
}

