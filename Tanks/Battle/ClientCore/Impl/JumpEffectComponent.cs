namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x166330aff16L)]
    public class JumpEffectComponent : Component
    {
        public float BaseImpact { get; set; }

        public float GravityPenalty { get; set; }

        public bool ScaleByMass { get; set; }

        public bool AlwaysUp { get; set; }

        public long FlyComponentDelayInMs { get; set; }
    }
}

