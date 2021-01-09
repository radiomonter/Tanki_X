namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class UnitTargetingComponent : Component
    {
        public UnitTargetingComponent()
        {
            this.Period = 2f;
        }

        public float Period { get; set; }

        public ScheduledEvent UpdateEvent { get; set; }

        public float LastUpdateTime { get; set; }
    }
}

