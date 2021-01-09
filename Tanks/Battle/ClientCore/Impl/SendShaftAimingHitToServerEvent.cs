namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;

    public class SendShaftAimingHitToServerEvent : Event
    {
        public SendShaftAimingHitToServerEvent()
        {
        }

        public SendShaftAimingHitToServerEvent(Tanks.Battle.ClientCore.API.TargetingData targetingData)
        {
            this.TargetingData = targetingData;
        }

        public Tanks.Battle.ClientCore.API.TargetingData TargetingData { get; set; }
    }
}

