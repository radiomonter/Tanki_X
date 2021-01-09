namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(-6274985110858845212L)]
    public class StreamHitComponent : Component
    {
        [ProtocolOptional]
        public HitTarget TankHit { get; set; }

        [ProtocolOptional]
        public Tanks.Battle.ClientCore.API.StaticHit StaticHit { get; set; }
    }
}

