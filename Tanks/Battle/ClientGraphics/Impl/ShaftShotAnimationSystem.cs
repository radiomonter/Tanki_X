namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using UnityEngine;

    public class ShaftShotAnimationSystem : ECSSystem
    {
        [OnEventFire]
        public void InitShaftShotAnimation(NodeAddedEvent evt, InitialShaftShotAnimationNode weapon)
        {
            Animator animator = weapon.animation.Animator;
            ShaftEnergyComponent shaftEnergy = weapon.shaftEnergy;
            Entity entity = weapon.Entity;
            weapon.shaftShotAnimation.Init(animator, weapon.weaponCooldown.CooldownIntervalSec, shaftEnergy.UnloadEnergyPerQuickShot, shaftEnergy.ReloadEnergyPerSec, shaftEnergy.PossibleUnloadEnergyPerAimingShot);
            weapon.shaftShotAnimationTrigger.Entity = entity;
            ShaftShotAnimationESMComponent component = new ShaftShotAnimationESMComponent();
            entity.AddComponent(component);
            EntityStateMachine esm = component.Esm;
            esm.AddState<ShaftShotAnimationStates.ShaftShotAnimationIdleState>();
            esm.AddState<ShaftShotAnimationStates.ShaftShotAnimationBounceState>();
            esm.AddState<ShaftShotAnimationStates.ShaftShotAnimationCooldownState>();
            esm.ChangeState<ShaftShotAnimationStates.ShaftShotAnimationIdleState>();
        }

        [OnEventFire]
        public void PlayShot(BaseShotEvent evt, ReadyShaftShotAnimationNode weapon, [JoinByTank] ActiveTankNode tank)
        {
            weapon.shaftShotAnimation.PlayShot();
            weapon.shaftShotAnimationEsm.Esm.ChangeState<ShaftShotAnimationStates.ShaftShotAnimationBounceState>();
        }

        [OnEventFire]
        public void StartCooldown(ShaftShotAnimationCooldownStartEvent evt, ReadyShaftShotAnimationNode weapon, [JoinByTank] ActiveTankNode tank)
        {
            weapon.shaftShotAnimationEsm.Esm.ChangeState<ShaftShotAnimationStates.ShaftShotAnimationCooldownState>();
        }

        [OnEventFire]
        public void StopCooldown(ShaftShotAnimationCooldownEndEvent evt, ReadyShaftShotAnimationNode weapon, [JoinByTank] ActiveTankNode tank)
        {
            weapon.shaftShotAnimationEsm.Esm.ChangeState<ShaftShotAnimationStates.ShaftShotAnimationIdleState>();
        }

        [OnEventFire]
        public void StopShaftShotAnimtionEffect(NodeRemoveEvent evt, ActiveTankNode tank, [JoinByTank] ReadyShaftShotAnimationNode weapon)
        {
            weapon.shaftShotAnimationEsm.Esm.ChangeState<ShaftShotAnimationStates.ShaftShotAnimationIdleState>();
        }

        [OnEventFire]
        public void UpdateShotAnimationDataWithinCooldown(UpdateEvent evt, ShaftShotAnimationCooldownStateNode state, [JoinByTank] ReadyShaftShotAnimationNode weapon)
        {
            weapon.shaftShotAnimation.UpdateShotCooldownAnimation(evt.DeltaTime);
        }

        public class ActiveTankNode : Node
        {
            public TankComponent tank;
            public TankActiveStateComponent tankActiveState;
            public TankGroupComponent tankGroup;
        }

        public class InitialShaftShotAnimationNode : Node
        {
            public AnimationComponent animation;
            public AnimationPreparedComponent animationPrepared;
            public ShaftShotAnimationComponent shaftShotAnimation;
            public ShaftEnergyComponent shaftEnergy;
            public ShaftShotAnimationTriggerComponent shaftShotAnimationTrigger;
            public WeaponCooldownComponent weaponCooldown;
            public TankGroupComponent tankGroup;
        }

        public class ReadyShaftShotAnimationNode : Node
        {
            public AnimationComponent animation;
            public AnimationPreparedComponent animationPrepared;
            public ShaftShotAnimationComponent shaftShotAnimation;
            public ShaftShotAnimationESMComponent shaftShotAnimationEsm;
            public TankGroupComponent tankGroup;
        }

        public class ShaftShotAnimationCooldownStateNode : Node
        {
            public ShaftShotAnimationCooldownStateComponent shaftShotAnimationCooldownState;
            public TankGroupComponent tankGroup;
        }
    }
}

