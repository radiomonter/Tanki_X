namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x1626c565590L)]
    public class FavoriteEquipmentStatisticsComponent : Component
    {
        public Dictionary<long, long> HullStatistics { get; set; }

        public Dictionary<long, long> TurretStatistics { get; set; }
    }
}

