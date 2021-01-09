namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Battle.ClientGraphics.API;
    using UnityEngine;

    public class TankPartIntersectionWithCameraSystem : ECSSystem
    {
        private void AttachColliderToIntersectionMap(VisualTriggerMarkerComponent trigger, Entity entity, TankPartIntersectionWithCameraData[] map, int index)
        {
            MeshCollider visualTriggerMeshCollider = trigger.VisualTriggerMeshCollider;
            map[index] = new TankPartIntersectionWithCameraData(visualTriggerMeshCollider, entity);
            entity.AddComponent<TankPartNotIntersectedWithCameraStateComponent>();
        }

        private void CheckCameraVisualIntersection(TankPartIntersectionWithCameraMapVisibleNode tank, CameraNode camera)
        {
            Vector3 position = camera.cameraRootTransform.Root.position;
            TankPartIntersectionWithCameraData[] tankPartIntersectionMap = tank.tankPartIntersectionWithCameraMap.TankPartIntersectionMap;
            int length = tankPartIntersectionMap.Length;
            for (int i = 0; i < length; i++)
            {
                TankPartIntersectionWithCameraData data = tankPartIntersectionMap[i];
                Collider collider = data.collider;
                Entity tankPart = data.entity;
                bool hasIntersection = this.CheckPointInsideCollider(position, collider);
                this.UpdateState(tankPart, hasIntersection);
            }
        }

        [OnEventFire]
        public void CheckCameraVisualIntersection(EarlyUpdateEvent evt, TankPartIntersectionWithCameraMapVisibleNode tank, [JoinAll] CameraNode camera)
        {
            this.CheckCameraVisualIntersection(tank, camera);
        }

        private bool CheckPointInsideCollider(Vector3 cameraPos, Collider collider) => 
            collider.bounds.Contains(cameraPos) ? this.MakeDeepIntersectionTest(cameraPos, collider) : false;

        [OnEventFire]
        public void InitCollidersForChecking(NodeAddedEvent evt, TankNode tank, [Context, JoinByTank] WeaponNode weapon)
        {
            VisualTriggerMarkerComponent visualTriggerMarker = tank.tankVisualRoot.VisualTriggerMarker;
            VisualTriggerMarkerComponent trigger = weapon.weaponVisualRoot.VisualTriggerMarker;
            TankPartIntersectionWithCameraData[] map = new TankPartIntersectionWithCameraData[2];
            this.AttachColliderToIntersectionMap(visualTriggerMarker, tank.Entity, map, 0);
            this.AttachColliderToIntersectionMap(trigger, weapon.Entity, map, 1);
            tank.Entity.AddComponent(new TankPartIntersectionWithCameraMapComponent(map));
        }

        private bool MakeDeepIntersectionTest(Vector3 cameraPos, Collider collider)
        {
            RaycastHit hit;
            Vector3 vector2 = collider.bounds.center - cameraPos;
            return !collider.Raycast(new Ray(cameraPos, vector2.normalized), out hit, vector2.magnitude);
        }

        [OnEventFire]
        public void ResetState(NodeRemoveEvent evt, TankPartIntersectionWithCameraMapVisibleNode tank)
        {
            TankPartIntersectionWithCameraData[] tankPartIntersectionMap = tank.tankPartIntersectionWithCameraMap.TankPartIntersectionMap;
            int length = tankPartIntersectionMap.Length;
            for (int i = 0; i < length; i++)
            {
                TankPartIntersectionWithCameraData data = tankPartIntersectionMap[i];
                Entity tankPart = data.entity;
                this.UpdateState(tankPart, false);
            }
        }

        private void UpdateState(Entity tankPart, bool hasIntersection)
        {
            if (!hasIntersection)
            {
                if (tankPart.HasComponent<TankPartIntersectedWithCameraStateComponent>())
                {
                    tankPart.RemoveComponent<TankPartIntersectedWithCameraStateComponent>();
                    tankPart.AddComponent<TankPartNotIntersectedWithCameraStateComponent>();
                }
            }
            else if (tankPart.HasComponent<TankPartNotIntersectedWithCameraStateComponent>())
            {
                tankPart.RemoveComponent<TankPartNotIntersectedWithCameraStateComponent>();
                tankPart.AddComponent<TankPartIntersectedWithCameraStateComponent>();
            }
        }

        public class CameraNode : Node
        {
            public BattleCameraComponent battleCamera;
            public CameraRootTransformComponent cameraRootTransform;
            public CameraComponent camera;
        }

        public class TankNode : Node
        {
            public AssembledTankInactiveStateComponent assembledTankInactiveState;
            public TankVisualRootComponent tankVisualRoot;
            public TankGroupComponent tankGroup;
        }

        public class TankPartIntersectionWithCameraMapVisibleNode : Node
        {
            public TankComponent tank;
            public TankPartIntersectionWithCameraMapComponent tankPartIntersectionWithCameraMap;
            public CameraVisibleTriggerComponent cameraVisibleTrigger;
        }

        public class WeaponNode : Node
        {
            public WeaponComponent weapon;
            public WeaponInstanceComponent weaponInstance;
            public WeaponVisualRootComponent weaponVisualRoot;
            public TankGroupComponent tankGroup;
        }
    }
}

