namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class TargetValidator
    {
        public static float STRIKE_EPSILON = 0.05f;
        protected int hitCount;
        protected Entity ownerEntity;

        public TargetValidator(Entity ownerEntity)
        {
            this.LayerMask = LayerMasks.GUN_TARGETING_WITH_DEAD_UNITS;
            this.ownerEntity = ownerEntity;
            this.ExcludeSelf = true;
            this.ExcludeDead = true;
        }

        public virtual bool AcceptAsTarget(Entity targetEntity)
        {
            if (this.ExcludeDead && !targetEntity.HasComponent<TankActiveStateComponent>())
            {
                return false;
            }
            this.hitCount++;
            return true;
        }

        public virtual void Begin()
        {
            this.hitCount = 0;
        }

        public virtual bool BreakOnStaticHit() => 
            true;

        public virtual bool BreakOnTargetHit(Entity target) => 
            true;

        public virtual bool CanSkip(Entity targetEntity) => 
            this.ExcludeSelf && targetEntity.Equals(this.ownerEntity);

        public virtual unsafe Ray Continue(Ray direction, float distance)
        {
            Ray* rayPtr1 = &direction;
            rayPtr1.origin += direction.direction * (distance + STRIKE_EPSILON);
            return direction;
        }

        public virtual unsafe Ray ContinueOnStaticHit(Ray direction, Vector3 hitNormal, float distance)
        {
            Ray* rayPtr1 = &direction;
            rayPtr1.origin += direction.direction * (distance + STRIKE_EPSILON);
            return direction;
        }

        public virtual unsafe Ray ContinueOnTargetHit(Ray direction, Vector3 hitNormal, float distance)
        {
            Ray* rayPtr1 = &direction;
            rayPtr1.origin += direction.direction * (distance + STRIKE_EPSILON);
            return direction;
        }

        public virtual void FillTargetData(TargetData targetData, RaycastHit hitInfo, GameObject hitRootGo, Ray ray, float fullDistance)
        {
            targetData.HitPoint = hitInfo.point;
            targetData.LocalHitPoint = MathUtil.WorldPositionToLocalPosition(PhysicsUtil.GetPulledHitPoint(hitInfo), hitRootGo);
            targetData.TargetPosition = hitRootGo.transform.position;
            targetData.HitDistance = fullDistance;
            targetData.HitDirection = ray.direction;
            targetData.PriorityWeakeningCount = this.hitCount - 1;
        }

        public int LayerMask { get; set; }

        public bool ExcludeSelf { get; set; }

        public bool ExcludeDead { get; set; }
    }
}

