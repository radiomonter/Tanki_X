namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class VulcanImpactSystem : AbstractImpactSystem
    {
        [OnEventFire]
        public void ApplyVulcanImpact(VulcanImpactEvent evt, TankNode tank)
        {
            Rigidbody body = tank.rigidbody.Rigidbody;
            VulcanPhysicsUtils.ApplyVulcanForce(evt.Force, body, MathUtil.LocalPositionToWorldPosition(evt.LocalHitPoint, body.gameObject), tank.tankFalling, tank.track);
        }

        [OnEventFire]
        public void PrepareImpactOnShot(FixedUpdateEvent evt, ImpactNode weapon)
        {
            ImpactComponent impact = weapon.impact;
            DamageWeakeningByDistanceComponent damageWeakeningByDistance = weapon.damageWeakeningByDistance;
            HitTarget tankHit = weapon.streamHit.TankHit;
            if (tankHit != null)
            {
                float deltaTime = evt.DeltaTime;
                VulcanImpactEvent eventInstance = new VulcanImpactEvent();
                float hitDistance = tankHit.HitDistance;
                float impactWeakeningByRange = base.GetImpactWeakeningByRange(hitDistance, damageWeakeningByDistance);
                eventInstance.Force = (((Vector3.Normalize(tankHit.HitDirection) * impact.ImpactForce) * WeaponConstants.WEAPON_FORCE_MULTIPLIER) * deltaTime) * impactWeakeningByRange;
                eventInstance.LocalHitPoint = tankHit.LocalHitPoint;
                eventInstance.WeakeningCoeff = impactWeakeningByRange;
                Entity[] entities = new Entity[] { weapon.Entity, tankHit.Entity };
                base.NewEvent(eventInstance).AttachAll(entities).Schedule();
            }
        }

        public class ImpactNode : Node
        {
            public ImpactComponent impact;
            public DamageWeakeningByDistanceComponent damageWeakeningByDistance;
            public StreamHitComponent streamHit;
            public StreamHitTargetLoadedComponent streamHitTargetLoaded;
        }

        public class TankNode : Node
        {
            public RigidbodyComponent rigidbody;
            public TankFallingComponent tankFalling;
            public TrackComponent track;
        }
    }
}

