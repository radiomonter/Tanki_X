namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x6ffe8cd421d15cbfL)]
    public class SelfShaftAimingHitEvent : SelfHitEvent
    {
        public SelfShaftAimingHitEvent()
        {
        }

        public SelfShaftAimingHitEvent(List<HitTarget> targets, StaticHit staticHit) : base(targets, staticHit)
        {
        }

        public float HitPower { get; set; }
    }
}

