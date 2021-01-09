namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class TankFallEvent : Event
    {
        public float FallingPower { get; set; }

        public TankFallingType FallingType { get; set; }

        public Transform FallingTransform { get; set; }

        public Vector3 Velocity { get; set; }
    }
}

