namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Battle.ClientCore.API;

    public class ShaftImpactSystem : AbstractImpactSystem
    {
        [OnEventFire]
        public void ApplyBaseHitImpact(HitEvent evt, ShaftAimingImpactNode weapon)
        {
            List<HitTarget> targets = evt.Targets;
            int count = targets.Count;
            float impactForce = weapon.impact.ImpactForce;
            for (int i = 0; i < count; i++)
            {
                HitTarget target = targets[i];
                base.PrepareImpactForHitTarget(weapon.Entity, target, impactForce, 1f);
            }
        }

        [OnEventFire]
        public void MakeAimingHitImpact(RemoteShaftAimingHitEvent evt, ShaftAimingImpactNode weapon)
        {
            this.MakeImpactOnAnyAimingShot(evt.HitPower, evt.Targets, weapon);
        }

        [OnEventFire]
        public void MakeAimingHitImpact(SelfShaftAimingHitEvent evt, ShaftAimingImpactNode weapon)
        {
            this.MakeImpactOnAnyAimingShot(evt.HitPower, evt.Targets, weapon);
        }

        private void MakeImpactOnAnyAimingShot(float aimingHitPower, List<HitTarget> targets, ShaftAimingImpactNode weapon)
        {
            float maxImpactForce = (weapon.shaftAimingImpact.MaxImpactForce - weapon.impact.ImpactForce) * aimingHitPower;
            int count = targets.Count;
            for (int i = 0; i < count; i++)
            {
                HitTarget target = targets[i];
                base.PrepareImpactForHitTarget(weapon.Entity, target, maxImpactForce, 1f);
            }
        }

        public class ShaftAimingImpactNode : Node
        {
            public ShaftAimingImpactComponent shaftAimingImpact;
            public ImpactComponent impact;
        }
    }
}

