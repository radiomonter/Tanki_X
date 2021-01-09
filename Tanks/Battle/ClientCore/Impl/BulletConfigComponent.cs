namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class BulletConfigComponent : Component
    {
        public float FullDistance { get; set; }

        public float RadialRaysCount { get; set; }
    }
}

