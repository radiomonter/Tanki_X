namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Battle.ClientGraphics.API;
    using UnityEngine;

    public class RailgunChargingEffectSystem : ECSSystem
    {
        private const string RAILGUN_CHARGING_ENTITY_NAME = "railgun_charging";

        [OnEventFire]
        public void DestroyWeaponChargingEffect(NodeRemoveEvent evt, ChargingGraphicsNode effect)
        {
            base.DeleteEntity(effect.Entity);
        }

        [OnEventFire]
        public void StartRailgunCharging(RemoteRailgunChargingShotEvent evt, RailgunChargingNode muzzle, [JoinBy(typeof(TankGroupComponent))] TankActiveNode tank)
        {
            this.StartRailgunChargingByBaseEvent(evt, muzzle, tank);
        }

        [OnEventFire, Mandatory]
        public void StartRailgunCharging(SelfRailgunChargingShotEvent evt, RailgunChargingNode muzzle, [JoinBy(typeof(TankGroupComponent))] TankActiveNode tank)
        {
            this.StartRailgunChargingByBaseEvent(evt, muzzle, tank);
        }

        private void StartRailgunChargingByBaseEvent(BaseRailgunChargingShotEvent evt, RailgunChargingNode muzzle, TankActiveNode tank)
        {
            RailgunChargingWeaponComponent railgunChargingWeapon = muzzle.railgunChargingWeapon;
            GetInstanceFromPoolEvent eventInstance = new GetInstanceFromPoolEvent {
                Prefab = muzzle.railgunChargingEffect.Prefab,
                AutoRecycleTime = railgunChargingWeapon.ChargingTime
            };
            base.ScheduleEvent(eventInstance, muzzle);
            Transform instance = eventInstance.Instance;
            GameObject gameObject = instance.gameObject;
            CustomRenderQueue.SetQueue(gameObject, 0xc4e);
            ParticleSystem component = gameObject.GetComponent<ParticleSystem>();
            component.main.startLifetime = railgunChargingWeapon.ChargingTime;
            component.emission.enabled = true;
            UnityUtil.InheritAndEmplace(instance, muzzle.muzzlePoint.Current);
            gameObject.SetActive(true);
            base.CreateEntity("railgun_charging").AddComponent(new TankGroupComponent(tank.Entity));
        }

        public class ChargingGraphicsNode : Node
        {
            public TankGroupComponent tankGroup;
            public InstanceDestructionComponent instanceDestruction;
        }

        public class RailgunChargingNode : Node
        {
            public MuzzlePointComponent muzzlePoint;
            public RailgunChargingEffectComponent railgunChargingEffect;
            public RailgunChargingWeaponComponent railgunChargingWeapon;
        }

        public class TankActiveNode : Node
        {
            public TankActiveStateComponent tankActiveState;
        }
    }
}

