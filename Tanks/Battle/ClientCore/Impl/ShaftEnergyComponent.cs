namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x19589f47dcb432b4L)]
    public class ShaftEnergyComponent : Component
    {
        public float UnloadEnergyPerQuickShot { get; set; }

        public float PossibleUnloadEnergyPerAimingShot { get; set; }

        public float UnloadAimingEnergyPerSec { get; set; }

        public float ReloadEnergyPerSec { get; set; }
    }
}

