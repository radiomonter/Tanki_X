namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class TransformTimeSmoothingDataComponent : Component
    {
        public Vector3 LastPosition { get; set; }

        public Quaternion LastRotation { get; set; }

        public float LerpFactor { get; set; }

        public float LastRotationDeltaAngle { get; set; }
    }
}

