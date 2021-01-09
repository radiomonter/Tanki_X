namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class SpiderDirectionCollectorSystem : AbstractDirectionsCollectorSystem
    {
        [OnEventFire]
        public void CollectDirections(TargetingEvent evt, TargetingNode targeting, [JoinAll] ICollection<ActiveRemoteTank> enemyTankNodes)
        {
            Rigidbody rigidbody = targeting.rigidbody.Rigidbody;
            Vector3 origin = rigidbody.position + (Vector3.up * 1.5f);
            TargetingData targetingData = evt.TargetingData;
            targetingData.FullDistance = targeting.unitTargetingConfig.WorkDistance;
            foreach (ActiveRemoteTank tank in enemyTankNodes)
            {
                Rigidbody rigidbody2 = tank.rigidbody.Rigidbody;
                Vector3 forward = rigidbody.transform.forward;
                Vector3 vector3 = rigidbody2.position - origin;
                if (vector3.magnitude <= targeting.unitTargetingConfig.WorkDistance)
                {
                    Vector3 normalized = (rigidbody2.position - origin).normalized;
                    CollectDirection(origin, normalized, Mathf.Acos(Vector3.Dot(forward, normalized)), targetingData);
                }
            }
            base.ScheduleEvent(AbstractDirectionsCollectorSystem.BattleCache.collectTargetsEvent.GetInstance().Init(targetingData), targeting);
            base.ScheduleEvent(AbstractDirectionsCollectorSystem.BattleCache.targetEvaluateEvent.GetInstance().Init(targetingData), targeting);
        }

        public class ActiveRemoteTank : Node
        {
            public TankComponent tank;
            public RemoteTankComponent remoteTank;
            public TankActiveStateComponent tankActiveState;
            public RigidbodyComponent rigidbody;
        }

        public class TargetingNode : Node
        {
            public UnitTargetingConfigComponent unitTargetingConfig;
            public UnitReadyComponent unitReady;
            public RigidbodyComponent rigidbody;
        }
    }
}

