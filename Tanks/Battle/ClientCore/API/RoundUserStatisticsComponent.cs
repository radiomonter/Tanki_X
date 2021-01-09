namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x600f09a7da580dd9L)]
    public class RoundUserStatisticsComponent : Component, IComparable<RoundUserStatisticsComponent>
    {
        public int CompareTo(RoundUserStatisticsComponent other) => 
            this.Place.CompareTo(other.Place);

        public int Place { get; set; }

        public int ScoreWithoutBonuses { get; set; }

        public int Kills { get; set; }

        public int KillAssists { get; set; }

        public int Deaths { get; set; }
    }
}

