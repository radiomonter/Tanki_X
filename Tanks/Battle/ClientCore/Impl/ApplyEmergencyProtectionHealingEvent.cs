namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x8d4d02feae98301L)]
    public class ApplyEmergencyProtectionHealingEvent : Event
    {
        public float FixedHealingAmount { get; set; }

        public float RelativeHealingAmount { get; set; }
    }
}

