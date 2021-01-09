namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class TankMovementReceiverSystem : ECSSystem
    {
        private static readonly float PATH_SMALL_DISTANCE = 5f;
        private static readonly float BIG_ROTATION_DEGREES = 60f;
        private static readonly Vector3 PATH_OFFSET = (Vector3.up * 0.2f);

        private void ApplyMoveControl(TankNode tank, MoveControl? moveControl)
        {
            if (moveControl != null)
            {
                this.ApplyMoveControl(tank.chassis, moveControl.Value.MoveAxis, moveControl.Value.TurnAxis);
            }
        }

        private void ApplyMoveControl(ChassisComponent chassis, float moveAxis, float turnAxis)
        {
            base.Log.Debug("APPLY MOVE_CONTROL");
            chassis.MoveAxis = moveAxis;
            chassis.TurnAxis = turnAxis;
        }

        private void ApplyMovement(TankNode tank, ref Movement? movement, bool init)
        {
            if (movement != 0)
            {
                Movement m = movement.Value;
                if (PhysicsUtil.ValidateMovement(m))
                {
                    bool flag = this.HalveMoveCommandIfNeed(tank, init, ref m);
                    base.Log.Debug(!flag ? "APPLY MOVEMENT" : "APPLY HALVED MOVEMENT");
                    Transform rigidbodyTransform = tank.rigidbody.RigidbodyTransform;
                    Rigidbody rigidbody = tank.rigidbody.Rigidbody;
                    rigidbodyTransform.SetRotationSafe(m.Orientation);
                    rigidbodyTransform.SetPositionSafe(TankPositionConverter.ConvertedReceptionFromServer(m.Position, tank.tankCollidersUnity, rigidbodyTransform.position));
                    rigidbody.SetVelocitySafe(m.Velocity);
                    rigidbody.SetAngularVelocitySafe(m.AngularVelocity);
                }
            }
        }

        private void ApplyWeaponControl(WeaponRotationControlComponent weaponRotationComponent, float? weaponControl)
        {
            if (weaponControl != null)
            {
                base.Log.Debug("APPLY WEAPON_CONTROL");
                weaponRotationComponent.Control = weaponControl.Value;
            }
        }

        private void ApplyWeaponControl(WeaponNode weapon, float? weaponControl)
        {
            this.ApplyWeaponControl(weapon.weaponRotationControl, weaponControl);
        }

        private void ApplyWeaponRotation(WeaponInstanceComponent weaponInstanceComponent, float weaponRotation)
        {
            base.Log.Debug("APPLY WEAPON_ROTATION");
            Transform transform = weaponInstanceComponent.WeaponInstance.transform;
            transform.SetLocalRotationSafe(Quaternion.AngleAxis(weaponRotation, Vector3.up));
            transform.localPosition = Vector3.zero;
        }

        private void ApplyWeaponRotation(WeaponNode weapon, float? weaponRotation)
        {
            if (weaponRotation != null)
            {
                this.ApplyWeaponRotation(weapon.weaponInstance, weaponRotation.Value);
                weapon.weaponRotationControl.Rotation = weaponRotation.Value;
            }
        }

        private Movement DumpMovement(Transform transform, Rigidbody rigidbody, TankCollidersUnityComponent tankCollidersUnity) => 
            new Movement { 
                Position = TankPositionConverter.ConvertedSentToServer(tankCollidersUnity),
                Orientation = transform.rotation,
                Velocity = rigidbody.velocity,
                AngularVelocity = rigidbody.angularVelocity
            };

        private bool HalveMoveCommandIfNeed(TankNode tank, bool init, ref Movement movement)
        {
            if (init || tank.Entity.HasComponent<TankDeadStateComponent>())
            {
                return false;
            }
            Transform rigidbodyTransform = tank.rigidbody.RigidbodyTransform;
            Movement currentMoveDump = this.DumpMovement(rigidbodyTransform, tank.rigidbody.Rigidbody, tank.tankCollidersUnity);
            if (!this.IsMovePathClean(ref currentMoveDump, ref movement))
            {
                return false;
            }
            if (this.IsBigRotation(ref movement, ref currentMoveDump))
            {
                return false;
            }
            movement = this.InterpolateMoveCommand(ref currentMoveDump, ref movement, 0.5f);
            return true;
        }

        private Movement InterpolateMoveCommand(ref Movement moveCommand1, ref Movement moveCommand2, float interpolationCoeff) => 
            new Movement { 
                Position = Vector3.Lerp(moveCommand1.Position, moveCommand2.Position, interpolationCoeff),
                Orientation = Quaternion.Slerp(moveCommand1.Orientation, moveCommand2.Orientation, interpolationCoeff),
                Velocity = Vector3.Lerp(moveCommand1.Velocity, moveCommand2.Velocity, interpolationCoeff),
                AngularVelocity = Vector3.Slerp(moveCommand1.AngularVelocity, moveCommand2.AngularVelocity, interpolationCoeff)
            };

        private bool IsBigRotation(ref Movement movement, ref Movement currentMovement) => 
            Quaternion.Angle(currentMovement.Orientation, movement.Orientation) > BIG_ROTATION_DEGREES;

        private bool IsMovePathClean(ref Movement currentMoveDump, ref Movement movement)
        {
            Vector3 a = movement.Position - currentMoveDump.Position;
            float f = Vector3.SqrMagnitude(a);
            return ((f >= (PATH_SMALL_DISTANCE * PATH_SMALL_DISTANCE)) ? !Physics.Raycast(currentMoveDump.Position + PATH_OFFSET, a, Mathf.Sqrt(f), LayerMasks.STATIC) : true);
        }

        private static bool IsTankDead(TankNode tank) => 
            tank.Entity.HasComponent<TankDeadStateComponent>();

        [OnEventFire]
        public void OnMoveCommandAnalog(AnalogMoveCommandServerEvent e, TankNode tank, [JoinByTank] WeaponNode weapon)
        {
            base.Log.Debug("RECEIVE ANALOG");
            Movement? movement = e.Movement;
            this.ApplyMovement(tank, ref movement, false);
            this.ApplyMoveControl(tank, e.MoveControl);
            this.ApplyWeaponRotation(weapon, e.WeaponRotation);
            this.ApplyWeaponControl(weapon, e.WeaponControl);
        }

        [OnEventFire]
        public void OnMoveCommandDiscrete(MoveCommandServerEvent e, TankNode tank, [JoinByTank] WeaponNode weapon)
        {
            base.Log.Debug("RECEIVE DISCRETE");
            MoveCommand moveCommand = e.MoveCommand;
            Movement? movement = e.MoveCommand.Movement;
            this.ApplyMovement(tank, ref movement, false);
            MoveControl control = new MoveControl {
                MoveAxis = moveCommand.TankControlVertical,
                TurnAxis = moveCommand.TankControlHorizontal
            };
            this.ApplyMoveControl(tank, new MoveControl?(control));
            this.ApplyWeaponRotation(weapon, e.MoveCommand.WeaponRotation);
            this.ApplyWeaponControl(weapon, new float?(moveCommand.WeaponRotationControl));
        }

        [OnEventFire]
        public void OnTankAdded(NodeAddedEvent e, TankNode tank, [Context, JoinByTank] WeaponNode weapon)
        {
            base.Log.DebugFormat("INIT {0}", tank);
            TankMovementComponent tankMovement = tank.tankMovement;
            Movement? movement = new Movement?(tankMovement.Movement);
            this.ApplyMovement(tank, ref movement, true);
            MoveControl control = new MoveControl {
                MoveAxis = tankMovement.MoveControl.MoveAxis,
                TurnAxis = tankMovement.MoveControl.TurnAxis
            };
            this.ApplyMoveControl(tank, new MoveControl?(control));
            this.ApplyWeaponRotation(weapon, new float?(tankMovement.WeaponRotation));
            this.ApplyWeaponControl(weapon, new float?(tankMovement.WeaponControl));
            base.ScheduleEvent<TankMovementInitEvent>(tank);
        }

        public class TankNode : Node
        {
            public TankGroupComponent tankGroup;
            public TankComponent tank;
            public TankMovementComponent tankMovement;
            public RigidbodyComponent rigidbody;
            public ChassisComponent chassis;
            public AssembledTankComponent assembledTank;
            public TankCollidersUnityComponent tankCollidersUnity;
        }

        public class WeaponNode : Node
        {
            public TankGroupComponent tankGroup;
            public WeaponRotationControlComponent weaponRotationControl;
            public WeaponInstanceComponent weaponInstance;
        }
    }
}

