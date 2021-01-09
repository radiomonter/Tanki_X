namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientLogger.API;
    using Platform.Library.ClientProtocol.Impl;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class TankMovementSenderSystem : ECSSystem
    {
        private const float SEND_MAX_TIME = 2f;
        private const float MAX_DISTANCE = 5f;
        private const float MAX_DISTANCE_Y = 1.5f;
        private const float MAX_ACCELERATION_Y = 20f;
        private const float MAX_DISTANCE_SQUARED = 25f;

        [OnEventFire]
        public void AddComponent(NodeAddedEvent e, SingleNode<SelfTankComponent> selfTank)
        {
            selfTank.Entity.AddComponent<TankSyncComponent>();
        }

        [OnEventFire]
        public void AddComponent(NodeAddedEvent e, TankInitNode tank)
        {
            tank.Entity.AddComponent<TankMovementSenderComponent>();
        }

        private void CollectCommandsAndSend(TankNode tankNode, WeaponNode weaponNode, MoveCommandType commandType)
        {
            TankMovementSenderComponent tankMovementSender = tankNode.tankMovementSender;
            if (((commandType & MoveCommandType.TANK) != MoveCommandType.NONE) && (tankMovementSender.LastSentMovementTime >= PreciseTime.Time))
            {
                commandType ^= MoveCommandType.TANK;
            }
            if (((commandType & MoveCommandType.WEAPON) != MoveCommandType.NONE) && (tankMovementSender.LastSentWeaponRotationTime >= PreciseTime.Time))
            {
                commandType ^= MoveCommandType.WEAPON;
            }
            Movement? nullable = null;
            float? nullable3 = null;
            if ((commandType & MoveCommandType.TANK) != MoveCommandType.NONE)
            {
                nullable = new Movement?(this.GetMovement(tankNode));
            }
            if ((commandType & MoveCommandType.WEAPON) != MoveCommandType.NONE)
            {
                nullable3 = new float?(weaponNode.weaponRotationControl.Rotation);
            }
            MoveCommand moveCommand = new MoveCommand {
                Movement = nullable,
                WeaponRotation = nullable3,
                TankControlVertical = this.GetMoveAxis(tankNode),
                TankControlHorizontal = this.GetTurnAxis(tankNode),
                WeaponRotationControl = this.GetWeaponControl(weaponNode),
                ClientTime = (int) (PreciseTime.Time * 1000.0)
            };
            this.SendCommand(tankNode, moveCommand);
        }

        private DiscreteTankControl GetControl(TankNode tankNode, WeaponNode weaponNode)
        {
            DiscreteTankControl control = new DiscreteTankControl();
            ChassisComponent chassis = tankNode.chassis;
            MoveControl control2 = new MoveControl {
                MoveAxis = chassis.EffectiveMoveAxis,
                TurnAxis = chassis.EffectiveTurnAxis
            };
            control.TurnAxis = Mathf.RoundToInt(control2.TurnAxis);
            control.MoveAxis = Mathf.RoundToInt(control2.MoveAxis);
            control.WeaponControl = Mathf.RoundToInt(weaponNode.weaponRotationControl.EffectiveControl);
            return control;
        }

        private float GetMoveAxis(TankNode tankNode) => 
            tankNode.chassis.EffectiveMoveAxis;

        private Movement GetMovement(TankNode tankNode)
        {
            if (PreciseTime.TimeType == TimeType.LAST_FIXED)
            {
                return tankNode.tankMovementSender.LastPhysicsMovement;
            }
            return new Movement { 
                Position = TankPositionConverter.ConvertedSentToServer(tankNode.tankCollidersUnity),
                Orientation = tankNode.rigidbody.RigidbodyTransform.rotation,
                Velocity = this.GetVelocity(tankNode),
                AngularVelocity = tankNode.rigidbody.Rigidbody.angularVelocity
            };
        }

        private float GetTurnAxis(TankNode tankNode) => 
            tankNode.chassis.EffectiveTurnAxis;

        private Vector3 GetVelocity(TankNode tankNode)
        {
            Vector3 velocity = tankNode.rigidbody.Rigidbody.velocity;
            float f = this.SqrMagnitudeXZ(velocity);
            float currentValue = tankNode.chassisSmoother.maxSpeedSmoother.CurrentValue;
            if (f > (currentValue * currentValue))
            {
                Vector3 vector2 = velocity;
                vector2.y = 0f;
                velocity += (currentValue - Mathf.Sqrt(f)) * vector2.normalized;
            }
            return velocity;
        }

        private float GetWeaponControl(WeaponNode weaponNode) => 
            weaponNode.weaponRotationControl.EffectiveControl;

        [OnEventFire]
        public void InitializeClientTime(NodeAddedEvent e, TankInitNode tankNode, [JoinByTank, Context] WeaponNode weaponNode)
        {
            base.ScheduleEvent<InitializeTimeCheckerEvent>(tankNode);
        }

        [OnEventFire]
        public void InitializeClientTime(NodeAddedEvent e, TankNode tankNode, [JoinByTank] WeaponNode weaponNode)
        {
            base.ScheduleEvent<InitializeTimeCheckerEvent>(tankNode);
            this.CollectCommandsAndSend(tankNode, weaponNode, MoveCommandType.FULL);
        }

        private bool IsXZDistanceTooLarge(Vector3 lastPosition, Vector3 position)
        {
            float num = position.x - lastPosition.x;
            float num2 = position.z - lastPosition.z;
            return (((num * num) + (num2 * num2)) > 25f);
        }

        private bool IsYDistanceTooLarge(Vector3 lastPosition, Vector3 position) => 
            Mathf.Abs((float) (position.y - lastPosition.y)) > 1.5f;

        private bool NeedMandatoryCorrection(TankNode tankNode)
        {
            TankMovementSenderComponent tankMovementSender = tankNode.tankMovementSender;
            if (tankMovementSender.LastSentMovement != null)
            {
                if (tankNode.tankCollision.HasCollision == tankMovementSender.LastHasCollision)
                {
                    return false;
                }
                tankMovementSender.LastHasCollision = tankNode.tankCollision.HasCollision;
            }
            return true;
        }

        private bool NeedRegularCorrection(TankNode tankNode)
        {
            TankMovementSenderComponent tankMovementSender = tankNode.tankMovementSender;
            return (PreciseTime.Time >= (tankMovementSender.LastSentMovementTime + 2.0));
        }

        [OnEventComplete]
        public void OnAfterFixedUpdate(AfterFixedUpdateEvent e, TankNode tankNode, [JoinByTank] WeaponNode weaponNode)
        {
            TankMovementSenderComponent tankMovementSender = tankNode.tankMovementSender;
            Rigidbody rigidbody = tankNode.rigidbody.Rigidbody;
            Vector3 velocity = rigidbody.velocity;
            float num = this.SqrMagnitudeXZ(velocity);
            Vector3 vector = tankMovementSender.LastPhysicsMovement.Velocity;
            if (((((tankMovementSender.LastSentMovement == null) || (((Mathf.Clamp(velocity.y, 0f, float.MaxValue) - Mathf.Clamp(vector.y, 0f, float.MaxValue)) / Time.fixedDeltaTime) > 20f)) || ((this.SqrMagnitudeXZ(vector) > (num + 0.01)) && ((this.SqrMagnitudeXZ(tankMovementSender.LastSentMovement.Value.Velocity) + 0.01) < this.SqrMagnitudeXZ(vector)))) || this.IsXZDistanceTooLarge(tankMovementSender.LastSentMovement.Value.Position, rigidbody.position)) || this.IsYDistanceTooLarge(tankMovementSender.LastSentMovement.Value.Position, rigidbody.position))
            {
                this.CollectCommandsAndSend(tankNode, weaponNode, MoveCommandType.TANK);
            }
        }

        [OnEventFire]
        public void OnChassisControlChanged(ChassisControlChangedEvent e, TankNode tankNode, [JoinByTank] WeaponNode weaponNode)
        {
            this.CollectCommandsAndSend(tankNode, weaponNode, MoveCommandType.TANK);
        }

        [OnEventFire]
        public void OnEMPEffectTargetsCollection(SynchronizeSelfTankPositionBeforeEffectEvent e, TankNode tankNode, [JoinByTank] WeaponNode weaponNode)
        {
            this.CollectCommandsAndSend(tankNode, weaponNode, MoveCommandType.TANK);
        }

        [OnEventFire]
        public void OnReset(NodeAddedEvent e, TankIncarnationNode incarnation, [JoinByTank] TankNode tankNode)
        {
            tankNode.tankMovementSender.LastSentMovementTime = 0.0;
        }

        [OnEventFire]
        public void OnUpdate(UpdateEvent e, TankNode tankNode, [JoinByTank] WeaponNode weaponNode)
        {
            if (this.NeedRegularCorrection(tankNode) || this.NeedMandatoryCorrection(tankNode))
            {
                this.CollectCommandsAndSend(tankNode, weaponNode, MoveCommandType.FULL);
            }
            if ((FloatCodec.DECODE_NAN_ERRORS != 0) || (FloatCodec.ENCODE_NAN_ERRORS != 0))
            {
                long num = !tankNode.Entity.HasComponent<BattleGroupComponent>() ? 0L : tankNode.Entity.GetComponent<BattleGroupComponent>().Key;
                object[] args = new object[] { num, FloatCodec.DECODE_NAN_ERRORS, FloatCodec.ENCODE_NAN_ERRORS, (FloatCodec.encodeErrorStack == null) ? "null" : FloatCodec.encodeErrorStack.StackTrace };
                base.Log.ErrorFormat("NaN detected: battle={0} DECODE_NAN_ERRORS={1} ENCODE_NAN_ERRORS={2} encodeErrorStack={3}", args);
                FloatCodec.DECODE_NAN_ERRORS = 0;
                FloatCodec.ENCODE_NAN_ERRORS = 0;
            }
        }

        [OnEventFire]
        public void OnWeaponRotationControlChanged(WeaponRotationControlChangedEvent e, WeaponNode weaponNode, [JoinByTank] TankNode tankNode)
        {
            this.CollectCommandsAndSend(tankNode, weaponNode, MoveCommandType.WEAPON);
        }

        [OnEventFire]
        public void RemoveComponent(NodeRemoveEvent e, TankInitNode tank)
        {
            tank.Entity.RemoveComponent<TankMovementSenderComponent>();
        }

        private void SendCommand(TankNode tankNode, MoveCommand moveCommand)
        {
            TankMovementSenderComponent tankMovementSender = tankNode.tankMovementSender;
            Movement? movement = moveCommand.Movement;
            if (movement != null)
            {
                if (!PhysicsUtil.ValidateMovement(movement.Value))
                {
                    return;
                }
                tankMovementSender.LastSentMovement = movement;
                tankMovementSender.LastSentMovementTime = PreciseTime.Time;
                base.Log.Debug("SEND MOVEMENT");
            }
            if (moveCommand.WeaponRotation != null)
            {
                if (!PhysicsUtil.IsValidFloat(moveCommand.WeaponRotation.Value))
                {
                    LoggerProvider.GetLogger(typeof(PhysicsUtil)).ErrorFormat("Invalid WeaponRotation. StackTrace:[{0}]", Environment.StackTrace);
                    return;
                }
                tankMovementSender.LastSentWeaponRotationTime = PreciseTime.Time;
                base.Log.Debug("SEND WEAPON_ROTATION");
            }
            base.ScheduleEvent(new MoveCommandEvent(moveCommand), tankNode.Entity);
            base.Log.Debug("SEND DISCRETE");
        }

        [OnEventFire]
        public void SendShotToServerEvent(BeforeShotEvent e, WeaponNode weaponNode, [JoinByTank] TankNode tank)
        {
            this.CollectCommandsAndSend(tank, weaponNode, MoveCommandType.FULL);
        }

        [OnEventFire]
        public void SendTankMovement(SendTankMovementEvent e, TankNode tankNode, [JoinByTank] WeaponNode weaponNode)
        {
            this.CollectCommandsAndSend(tankNode, weaponNode, MoveCommandType.TANK);
        }

        private float SqrMagnitudeXZ(Vector3 vector) => 
            (vector.x * vector.x) + (vector.z * vector.z);

        [OnEventComplete]
        public void UpdateLastPosition(FixedUpdateEvent e, TankNode tank)
        {
            tank.tankMovementSender.LastPhysicsMovement = this.GetMovement(tank);
        }

        [Inject]
        public static Platform.Library.ClientUnityIntegration.API.UnityTime UnityTime { get; set; }

        public class AnyStateTankNode : Node
        {
            public TankGroupComponent tankGroup;
            public TankSyncComponent tankSync;
            public TankMovementSenderComponent tankMovementSender;
            public ChassisComponent chassis;
            public RigidbodyComponent rigidbody;
            public TankCollisionComponent tankCollision;
            public TankCollidersUnityComponent tankCollidersUnity;
        }

        public class TankIncarnationNode : Node
        {
            public TankIncarnationComponent tankIncarnation;
            public TankGroupComponent tankGroup;
        }

        public class TankInitNode : Node
        {
            public TankGroupComponent tankGroup;
            public TankSyncComponent tankSync;
            public TankMovableComponent tankMovable;
        }

        public class TankNode : Node
        {
            public TankGroupComponent tankGroup;
            public TankSyncComponent tankSync;
            public TankMovableComponent tankMovable;
            public TankMovementSenderComponent tankMovementSender;
            public ChassisComponent chassis;
            public RigidbodyComponent rigidbody;
            public TankCollisionComponent tankCollision;
            public TankCollidersUnityComponent tankCollidersUnity;
            public ChassisSmootherComponent chassisSmoother;
        }

        public class WeaponNode : Node
        {
            public TankGroupComponent tankGroup;
            public WeaponRotationControlComponent weaponRotationControl;
        }
    }
}

