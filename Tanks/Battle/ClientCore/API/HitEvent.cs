namespace Tanks.Battle.ClientCore.API
{
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.Impl;

    public class HitEvent : TimeValidateEvent
    {
        [CompilerGenerated]
        private static Func<HitTarget, string> <>f__am$cache0;

        public HitEvent()
        {
            this.Targets = new List<HitTarget>();
        }

        public HitEvent(List<HitTarget> targets, Tanks.Battle.ClientCore.API.StaticHit staticHit)
        {
            this.Targets = targets;
            this.StaticHit = staticHit;
        }

        public override string ToString()
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = t => t.ToString();
            }
            return $"Targets: {string.Join(",", this.Targets.Select<HitTarget, string>(<>f__am$cache0).ToArray<string>())}, StaticHit: {this.StaticHit}";
        }

        [ProtocolOptional]
        public List<HitTarget> Targets { get; set; }

        [ProtocolOptional]
        public Tanks.Battle.ClientCore.API.StaticHit StaticHit { get; set; }

        public int ShotId { get; set; }
    }
}

