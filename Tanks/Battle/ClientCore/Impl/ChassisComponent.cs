namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class ChassisComponent : Component
    {
        public void Reset()
        {
            this.MoveAxis = 0f;
            this.TurnAxis = 0f;
            this.EffectiveMoveAxis = 0f;
            this.SpringCoeff = 0f;
        }

        public float MoveAxis { get; set; }

        public float TurnAxis { get; set; }

        public float EffectiveMoveAxis { get; set; }

        public float EffectiveTurnAxis { get; set; }

        public float SpringCoeff { get; set; }
    }
}

