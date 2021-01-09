namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class HammerTargetCollectorSystem : ECSSystem
    {
        private void CollectPelletTargets(TargetingData targetingData, DirectionData directionData, TargetCollectorNode targetCollectorNode)
        {
            MuzzlePointComponent muzzlePoint = targetCollectorNode.muzzlePoint;
            Vector3 dir = directionData.Dir;
            Vector3 localDirection = muzzlePoint.Current.InverseTransformVector(dir);
            Vector3[] vectorArray = PelletDirectionsCalculator.GetRandomDirections(targetCollectorNode.hammerPelletCone, muzzlePoint.Current.rotation, localDirection);
            for (int i = 0; i < vectorArray.Length; i++)
            {
                directionData.Dir = vectorArray[i];
                targetCollectorNode.hammerTargetCollector.Collect(targetingData.FullDistance, directionData, 0);
            }
            directionData.Dir = dir;
        }

        [OnEventFire]
        public void CollectTargetsOnDirections(CollectTargetsEvent evt, TargetCollectorNode targetCollectorNode, [JoinByTank] SingleNode<TankComponent> tank)
        {
            TargetingData targetingData = evt.TargetingData;
            List<DirectionData> directions = targetingData.Directions;
            int count = targetingData.Directions.Count;
            for (int i = 0; i < count; i++)
            {
                DirectionData directionData = directions[i];
                directionData.Clean();
                this.CollectPelletTargets(targetingData, directionData, targetCollectorNode);
            }
        }

        public class TargetCollectorNode : Node
        {
            public TankGroupComponent tankGroup;
            public HammerPelletConeComponent hammerPelletCone;
            public HammerTargetCollectorComponent hammerTargetCollector;
            public MuzzlePointComponent muzzlePoint;
        }
    }
}

