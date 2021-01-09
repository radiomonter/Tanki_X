namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class TankInvisibilityEffectStates
    {
        public class TankInvisibilityEffectActivationState : Node
        {
            public TankInvisibilityEffectActivationStateComponent tankInvisibilityEffectActivationState;
        }

        public class TankInvisibilityEffectDeactivationState : Node
        {
            public TankInvisibilityEffectDeactivationStateComponent tankInvisibilityEffectDeactivationState;
        }

        public class TankInvisibilityEffectIdleState : Node
        {
            public TankInvisibilityEffectIdleStateComponent tankInvisibilityEffectIdleState;
        }

        public class TankInvisibilityEffectWorkingState : Node
        {
            public TankInvisibilityEffectWorkingStateComponent tankInvisibilityEffectWorkingState;
        }
    }
}

