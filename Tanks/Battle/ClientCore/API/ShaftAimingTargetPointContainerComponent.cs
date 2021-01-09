namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class ShaftAimingTargetPointContainerComponent : Component
    {
        public Vector3 Point { get; set; }

        public bool IsInsideTankPart { get; set; }

        public float PrevVerticalAngle { get; set; }
    }
}

