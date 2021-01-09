namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class BulletTargetingComponent : Component
    {
        public float Radius { get; set; }

        public float RadialRaysCount { get; set; }
    }
}

