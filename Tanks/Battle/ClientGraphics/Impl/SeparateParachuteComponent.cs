namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class SeparateParachuteComponent : Component
    {
        public float parachuteFoldingScaleByY { get; set; }

        public float parachuteFoldingScaleByXZ { get; set; }
    }
}

