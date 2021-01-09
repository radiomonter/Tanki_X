namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientGraphics.Impl;
    using Tanks.Lobby.ClientEntrance.API;
    using UnityEngine;

    public class DroneWeaponSystem : ECSSystem
    {
        public static float WEAPON_CONTROL_PERIOD = 1f;

        [OnEventFire]
        public void ControllDroneWeapon(NodeRemoveEvent e, SelfDroneWithTargetNode drone, [JoinByUnit] SelfDroneWeapon droneWeapon)
        {
            this.StopShooting(droneWeapon.Entity);
        }

        [OnEventFire]
        public void ControllDroneWeapon(UpdateEvent e, SelfDroneWithTargetNode drone, [JoinByUnit] SelfDroneWeapon droneWeapon)
        {
            if ((Time.timeSinceLevelLoad - droneWeapon.droneWeapon.lastControlTime) >= WEAPON_CONTROL_PERIOD)
            {
                droneWeapon.droneWeapon.lastControlTime = Time.timeSinceLevelLoad;
                if (this.IsTargetVisable(droneWeapon))
                {
                    this.StartShooting(droneWeapon.Entity);
                    drone.droneAnimator.StartShoot();
                    droneWeapon.droneWeapon.lastTimeTargetSeen = Time.time;
                }
                else
                {
                    this.StopShooting(droneWeapon.Entity);
                    drone.droneAnimator.StartIdle();
                    drone.Entity.RemoveComponentIfPresent<UnitTargetComponent>();
                }
            }
        }

        [OnEventFire]
        public void InitDroneSelfWeapon(NodeAddedEvent e, [Combine] SelfDroneNode drone, [JoinByUnit] SingleNode<WeaponComponent> weapon, [JoinAll, Context] SingleNode<MapInstanceComponent> map)
        {
            weapon.Entity.AddComponent(new WeaponHitComponent(false, false));
            weapon.Entity.AddComponent<CooldownTimerComponent>();
            this.StartShooting(weapon.Entity);
            this.StopShooting(weapon.Entity);
        }

        [OnEventFire]
        public void InitDroneWeapon(NodeAddedEvent e, [Combine] DroneNode drone, [JoinByUnit] SingleNode<WeaponComponent> weapon, [JoinAll, Context] SingleNode<MapInstanceComponent> map)
        {
            MuzzlePointMarkerComponent componentInChildren = drone.rigidbody.Rigidbody.GetComponentInChildren<MuzzlePointMarkerComponent>();
            componentInChildren.gameObject.GetComponent<EntityBehaviour>().BuildEntity(weapon.Entity);
            weapon.Entity.AddComponent(new WeaponInstanceComponent(componentInChildren.gameObject));
            weapon.Entity.AddComponent(new TargetCollectorComponent(new TargetCollector(drone.Entity), new TargetValidator(drone.Entity)));
        }

        private bool IsTargetVisable(SelfDroneWeapon droneWeapon)
        {
            TargetingData targetingData = BattleCache.targetingData.GetInstance().Init();
            TargetingEvent eventInstance = BattleCache.targetingEvent.GetInstance().Init(targetingData);
            base.ScheduleEvent(eventInstance, droneWeapon);
            if (targetingData.HasTargetHit())
            {
                using (List<DirectionData>.Enumerator enumerator = targetingData.Directions.GetEnumerator())
                {
                    while (true)
                    {
                        if (!enumerator.MoveNext())
                        {
                            break;
                        }
                        DirectionData current = enumerator.Current;
                        if (current.HasTargetHit() && current.Targets[0].ValidTarget)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        [OnEventFire]
        public void ShootDroneWeapon(UpdateEvent e, SelfDroneWithTargetNode drone, [JoinByUnit] ShootingDroneWeapon droneWeapon)
        {
            if (droneWeapon.cooldownTimer.CooldownTimerSec <= 0f)
            {
                base.ScheduleEvent<ShotPrepareEvent>(droneWeapon);
            }
        }

        private void StartShooting(Entity droneWeapon)
        {
            droneWeapon.AddComponentIfAbsent<WeaponStreamShootingComponent>();
            droneWeapon.AddComponentIfAbsent<StreamHitCheckingComponent>();
        }

        private void StopShooting(Entity droneWeapon)
        {
            droneWeapon.RemoveComponentIfPresent<WeaponStreamShootingComponent>();
            droneWeapon.RemoveComponentIfPresent<StreamHitCheckingComponent>();
        }

        [Inject]
        public static BattleFlowInstancesCache BattleCache { get; set; }

        public class DroneLoadedNode : Node
        {
            public DroneEffectComponent droneEffect;
            public UnitMoveComponent unitMove;
            public UnitGroupComponent unitGroup;
        }

        public class DroneNode : DroneWeaponSystem.DroneLoadedNode
        {
            public UnitMoveComponent unitMove;
            public RigidbodyComponent rigidbody;
        }

        public class SelfDroneNode : DroneWeaponSystem.DroneNode
        {
            public SelfComponent self;
            public DroneAnimatorComponent droneAnimator;
            public EffectActiveComponent effectActive;
        }

        public class SelfDroneWeapon : Node
        {
            public SelfComponent self;
            public WeaponComponent weapon;
            public DroneWeaponComponent droneWeapon;
            public MuzzlePointComponent muzzlePoint;
            public WeaponInstanceComponent weaponInstance;
            public VerticalTargetingComponent verticalTargeting;
            public CooldownTimerComponent cooldownTimer;
            public WeaponCooldownComponent weaponCooldown;
        }

        public class SelfDroneWithTargetNode : DroneWeaponSystem.SelfDroneNode
        {
            public UnitTargetComponent unitTarget;
        }

        public class ShootingDroneWeapon : DroneWeaponSystem.SelfDroneWeapon
        {
            public StreamHitCheckingComponent streamHitChecking;
            public StreamHitComponent streamHit;
        }
    }
}

