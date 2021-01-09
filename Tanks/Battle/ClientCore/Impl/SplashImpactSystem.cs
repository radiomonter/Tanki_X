namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class SplashImpactSystem : AbstractImpactSystem
    {
        [OnEventFire]
        public void CalculateAndSendSplashImpactEffect(RemoteSplashHitEvent evt, SplashImpactNode weapon, [JoinByTank] TankPhysicsNode tank)
        {
            this.CalculateAndSendSplashImpactEffectByBaseEvent(evt.SplashTargets, evt.StaticHit, evt.Targets, weapon, tank);
        }

        [OnEventFire]
        public void CalculateAndSendSplashImpactEffect(SelfSplashHitEvent evt, SplashImpactNode weapon, [JoinByTank] TankPhysicsNode tank)
        {
            this.CalculateAndSendSplashImpactEffectByBaseEvent(evt.SplashTargets, evt.StaticHit, evt.Targets, weapon, tank);
        }

        private void CalculateAndSendSplashImpactEffectByBaseEvent(List<HitTarget> splashTargets, StaticHit staticHit, List<HitTarget> targets, SplashImpactNode weapon, TankPhysicsNode tank)
        {
            SplashImpactComponent splashImpact = weapon.splashImpact;
            SplashWeaponComponent splashWeapon = weapon.splashWeapon;
            Vector3 vector = (staticHit == null) ? targets[0].TargetPosition : staticHit.Position;
            float impactWeakeningByRange = 1f;
            if (weapon.Entity.HasComponent<DamageWeakeningByDistanceComponent>())
            {
                float magnitude = (tank.rigidBody.Rigidbody.position - vector).magnitude;
                impactWeakeningByRange = base.GetImpactWeakeningByRange(magnitude, weapon.Entity.GetComponent<DamageWeakeningByDistanceComponent>());
            }
            foreach (HitTarget target in splashTargets)
            {
                float hitDistance = target.HitDistance;
                float splashImpactWeakeningByRange = this.GetSplashImpactWeakeningByRange(hitDistance, splashWeapon);
                ImpactEvent eventInstance = new ImpactEvent();
                Vector3 vector4 = (Vector3.Normalize(target.HitDirection) * splashImpact.ImpactForce) * WeaponConstants.WEAPON_FORCE_MULTIPLIER;
                eventInstance.Force = (vector4 * impactWeakeningByRange) * splashImpactWeakeningByRange;
                eventInstance.LocalHitPoint = target.LocalHitPoint;
                eventInstance.WeakeningCoeff = splashImpactWeakeningByRange;
                Entity[] entities = new Entity[] { weapon.Entity, target.Entity };
                base.NewEvent(eventInstance).AttachAll(entities).Schedule();
            }
        }

        private float GetSplashImpactWeakeningByRange(float distance, SplashWeaponComponent splashWeapon)
        {
            float radiusOfMaxSplashDamage = splashWeapon.RadiusOfMaxSplashDamage;
            float radiusOfMinSplashDamage = splashWeapon.RadiusOfMinSplashDamage;
            float minSplashDamagePercent = splashWeapon.MinSplashDamagePercent;
            return ((distance >= radiusOfMaxSplashDamage) ? ((distance <= radiusOfMinSplashDamage) ? (0.01f * (minSplashDamagePercent + (((radiusOfMinSplashDamage - distance) * (100f - minSplashDamagePercent)) / (radiusOfMinSplashDamage - radiusOfMaxSplashDamage)))) : 0f) : 1f);
        }

        public class SplashImpactNode : Node
        {
            public SplashImpactComponent splashImpact;
            public TankGroupComponent tankGroup;
            public SplashWeaponComponent splashWeapon;
        }

        public class TankPhysicsNode : Node
        {
            public TankGroupComponent tankGroup;
            public TankComponent tank;
            public RigidbodyComponent rigidBody;
        }
    }
}

