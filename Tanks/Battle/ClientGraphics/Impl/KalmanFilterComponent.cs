namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class KalmanFilterComponent : Component
    {
        public KalmanFilter kalmanFilterPosition;
        public float kalmanUpdatePeriod = 0.033f;
        public float kalmanUpdateTimeAccumulator;
    }
}

