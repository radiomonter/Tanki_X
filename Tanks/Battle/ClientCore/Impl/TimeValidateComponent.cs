namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class TimeValidateComponent : Component
    {
        public TimeValidateComponent()
        {
            this.Time = (int) (PreciseTime.Time * 1000.0);
        }

        public int Time { get; set; }
    }
}

