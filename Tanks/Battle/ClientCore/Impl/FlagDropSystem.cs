namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;

    public class FlagDropSystem : ECSSystem
    {
        [OnEventComplete]
        public void FlagCheckCollision(NodeAddedEvent e, GroundedFlagNode flag)
        {
            flag.flagInstance.FlagInstance.SetActive(false);
            flag.flagInstance.FlagInstance.SetActive(true);
        }

        [OnEventFire]
        public void FlagDropRequest(UpdateEvent e, CarriedFlagNode flag, [JoinByTank] SelfTankNode tank, [JoinByBattle] SingleNode<RoundActiveStateComponent> round, [JoinByBattle] SingleNode<CTFConfigComponent> ctfConfig)
        {
            if (InputManager.GetActionKeyDown(CTFActions.DROP_FLAG))
            {
                base.ScheduleEvent<SendTankMovementEvent>(tank);
                base.NewEvent<FlagDropRequestEvent>().Attach(flag).Attach(tank).Schedule();
                base.NewEvent<TankFlagCollisionEvent>().Attach(flag).Attach(tank).ScheduleDelayed(ctfConfig.component.enemyFlagActionMinIntervalSec);
            }
        }

        [Inject]
        public static Tanks.Battle.ClientCore.Impl.InputManager InputManager { get; set; }

        public class CarriedFlagNode : Node
        {
            public FlagComponent flag;
            public TankGroupComponent tankGroup;
        }

        public class GroundedFlagNode : Node
        {
            public FlagPositionComponent flagPosition;
            public FlagGroundedStateComponent flagGroundedState;
            public FlagInstanceComponent flagInstance;
        }

        public class SelfTankNode : Node
        {
            public SelfTankComponent selfTank;
            public TankGroupComponent tankGroup;
            public TankActiveStateComponent tankActiveState;
        }
    }
}

