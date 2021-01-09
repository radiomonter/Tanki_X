namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x1672fce57e9L)]
    public class FireRingEffectComponent : Component
    {
        public long Duration { get; set; }

        public float TemperatureDelta { get; set; }

        public float TemperatureLimit { get; set; }
    }
}

