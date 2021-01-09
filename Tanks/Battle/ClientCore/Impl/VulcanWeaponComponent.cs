namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x3a63a79d3b453eeeL)]
    public class VulcanWeaponComponent : Component
    {
        public float SpeedUpTime { get; set; }

        public float SlowDownTime { get; set; }

        public float TemperatureIncreasePerSec { get; set; }

        public float TemperatureLimit { get; set; }

        public float TemperatureHittingTime { get; set; }

        public float WeaponTurnDecelerationCoeff { get; set; }

        public float TargetHeatingMult { get; set; }
    }
}

