namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Tanks.Battle.ClientCore.API;

    public class DirectionEvaluatorSystem : ECSSystem
    {
        [OnEventComplete]
        public void EvaluateDirections(TargetingEvaluateEvent evt, EvaluatorNode evaluator)
        {
            List<DirectionData> directions = evt.TargetingData.Directions;
            float minValue = float.MinValue;
            TargetingData targetingData = evt.TargetingData;
            float num2 = !evaluator.Entity.HasComponent<DamageWeakeningByTargetComponent>() ? 1f : (((EntityInternal) evaluator.Entity).GetComponent<DamageWeakeningByTargetComponent>().DamagePercent / 100f);
            if ((directions != null) && (directions.Count != 0))
            {
                DirectionData data2 = directions.First<DirectionData>();
                int num3 = 0;
                int num4 = 0;
                while (num4 < targetingData.Directions.Count)
                {
                    DirectionData data3 = targetingData.Directions[num4];
                    bool flag = false;
                    List<TargetData>.Enumerator enumerator = data3.Targets.GetEnumerator();
                    while (true)
                    {
                        if (!enumerator.MoveNext())
                        {
                            if (flag && (data3.Priority > minValue))
                            {
                                data2 = data3;
                                num3 = num4;
                                minValue = data2.Priority;
                            }
                            num4++;
                            break;
                        }
                        TargetData current = enumerator.Current;
                        if (current.ValidTarget)
                        {
                            flag = true;
                            data3.Priority += current.Priority * ((float) Math.Pow((double) num2, (double) current.PriorityWeakeningCount));
                        }
                    }
                }
                evt.TargetingData.BestDirection = data2;
                evt.TargetingData.BestDirectionIndex = num3;
            }
        }

        public class EvaluatorNode : Node
        {
            public DirectionEvaluatorComponent directionEvaluator;
        }
    }
}

