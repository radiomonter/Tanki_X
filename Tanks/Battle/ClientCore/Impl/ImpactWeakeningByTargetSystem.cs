namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;

    public class ImpactWeakeningByTargetSystem : AbstractImpactSystem
    {
        [OnEventFire]
        public void PrepareImpactOnHit(HitEvent evt, ImpactNode weapon)
        {
            ImpactComponent impact = weapon.impact;
            DamageWeakeningByTargetComponent damageWeakeningByTarget = weapon.damageWeakeningByTarget;
            base.ApplyImpactByTargetWeakening(weapon.Entity, evt.Targets, impact.ImpactForce, damageWeakeningByTarget.DamagePercent);
        }

        public class ImpactNode : Node
        {
            public ImpactComponent impact;
            public DamageWeakeningByTargetComponent damageWeakeningByTarget;
        }
    }
}

