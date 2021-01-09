namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class VerticalDirectionsCollectorSystem : AbstractDirectionsCollectorSystem
    {
        [OnEventFire]
        public void CollectDirections(CollectDirectionsEvent evt, VerticalTargetingNode verticalTargeting)
        {
            TargetingData targetingData = evt.TargetingData;
            VerticalTargetingComponent component = verticalTargeting.verticalTargeting;
            CollectDirection(targetingData.Origin, targetingData.Dir, 0f, targetingData);
            Vector3 leftDirectionWorld = new MuzzleLogicAccessor(verticalTargeting.muzzlePoint, verticalTargeting.weaponInstance).GetLeftDirectionWorld();
            if (component.NumRaysUp > 0)
            {
                base.CollectSectorDirections(targetingData.Origin, targetingData.Dir, leftDirectionWorld, component.AngleUp / ((float) component.NumRaysUp), component.NumRaysUp, targetingData);
            }
            if (component.NumRaysDown > 0)
            {
                base.CollectSectorDirections(targetingData.Origin, targetingData.Dir, leftDirectionWorld, -component.AngleDown / ((float) component.NumRaysDown), component.NumRaysDown, targetingData);
            }
        }

        public class VerticalTargetingNode : Node
        {
            public VerticalTargetingComponent verticalTargeting;
            public MuzzlePointComponent muzzlePoint;
            public WeaponInstanceComponent weaponInstance;
        }
    }
}

