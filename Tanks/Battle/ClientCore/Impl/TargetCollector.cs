namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class TargetCollector
    {
        public static int SAFE_ITERATION_LIMIT = 20;
        private Entity ownerEntity;

        public TargetCollector(Entity ownerEntity)
        {
            this.ownerEntity = ownerEntity;
        }

        public void Collect(TargetValidator validator, TargetingData targetingData, int layerMask = 0)
        {
            foreach (DirectionData data in targetingData.Directions)
            {
                this.Collect(validator, targetingData.FullDistance, data, layerMask);
            }
        }

        public void Collect(TargetValidator validator, float fullDistance, DirectionData direction, int layerMask = 0)
        {
            Ray ray = new Ray {
                origin = direction.Origin,
                direction = direction.Dir
            };
            float num = 0f;
            validator.Begin();
            Entity entity = null;
            HashSet<Entity> set = new HashSet<Entity>();
            int num2 = SAFE_ITERATION_LIMIT;
            int num3 = (layerMask == 0) ? validator.LayerMask : layerMask;
            while (true)
            {
                RaycastHit hit;
                if ((num2 > 0) && Physics.Raycast(ray, out hit, fullDistance - num, num3))
                {
                    StaticHit hit2;
                    num2--;
                    float distance = hit.distance;
                    num += distance;
                    Rigidbody rigidbody = hit.rigidbody;
                    TargetBehaviour behaviour = !rigidbody ? null : rigidbody.GetComponentInParent<TargetBehaviour>();
                    Vector3 normal = hit.normal;
                    if (!rigidbody || ((behaviour == null) || (behaviour.TargetEntity == null)))
                    {
                        hit2 = new StaticHit {
                            Position = PhysicsUtil.GetPulledHitPoint(hit),
                            Normal = hit.normal
                        };
                        direction.StaticHit = hit2;
                        if (!validator.BreakOnStaticHit())
                        {
                            ray = validator.ContinueOnStaticHit(ray, normal, distance);
                            continue;
                        }
                    }
                    else
                    {
                        Entity targetEntity = behaviour.TargetEntity;
                        if (set.Contains(targetEntity) || targetEntity.Equals(entity))
                        {
                            ray = validator.Continue(ray, hit.distance);
                            continue;
                        }
                        entity = targetEntity;
                        if (validator.CanSkip(targetEntity) || behaviour.CanSkip(this.ownerEntity))
                        {
                            ray = validator.Continue(ray, hit.distance);
                            continue;
                        }
                        if (validator.AcceptAsTarget(targetEntity) && behaviour.AcceptAsTarget(this.ownerEntity))
                        {
                            TargetData targetData = BattleCache.targetData.GetInstance().Init(behaviour.TargetEntity, behaviour.TargetIcarnationEntity);
                            validator.FillTargetData(targetData, hit, behaviour.gameObject, ray, num);
                            direction.Targets.Add(targetData);
                            set.Add(targetEntity);
                            if (!validator.BreakOnTargetHit(targetEntity))
                            {
                                ray = validator.ContinueOnTargetHit(ray, normal, distance);
                                continue;
                            }
                        }
                        else
                        {
                            hit2 = new StaticHit {
                                Position = PhysicsUtil.GetPulledHitPoint(hit),
                                Normal = hit.normal
                            };
                            direction.StaticHit = hit2;
                            if (!validator.BreakOnStaticHit())
                            {
                                ray = validator.ContinueOnStaticHit(ray, normal, distance);
                                continue;
                            }
                        }
                    }
                }
                return;
            }
        }

        public DirectionData Collect(TargetValidator validator, float fullDistance, Vector3 origin, Vector3 dir, int layerMask = 0)
        {
            DirectionData direction = BattleCache.directionData.GetInstance().Init(origin, dir, 0f);
            this.Collect(validator, fullDistance, direction, layerMask);
            return direction;
        }

        [Inject]
        public static BattleFlowInstancesCache BattleCache { get; set; }
    }
}

