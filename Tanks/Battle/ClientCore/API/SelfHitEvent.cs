namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x7a5450768d506df1L)]
    public class SelfHitEvent : HitEvent
    {
        [CompilerGenerated]
        private static Func<HitTarget, string> <>f__am$cache0;

        public SelfHitEvent()
        {
        }

        public SelfHitEvent(List<HitTarget> targets, StaticHit staticHit) : base(targets, staticHit)
        {
        }

        public override string ToString()
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = t => t.ToString();
            }
            return $"Targets: {string.Join(",", base.Targets.Select<HitTarget, string>(<>f__am$cache0).ToArray<string>())}, StaticHit: {base.StaticHit}";
        }
    }
}

