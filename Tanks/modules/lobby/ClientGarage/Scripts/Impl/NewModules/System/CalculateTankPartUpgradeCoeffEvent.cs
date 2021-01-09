namespace tanks.modules.lobby.ClientGarage.Scripts.Impl.NewModules.System
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class CalculateTankPartUpgradeCoeffEvent : Event
    {
        public float UpgradeCoeff { get; set; }
    }
}

