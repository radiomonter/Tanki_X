namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class ShaftAimingRendererQueueMapComponent : Component
    {
        public ShaftAimingRendererQueueMapComponent()
        {
            this.QueueMap = new Dictionary<Material, int>();
        }

        public Dictionary<Material, int> QueueMap { get; set; }
    }
}

