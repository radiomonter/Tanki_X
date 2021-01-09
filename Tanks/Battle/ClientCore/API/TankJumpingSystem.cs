namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.Impl;

    public class TankJumpingSystem : ECSSystem
    {
        [OnEventFire]
        public void ControlJump(FixedUpdateEvent e, JumpingContolledTankNode tank)
        {
            if (tank.tankJump.isFinished())
            {
                tank.Entity.RemoveComponent<TankJumpComponent>();
            }
            else if (tank.tankJump.isBegin())
            {
                tank.rigidbody.Rigidbody.velocity = tank.tankJump.Velocity;
            }
        }

        public class JumpingContolledTankNode : Node
        {
            public TankSyncComponent tankSync;
            public TankJumpComponent tankJump;
            public RigidbodyComponent rigidbody;
        }
    }
}

