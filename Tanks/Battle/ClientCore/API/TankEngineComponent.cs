namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x8d2e6e10d37640aL)]
    public class TankEngineComponent : Component
    {
        public float MovingBorder { get; set; }

        public float Value { get; set; }

        public float CollisionTimerSec { get; set; }

        public bool HasValuableCollision { get; set; }
    }
}

