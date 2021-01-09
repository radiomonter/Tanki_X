namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class DiscreteImpactSystem : AbstractImpactSystem
    {
        [OnEventFire]
        public void Impact(ImpactEvent evt, TankNode tank)
        {
            Rigidbody rigidbody = tank.rigidbody.Rigidbody;
            Vector3 position = MathUtil.LocalPositionToWorldPosition(evt.LocalHitPoint, rigidbody.gameObject);
            rigidbody.AddForceAtPositionSafe(evt.Force, position);
        }

        [OnEventFire]
        public void PrepareImpact(HitEvent evt, ImpactWeakeningNode weapon)
        {
            this.PrepareImpactByBaseHitEvent(evt, weapon);
        }

        private void PrepareImpactByBaseHitEvent(HitEvent evt, ImpactWeakeningNode weapon)
        {
            DamageWeakeningByDistanceComponent damageWeakeningByDistance = weapon.damageWeakeningByDistance;
            List<HitTarget> targets = evt.Targets;
            int count = targets.Count;
            float impactForce = weapon.impact.ImpactForce;
            for (int i = 0; i < count; i++)
            {
                HitTarget target = targets[i];
                float hitDistance = target.HitDistance;
                float impactWeakeningByRange = base.GetImpactWeakeningByRange(hitDistance, damageWeakeningByDistance);
                base.PrepareImpactForHitTarget(weapon.Entity, target, impactForce, impactWeakeningByRange);
            }
        }

        public class ImpactWeakeningNode : Node
        {
            public ImpactComponent impact;
            public DamageWeakeningByDistanceComponent damageWeakeningByDistance;
            public DiscreteWeaponComponent discreteWeapon;
        }

        public class TankNode : Node
        {
            public TankComponent tank;
            public RigidbodyComponent rigidbody;
        }
    }
}

