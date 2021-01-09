namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class TankOutlineMapEffectStates
    {
        public class ActivationState : Node
        {
            public TankOutlineMapEffectActivationStateComponent tankOutlineMapEffectActivationState;
        }

        public class BlinkerState : Node
        {
            public TankOutlineMapEffectBlinkerStateComponent tankOutlineMapEffectBlinkerState;
        }

        public class DeactivationState : Node
        {
            public TankOutlineMapEffectDeactivationStateComponent tankOutlineMapEffectDeactivationState;
        }

        public class IdleState : Node
        {
            public TankOutlineMapEffectIdleStateComponent tankOutlineMapEffectIdleState;
        }

        public class WorkingState : Node
        {
            public TankOutlineMapEffectWorkingStateComponent tankOutlineMapEffectWorkingState;
        }
    }
}

