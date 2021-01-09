namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class CollectTargetSectorsEvent : Event
    {
        public CollectTargetSectorsEvent Init()
        {
            Tanks.Battle.ClientCore.API.TargetingCone cone = new Tanks.Battle.ClientCore.API.TargetingCone();
            this.TargetingCone = cone;
            this.TargetSectors = null;
            this.HAllowableAngleAcatter = 0f;
            this.VAllowableAngleAcatter = 0f;
            return this;
        }

        public Tanks.Battle.ClientCore.API.TargetingCone TargetingCone { get; set; }

        public ICollection<TargetSector> TargetSectors { get; set; }

        public float HAllowableAngleAcatter { get; set; }

        public float VAllowableAngleAcatter { get; set; }
    }
}

