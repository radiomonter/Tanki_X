namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x8d2e6e14ed30b8aL)]
    public class StreamHitCheckingComponent : Component
    {
        public float LastSendToServerTime { get; set; }

        public float LastCheckTime { get; set; }

        public HitTarget LastSentTankHit { get; set; }

        public StaticHit LastSentStaticHit { get; set; }
    }
}

