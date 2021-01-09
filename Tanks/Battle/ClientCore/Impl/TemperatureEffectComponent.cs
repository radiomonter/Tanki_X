namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x15a1ddc2c2eL)]
    public class TemperatureEffectComponent : Component
    {
        public float Factor { get; set; }
    }
}

