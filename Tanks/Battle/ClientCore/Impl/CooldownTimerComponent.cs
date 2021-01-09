namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class CooldownTimerComponent : Component
    {
        public float CooldownTimerSec { get; set; }
    }
}

