namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class BonusDataComponent : Component
    {
        public float BoxHeight { get; set; }

        public float ParachuteHalfHeight { get; set; }

        public float SwingPivotY { get; set; }

        public Vector3 GroundPoint { get; set; }

        public Vector3 GroundPointNormal { get; set; }

        public Vector3 LandingPoint { get; set; }

        public Vector3 LandingAxis { get; set; }

        public float FallDuration { get; set; }

        public float AlignmentToGroundDuration { get; set; }
    }
}

