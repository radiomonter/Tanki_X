namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Battle.ClientCore.API;

    public class DistanceAndAngleTargetEvaluatorSystem : ECSSystem
    {
        public const float MAX_PRIORITY = 1000f;

        [OnEventFire]
        public void EvaluateTargets(TargetingEvaluateEvent evt, EvaluatorNode evaluator)
        {
            TargetingData targetingData = evt.TargetingData;
            List<DirectionData>.Enumerator enumerator = targetingData.Directions.GetEnumerator();
            while (enumerator.MoveNext())
            {
                DirectionData current = enumerator.Current;
                List<TargetData>.Enumerator enumerator2 = current.Targets.GetEnumerator();
                while (enumerator2.MoveNext())
                {
                    TargetData data3 = enumerator2.Current;
                    float num = (evaluator.distanceAndAngleTargetEvaluator.DistanceWeight * data3.HitDistance) / targetingData.FullDistance;
                    float num2 = (evaluator.distanceAndAngleTargetEvaluator.AngleWeight * current.Angle) / targetingData.MaxAngle;
                    data3.Priority += 1000f - (num + num2);
                }
            }
        }

        public class EvaluatorNode : Node
        {
            public DistanceAndAngleTargetEvaluatorComponent distanceAndAngleTargetEvaluator;
        }
    }
}

