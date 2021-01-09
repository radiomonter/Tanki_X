namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class ShaftAimingCameraKickbackComponent : Component
    {
        public ShaftAimingCameraKickbackComponent(Vector3 lastPosition, Quaternion lastRotation)
        {
            this.LastPosition = lastPosition;
            this.LastRotation = lastRotation;
        }

        public Vector3 LastPosition { get; set; }

        public Quaternion LastRotation { get; set; }
    }
}

