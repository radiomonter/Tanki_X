namespace Tanks.Battle.ClientCore.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Battle.ClientCore.API;

    public class TankUpsideDownSystem : ECSSystem
    {
        [OnEventFire]
        public void ClearUpsideDownMark(CheckUpsideDownStateEvent e, UpsideDownSelfTankNode tank, [JoinByUser] SingleNode<UpsideDownConfigComponent> config)
        {
            if (tank.rigidbody.RigidbodyTransform.up.y >= config.component.GetUpsideDownCosToCheck())
            {
                tank.upsideDownTank.Removed = true;
                tank.Entity.RemoveComponent<UpsideDownTankComponent>();
            }
        }

        [OnEventFire]
        public void ClearUpsideDownMarkOnTankRemove(NodeRemoveEvent e, UpsideDownSelfTankForNRNode nr, [JoinSelf] UpsideDownSelfTankNode tank)
        {
            if (!tank.upsideDownTank.Removed)
            {
                tank.Entity.RemoveComponent<UpsideDownTankComponent>();
            }
        }

        [OnEventFire]
        public void InitTankChecking(NodeAddedEvent e, SelfTankNode selfTank)
        {
            base.NewEvent<CheckUpsideDownStateEvent>().Attach(selfTank).SchedulePeriodic(1f);
        }

        [OnEventFire]
        public void MarkTankAsUpsideDown(CheckUpsideDownStateEvent e, SelfTankNode tank, [JoinByUser] SingleNode<UpsideDownConfigComponent> config)
        {
            if (tank.rigidbody.RigidbodyTransform.up.y < config.component.GetUpsideDownCosToCheck())
            {
                UpsideDownTankComponent component = new UpsideDownTankComponent {
                    TimeTankBecomesUpsideDown = Date.Now
                };
                tank.Entity.AddComponent(component);
            }
        }

        [Not(typeof(UpsideDownTankComponent))]
        public class SelfTankNode : Node
        {
            public RigidbodyComponent rigidbody;
            public SelfTankComponent selfTank;
            public AssembledTankActivatedStateComponent assembledTankActivatedState;
            public TankActiveStateComponent tankActiveState;
        }

        public class UpsideDownSelfTankForNRNode : Node
        {
            public RigidbodyComponent rigidbody;
            public SelfTankComponent selfTank;
            public AssembledTankActivatedStateComponent assembledTankActivatedState;
            public TankActiveStateComponent tankActiveState;
        }

        public class UpsideDownSelfTankNode : Node
        {
            public RigidbodyComponent rigidbody;
            public SelfTankComponent selfTank;
            public AssembledTankActivatedStateComponent assembledTankActivatedState;
            public TankActiveStateComponent tankActiveState;
            public UpsideDownTankComponent upsideDownTank;
        }
    }
}

