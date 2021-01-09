namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class TankFallingComponent : Component
    {
        public int PreviousTrackContactsCount { get; set; }

        public int PreviousCollisionContactsCount { get; set; }

        public bool IsGrounded { get; set; }

        public Vector3 PreviousVelocity { get; set; }
    }
}

