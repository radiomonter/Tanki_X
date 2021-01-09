namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;

    public class SendHitToServerEvent : Event
    {
        public SendHitToServerEvent()
        {
            this.Targets = new List<HitTarget>();
        }

        public SendHitToServerEvent(Tanks.Battle.ClientCore.API.TargetingData targetingData)
        {
            this.TargetingData = targetingData;
        }

        public SendHitToServerEvent(Tanks.Battle.ClientCore.API.TargetingData targetingData, List<HitTarget> targets, Tanks.Battle.ClientCore.API.StaticHit staticHit)
        {
            this.TargetingData = targetingData;
            this.Targets = targets;
            this.StaticHit = staticHit;
        }

        public Tanks.Battle.ClientCore.API.TargetingData TargetingData { get; set; }

        public List<HitTarget> Targets { get; set; }

        public Tanks.Battle.ClientCore.API.StaticHit StaticHit { get; set; }
    }
}

