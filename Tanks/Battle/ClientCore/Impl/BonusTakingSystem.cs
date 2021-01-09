namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientGraphics.Impl;

    public class BonusTakingSystem : ECSSystem
    {
        [OnEventComplete]
        public void DestroyBonusBox(BonusTakenEvent e, SingleNode<BonusBoxInstanceComponent> bonus)
        {
            bonus.component.BonusBoxInstance.RecycleObject();
        }

        [OnEventFire]
        public void TakeBonus(TriggerEnterEvent e, SingleNode<BonusActiveStateComponent> bonus, SelfTankNode tank)
        {
            base.ScheduleEvent<SendTankMovementEvent>(tank);
            base.NewEvent<BonusTakingRequestEvent>().Attach(bonus).Attach(tank).Schedule();
        }

        [Not(typeof(BonusActiveStateComponent))]
        public class NotActiveBonusNode : Node
        {
            public BonusComponent bonus;
        }

        public class SelfTankNode : Node
        {
            public TankSyncComponent tankSync;
            public TankActiveStateComponent tankActiveState;
        }
    }
}

