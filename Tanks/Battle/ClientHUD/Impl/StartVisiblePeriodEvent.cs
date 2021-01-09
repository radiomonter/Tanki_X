namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class StartVisiblePeriodEvent : Event
    {
        public StartVisiblePeriodEvent()
        {
        }

        public StartVisiblePeriodEvent(float durationInSec)
        {
            this.DurationInSec = durationInSec;
        }

        public float DurationInSec { get; set; }
    }
}

