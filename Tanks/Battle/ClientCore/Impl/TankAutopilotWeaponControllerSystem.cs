namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;

    public class TankAutopilotWeaponControllerSystem : ECSSystem
    {
        [OnEventComplete]
        public void StartShotIfPossible(EarlyUpdateEvent evt, DiscreteWeaponControllerNode discreteWeaponEnergyController, [JoinByTank] AutopilotTankNode autopilotTank)
        {
            float unloadEnergyPerShot = discreteWeaponEnergyController.discreteWeaponEnergy.UnloadEnergyPerShot;
            float energy = discreteWeaponEnergyController.weaponEnergy.Energy;
            CooldownTimerComponent cooldownTimer = discreteWeaponEnergyController.cooldownTimer;
            if ((autopilotTank.autopilotWeaponController.Fire && (energy >= unloadEnergyPerShot)) && (cooldownTimer.CooldownTimerSec <= 0f))
            {
                base.ScheduleEvent<BeforeShotEvent>(discreteWeaponEnergyController);
                base.ScheduleEvent<ShotPrepareEvent>(discreteWeaponEnergyController);
                base.ScheduleEvent<PostShotEvent>(discreteWeaponEnergyController);
            }
        }

        [OnEventFire]
        public void StartShotIfPossible(EarlyUpdateEvent evt, DiscreteWeaponMagazineControllerNode discreteWeaponMagazineController, [JoinByTank] AutopilotTankNode autopilotTank)
        {
            if (autopilotTank.autopilotWeaponController.Fire && (discreteWeaponMagazineController.cooldownTimer.CooldownTimerSec <= 0f))
            {
                base.ScheduleEvent<BeforeShotEvent>(discreteWeaponMagazineController);
                base.ScheduleEvent<ShotPrepareEvent>(discreteWeaponMagazineController);
            }
        }

        [Inject]
        public static Tanks.Battle.ClientCore.Impl.InputManager InputManager { get; set; }

        public class AutopilotTankNode : Node
        {
            public TankSyncComponent tankSync;
            public TankAutopilotComponent tankAutopilot;
            public TankActiveStateComponent tankActiveState;
            public AutopilotWeaponControllerComponent autopilotWeaponController;
        }

        public class DiscreteWeaponControllerNode : Node
        {
            public DiscreteWeaponControllerComponent discreteWeaponController;
            public DiscreteWeaponEnergyComponent discreteWeaponEnergy;
            public WeaponEnergyComponent weaponEnergy;
            public CooldownTimerComponent cooldownTimer;
            public DiscreteWeaponComponent discreteWeapon;
        }

        public class DiscreteWeaponMagazineControllerNode : Node
        {
            public DiscreteWeaponControllerComponent discreteWeaponController;
            public MagazineStorageComponent magazineStorage;
            public MagazineReadyStateComponent magazineReadyState;
            public CooldownTimerComponent cooldownTimer;
            public DiscreteWeaponComponent discreteWeapon;
        }
    }
}

