namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class BulletBuildEvent : Event
    {
        public BulletBuildEvent()
        {
        }

        public BulletBuildEvent(Vector3 direction)
        {
            this.Direction = direction;
        }

        public Vector3 Direction { get; set; }
    }
}

