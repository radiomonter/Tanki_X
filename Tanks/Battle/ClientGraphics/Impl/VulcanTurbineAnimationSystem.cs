namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using UnityEngine;

    public class VulcanTurbineAnimationSystem : ECSSystem
    {
        [OnEventFire]
        public void InitVulcanTurbineAnimation(NodeAddedEvent evt, InitialVulcanTurbineAnimationNode weapon)
        {
            Animator animator = weapon.animation.Animator;
            weapon.vulcanTurbineAnimation.Init(animator, weapon.vulcanWeapon.SpeedUpTime, weapon.vulcanWeapon.SlowDownTime);
            weapon.Entity.AddComponent<VulcanTurbineAnimationReadyComponent>();
        }

        [OnEventFire]
        public void StartShooting(NodeAddedEvent evt, VulcanShootingNode shootingState, [Context, JoinByTank] ReadyVulcanTurbineAnimationNode weapon, [Context, JoinByTank] ActiveTankNode tank)
        {
            weapon.vulcanTurbineAnimation.StartShooting();
        }

        [OnEventFire]
        public void StartSlowDown(NodeAddedEvent evt, VulcanSlowDownNode slowDownState, [Context, JoinByTank] ReadyVulcanTurbineAnimationNode weapon, [Context, JoinByTank] ActiveTankNode tank)
        {
            weapon.vulcanTurbineAnimation.StartSlowDown();
        }

        [OnEventFire]
        public void StartSpeedUp(NodeAddedEvent evt, VulcanSpeedUpNode speedUpState, [Context, JoinByTank] ReadyVulcanTurbineAnimationNode weapon, [Context, JoinByTank] ActiveTankNode tank)
        {
            weapon.vulcanTurbineAnimation.StartSpeedUp();
        }

        [OnEventFire]
        public void StopTurbine(NodeAddedEvent evt, VulcanIdleNode idle, [Context, JoinByTank] ReadyVulcanTurbineAnimationNode weapon, [Context, JoinByTank] ActiveTankNode tank)
        {
            weapon.vulcanTurbineAnimation.StopTurbine();
        }

        [OnEventFire]
        public void StopTurbineOnDeath(NodeRemoveEvent evt, ActiveTankNode tank, [JoinByTank] ReadyVulcanTurbineAnimationNode weapon)
        {
            weapon.vulcanTurbineAnimation.StartSlowDown();
        }

        public class ActiveTankNode : Node
        {
            public TankActiveStateComponent tankActiveState;
            public TankGroupComponent tankGroup;
        }

        public class InitialVulcanTurbineAnimationNode : Node
        {
            public VulcanWeaponComponent vulcanWeapon;
            public VulcanTurbineAnimationComponent vulcanTurbineAnimation;
            public AnimationComponent animation;
            public AnimationPreparedComponent animationPrepared;
            public TankGroupComponent tankGroup;
        }

        public class ReadyVulcanTurbineAnimationNode : VulcanTurbineAnimationSystem.InitialVulcanTurbineAnimationNode
        {
            public VulcanTurbineAnimationReadyComponent vulcanTurbineAnimationReady;
        }

        public class VulcanIdleNode : VulcanTurbineAnimationSystem.ReadyVulcanTurbineAnimationNode
        {
            public VulcanIdleComponent vulcanIdle;
        }

        public class VulcanShootingNode : VulcanTurbineAnimationSystem.ReadyVulcanTurbineAnimationNode
        {
            public WeaponStreamShootingComponent weaponStreamShooting;
        }

        public class VulcanSlowDownNode : VulcanTurbineAnimationSystem.ReadyVulcanTurbineAnimationNode
        {
            public VulcanSlowDownComponent vulcanSlowDown;
        }

        public class VulcanSpeedUpNode : VulcanTurbineAnimationSystem.ReadyVulcanTurbineAnimationNode
        {
            public VulcanSpeedUpComponent vulcanSpeedUp;
        }
    }
}

