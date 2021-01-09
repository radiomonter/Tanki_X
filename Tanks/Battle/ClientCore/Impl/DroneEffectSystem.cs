namespace Tanks.Battle.ClientCore.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Lobby.ClientEntrance.API;
    using UnityEngine;

    public class DroneEffectSystem : ECSSystem
    {
        [OnEventFire]
        public void AddCTFEvaluator(NodeAddedEvent evt, [Combine] SelfDroneNode spider, [JoinByBattle] SingleNode<CTFComponent> battle)
        {
            spider.Entity.AddComponent<CTFTargetEvaluatorComponent>();
        }

        [OnEventFire]
        public void AddTeamEvaluator(NodeAddedEvent evt, [Combine] SelfDroneNode spider, [JoinByBattle] SingleNode<TeamBattleComponent> battle)
        {
            spider.Entity.AddComponent<TeamTargetEvaluatorComponent>();
        }

        [OnEventFire]
        public void Instantiate(NodeAddedEvent e, SingleNode<PreloadedModuleEffectsComponent> mapEffect, SingleNode<MapInstanceComponent> map, [Combine] DroneLoadedNode drone, [Context, JoinByUser] TankNode tank, [JoinByTank] SingleNode<TankIncarnationComponent> incarnation, [JoinByUser] Optional<SingleNode<UserAvatarComponent>> avatar)
        {
            string str = (!avatar.IsPresent() || (avatar.Get().component.Id != "457e8f5f-953a-424c-bd97-67d9e116ab7a")) ? "drone" : "droneHolo";
            GameObject original = mapEffect.component.PreloadedEffects[str];
            if (original)
            {
                GameObject gameObject = Object.Instantiate<GameObject>(original, null);
                gameObject.SetActive(true);
                Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
                RigidbodyComponent component = new RigidbodyComponent {
                    Rigidbody = rigidbody
                };
                drone.Entity.AddComponent(component);
                rigidbody.GetComponent<EntityBehaviour>().BuildEntity(drone.Entity);
                DroneOwnerComponent component2 = new DroneOwnerComponent {
                    Incarnation = incarnation.Entity,
                    Rigidbody = tank.rigidbody.Rigidbody
                };
                drone.Entity.AddComponent(component2);
                drone.Entity.AddComponent(new EffectInstanceComponent(gameObject));
            }
        }

        [OnEventFire]
        public void Targeting(UpdateEvent e, SelfDroneWithoutTargetNode drone)
        {
            base.ScheduleEvent<UnitSelectTargetEvent>(drone);
        }

        [OnEventFire]
        public void Targeting(UpdateEvent e, SelfDroneWithTargetNode drone)
        {
            Entity target = drone.unitTarget.Target;
            if (!(target.Alive && target.HasComponent<TankActiveStateComponent>()))
            {
                drone.Entity.RemoveComponent<UnitTargetComponent>();
            }
        }

        public class DroneLoadedNode : Node
        {
            public DroneEffectComponent droneEffect;
            public UnitMoveComponent unitMove;
            public UnitGroupComponent unitGroup;
        }

        public class DroneNode : DroneEffectSystem.DroneLoadedNode
        {
            public RigidbodyComponent rigidbody;
        }

        public class SelfDroneNode : DroneEffectSystem.DroneNode
        {
            public SelfComponent self;
        }

        [Not(typeof(UnitTargetComponent))]
        public class SelfDroneWithoutTargetNode : DroneEffectSystem.SelfDroneNode
        {
        }

        public class SelfDroneWithTargetNode : DroneEffectSystem.SelfDroneNode
        {
            public UnitTargetComponent unitTarget;
        }

        public class TankNode : Node
        {
            public TankComponent tank;
            public UserGroupComponent userGroup;
            public TankGroupComponent tankGroup;
            public RigidbodyComponent rigidbody;
        }
    }
}

