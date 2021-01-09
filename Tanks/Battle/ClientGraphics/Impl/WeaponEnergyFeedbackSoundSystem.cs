namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Battle.ClientGraphics.API;
    using Tanks.Lobby.ClientSettings.API;
    using UnityEngine;

    public class WeaponEnergyFeedbackSoundSystem : ECSSystem
    {
        private void PlayLowEnergyFeedback(TankNode tank)
        {
            this.StopSound(tank);
            WeaponFeedbackSoundBehaviour behaviour = Object.Instantiate<WeaponFeedbackSoundBehaviour>(tank.shootingEnergyFeedbackSound.LowEnergyFeedbackSoundAsset);
            tank.shootingEnergyFeedbackSound.Instance = behaviour;
            behaviour.Play(-1f, 1f, true);
        }

        [OnEventFire]
        public void PlayLowEnergyForHammerWeapon(TimeUpdateEvent e, HammerEnergyNode weapon, [JoinByTank] TankNode tank, [JoinAll] SoundListenerNode listener)
        {
            if (InputManager.GetActionKeyDown(ShotActions.SHOT) && ((weapon.cooldownTimer.CooldownTimerSec > 0f) || (weapon.Entity.HasComponent<MagazineReloadStateComponent>() || !weapon.Entity.HasComponent<ShootableComponent>())))
            {
                this.PlayLowEnergyFeedback(tank);
            }
        }

        [OnEventFire]
        public void PlayLowEnergyForShaft(TimeUpdateEvent e, ShaftEnergyNode weapon, [JoinByTank] TankNode tank, [JoinAll] SoundListenerNode listener)
        {
            if (InputManager.GetActionKeyDown(ShotActions.SHOT))
            {
                CooldownTimerComponent cooldownTimer = weapon.cooldownTimer;
                if ((weapon.weaponEnergy.Energy < weapon.shaftEnergy.UnloadEnergyPerQuickShot) || ((cooldownTimer.CooldownTimerSec > 0f) || !weapon.Entity.HasComponent<ShootableComponent>()))
                {
                    this.PlayLowEnergyFeedback(tank);
                }
            }
        }

        [OnEventFire]
        public void PlayLowEnergyForSimpleDiscreteEnergyWeapon(TimeUpdateEvent e, SimpleDiscreteWeaponEnergyNode weapon, [JoinByTank] TankNode tank, [JoinAll] SoundListenerNode listener)
        {
            if (InputManager.GetActionKeyDown(ShotActions.SHOT))
            {
                if (!weapon.Entity.HasComponent<ShootableComponent>())
                {
                    this.PlayLowEnergyFeedback(tank);
                }
                else if (weapon.weaponEnergy.Energy < weapon.discreteWeaponEnergy.UnloadEnergyPerShot)
                {
                    this.PlayLowEnergyFeedback(tank);
                }
            }
        }

        [OnEventFire]
        public void PlayLowEnergyForStreamEnergyWeapon(TimeUpdateEvent e, StreamEnergyNode weapon, [JoinByTank] TankNode tank, [JoinAll] SoundListenerNode listener)
        {
            if (InputManager.GetActionKeyDown(ShotActions.SHOT) && !weapon.Entity.HasComponent<ShootableComponent>())
            {
                this.PlayLowEnergyFeedback(tank);
            }
        }

        [OnEventFire]
        public void PlayLowEnergyForVulcanIdleWeapon(TimeUpdateEvent e, VulcanWeaponIdleNode weapon, [JoinByTank] TankNode tank, [JoinAll] SoundListenerNode listener)
        {
            if (InputManager.GetActionKeyDown(ShotActions.SHOT) && !weapon.Entity.HasComponent<ShootableComponent>())
            {
                this.PlayLowEnergyFeedback(tank);
            }
        }

        [OnEventFire]
        public void PlayLowEnergyForVulcanSlowDownWeapon(TimeUpdateEvent e, VulcanWeaponSlowDownNode weapon, [JoinByTank] TankNode tank, [JoinAll] SoundListenerNode listener)
        {
            if (InputManager.GetActionKeyDown(ShotActions.SHOT))
            {
                this.PlayLowEnergyFeedback(tank);
            }
        }

        [OnEventFire]
        public void StopLowEnergyOnHitFeedback(HitFeedbackEvent e, TankNode tank, [JoinAll] SoundListenerNode listener)
        {
            this.StopSound(tank);
        }

        private void StopSound(TankNode tank)
        {
            WeaponFeedbackSoundBehaviour instance = tank.shootingEnergyFeedbackSound.Instance;
            if (instance)
            {
                instance.GetComponent<WeaponEnergyFeedbackFadeBehaviour>().enabled = true;
            }
        }

        [Inject]
        public static Tanks.Battle.ClientCore.Impl.InputManager InputManager { get; set; }

        public class HammerEnergyNode : Node
        {
            public WeaponComponent weapon;
            public HammerComponent hammer;
            public MagazineWeaponComponent magazineWeapon;
            public MagazineStorageComponent magazineStorage;
            public TankGroupComponent tankGroup;
            public HammerEnergyBarComponent hammerEnergyBar;
            public CooldownTimerComponent cooldownTimer;
        }

        public class ShaftEnergyNode : Node
        {
            public ShaftEnergyComponent shaftEnergy;
            public WeaponEnergyComponent weaponEnergy;
            public TankGroupComponent tankGroup;
            public CooldownTimerComponent cooldownTimer;
        }

        [Not(typeof(TwinsComponent))]
        public class SimpleDiscreteWeaponEnergyNode : WeaponEnergyFeedbackSoundSystem.WeaponEnergyNode
        {
            public DiscreteWeaponEnergyComponent discreteWeaponEnergy;
        }

        public class SoundListenerNode : Node
        {
            public SoundListenerComponent soundListener;
            public SoundListenerBattleStateComponent soundListenerBattleState;
            public SoundListenerReadyForHitFeedbackComponent soundListenerReadyForHitFeedback;
        }

        public class StreamEnergyNode : WeaponEnergyFeedbackSoundSystem.WeaponEnergyNode
        {
            public StreamWeaponComponent streamWeapon;
        }

        public class TankNode : Node
        {
            public ShootingEnergyFeedbackSoundComponent shootingEnergyFeedbackSound;
            public AssembledTankActivatedStateComponent assembledTankActivatedState;
            public TankActiveStateComponent tankActiveState;
            public SelfTankComponent selfTank;
            public TankGroupComponent tankGroup;
        }

        public class VulcanWeaponIdleNode : WeaponEnergyFeedbackSoundSystem.VulcanWeaponNode
        {
            public VulcanIdleComponent vulcanIdle;
        }

        public class VulcanWeaponNode : Node
        {
            public VulcanWeaponComponent vulcanWeapon;
            public VulcanWeaponStateComponent vulcanWeaponState;
            public TankGroupComponent tankGroup;
            public CooldownTimerComponent cooldownTimer;
        }

        public class VulcanWeaponSlowDownNode : WeaponEnergyFeedbackSoundSystem.VulcanWeaponNode
        {
            public VulcanSlowDownComponent vulcanSlowDown;
        }

        public class WeaponEnergyNode : WeaponEnergyFeedbackSoundSystem.WeaponNode
        {
            public WeaponEnergyComponent weaponEnergy;
        }

        public class WeaponNode : Node
        {
            public WeaponComponent weapon;
            public TankGroupComponent tankGroup;
        }
    }
}

