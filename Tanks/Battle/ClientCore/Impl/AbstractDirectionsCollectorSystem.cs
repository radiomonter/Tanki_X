namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public abstract class AbstractDirectionsCollectorSystem : ECSSystem
    {
        private const float MAXIMUM_RAYS_PER_DEG = 8f;
        private const float MIN_TANK_SIZE = 0.9f;

        protected AbstractDirectionsCollectorSystem()
        {
        }

        protected float CalculateRaysPerDeg(float distance) => 
            (float) Math.Min((double) 8.0, (double) (1.0 / (2.0 * Math.Atan((double) (0.9f / (2f * distance))))));

        protected static DirectionData CollectDirection(Vector3 origin, Vector3 dir, float angle, TargetingData targetingData)
        {
            DirectionData item = BattleCache.directionData.GetInstance().Init(origin, dir, angle);
            targetingData.Directions.Add(item);
            return item;
        }

        public static void CollectExtraDirection(Vector3 origin, Vector3 dir, float angle, TargetingData targetingData)
        {
            CollectDirection(origin, dir, angle, targetingData).Extra = true;
        }

        protected void CollectSectorDirections(Vector3 origin, Vector3 dir, Vector3 rotationAxis, float angleStep, int numRays, TargetingData targetingData)
        {
            float f = 0f;
            for (int i = 1; i <= numRays; i++)
            {
                f += angleStep;
                Vector3 vector = (Vector3) (Quaternion.AngleAxis(-f, rotationAxis) * dir);
                CollectDirection(origin, vector, Mathf.Abs(f), targetingData);
            }
        }

        [Inject]
        public static BattleFlowInstancesCache BattleCache { get; set; }
    }
}

