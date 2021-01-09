namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x24d66160c6bae21eL)]
    public class RailgunChargingWeaponComponent : Component
    {
        public float ChargingTime { get; set; }
    }
}

