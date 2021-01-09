namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using UnityEngine;

    public class ShaftAimingAnimationSystem : ECSSystem
    {
        [OnEventFire]
        public void InitShaftAimingAnimation(NodeAddedEvent evt, InitialShaftAimingAnimationNode weapon)
        {
            Animator animator = weapon.animation.Animator;
            weapon.shaftAimingAnimation.InitShaftAimingAnimation(animator);
            weapon.Entity.AddComponent<ShaftAimingAnimationReadyComponent>();
        }

        [OnEventFire]
        public void InitShaftAimingAnimation(NodeAddedEvent evt, ActiveTankNode tank, [JoinByTank] InititedShaftAimingAnimationNode weapon)
        {
            Animator animator = weapon.animation.Animator;
            weapon.shaftAimingAnimation.InitShaftAimingAnimation(animator);
        }

        [OnEventFire]
        public void StartAiming(NodeAddedEvent evt, ShaftAimingWorkingStateNode weapon)
        {
            weapon.shaftAimingAnimation.StartAiming();
        }

        [OnEventFire]
        public void StopAiming(NodeRemoveEvent evt, ShaftAimingWorkingStateNode weapon)
        {
            weapon.shaftAimingAnimation.StopAiming();
        }

        public class ActiveTankNode : Node
        {
            public TankActiveStateComponent tankActiveState;
            public TankGroupComponent tankGroup;
        }

        public class InitialShaftAimingAnimationNode : Node
        {
            public ShaftAimingAnimationComponent shaftAimingAnimation;
            public AnimationComponent animation;
            public AnimationPreparedComponent animationPrepared;
        }

        public class InititedShaftAimingAnimationNode : Node
        {
            public ShaftAimingAnimationComponent shaftAimingAnimation;
            public AnimationComponent animation;
            public AnimationPreparedComponent animationPrepared;
            public ShaftAimingAnimationReadyComponent shaftAimingAnimationReady;
        }

        public class ShaftAimingWorkingStateNode : Node
        {
            public ShaftAimingWorkingStateComponent shaftAimingWorkingState;
            public ShaftAimingAnimationComponent shaftAimingAnimation;
            public ShaftAimingAnimationReadyComponent shaftAimingAnimationReady;
            public AnimationComponent animation;
            public AnimationPreparedComponent animationPrepared;
        }
    }
}

