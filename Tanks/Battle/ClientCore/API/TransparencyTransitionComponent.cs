namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class TransparencyTransitionComponent : Component
    {
        public float TransparencyTransitionTime { get; set; }

        public float TargetAlpha { get; set; }

        public float OriginAlpha { get; set; }

        public float CurrentAlpha { get; set; }

        public float CurrentTransitionTime { get; set; }

        public float AlphaSpeed { get; set; }
    }
}

