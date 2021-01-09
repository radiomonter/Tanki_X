namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x3c7261ac72bea885L)]
    public class MagazineWeaponComponent : Component
    {
        public float ReloadMagazineTimePerSec { get; set; }

        public int MaxCartridgeCount { get; set; }
    }
}

