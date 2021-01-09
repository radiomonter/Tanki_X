namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class CollectTargetsInRadius : Event
    {
        public float Radius;

        public CollectTargetsInRadius()
        {
            this.Targets = new List<Entity>();
        }

        public List<Entity> Targets { get; set; }
    }
}

