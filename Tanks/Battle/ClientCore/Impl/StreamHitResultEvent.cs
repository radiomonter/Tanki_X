namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;

    public class StreamHitResultEvent : Event
    {
        public StreamHitResultEvent()
        {
        }

        public StreamHitResultEvent(Tanks.Battle.ClientCore.API.TargetingData targetingData)
        {
            this.TargetingData = targetingData;
        }

        public Tanks.Battle.ClientCore.API.TargetingData TargetingData { get; set; }
    }
}

