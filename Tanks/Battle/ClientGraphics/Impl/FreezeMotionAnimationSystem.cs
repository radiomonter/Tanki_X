namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;

    public class FreezeMotionAnimationSystem : ECSSystem
    {
        [OnEventFire]
        public void Init(NodeAddedEvent evt, InitialFreezeMotionAnimationNode weapon)
        {
            weapon.freezeMotionAnimation.Init(weapon.animation.Animator, weapon.streamWeaponEnergy.ReloadEnergyPerSec);
            weapon.Entity.AddComponent<FreezeMotionAnimationReadyComponent>();
        }

        [OnEventFire]
        public void Init(NodeAddedEvent evt, ActiveTankNode tank, [Context, JoinByTank] ReadyFreezeMotionAnimationNode weapon)
        {
            weapon.freezeMotionAnimation.ResetMotion();
        }

        [OnEventFire]
        public void StopMotion(NodeRemoveEvent evt, ActiveTankNode tank, [JoinByTank] ReadyFreezeMotionAnimationNode weapon)
        {
            weapon.freezeMotionAnimation.StopMotion();
        }

        [OnEventFire]
        public void SwitchState(NodeAddedEvent evt, IdleFreezeMotionAnimationNode weapon)
        {
            weapon.freezeMotionAnimation.StartIdle(weapon.weaponEnergy.Energy);
        }

        [OnEventFire]
        public void SwitchState(NodeAddedEvent evt, WorkingFreezeMotionAnimationNode weapon)
        {
            weapon.freezeMotionAnimation.StartWorking(weapon.weaponEnergy.Energy);
        }

        public class ActiveTankNode : Node
        {
            public TankActiveStateComponent tankActiveState;
            public TankGroupComponent tankGroup;
        }

        public class IdleFreezeMotionAnimationNode : Node
        {
            public FreezeMotionAnimationComponent freezeMotionAnimation;
            public FreezeMotionAnimationReadyComponent freezeMotionAnimationReady;
            public AnimationComponent animation;
            public AnimationPreparedComponent animationPrepared;
            public StreamWeaponIdleComponent streamWeaponIdle;
            public WeaponEnergyComponent weaponEnergy;
        }

        public class InitialFreezeMotionAnimationNode : Node
        {
            public FreezeMotionAnimationComponent freezeMotionAnimation;
            public AnimationComponent animation;
            public AnimationPreparedComponent animationPrepared;
            public StreamWeaponEnergyComponent streamWeaponEnergy;
        }

        public class ReadyFreezeMotionAnimationNode : Node
        {
            public TankGroupComponent tankGroup;
            public FreezeMotionAnimationComponent freezeMotionAnimation;
            public FreezeMotionAnimationReadyComponent freezeMotionAnimationReady;
            public AnimationComponent animation;
            public AnimationPreparedComponent animationPrepared;
            public StreamWeaponEnergyComponent streamWeaponEnergy;
        }

        public class WorkingFreezeMotionAnimationNode : Node
        {
            public FreezeMotionAnimationComponent freezeMotionAnimation;
            public FreezeMotionAnimationReadyComponent freezeMotionAnimationReady;
            public AnimationComponent animation;
            public AnimationPreparedComponent animationPrepared;
            public StreamWeaponWorkingComponent streamWeaponWorking;
            public WeaponEnergyComponent weaponEnergy;
        }
    }
}

