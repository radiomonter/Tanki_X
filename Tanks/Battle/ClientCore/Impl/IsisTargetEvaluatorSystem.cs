namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Battle.ClientCore.API;

    public class IsisTargetEvaluatorSystem : ECSSystem
    {
        [OnEventFire]
        public void AddComponent(NodeAddedEvent e, WeaponNode weapon)
        {
            weapon.Entity.AddComponent<IsisTargetEvaluatorComponent>();
        }

        [OnEventFire]
        public void ClearLastDirection(NodeRemoveEvent e, SingleNode<StreamHitComponent> weaponContext, [JoinSelf] SingleNode<IsisTargetEvaluatorComponent> weaponJoin)
        {
            base.Log.Debug("ClearLastDirection");
            weaponJoin.component.LastDirectionIndex = null;
        }

        [OnEventFire]
        public void EvaluateLastDirection(TargetingEvaluateEvent e, EvaluatorNode weapon)
        {
            int? lastDirectionIndex = weapon.isisTargetEvaluator.LastDirectionIndex;
            if (lastDirectionIndex != null)
            {
                base.Log.Debug("EvaluateLastDirection");
                DirectionData local1 = e.TargetingData.Directions[lastDirectionIndex.Value];
                local1.Priority += 10f;
            }
        }

        [OnEventFire]
        public void EvaluateLastTank(TargetingEvaluateEvent e, HitNode weapon)
        {
            List<DirectionData>.Enumerator enumerator = e.TargetingData.Directions.GetEnumerator();
            while (enumerator.MoveNext())
            {
                List<TargetData>.Enumerator enumerator2 = enumerator.Current.Targets.GetEnumerator();
                while (enumerator2.MoveNext())
                {
                    TargetData current = enumerator2.Current;
                    if (ReferenceEquals(current.TargetEntity, weapon.streamHit.TankHit.Entity))
                    {
                        base.Log.Debug("EvaluateLastTank");
                        current.Priority += 100f;
                    }
                }
            }
        }

        [OnEventFire]
        public void EvaluateTeamTank(IsisEvaluateTeamTankEvent e, WeaponNode weapon, TeamTankNode targetTank)
        {
            TargetData targetData = e.TargetData;
            if (e.ShooterTeamKey != targetTank.teamGroup.Key)
            {
                base.Log.Debug("EvaluateTeamTank: enemy team");
                targetData.Priority += 2f;
            }
            else
            {
                base.Log.Debug("EvaluateTeamTank: same team");
                targetData.Priority++;
                HealthComponent health = targetTank.health;
                if (health.CurrentHealth != health.MaxHealth)
                {
                    base.Log.Debug("EvaluateTeamTank: not full health");
                    targetData.Priority += 2f;
                }
                TemperatureComponent temperature = targetTank.temperature;
                if (temperature.Temperature > 0f)
                {
                    base.Log.Debug("EvaluateTeamTank: positive temperature");
                    targetData.Priority += 5f;
                }
                else if (temperature.Temperature < 0f)
                {
                    base.Log.Debug("EvaluateTeamTank: negative temperature");
                    targetData.Priority += 4f;
                }
            }
        }

        [OnEventFire]
        public void ResendEvaluateTeamTank(TargetingEvaluateEvent e, WeaponNode weapon, [JoinByTank] TeamTankNode shooterTank)
        {
            long key = shooterTank.teamGroup.Key;
            List<DirectionData>.Enumerator enumerator = e.TargetingData.Directions.GetEnumerator();
            while (enumerator.MoveNext())
            {
                DirectionData current = enumerator.Current;
                List<TargetData>.Enumerator enumerator2 = current.Targets.GetEnumerator();
                while (enumerator2.MoveNext())
                {
                    TargetData targetData = enumerator2.Current;
                    base.NewEvent(new IsisEvaluateTeamTankEvent(targetData, key)).Attach(weapon).Attach(targetData.TargetEntity).Schedule();
                }
            }
        }

        [OnEventComplete]
        public void SaveLastDirection(TargetingEvent e, EvaluatorNode weapon)
        {
            if (e.TargetingData.HasTargetHit())
            {
                base.Log.Debug("SaveLastDirection");
                weapon.isisTargetEvaluator.LastDirectionIndex = new int?(e.TargetingData.BestDirectionIndex);
            }
        }

        public class EvaluatorNode : Node
        {
            public IsisTargetEvaluatorComponent isisTargetEvaluator;
        }

        public class HitNode : Node
        {
            public IsisComponent isis;
            public StreamHitComponent streamHit;
            public IsisTargetEvaluatorComponent isisTargetEvaluator;
        }

        public class TeamTankNode : Node
        {
            public TankComponent tank;
            public TeamGroupComponent teamGroup;
            public HealthComponent health;
            public TemperatureComponent temperature;
        }

        public class WeaponNode : Node
        {
            public IsisComponent isis;
        }
    }
}

