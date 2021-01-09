namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;

    public class DiscreteWeaponControllerSystem : ECSSystem
    {
        [OnEventComplete]
        public void StartShotIfPossible(EarlyUpdateEvent evt, SelfTankNode selfTank, [JoinByTank] DiscreteWeaponControllerNode discreteWeaponEnergyController)
        {
            CooldownTimerComponent cooldownTimer = discreteWeaponEnergyController.cooldownTimer;
            if (((discreteWeaponEnergyController.weaponEnergy.Energy >= discreteWeaponEnergyController.discreteWeaponEnergy.UnloadEnergyPerShot) && (cooldownTimer.CooldownTimerSec <= 0f)) && (InputManager.GetAxisOrKey(ShotActions.SHOT) != 0f))
            {
                base.ScheduleEvent<BeforeShotEvent>(discreteWeaponEnergyController);
                base.ScheduleEvent<ShotPrepareEvent>(discreteWeaponEnergyController);
                base.ScheduleEvent<PostShotEvent>(discreteWeaponEnergyController);
            }
        }

        [OnEventFire]
        public void StartShotIfPossible(EarlyUpdateEvent evt, SelfTankNode selfTank, [JoinByTank] DiscreteWeaponMagazineControllerNode discreteWeaponMagazineController)
        {
            if ((discreteWeaponMagazineController.cooldownTimer.CooldownTimerSec <= 0f) && InputManager.CheckAction(ShotActions.SHOT))
            {
                base.ScheduleEvent<BeforeShotEvent>(discreteWeaponMagazineController);
                base.ScheduleEvent<ShotPrepareEvent>(discreteWeaponMagazineController);
            }
        }

        [Inject]
        public static Tanks.Battle.ClientCore.Impl.InputManager InputManager { get; set; }

        public class CommonDiscreteWeaponControllerNode : Node
        {
            public DiscreteWeaponControllerComponent discreteWeaponController;
            public CooldownTimerComponent cooldownTimer;
            public DiscreteWeaponComponent discreteWeapon;
            public ShootableComponent shootable;
        }

        public class DiscreteWeaponControllerNode : DiscreteWeaponControllerSystem.CommonDiscreteWeaponControllerNode
        {
            public DiscreteWeaponEnergyComponent discreteWeaponEnergy;
            public WeaponEnergyComponent weaponEnergy;
        }

        public class DiscreteWeaponMagazineControllerNode : DiscreteWeaponControllerSystem.CommonDiscreteWeaponControllerNode
        {
            public MagazineStorageComponent magazineStorage;
            public MagazineReadyStateComponent magazineReadyState;
        }

        public class SelfTankNode : Node
        {
            public TankActiveStateComponent tankActiveState;
            public SelfTankComponent selfTank;
        }
    }
}

