namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class BulletTargetingSystem : AbstractDirectionsCollectorSystem
    {
        [OnEventFire]
        public void PrepareTargeting(TargetingEvent evt, BulletTargetingNode barelTargetingNode)
        {
            TargetingData targetingData = evt.TargetingData;
            BulletTargetingComponent bulletTargeting = barelTargetingNode.bulletTargeting;
            CollectDirection(targetingData.Origin, targetingData.Dir, 0f, targetingData);
            Vector3 normalized = Vector3.Cross(targetingData.Dir, Vector3.up).normalized;
            float num = 360f / bulletTargeting.RadialRaysCount;
            for (int i = 0; i < bulletTargeting.RadialRaysCount; i++)
            {
                Vector3 vector3 = (Vector3) (Quaternion.AngleAxis(num * i, targetingData.Dir) * normalized);
                Vector3 origin = targetingData.Origin + (vector3 * bulletTargeting.Radius);
                CollectDirection(origin, targetingData.Dir, 0f, targetingData);
            }
            base.ScheduleEvent(BattleCache.collectTargetsEvent.GetInstance().Init(targetingData), barelTargetingNode);
            base.ScheduleEvent(BattleCache.targetEvaluateEvent.GetInstance().Init(targetingData), barelTargetingNode);
        }

        [Inject]
        public static BattleFlowInstancesCache BattleCache { get; set; }

        public class BulletTargetingNode : Node
        {
            public BulletTargetingComponent bulletTargeting;
        }
    }
}

