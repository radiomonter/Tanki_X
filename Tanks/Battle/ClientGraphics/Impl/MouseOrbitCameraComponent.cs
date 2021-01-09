namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class MouseOrbitCameraComponent : Component
    {
        public float distance = MouseOrbitCameraConstants.DEFAULT_MOUSE_ORBIT_DISTANCE;

        public Quaternion targetRotation { get; set; }
    }
}

