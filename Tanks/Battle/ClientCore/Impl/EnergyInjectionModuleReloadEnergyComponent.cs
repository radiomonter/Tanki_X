namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x8d4d4e04e8f5c52L)]
    public class EnergyInjectionModuleReloadEnergyComponent : Component
    {
        public float ReloadEnergyPercent { get; set; }
    }
}

