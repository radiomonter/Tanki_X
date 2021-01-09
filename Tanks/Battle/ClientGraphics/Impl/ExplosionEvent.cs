namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class ExplosionEvent : Event
    {
        public HitTarget Target;

        public GameObject Asset { get; set; }

        public Vector3 ExplosionOffset { get; set; }

        public Vector3 HitDirection { get; set; }

        public float Duration { get; set; }
    }
}

