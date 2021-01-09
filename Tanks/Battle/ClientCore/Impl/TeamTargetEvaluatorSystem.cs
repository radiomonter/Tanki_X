namespace Tanks.Battle.ClientCore.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Battle.ClientCore.API;

    public class TeamTargetEvaluatorSystem : ECSSystem
    {
        [OnEventFire]
        public void EvaluateTargets(TargetingEvaluateEvent evt, EvaluatorNode evaluator, [JoinByUser] TankNode tankNode, [JoinByTeam] TeamNode team)
        {
            long key = team.teamGroup.Key;
            List<DirectionData>.Enumerator enumerator = evt.TargetingData.Directions.GetEnumerator();
            while (enumerator.MoveNext())
            {
                DirectionData current = enumerator.Current;
                List<TargetData>.Enumerator enumerator2 = current.Targets.GetEnumerator();
                while (enumerator2.MoveNext())
                {
                    TargetData data3 = enumerator2.Current;
                    if (data3.TargetEntity.GetComponent<TeamGroupComponent>().Key == key)
                    {
                        data3.ValidTarget = false;
                    }
                }
            }
        }

        public class EvaluatorNode : Node
        {
            public TeamTargetEvaluatorComponent teamTargetEvaluator;
        }

        public class TankNode : Node
        {
            public TankComponent tank;
            public TeamGroupComponent teamGroup;
        }

        public class TeamNode : Node
        {
            public TeamComponent team;
            public TeamGroupComponent teamGroup;
        }
    }
}

