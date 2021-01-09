namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class HitTarget
    {
        public Platform.Kernel.ECS.ClientEntitySystem.API.Entity Entity { get; set; }

        public Platform.Kernel.ECS.ClientEntitySystem.API.Entity IncarnationEntity { get; set; }

        public Vector3 LocalHitPoint { get; set; }

        public Vector3 TargetPosition { get; set; }

        public float HitDistance { get; set; }

        public Vector3 HitDirection { get; set; }
    }
}

