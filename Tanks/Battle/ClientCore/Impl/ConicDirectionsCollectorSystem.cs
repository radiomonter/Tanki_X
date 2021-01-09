namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class ConicDirectionsCollectorSystem : AbstractDirectionsCollectorSystem
    {
        [OnEventFire]
        public void CollectDirections(CollectDirectionsEvent evt, TargetingNode conicTargeting)
        {
            TargetingData targetingData = evt.TargetingData;
            ConicTargetingComponent component = conicTargeting.conicTargeting;
            CollectDirection(targetingData.Origin, targetingData.Dir, 0f, targetingData);
            float angleStep = component.HalfConeAngle / ((float) component.HalfConeNumRays);
            Vector3 leftDirectionWorld = new MuzzleLogicAccessor(conicTargeting.muzzlePoint, conicTargeting.weaponInstance).GetLeftDirectionWorld();
            for (int i = 0; i < component.NumSteps; i++)
            {
                base.CollectSectorDirections(targetingData.Origin, targetingData.Dir, leftDirectionWorld, angleStep, component.HalfConeNumRays, targetingData);
                base.CollectSectorDirections(targetingData.Origin, targetingData.Dir, leftDirectionWorld, -angleStep, component.HalfConeNumRays, targetingData);
                leftDirectionWorld = (Vector3) (Quaternion.AngleAxis(180f / ((float) component.NumSteps), targetingData.Dir) * leftDirectionWorld);
            }
        }

        public class TargetingNode : Node
        {
            public ConicTargetingComponent conicTargeting;
            public MuzzlePointComponent muzzlePoint;
            public WeaponInstanceComponent weaponInstance;
        }
    }
}

