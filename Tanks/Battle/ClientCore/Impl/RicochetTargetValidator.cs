namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public class RicochetTargetValidator : TargetValidator
    {
        private float bulletRadius;

        public RicochetTargetValidator(Entity ownerEntity, float bulletRadius) : base(ownerEntity)
        {
            this.bulletRadius = bulletRadius;
        }

        private Ray CalculateRicochet(Ray ray, Vector3 hitNormal, float distance)
        {
            Vector3 direction = ray.direction;
            return new Ray { 
                origin = ray.GetPoint(distance - this.bulletRadius),
                direction = (direction - ((2f * Vector3.Dot(direction, hitNormal)) * hitNormal)).normalized
            };
        }

        public override bool CanSkip(Entity targetEntity) => 
            targetEntity.Equals(base.ownerEntity) || targetEntity.IsSameGroup<TeamGroupComponent>(base.ownerEntity);

        public override Ray ContinueOnStaticHit(Ray ray, Vector3 hitNormal, float distance) => 
            this.CalculateRicochet(ray, hitNormal, distance);

        public override Ray ContinueOnTargetHit(Ray ray, Vector3 hitNormal, float distance) => 
            this.CalculateRicochet(ray, hitNormal, distance);
    }
}

