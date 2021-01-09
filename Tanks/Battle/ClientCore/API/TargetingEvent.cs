﻿namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class TargetingEvent : Event
    {
        public TargetingEvent()
        {
        }

        public TargetingEvent(Tanks.Battle.ClientCore.API.TargetingData targetingData)
        {
            this.TargetingData = targetingData;
        }

        public TargetingEvent Init(Tanks.Battle.ClientCore.API.TargetingData targetingData)
        {
            this.TargetingData = targetingData;
            return this;
        }

        public Tanks.Battle.ClientCore.API.TargetingData TargetingData { get; set; }
    }
}
