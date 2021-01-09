namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using UnityEngine;

    public class ShotAnimationSystem : ECSSystem
    {
        [OnEventFire]
        public void InitShotAnimation(NodeAddedEvent e, InitialShotAnimationNode weapon)
        {
            Animator animator = weapon.animation.Animator;
            weapon.shotAnimation.Init(animator, weapon.weaponCooldown.CooldownIntervalSec, weapon.discreteWeaponEnergy.UnloadEnergyPerShot, weapon.discreteWeaponEnergy.ReloadEnergyPerSec);
            weapon.Entity.AddComponent<ShotAnimationReadyComponent>();
        }

        [OnEventFire]
        public void StartShotAnimation(BaseShotEvent evt, ReadyShotAnimationNode weapon)
        {
            weapon.shotAnimation.Play();
        }

        public class InitialShotAnimationNode : Node
        {
            public TankGroupComponent tankGroup;
            public AnimationComponent animation;
            public ShotAnimationComponent shotAnimation;
            public AnimationPreparedComponent animationPrepared;
            public WeaponCooldownComponent weaponCooldown;
            public DiscreteWeaponEnergyComponent discreteWeaponEnergy;
        }

        public class ReadyShotAnimationNode : Node
        {
            public TankGroupComponent tankGroup;
            public AnimationComponent animation;
            public ShotAnimationComponent shotAnimation;
            public ShotAnimationReadyComponent shotAnimationReady;
            public AnimationPreparedComponent animationPrepared;
        }
    }
}

