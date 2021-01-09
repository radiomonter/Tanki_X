namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientResources.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class ClientHullBuilderSystem : ECSSystem
    {
        [OnEventFire]
        public void BuildHull(HullInstanceIsReadyEvent evt, PrefabLoadedNode node)
        {
            node.Entity.AddComponent(new TankCommonInstanceComponent(evt.HullInstance));
        }

        private Rigidbody BuildRigidBody(GameObject hullInstance)
        {
            Rigidbody rigidbody = hullInstance.AddComponent<Rigidbody>();
            rigidbody.mass = 1100f;
            rigidbody.drag = 0f;
            rigidbody.angularDrag = 0.05f;
            rigidbody.useGravity = true;
            rigidbody.isKinematic = false;
            rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
            rigidbody.sleepThreshold = 0.1f;
            return rigidbody;
        }

        [OnEventFire]
        public void InstantiateHull(InstantiateHullEvent e, HullSkin hullSkin, [JoinByTank] TankNode tank, PrefabLoadedNode node)
        {
            Entity entity = tank.Entity;
            GameObject hullInstance = Object.Instantiate<GameObject>((GameObject) hullSkin.resourceData.Data);
            hullInstance.SetActive(false);
            if (entity.HasComponent<HullInstanceComponent>())
            {
                entity.GetComponent<HullInstanceComponent>().HullInstance = hullInstance;
            }
            else
            {
                HullInstanceComponent component = new HullInstanceComponent {
                    HullInstance = hullInstance
                };
                entity.AddComponent(component);
            }
            Rigidbody rigidbody = this.BuildRigidBody(hullInstance);
            entity.AddComponent(new RigidbodyComponent(rigidbody));
            PhysicsUtil.SetGameObjectLayer(hullInstance, Layers.INVISIBLE_PHYSICS);
            hullInstance.AddComponent<NanFixer>().Init(rigidbody, hullInstance.transform, tank.Entity.GetComponent<UserGroupComponent>().Key);
            base.NewEvent<InstantiateTankCommonPartEvent>().Attach(node).ScheduleDelayed(0.3f);
        }

        [OnEventFire]
        public void InstantiateTankCommonPart(InstantiateTankCommonPartEvent e, [Combine] PrefabLoadedNode node)
        {
            HullInstanceIsReadyEvent eventInstance = new HullInstanceIsReadyEvent {
                HullInstance = Object.Instantiate<GameObject>(node.resourceData.Data as GameObject)
            };
            base.NewEvent(eventInstance).Attach(node).ScheduleDelayed(0.3f);
        }

        [OnEventFire]
        public void RequestHullInstantiating(NodeAddedEvent e, SingleNode<MapInstanceComponent> map, [Combine] PrefabLoadedNode node, [JoinByTank] HullSkin hullSkin)
        {
            base.NewEvent<InstantiateHullEvent>().Attach(hullSkin).Attach(node).ScheduleDelayed(0.2f);
        }

        [OnEventFire]
        public void RequestPrefabs(RequestHullPrefabsEvent e, TankNode tank, HullSkin hullSkin)
        {
            Entity entity = tank.Entity;
            entity.AddComponent<TankCommonPrefabComponent>();
            entity.AddComponent(new AssetReferenceComponent(new AssetReference(entity.GetComponent<TankCommonPrefabComponent>().AssetGuid)));
            entity.AddComponent<AssetRequestComponent>();
        }

        [OnEventFire]
        public void StartPrepareHull(NodeAddedEvent e, TankNode tank, [Context, JoinByTank] HullSkin hullSkin)
        {
            Entity entity = tank.Entity;
            entity.AddComponent<ChassisConfigComponent>();
            base.NewEvent<RequestHullPrefabsEvent>().Attach(entity).Attach(hullSkin.Entity).ScheduleDelayed(0.2f);
        }

        public class HullInstanceIsReadyEvent : Event
        {
            public GameObject HullInstance;
        }

        public class HullSkin : Node
        {
            public HullSkinBattleItemComponent hullSkinBattleItem;
            public ResourceDataComponent resourceData;
            public TankGroupComponent tankGroup;
        }

        public class PrefabLoadedNode : Node
        {
            public TankCommonPrefabComponent tankCommonPrefab;
            public ResourceDataComponent resourceData;
            public TankGroupComponent tankGroup;
        }

        public class TankNode : Node
        {
            public TankComponent tank;
            public TankGroupComponent tankGroup;
        }
    }
}

