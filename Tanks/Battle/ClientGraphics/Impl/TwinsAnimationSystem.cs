namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using UnityEngine;

    public class TwinsAnimationSystem : ECSSystem
    {
        [OnEventFire]
        public void InitTwinsAnimation(NodeAddedEvent evt, InitialTwinsAnimationNode weapon)
        {
            Animator animator = weapon.animation.Animator;
            weapon.twinsAnimation.Init(animator, weapon.weaponCooldown.CooldownIntervalSec, weapon.discreteWeaponEnergy.UnloadEnergyPerShot, weapon.discreteWeaponEnergy.ReloadEnergyPerSec);
            weapon.Entity.AddComponent<TwinsAnimationReadyComponent>();
        }

        [OnEventFire]
        public void PlayTwinsShotAnimation(BaseShotEvent evt, ReadyTwinsAnimationNode weapon)
        {
            int currentIndex = new MuzzleVisualAccessor(weapon.muzzlePoint).GetCurrentIndex();
            weapon.twinsAnimation.Play(currentIndex);
        }

        public class InitialTwinsAnimationNode : Node
        {
            public TwinsAnimationComponent twinsAnimation;
            public MuzzlePointComponent muzzlePoint;
            public AnimationComponent animation;
            public AnimationPreparedComponent animationPrepared;
            public WeaponCooldownComponent weaponCooldown;
            public DiscreteWeaponEnergyComponent discreteWeaponEnergy;
        }

        public class ReadyTwinsAnimationNode : Node
        {
            public TwinsAnimationComponent twinsAnimation;
            public TwinsAnimationReadyComponent twinsAnimationReady;
            public MuzzlePointComponent muzzlePoint;
            public AnimationComponent animation;
            public AnimationPreparedComponent animationPrepared;
        }
    }
}

