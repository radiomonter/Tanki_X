namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x14ed415d900L)]
    public class StreamWeaponEnergyComponent : Component
    {
        public float UnloadEnergyPerSec { get; set; }

        public float ReloadEnergyPerSec { get; set; }
    }
}

