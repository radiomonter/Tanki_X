namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class PerformanceStatisticsHelperComponent : Component
    {
        public float startRoundTimeInSec;
        public FramesCollection frames;
        public StatisticCollection tankCount;
    }
}

