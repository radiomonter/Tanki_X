namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class RicochetBulletBounceEvent : Event
    {
        public RicochetBulletBounceEvent()
        {
        }

        public RicochetBulletBounceEvent(Vector3 worldSpaceBouncePosition)
        {
            this.WorldSpaceBouncePosition = worldSpaceBouncePosition;
        }

        public Vector3 WorldSpaceBouncePosition { get; set; }
    }
}

