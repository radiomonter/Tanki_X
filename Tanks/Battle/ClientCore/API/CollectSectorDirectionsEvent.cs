namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class CollectSectorDirectionsEvent : Event
    {
        public CollectSectorDirectionsEvent Init()
        {
            this.TargetSectors = null;
            this.TargetingData = null;
            return this;
        }

        public ICollection<TargetSector> TargetSectors { get; set; }

        public Tanks.Battle.ClientCore.API.TargetingData TargetingData { get; set; }
    }
}

