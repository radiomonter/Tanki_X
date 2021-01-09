namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class PerformanceStatisticsSettingsComponent : Component
    {
        public int DelayInSecBeforeMeasuringStarted { get; set; }

        public int HugeFrameDurationInMs { get; set; }

        public int MeasuringIntervalInSec { get; set; }
    }
}

