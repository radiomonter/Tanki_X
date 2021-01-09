namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;

    public class TankAnimationSystem : ECSSystem
    {
        private static readonly string INACTIVE_STATE_TAG = "Inaction";

        [OnEventFire]
        public void SetNewAnimationAsNotPrepared(NodeAddedEvent evt, [Combine] AnimationNode animationNode, [Context, JoinByTank] ActivatedTankNode tank)
        {
            animationNode.Entity.AddComponent<AnimationPreparedComponent>();
        }

        public class ActivatedTankNode : Node
        {
            public AssembledTankActivatedStateComponent assembledTankActivatedState;
            public TankGroupComponent tankGroup;
        }

        public class AnimationNode : Node
        {
            public AnimationComponent animation;
            public TankGroupComponent tankGroup;
        }

        public class PreparedAnimationNode : Node
        {
            public AnimationComponent animation;
            public AnimationPreparedComponent animationPrepared;
        }
    }
}

