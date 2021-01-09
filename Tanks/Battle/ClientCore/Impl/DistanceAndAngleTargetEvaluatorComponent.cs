namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class DistanceAndAngleTargetEvaluatorComponent : Component
    {
        public float AngleWeight { get; set; }

        public float DistanceWeight { get; set; }
    }
}

