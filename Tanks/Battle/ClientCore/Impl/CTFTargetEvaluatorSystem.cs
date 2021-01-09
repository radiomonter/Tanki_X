namespace Tanks.Battle.ClientCore.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;

    public class CTFTargetEvaluatorSystem : ECSSystem
    {
        [OnEventFire]
        public void EvaluateTargets(TargetingEvaluateEvent evt, EvaluatorNode evaluator, [JoinByUser] TankNode tankNode)
        {
            foreach (DirectionData data in evt.TargetingData.Directions)
            {
                foreach (TargetData data2 in data.Targets)
                {
                    base.NewEvent(GetFlagTargetBonusEvent.INSTANCE).Attach(data2.TargetEntity).Attach(evaluator).Schedule();
                    data2.Priority += GetFlagTargetBonusEvent.INSTANCE.Value;
                }
            }
        }

        [OnEventFire]
        public void GetFlagTargetBonus(GetFlagTargetBonusEvent e, EvaluatorNode evaluator, TankNode tank, [Combine, JoinByTank] FlagNode flag)
        {
            e.Value = evaluator.ctfTargetEvaluator.FlagCarrierPriorityBonus;
        }

        public class EvaluatorNode : Node
        {
            public CTFTargetEvaluatorComponent ctfTargetEvaluator;
        }

        public class FlagNode : Node
        {
            public FlagComponent flag;
            public TankGroupComponent tankGroup;
        }

        public class GetFlagTargetBonusEvent : Event
        {
            public static CTFTargetEvaluatorSystem.GetFlagTargetBonusEvent INSTANCE = new CTFTargetEvaluatorSystem.GetFlagTargetBonusEvent();

            private GetFlagTargetBonusEvent()
            {
                this.Value = 0f;
            }

            public float Value { get; set; }
        }

        public class TankNode : Node
        {
            public TankComponent tank;
            public TankGroupComponent tankGroup;
        }
    }
}

