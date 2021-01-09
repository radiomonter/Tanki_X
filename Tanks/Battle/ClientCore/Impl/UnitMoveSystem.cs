namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientEntrance.API;
    using UnityEngine;

    public class UnitMoveSystem : ECSSystem
    {
        public static float SEND_MOVE_PERIOD = 1f;
        public static float MAX_VELOCITY_DELTA = 5f;

        private Movement GetMoveFromRigidbody(Rigidbody rigidbody) => 
            new Movement { 
                Position = rigidbody.position,
                Orientation = rigidbody.rotation,
                Velocity = rigidbody.velocity,
                AngularVelocity = rigidbody.angularVelocity
            };

        [OnEventFire]
        public void Init(NodeAddedEvent evt, UnitMoveNode unit)
        {
            this.UpdateRigidbody(unit.unitMove.Movement, unit.rigidBody);
            unit.Entity.AddComponent<UnitReadyComponent>();
        }

        [OnEventFire]
        public void InitSelf(NodeAddedEvent evt, UnitMoveInitSelfNode unit)
        {
            unit.Entity.AddComponent<UnitMoveSyncComponent>();
        }

        private bool IsNeedSendToServer(Movement move, UnitMoveSyncComponent unitMoveSync)
        {
            bool flag = false;
            float time = (float) PreciseTime.Time;
            flag = (time > (unitMoveSync.LastSendMoveTime + SEND_MOVE_PERIOD)) || ((unitMoveSync.LastSendVelocity - move.Velocity).sqrMagnitude > (MAX_VELOCITY_DELTA * MAX_VELOCITY_DELTA));
            if (flag)
            {
                unitMoveSync.LastSendMoveTime = time;
                unitMoveSync.LastSendVelocity = move.Velocity;
            }
            return flag;
        }

        [OnEventFire]
        public void RecieveMoveFromServer(UnitMoveRemoteEvent evt, UnitMoveNode unit, [JoinSelf] Optional<SingleNode<UnitMoveSmootherComponent>> smoother)
        {
            Movement unitMove = evt.UnitMove;
            unit.unitMove.Movement = unitMove;
            if (!smoother.IsPresent())
            {
                this.UpdateRigidbody(unitMove, unit.rigidBody);
            }
            else
            {
                smoother.Get().component.BeforeSetMovement();
                this.UpdateRigidbody(unitMove, unit.rigidBody);
                smoother.Get().component.AfterSetMovement();
            }
        }

        [OnEventFire]
        public void SendMoveToServer(FixedUpdateEvent evt, [Combine] UnitMoveSyncNode unit)
        {
            Movement moveFromRigidbody = this.GetMoveFromRigidbody(unit.rigidBody.Rigidbody);
            if (this.IsNeedSendToServer(moveFromRigidbody, unit.unitMoveSync))
            {
                base.ScheduleEvent(new UnitMoveSelfEvent(moveFromRigidbody), unit);
            }
        }

        private void UpdateRigidbody(Movement move, RigidbodyComponent rigidbody)
        {
            if (rigidbody.Rigidbody)
            {
                rigidbody.RigidbodyTransform.SetPositionSafe(move.Position);
                rigidbody.RigidbodyTransform.SetRotationSafe(move.Orientation);
                rigidbody.Rigidbody.SetVelocitySafe(move.Velocity);
                rigidbody.Rigidbody.SetAngularVelocitySafe(move.AngularVelocity);
            }
        }

        public class UnitMoveInitSelfNode : UnitMoveSystem.UnitMoveNode
        {
            public SelfComponent self;
        }

        public class UnitMoveNode : Node
        {
            public UnitComponent unit;
            public UnitMoveComponent unitMove;
            public RigidbodyComponent rigidBody;
        }

        public class UnitMoveSyncComponent : Component
        {
            public float LastSendMoveTime { get; set; }

            public Vector3 LastSendVelocity { get; set; }
        }

        public class UnitMoveSyncNode : UnitMoveSystem.UnitMoveNode
        {
            public UnitMoveSystem.UnitMoveSyncComponent unitMoveSync;
        }
    }
}

