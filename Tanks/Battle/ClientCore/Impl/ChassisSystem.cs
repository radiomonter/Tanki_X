namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientGraphics.API;
    using UnityEngine;

    public class ChassisSystem : ECSSystem
    {
        private const float SELF_TANK_UPDATE_PERIOD = 0f;
        private const float REMOTE_TANK_UPDATE_PERIOD = 0.05f;
        private const float REMOTE_INVISBILE_TANK_UPDATE_PERIOD = 0.1f;
        private const float REMOTE_RANDOM_TANK_UPDATE_PERIOD = 0.05f;
        private static readonly string RIGHT_AXIS = "MoveRight";
        private static readonly string LEFT_AXIS = "MoveLeft";
        private static readonly string FORWARD_AXIS = "MoveForward";
        private static readonly string BACKWARD_AXIS = "MoveBackward";
        private const float MIN_ACCELERATION = 4f;
        private const float SQRT1_2 = 0.7071068f;
        private const float FULL_FORCE_ANGLE = 0.7853982f;
        private const float ZERO_FORCE_ANGLE = 1.047198f;
        private const float FULL_SLOPE_ANGLE = 0.7853982f;
        private const float MAX_SLOPE_ANGLE = 1.047198f;
        private static readonly float FULL_SLOPE_COS_ANGLE = Mathf.Cos(0.7853982f);
        private static readonly float MAX_SLOPE_COS_ANGLE = Mathf.Cos(1.047198f);

        private void AddSurfaceVelocitiesFromRay(ChassisNode node, SuspensionRay ray, Vector3 contactsMidpoint, ref Vector3 surfaceVelocity, ref Vector3 angularSurfaceVelocity)
        {
            if (ray.hasCollision)
            {
                surfaceVelocity += ray.surfaceVelocity;
                Vector3 lhs = ray.rayHit.point - contactsMidpoint;
                float sqrMagnitude = lhs.sqrMagnitude;
                if (sqrMagnitude > 0.0001f)
                {
                    angularSurfaceVelocity += Vector3.Cross(lhs, ray.surfaceVelocity) / sqrMagnitude;
                }
            }
        }

        private void AdjustSuspensionSpringCoeff(ChassisConfigComponent chassisConfig, ChassisComponent chassis, Rigidbody rigidbody)
        {
            float num = Physics.gravity.magnitude * rigidbody.mass;
            chassis.SpringCoeff = num / ((2 * chassisConfig.NumRaysPerTrack) * (chassisConfig.MaxRayLength - chassisConfig.NominalRayLength));
        }

        private void ApplyForceFromRay(SuspensionRay ray, Rigidbody rigidbody, Vector3 bodyForwardAxis, float forcePerRay)
        {
            if (ray.hasCollision)
            {
                float num = Mathf.Abs(Mathf.Acos(ray.rayHit.normal.normalized.y));
                if (num < 1.047198f)
                {
                    float num2 = forcePerRay;
                    if (num > 1.047198f)
                    {
                        num2 *= (1.047198f - num) / 0.2617994f;
                    }
                    Vector3 force = bodyForwardAxis * num2;
                    rigidbody.AddForceAtPositionSafe(force, ray.GetGlobalOrigin());
                }
            }
        }

        private void ApplyMovementForces(ChassisNode node, float dt)
        {
            TrackComponent track = node.track;
            if ((track.LeftTrack.numContacts + track.RightTrack.numContacts) > 0)
            {
                Vector3 vector;
                Vector3 vector2;
                float num2;
                Rigidbody rigidbody = node.rigidbody.Rigidbody;
                ChassisConfigComponent chassisConfig = node.chassisConfig;
                this.CalculateNetSurfaceVelocities(node, out vector, out vector2);
                float num = this.CalculateSlopeCoefficient(rigidbody.transform.up.y);
                rigidbody.SetVelocitySafe(this.CalculateRigidBodyVelocity(rigidbody, vector, (node.speedConfig.SideAcceleration * num) * dt, out num2));
                if ((track.LeftTrack.numContacts > 0) || (track.RightTrack.numContacts > 0))
                {
                    Vector3 vector4 = this.CalculateRelativeAngularVelocity(node, dt, num * 1.2f, vector2, num2);
                    Vector3 normalized = rigidbody.transform.InverseTransformDirection(rigidbody.angularVelocity).normalized;
                    if ((Mathf.Abs(node.chassis.TurnAxis) > 0f) && (Mathf.Sign(normalized.y) != Mathf.Sign(node.chassis.TurnAxis)))
                    {
                        float y = Mathf.Lerp(0f, normalized.y, (0.2f * dt) * 60f);
                        vector2 -= rigidbody.transform.TransformDirection(new Vector3(0f, y, 0f));
                    }
                    rigidbody.SetAngularVelocitySafe(vector2 + vector4);
                }
            }
        }

        public void ApplyStaticFriction(TrackComponent tracks, Rigidbody rigidbody)
        {
            if ((tracks.RightTrack.numContacts >= (tracks.RightTrack.rays.Length >> 1)) || (tracks.LeftTrack.numContacts >= (tracks.LeftTrack.rays.Length >> 1)))
            {
                Vector3 up = rigidbody.transform.up;
                float num = Vector3.Dot(Physics.gravity, up);
                float num2 = 0.7071068f * Physics.gravity.magnitude;
                if ((num < -num2) || (num > num2))
                {
                    Vector3 force = ((up * num) - Physics.gravity) * rigidbody.mass;
                    rigidbody.AddForceSafe(force);
                }
            }
        }

        private float CalculateForcePerRay(ChassisNode node, float dt, float forwardRelativeSpeed)
        {
            ChassisConfigComponent chassisConfig = node.chassisConfig;
            ChassisComponent chassis = node.chassis;
            float maxSpeed = node.effectiveSpeed.MaxSpeed;
            TrackComponent track = node.track;
            Rigidbody rigidbody = node.rigidbody.Rigidbody;
            float acceleration = node.speed.Acceleration;
            float num3 = 0f;
            if (chassis.EffectiveMoveAxis == 0f)
            {
                num3 = (-MathUtil.Sign(forwardRelativeSpeed) * acceleration) * dt;
                if (MathUtil.Sign(forwardRelativeSpeed) != MathUtil.Sign(forwardRelativeSpeed + num3))
                {
                    num3 = -forwardRelativeSpeed;
                }
            }
            else
            {
                if (this.IsReversedMove(chassis.EffectiveMoveAxis, forwardRelativeSpeed))
                {
                    acceleration = node.speedConfig.ReverseAcceleration;
                }
                num3 = (chassis.EffectiveMoveAxis * acceleration) * dt;
            }
            float f = Mathf.Clamp(forwardRelativeSpeed + num3, -maxSpeed, maxSpeed);
            float num5 = f - forwardRelativeSpeed;
            float num6 = 1f;
            float num7 = (maxSpeed <= 0f) ? num6 : Mathf.Clamp01(1f - Mathf.Abs((float) (forwardRelativeSpeed / maxSpeed)));
            if ((num7 < num6) && ((chassis.EffectiveMoveAxis * MathUtil.Sign(forwardRelativeSpeed)) > 0f))
            {
                num5 *= num7 / num6;
            }
            float num8 = num5 / dt;
            if ((Mathf.Abs(num8) < 4f) && (Mathf.Abs(f) > (0.5f * maxSpeed)))
            {
                num8 = MathUtil.SignEpsilon(num8, 0.1f) * 4f;
            }
            int num10 = track.LeftTrack.numContacts + track.RightTrack.numContacts;
            int num11 = 2 * chassisConfig.NumRaysPerTrack;
            float num12 = ((num8 * rigidbody.mass) * (num10 + (0.42f * (num11 - track.LeftTrack.numContacts)))) / ((float) num11);
            return ((num10 <= 0) ? num12 : (num12 / ((float) num10)));
        }

        private void CalculateNetSurfaceVelocities(ChassisNode node, out Vector3 surfaceVelocity, out Vector3 angularSurfaceVelocity)
        {
            ChassisConfigComponent chassisConfig = node.chassisConfig;
            TrackComponent track = node.track;
            Vector3 contactsMidpoint = this.GetContactsMidpoint(chassisConfig, track);
            surfaceVelocity = Vector3.zero;
            angularSurfaceVelocity = Vector3.zero;
            for (int i = 0; i < chassisConfig.NumRaysPerTrack; i++)
            {
                this.AddSurfaceVelocitiesFromRay(node, track.LeftTrack.rays[i], contactsMidpoint, ref surfaceVelocity, ref angularSurfaceVelocity);
                this.AddSurfaceVelocitiesFromRay(node, track.RightTrack.rays[i], contactsMidpoint, ref surfaceVelocity, ref angularSurfaceVelocity);
            }
            float num2 = track.LeftTrack.numContacts + track.RightTrack.numContacts;
            surfaceVelocity = (num2 <= 0f) ? surfaceVelocity : (surfaceVelocity / num2);
            angularSurfaceVelocity = (num2 <= 0f) ? angularSurfaceVelocity : (angularSurfaceVelocity / num2);
        }

        private Vector3 CalculateRelativeAngularVelocity(ChassisNode node, float dt, float slopeCoeff, Vector3 surfaceAngularVelocity, float forwardRelativeSpeed)
        {
            ChassisConfigComponent chassisConfig = node.chassisConfig;
            TrackComponent track = node.track;
            Rigidbody rigidbody = node.rigidbody.Rigidbody;
            float maxTurnSpeed = node.effectiveSpeed.MaxTurnSpeed * 0.01745329f;
            Vector3 up = rigidbody.transform.up;
            Vector3 forward = rigidbody.transform.forward;
            Vector3 lhs = rigidbody.angularVelocity - surfaceAngularVelocity;
            float relativeTurnSpeed = Vector3.Dot(lhs, up);
            float forcePerRay = this.CalculateForcePerRay(node, dt, forwardRelativeSpeed);
            for (int i = 0; i < chassisConfig.NumRaysPerTrack; i++)
            {
                this.ApplyForceFromRay(track.LeftTrack.rays[i], rigidbody, forward, forcePerRay);
                this.ApplyForceFromRay(track.RightTrack.rays[i], rigidbody, forward, forcePerRay);
            }
            float num5 = Vector3.Dot(lhs, up);
            return (lhs + ((this.RecalculateRelativeTurnSpeed(node, dt, maxTurnSpeed, relativeTurnSpeed, slopeCoeff) - num5) * up));
        }

        private Vector3 CalculateRigidBodyVelocity(Rigidbody rigidbody, Vector3 surfaceVelocity, float sideSpeedDelta, out float forwardRelativeSpeed)
        {
            Vector3 right = rigidbody.transform.right;
            Vector3 lhs = rigidbody.velocity - surfaceVelocity;
            forwardRelativeSpeed = Vector3.Dot(lhs, rigidbody.transform.forward);
            lhs += this.CalculateSideVelocityDelta(lhs, right, sideSpeedDelta) * right;
            return (surfaceVelocity + lhs);
        }

        private float CalculateSideVelocityDelta(Vector3 relativeVelocity, Vector3 xAxis, float sideSpeedDelta)
        {
            float num = Vector3.Dot(relativeVelocity, xAxis);
            float num2 = num;
            if (num2 < 0f)
            {
                num2 = (sideSpeedDelta <= -num2) ? (num2 + sideSpeedDelta) : 0f;
            }
            else if (num2 > 0f)
            {
                num2 = (sideSpeedDelta <= num2) ? (num2 - sideSpeedDelta) : 0f;
            }
            return (num2 - num);
        }

        private float CalculateSlopeCoefficient(float upAxisY)
        {
            float num = 1f;
            if (upAxisY < FULL_SLOPE_COS_ANGLE)
            {
                num = (upAxisY >= MAX_SLOPE_COS_ANGLE) ? ((1.047198f - Mathf.Acos(upAxisY)) / 0.2617994f) : 0f;
            }
            return num;
        }

        private float CalculateTurnCoefficient(TrackComponent trackComponent)
        {
            float num = 1f;
            if ((trackComponent.LeftTrack.numContacts == 0) || (trackComponent.RightTrack.numContacts == 0))
            {
                num = 0.5f;
            }
            return num;
        }

        private void CreateTracks(ChassisInitNode node, ChassisComponent chassis)
        {
            Entity entity = node.Entity;
            ChassisConfigComponent chassisConfig = node.chassisConfig;
            Rigidbody rigidbody = node.rigidbody.Rigidbody;
            BoxCollider boundsCollider = node.tankColliders.BoundsCollider;
            float trackLength = boundsCollider.size.z * 0.8f;
            float num2 = boundsCollider.size.x - chassisConfig.TrackSeparation;
            Vector3 vector3 = boundsCollider.center - new Vector3(0f, boundsCollider.size.y / 2f, 0f);
            Vector3 trackCenterPosition = vector3 + new Vector3(-0.5f * num2, chassisConfig.NominalRayLength, 0f);
            Vector3 vector6 = vector3 + new Vector3(0.5f * num2, chassisConfig.NominalRayLength, 0f);
            float damping = node.damping.Damping;
            TrackComponent component = new TrackComponent {
                LeftTrack = new Track(rigidbody, chassisConfig.NumRaysPerTrack, trackCenterPosition, trackLength, chassisConfig, chassis, -1, damping),
                RightTrack = new Track(rigidbody, chassisConfig.NumRaysPerTrack, vector6, trackLength, chassisConfig, chassis, 1, damping)
            };
            int layerMask = LayerMasks.VISIBLE_FOR_CHASSIS_SEMI_ACTIVE;
            component.LeftTrack.SetRayсastLayerMask(layerMask);
            component.RightTrack.SetRayсastLayerMask(layerMask);
            entity.AddComponent(component);
        }

        [OnEventFire]
        public void FixedUpdate(FixedUpdateEvent evt, ChassisNode chassisNode, [JoinSelf] Optional<SingleNode<TankSyncComponent>> tankSync, [JoinByTank] Optional<SingleNode<TankJumpComponent>> tankJump, [JoinAll] SingleNode<GameTankSettingsComponent> gameTankSettings, [JoinAll] Optional<SingleNode<BattleActionsStateComponent>> inputState)
        {
            if (!tankJump.IsPresent() || !tankJump.Get().component.isNearBegin())
            {
                bool inputEnabled = inputState.IsPresent();
                if (chassisNode.Entity.HasComponent<SelfTankComponent>())
                {
                    this.UpdateSelfInput(chassisNode, inputEnabled, gameTankSettings.component.MovementControlsInverted);
                }
                this.UpdateInput(chassisNode, inputEnabled);
                ChassisSmootherComponent chassisSmoother = chassisNode.chassisSmoother;
                chassisSmoother.maxSpeedSmoother.SetTargetValue(chassisNode.speed.Speed);
                float num = chassisSmoother.maxSpeedSmoother.Update(evt.DeltaTime);
                chassisNode.effectiveSpeed.MaxSpeed = num;
                Rigidbody rigidbody = chassisNode.rigidbody.Rigidbody;
                if (rigidbody)
                {
                    float x = rigidbody.velocity.x;
                    float z = rigidbody.velocity.z;
                    float t = (!tankJump.IsPresent() || !tankJump.Get().component.OnFly) ? 1f : tankJump.Get().component.GetSlowdownLerp();
                    if (((x * x) + (z * z)) > (num * num))
                    {
                        float num5 = Mathf.Lerp(1f, num / ((float) Math.Sqrt((double) ((x * x) + (z * z)))), t);
                        Vector3 velocity = new Vector3(rigidbody.velocity.x * num5, rigidbody.velocity.y, rigidbody.velocity.z * num5);
                        rigidbody.SetVelocitySafe(velocity);
                    }
                    chassisSmoother.maxTurnSpeedSmoother.SetTargetValue(chassisNode.speed.TurnSpeed);
                    chassisNode.effectiveSpeed.MaxTurnSpeed = chassisSmoother.maxTurnSpeedSmoother.Update(evt.DeltaTime);
                    this.AdjustSuspensionSpringCoeff(chassisNode.chassisConfig, chassisNode.chassis, chassisNode.rigidbody.Rigidbody);
                    float updatePeriod = 0f;
                    if (!tankSync.IsPresent())
                    {
                        updatePeriod = chassisNode.cameraVisibleTrigger.IsVisible ? 0.05f : 0.1f;
                        updatePeriod += Random.value * 0.05f;
                    }
                    if (this.UpdateSuspensionContacts(chassisNode.track, evt.DeltaTime, updatePeriod) && tankJump.IsPresent())
                    {
                        tankJump.Get().component.FinishAndSlowdown();
                    }
                    this.ApplyMovementForces(chassisNode, evt.DeltaTime);
                    this.ApplyStaticFriction(chassisNode.track, chassisNode.rigidbody.Rigidbody);
                }
            }
        }

        private Vector3 GetContactsMidpoint(ChassisConfigComponent chassisConfig, TrackComponent tracks)
        {
            Vector3 vector = new Vector3();
            for (int i = 0; i < chassisConfig.NumRaysPerTrack; i++)
            {
                SuspensionRay ray = tracks.LeftTrack.rays[i];
                if (ray.hasCollision)
                {
                    vector += ray.rayHit.point;
                }
                ray = tracks.RightTrack.rays[i];
                if (ray.hasCollision)
                {
                    vector += ray.rayHit.point;
                }
            }
            int num2 = tracks.LeftTrack.numContacts + tracks.RightTrack.numContacts;
            return ((num2 != 0) ? (vector / ((float) num2)) : Vector3.zero);
        }

        [OnEventFire]
        public void InitTankChassis(NodeAddedEvent evt, ChassisInitNode node)
        {
            ChassisComponent chassis = new ChassisComponent();
            this.CreateTracks(node, chassis);
            node.Entity.AddComponent(chassis);
            node.Entity.AddComponent<EffectiveSpeedComponent>();
            ChassisSmootherComponent component = new ChassisSmootherComponent();
            component.maxSpeedSmoother.Reset(node.speed.Speed);
            component.maxTurnSpeedSmoother.Reset(node.speed.TurnSpeed);
            node.Entity.AddComponent(component);
            node.rigidbody.Rigidbody.mass = node.weight.Weight;
        }

        private bool IsReversedMove(float moveDirection, float relativeMovementSpeed) => 
            (moveDirection * relativeMovementSpeed) < 0f;

        private bool IsReversedTurn(float turnDirection, float relativeTurnSpeed) => 
            (turnDirection * relativeTurnSpeed) < 0f;

        private float RecalculateRelativeTurnSpeed(ChassisNode node, float dt, float maxTurnSpeed, float relativeTurnSpeed, float slopeCoeff)
        {
            ChassisComponent chassis = node.chassis;
            ChassisConfigComponent chassisConfig = node.chassisConfig;
            float num = node.speedConfig.TurnAcceleration * 0.01745329f;
            float num2 = this.CalculateTurnCoefficient(node.track);
            float num3 = 0f;
            if (chassis.EffectiveTurnAxis == 0f)
            {
                num3 = ((-MathUtil.Sign(relativeTurnSpeed) * num) * slopeCoeff) * dt;
                if (MathUtil.Sign(relativeTurnSpeed) != MathUtil.Sign(relativeTurnSpeed + num3))
                {
                    num3 = -relativeTurnSpeed;
                }
            }
            else
            {
                if (this.IsReversedTurn(chassis.EffectiveTurnAxis, relativeTurnSpeed))
                {
                    num = node.speedConfig.ReverseTurnAcceleration * 0.01745329f;
                }
                num3 = ((chassis.EffectiveTurnAxis * num) * slopeCoeff) * dt;
                if ((chassis.EffectiveMoveAxis == -1f) && chassisConfig.ReverseBackTurn)
                {
                    num3 = -num3;
                }
            }
            return Mathf.Clamp((float) (relativeTurnSpeed + num3), (float) (-maxTurnSpeed * num2), (float) (maxTurnSpeed * num2));
        }

        [OnEventFire]
        public void ResetTankChassis(ResetTankSpeedEvent e, SingleNode<ChassisSmootherComponent> smoother, [JoinSelf] SingleNode<SpeedComponent> speed)
        {
            smoother.component.maxSpeedSmoother.Reset(speed.component.Speed);
            smoother.component.maxTurnSpeedSmoother.Reset(speed.component.TurnSpeed);
        }

        [OnEventFire]
        public void SetTankCollisionLayerMask(NodeAddedEvent e, TankActiveStateNode node)
        {
            int layerMask = LayerMasks.VISIBLE_FOR_CHASSIS_ACTIVE;
            node.track.LeftTrack.SetRayсastLayerMask(layerMask);
            node.track.RightTrack.SetRayсastLayerMask(layerMask);
        }

        private void UpdateInput(ChassisNode tank, bool inputEnabled)
        {
            ChassisComponent chassis = tank.chassis;
            bool flag = tank.Entity.HasComponent<TankMovableComponent>();
            chassis.EffectiveMoveAxis = !flag ? 0f : chassis.MoveAxis;
            chassis.EffectiveTurnAxis = !flag ? 0f : chassis.TurnAxis;
        }

        private void UpdateSelfInput(ChassisNode tank, bool inputEnabled, bool inverse)
        {
            ChassisComponent chassis = tank.chassis;
            float x = !inputEnabled ? 0f : (InputManager.GetUnityAxis(RIGHT_AXIS) - InputManager.GetUnityAxis(LEFT_AXIS));
            float y = !inputEnabled ? 0f : (InputManager.GetUnityAxis(FORWARD_AXIS) - InputManager.GetUnityAxis(BACKWARD_AXIS));
            if (inverse && (y < 0f))
            {
                x *= -1f;
            }
            Vector2 vector = new Vector2(chassis.TurnAxis, chassis.MoveAxis);
            Vector2 vector2 = new Vector2(x, y);
            if (vector2 != vector)
            {
                chassis.TurnAxis = x;
                chassis.MoveAxis = y;
                bool flag = tank.Entity.HasComponent<TankMovableComponent>();
                chassis.EffectiveMoveAxis = !flag ? 0f : chassis.MoveAxis;
                chassis.EffectiveTurnAxis = !flag ? 0f : chassis.TurnAxis;
                base.ScheduleEvent<ChassisControlChangedEvent>(tank);
            }
        }

        private bool UpdateSuspensionContacts(TrackComponent trackComponent, float dt, float updatePeriod)
        {
            bool flag2 = trackComponent.RightTrack.UpdateSuspensionContacts(dt, updatePeriod);
            return (trackComponent.LeftTrack.UpdateSuspensionContacts(dt, updatePeriod) && flag2);
        }

        [Inject]
        public static Tanks.Battle.ClientCore.Impl.InputManager InputManager { get; set; }

        public class ChassisInitNode : Node
        {
            public RigidbodyComponent rigidbody;
            public ChassisConfigComponent chassisConfig;
            public TankCollidersComponent tankColliders;
            public SpeedComponent speed;
            public WeightComponent weight;
            public DampingComponent damping;
        }

        [Not(typeof(TankDeadStateComponent))]
        public class ChassisNode : Node
        {
            public TankGroupComponent tankGroup;
            public RigidbodyComponent rigidbody;
            public ChassisConfigComponent chassisConfig;
            public ChassisComponent chassis;
            public TankCollidersComponent tankColliders;
            public SpeedComponent speed;
            public EffectiveSpeedComponent effectiveSpeed;
            public TrackComponent track;
            public ChassisSmootherComponent chassisSmoother;
            public SpeedConfigComponent speedConfig;
            public CameraVisibleTriggerComponent cameraVisibleTrigger;
        }

        public class TankActiveStateNode : Node
        {
            public TrackComponent track;
            public TankActiveStateComponent tankActiveState;
        }
    }
}

