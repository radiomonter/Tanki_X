namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x16759f905b3L)]
    public class ExplosiveMassEffectComponent : Component
    {
        public float Radius { get; set; }

        public long Delay { get; set; }
    }
}

