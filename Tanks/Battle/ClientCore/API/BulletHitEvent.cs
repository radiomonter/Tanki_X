namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public abstract class BulletHitEvent : Event
    {
        protected BulletHitEvent()
        {
        }

        public Vector3 Position { get; set; }
    }
}

