﻿namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class StartHealingGraphicsEffectEvent : Event
    {
        public StartHealingGraphicsEffectEvent(float duration)
        {
            this.Duration = duration;
        }

        public float Duration { get; set; }
    }
}

