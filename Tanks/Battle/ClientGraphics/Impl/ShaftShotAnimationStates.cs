namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class ShaftShotAnimationStates
    {
        public class ShaftShotAnimationBounceState : Node
        {
            public ShaftShotAnimationBounceStateComponent shaftShotAnimationBounceState;
        }

        public class ShaftShotAnimationCooldownState : Node
        {
            public ShaftShotAnimationCooldownStateComponent shaftShotAnimationCooldownState;
        }

        public class ShaftShotAnimationIdleState : Node
        {
            public ShaftShotAnimationIdleStateComponent shaftShotAnimationIdleState;
        }
    }
}

