namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class EffectiveSpeedComponent : Component
    {
        public float MaxSpeed { get; set; }

        public float MaxTurnSpeed { get; set; }
    }
}

