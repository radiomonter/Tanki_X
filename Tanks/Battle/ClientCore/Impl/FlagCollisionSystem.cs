namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore;
    using Tanks.Battle.ClientCore.API;

    public class FlagCollisionSystem : ECSSystem
    {
        [OnEventFire]
        public void SendCollisionEvent(TankFlagCollisionEvent e, TankNode tank, [JoinByBattle] SingleNode<RoundActiveStateComponent> round, [Context, JoinByBattle] FlagNode flag)
        {
            base.ScheduleEvent<SendTankMovementEvent>(tank);
            base.NewEvent<FlagCollisionRequestEvent>().Attach(tank).Attach(flag).Schedule();
        }

        public class FlagNode : Node
        {
            public FlagComponent flag;
            public FlagInstanceComponent flagInstance;
            public TeamGroupComponent teamGroup;
            public FlagColliderComponent flagCollider;
            public BattleGroupComponent battleGroup;
        }

        public class TankNode : Node
        {
            public TankComponent tank;
            public TankActiveStateComponent tankActiveState;
            public TeamGroupComponent teamGroup;
            public TankSyncComponent tankSync;
            public BattleGroupComponent battleGroup;
        }
    }
}

