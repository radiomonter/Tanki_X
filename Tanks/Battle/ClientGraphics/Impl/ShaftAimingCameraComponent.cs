namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class ShaftAimingCameraComponent : Component
    {
        public Vector3 WorldInitialCameraPosition { get; set; }

        public Quaternion WorldInitialCameraRotation { get; set; }

        public float InitialFOV { get; set; }
    }
}

