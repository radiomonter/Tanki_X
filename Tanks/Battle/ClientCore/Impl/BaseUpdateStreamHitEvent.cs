namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;

    public class BaseUpdateStreamHitEvent : Event
    {
        public BaseUpdateStreamHitEvent()
        {
        }

        public BaseUpdateStreamHitEvent(HitTarget tankHit, Tanks.Battle.ClientCore.API.StaticHit staticHit)
        {
            this.TankHit = tankHit;
            this.StaticHit = staticHit;
        }

        [ProtocolOptional]
        public HitTarget TankHit { get; set; }

        [ProtocolOptional]
        public Tanks.Battle.ClientCore.API.StaticHit StaticHit { get; set; }
    }
}

