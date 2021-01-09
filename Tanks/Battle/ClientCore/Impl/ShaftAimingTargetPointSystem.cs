namespace Tanks.Battle.ClientCore.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class ShaftAimingTargetPointSystem : ECSSystem
    {
        private float EPS_ACTIVE = 0.25f;
        private float EPS_INACTIVE = 0.001f;

        [OnEventFire]
        public void CheckTargetPoint(FixedUpdateEvent evt, ShaftAimingTargetPointWorkingStateNode weapon, [JoinByTank] SelfTankNode selfTank, [JoinAll] ICollection<SingleNode<TankPartIntersectedWithCameraStateComponent>> intersectedTankParts)
        {
            ShaftAimingTargetPointComponent shaftAimingTargetPoint = weapon.shaftAimingTargetPoint;
            ShaftAimingTargetPointContainerComponent shaftAimingTargetPointContainer = weapon.shaftAimingTargetPointContainer;
            MuzzleLogicAccessor accessor = new MuzzleLogicAccessor(weapon.muzzlePoint, weapon.weaponInstance);
            bool isInsideTankPart = weapon.shaftAimingTargetPoint.IsInsideTankPart;
            Vector3 barrelOriginWorld = accessor.GetBarrelOriginWorld();
            float verticalAngle = weapon.shaftAimingWorkingState.VerticalAngle;
            Vector3 point = weapon.shaftAimingTargetPoint.Point;
            shaftAimingTargetPointContainer.Point = this.GetTargetPoint(barrelOriginWorld, weapon.shaftAimingWorkingState.WorkingDirection, weapon.verticalSectorsTargeting.WorkDistance, weapon.targetCollector);
            shaftAimingTargetPointContainer.IsInsideTankPart = intersectedTankParts.Count > 1;
            this.CheckTargetPointDiff(point, verticalAngle, shaftAimingTargetPoint, shaftAimingTargetPointContainer, isInsideTankPart, !weapon.shaftAimingWorkingState.IsActive ? this.EPS_INACTIVE : this.EPS_ACTIVE);
        }

        private void CheckTargetPointDiff(Vector3 currentPoint, float currentVerticalAngle, ShaftAimingTargetPointComponent targetPointComponent, ShaftAimingTargetPointContainerComponent targetPointContainerComponent, bool currentIntersectionTankPartStatus, float eps)
        {
            Vector3 point = targetPointContainerComponent.Point;
            bool isInsideTankPart = targetPointContainerComponent.IsInsideTankPart;
            if (currentIntersectionTankPartStatus != isInsideTankPart)
            {
                this.UpdateAndShareTargetPoint(targetPointComponent, targetPointContainerComponent, point, isInsideTankPart, currentVerticalAngle);
            }
            else if ((targetPointContainerComponent.PrevVerticalAngle != currentVerticalAngle) && !MathUtil.NearlyEqual(point, currentPoint, eps))
            {
                this.UpdateAndShareTargetPoint(targetPointComponent, targetPointContainerComponent, point, isInsideTankPart, currentVerticalAngle);
            }
        }

        [OnEventFire]
        public void CreateTargetPoint(NodeAddedEvent evt, ShaftAimingWorkingStateNode weapon, [JoinByTank] SelfTankNode selfTank, [JoinAll] ICollection<SingleNode<TankPartIntersectedWithCameraStateComponent>> intersectedTankParts)
        {
            bool flag = intersectedTankParts.Count > 1;
            Vector3 barrelOriginWorld = new MuzzleLogicAccessor(weapon.muzzlePoint, weapon.weaponInstance).GetBarrelOriginWorld();
            Vector3 vector3 = this.GetTargetPoint(barrelOriginWorld, weapon.shaftAimingWorkingState.WorkingDirection, weapon.verticalSectorsTargeting.WorkDistance, weapon.targetCollector);
            ShaftAimingTargetPointContainerComponent component = new ShaftAimingTargetPointContainerComponent {
                Point = vector3,
                IsInsideTankPart = flag,
                PrevVerticalAngle = weapon.shaftAimingWorkingState.VerticalAngle
            };
            weapon.Entity.AddComponent(component);
            ShaftAimingTargetPointComponent component3 = new ShaftAimingTargetPointComponent {
                Point = vector3,
                IsInsideTankPart = flag
            };
            weapon.Entity.AddComponent(component3);
        }

        private Vector3 GetTargetPoint(Vector3 start, Vector3 dir, float length, TargetCollectorComponent targetCollector)
        {
            DirectionData data = targetCollector.Collect(start, dir, length, LayerMasks.VISUAL_TARGETING);
            return (!data.HasAnyHit() ? (start + (dir * length)) : data.FirstAnyHitPosition());
        }

        [OnEventFire]
        public void RemoveTargetPoint(NodeRemoveEvent evt, ShaftAimingWorkingStateNode weapon, [JoinByUser] SingleNode<SelfUserComponent> selfUser)
        {
            weapon.Entity.RemoveComponent<ShaftAimingTargetPointContainerComponent>();
            weapon.Entity.RemoveComponent<ShaftAimingTargetPointComponent>();
        }

        private void UpdateAndShareTargetPoint(ShaftAimingTargetPointComponent targetPointComponent, ShaftAimingTargetPointContainerComponent targetPointContainerComponent, Vector3 actualPoint, bool isInsideTankPart, float currentVerticalAngle)
        {
            targetPointContainerComponent.PrevVerticalAngle = currentVerticalAngle;
            targetPointComponent.Point = actualPoint;
            targetPointComponent.IsInsideTankPart = isInsideTankPart;
            targetPointComponent.OnChange();
        }

        public class SelfTankNode : Node
        {
            public TankCollidersComponent tankColliders;
            public SelfTankComponent selfTank;
            public TankGroupComponent tankGroup;
        }

        public class ShaftAimingTargetPointWorkingStateNode : Node
        {
            public MuzzlePointComponent muzzlePoint;
            public ShaftAimingWorkingStateComponent shaftAimingWorkingState;
            public ShaftAimingTargetPointComponent shaftAimingTargetPoint;
            public ShaftAimingTargetPointContainerComponent shaftAimingTargetPointContainer;
            public VerticalSectorsTargetingComponent verticalSectorsTargeting;
            public TankGroupComponent tankGroup;
            public WeaponInstanceComponent weaponInstance;
            public TargetCollectorComponent targetCollector;
        }

        public class ShaftAimingWorkingStateNode : Node
        {
            public MuzzlePointComponent muzzlePoint;
            public ShaftAimingWorkingStateComponent shaftAimingWorkingState;
            public VerticalSectorsTargetingComponent verticalSectorsTargeting;
            public TankGroupComponent tankGroup;
            public WeaponInstanceComponent weaponInstance;
            public TargetCollectorComponent targetCollector;
        }
    }
}

