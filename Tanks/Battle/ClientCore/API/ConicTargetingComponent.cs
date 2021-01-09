namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class ConicTargetingComponent : Component
    {
        public float WorkDistance { get; set; }

        public float HalfConeAngle { get; set; }

        public int HalfConeNumRays { get; set; }

        public int NumSteps { get; set; }

        public float FireOriginOffsetCoeff { get; set; }
    }
}

