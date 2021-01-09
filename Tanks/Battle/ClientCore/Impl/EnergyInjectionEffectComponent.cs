namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x8d4d4d8f6d40f60L)]
    public class EnergyInjectionEffectComponent : Component
    {
        public float ReloadEnergyPercent { get; set; }
    }
}

