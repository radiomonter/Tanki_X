namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;

    public class HammerShotAnimationSystem : ECSSystem
    {
        [OnEventFire]
        public void InitHammerShotAnimation(NodeAddedEvent evt, InitialHammerShotAnimationNode weapon)
        {
            float cooldownIntervalSec = weapon.weaponCooldown.CooldownIntervalSec;
            Entity entity = weapon.Entity;
            weapon.hammerShotAnimation.InitHammerShotAnimation(entity, weapon.animation.Animator, weapon.magazineWeapon.ReloadMagazineTimePerSec, cooldownIntervalSec);
            entity.AddComponent<HammerShotAnimationReadyComponent>();
        }

        [OnEventFire]
        public void PlayShot(BaseShotEvent evt, ReadyHammerShotAnimationNode weapon, [JoinByTank] ActiveTankNode tank)
        {
            if (weapon.magazineLocalStorage.CurrentCartridgeCount > 1)
            {
                weapon.hammerShotAnimation.PlayShot();
            }
            else
            {
                weapon.hammerShotAnimation.PlayShotAndReload();
            }
        }

        [OnEventFire]
        public void Reset(ExecuteEnergyInjectionEvent e, ReadyHammerShotAnimationNode weapon)
        {
            weapon.hammerShotAnimation.Reset();
        }

        [OnEventComplete]
        public void Reset(NodeRemoveEvent evt, ActiveTankNode tank, [JoinByTank] ReadyHammerShotAnimationNode weapon)
        {
            weapon.hammerShotAnimation.Reset();
        }

        public class ActiveTankNode : Node
        {
            public TankActiveStateComponent tankActiveState;
            public TankGroupComponent tankGroup;
        }

        public class InitialHammerShotAnimationNode : Node
        {
            public MagazineWeaponComponent magazineWeapon;
            public MagazineStorageComponent magazineStorage;
            public MagazineLocalStorageComponent magazineLocalStorage;
            public AnimationComponent animation;
            public AnimationPreparedComponent animationPrepared;
            public HammerShotAnimationComponent hammerShotAnimation;
            public WeaponCooldownComponent weaponCooldown;
            public TankGroupComponent tankGroup;
        }

        public class ReadyHammerShotAnimationNode : Node
        {
            public MagazineWeaponComponent magazineWeapon;
            public MagazineStorageComponent magazineStorage;
            public MagazineLocalStorageComponent magazineLocalStorage;
            public HammerShotAnimationComponent hammerShotAnimation;
            public HammerShotAnimationReadyComponent hammerShotAnimationReady;
            public TankGroupComponent tankGroup;
        }
    }
}

