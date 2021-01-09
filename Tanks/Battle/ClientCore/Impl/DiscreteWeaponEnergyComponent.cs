namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x14ed41478acL)]
    public class DiscreteWeaponEnergyComponent : Component
    {
        public float UnloadEnergyPerShot { get; set; }

        public float ReloadEnergyPerSec { get; set; }
    }
}

