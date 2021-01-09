namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;

    public class RailgunChargingWeaponControllerSystem : ECSSystem
    {
        [OnEventFire]
        public void MakeChargingAndScheduleShot(SelfRailgunChargingShotEvent evt, ReadyRailgunChargingWeaponControllerNode chargingWeaponController)
        {
            Entity entity = chargingWeaponController.Entity;
            entity.RemoveComponent<ReadyRailgunChargingWeaponComponent>();
            entity.AddComponent<RailgunChargingStateComponent>();
            EventBuilder builder = base.NewEvent<RailgunDelayedShotPrepareEvent>();
            builder.Attach(chargingWeaponController);
            builder.ScheduleDelayed(chargingWeaponController.railgunChargingWeapon.ChargingTime);
        }

        [OnEventFire]
        public void Reset(NodeAddedEvent evt, ActiveTankNode selfActiveTank, [JoinByTank] CompleteChargingWeaponControllerNode chargingWeaponNode)
        {
            Entity entity = chargingWeaponNode.Entity;
            entity.RemoveComponent<RailgunChargingStateComponent>();
            entity.AddComponent<ReadyRailgunChargingWeaponComponent>();
        }

        [OnEventFire]
        public void SendShotPrepare(RailgunDelayedShotPrepareEvent evt, CompleteChargingWeaponControllerNode chargingWeaponNode, [JoinByTank] ActiveTankNode selfActiveTank)
        {
            Entity entity = chargingWeaponNode.Entity;
            entity.AddComponent<ReadyRailgunChargingWeaponComponent>();
            entity.RemoveComponent<RailgunChargingStateComponent>();
            if (chargingWeaponNode.Entity.HasComponent<ShootableComponent>())
            {
                base.ScheduleEvent<BeforeShotEvent>(entity);
                base.ScheduleEvent<ShotPrepareEvent>(entity);
            }
        }

        [OnEventFire]
        public void StartShotIfPossible(EarlyUpdateEvent evt, ReadyRailgunChargingWeaponControllerNode chargingWeaponController, [JoinSelf] SingleNode<ShootableComponent> node, [JoinByTank] ActiveTankNode selfActiveTank)
        {
            CooldownTimerComponent cooldownTimer = chargingWeaponController.cooldownTimer;
            if (((chargingWeaponController.weaponEnergy.Energy >= chargingWeaponController.discreteWeaponEnergy.UnloadEnergyPerShot) && (cooldownTimer.CooldownTimerSec <= 0f)) && InputManager.CheckAction(ShotActions.SHOT))
            {
                base.ScheduleEvent<SelfRailgunChargingShotEvent>(chargingWeaponController);
            }
        }

        [Inject]
        public static Tanks.Battle.ClientCore.Impl.InputManager InputManager { get; set; }

        public class ActiveTankNode : Node
        {
            public TankActiveStateComponent tankActiveState;
            public TankGroupComponent tankGroup;
        }

        public class CompleteChargingWeaponControllerNode : Node
        {
            public ChargingWeaponControllerComponent chargingWeaponController;
            public RailgunChargingWeaponComponent railgunChargingWeapon;
            public RailgunChargingStateComponent railgunChargingState;
        }

        public class ReadyRailgunChargingWeaponControllerNode : Node
        {
            public ChargingWeaponControllerComponent chargingWeaponController;
            public DiscreteWeaponEnergyComponent discreteWeaponEnergy;
            public WeaponEnergyComponent weaponEnergy;
            public CooldownTimerComponent cooldownTimer;
            public RailgunChargingWeaponComponent railgunChargingWeapon;
            public ReadyRailgunChargingWeaponComponent readyRailgunChargingWeapon;
        }
    }
}

