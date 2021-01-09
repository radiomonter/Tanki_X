namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    [SkipAutoRemove]
    public class TankCollisionDetectionComponent : Component
    {
        public TankCollisionDetectionComponent()
        {
            this.Phase = -1L;
        }

        public long Phase { get; set; }

        public TankCollisionDetector Detector { get; set; }
    }
}

