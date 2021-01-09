namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x2bb4aed2fba9cceL)]
    public class SelfSplashHitEvent : SelfHitEvent
    {
        public SelfSplashHitEvent()
        {
        }

        public SelfSplashHitEvent(List<HitTarget> targets, StaticHit staticHit, List<HitTarget> splashTargets) : base(targets, staticHit)
        {
            this.SplashTargets = splashTargets;
        }

        [ProtocolOptional]
        public List<HitTarget> SplashTargets { get; set; }
    }
}

