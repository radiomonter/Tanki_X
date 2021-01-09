namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class UpdateWeaponStreamHitGraphicsByTargetTankEvent : Event
    {
        public ParticleSystem HitTargetParticleSystem { get; set; }

        public Light HitTargetLight { get; set; }

        public HitTarget TankHit { get; set; }

        public float HitOffset { get; set; }
    }
}

