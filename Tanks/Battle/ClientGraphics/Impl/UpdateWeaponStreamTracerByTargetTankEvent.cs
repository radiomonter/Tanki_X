namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class UpdateWeaponStreamTracerByTargetTankEvent : Event
    {
        public GameObject WeaponStreamTracerInstance { get; set; }

        public Tanks.Battle.ClientGraphics.Impl.WeaponStreamTracerBehaviour WeaponStreamTracerBehaviour { get; set; }

        public HitTarget Hit { get; set; }
    }
}

