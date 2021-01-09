namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [Shared, SerialVersionUID(0x1511e9c0ac1L)]
    public class DetachWeaponEvent : Event
    {
        public DetachWeaponEvent()
        {
        }

        public DetachWeaponEvent(Vector3 velocity, Vector3 angularVelocity)
        {
            this.Velocity = velocity;
            this.AngularVelocity = angularVelocity;
        }

        public Vector3 Velocity { get; set; }

        public Vector3 AngularVelocity { get; set; }
    }
}

