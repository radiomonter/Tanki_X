namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class TargetingEvaluateEvent : Event
    {
        public TargetingEvaluateEvent()
        {
        }

        public TargetingEvaluateEvent(Tanks.Battle.ClientCore.API.TargetingData targetingData)
        {
            this.TargetingData = targetingData;
        }

        public TargetingEvaluateEvent Init(Tanks.Battle.ClientCore.API.TargetingData targetingData)
        {
            this.TargetingData = targetingData;
            return this;
        }

        public Tanks.Battle.ClientCore.API.TargetingData TargetingData { get; set; }
    }
}

