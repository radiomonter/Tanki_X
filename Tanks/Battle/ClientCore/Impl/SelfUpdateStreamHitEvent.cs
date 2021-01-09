namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using Tanks.Battle.ClientCore.API;

    [Shared, SerialVersionUID(0x14cff3117f8L)]
    public class SelfUpdateStreamHitEvent : BaseUpdateStreamHitEvent
    {
        public SelfUpdateStreamHitEvent()
        {
        }

        public SelfUpdateStreamHitEvent(HitTarget tankHit, StaticHit staticHit) : base(tankHit, staticHit)
        {
        }
    }
}

