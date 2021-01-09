namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class AbstractImpactSystem : ECSSystem
    {
        private const float DEFAULT_WEAKENING_COEFF = 1f;
        private const float PERCENT_MULTIPLIER = 0.01f;

        protected void ApplyImpactByTargetWeakening(Entity weaponDamager, List<HitTarget> targets, float forceVal, float weakeningByTargetPercent)
        {
            float num = 1f;
            float num2 = weakeningByTargetPercent * 0.01f;
            int count = targets.Count;
            for (int i = 0; i < count; i++)
            {
                HitTarget target = targets[i];
                ImpactEvent eventInstance = new ImpactEvent();
                Vector3 vector = (Vector3.Normalize(target.HitDirection) * forceVal) * WeaponConstants.WEAPON_FORCE_MULTIPLIER;
                eventInstance.Force = vector * num;
                eventInstance.LocalHitPoint = target.LocalHitPoint;
                eventInstance.WeakeningCoeff = num;
                num *= num2;
                Entity[] entities = new Entity[] { weaponDamager, target.Entity };
                base.NewEvent(eventInstance).AttachAll(entities).Schedule();
            }
        }

        protected float GetImpactWeakeningByRange(float distance, DamageWeakeningByDistanceComponent weakeningConfig)
        {
            float minDamagePercent = weakeningConfig.MinDamagePercent;
            float radiusOfMaxDamage = weakeningConfig.RadiusOfMaxDamage;
            float radiusOfMinDamage = weakeningConfig.RadiusOfMinDamage;
            float num4 = radiusOfMinDamage - radiusOfMaxDamage;
            return ((num4 > 0f) ? ((distance > radiusOfMaxDamage) ? ((distance < radiusOfMinDamage) ? (0.01f * (minDamagePercent + (((radiusOfMinDamage - distance) * (100f - minDamagePercent)) / num4))) : (0.01f * minDamagePercent)) : 1f) : 1f);
        }

        protected void PrepareImpactForHitTarget(Entity weaponDamager, HitTarget target, float maxImpactForce, float weakeningCoeff = 1f)
        {
            ImpactEvent eventInstance = new ImpactEvent {
                Force = ((Vector3.Normalize(target.HitDirection) * maxImpactForce) * WeaponConstants.WEAPON_FORCE_MULTIPLIER) * weakeningCoeff,
                LocalHitPoint = target.LocalHitPoint,
                WeakeningCoeff = weakeningCoeff
            };
            Entity[] entities = new Entity[] { target.Entity, weaponDamager };
            base.NewEvent(eventInstance).AttachAll(entities).Schedule();
        }
    }
}

