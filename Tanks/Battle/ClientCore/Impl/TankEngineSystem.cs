namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class TankEngineSystem : ECSSystem
    {
        [OnEventFire]
        public void InitTankEngine(NodeAddedEvent evt, SingleNode<TankEngineConfigComponent> tank)
        {
            TankEngineComponent component = new TankEngineComponent {
                MovingBorder = tank.component.MinEngineMovingBorder,
                Value = 0f,
                CollisionTimerSec = 0f,
                HasValuableCollision = false
            };
            tank.Entity.AddComponent(component);
        }

        [OnEventFire]
        public void RecalculateParametersOnSpeedEffectStart(NodeAddedEvent evt, SpeedEffectNode effect, [Context, JoinByTank] TankEngineNode tank)
        {
            tank.tankEngine.MovingBorder = tank.tankEngineConfig.MaxEngineMovingBorder;
        }

        [OnEventFire]
        public void RecalculateParametersOnSpeedEffectStop(NodeRemoveEvent evt, SpeedEffectNode effect, [JoinByTank] TankEngineNode tank)
        {
            tank.tankEngine.MovingBorder = tank.tankEngineConfig.MinEngineMovingBorder;
        }

        [OnEventFire]
        public void ResetTankEngine(NodeAddedEvent evt, TankEngineMovableNode engine)
        {
            engine.chassis.Reset();
        }

        [OnEventFire]
        public void UpdateTankEngine(FixedUpdateEvent evt, TankEngineMovableNode tank)
        {
            TankEngineComponent tankEngine = tank.tankEngine;
            TankEngineConfigComponent tankEngineConfig = tank.tankEngineConfig;
            TrackComponent track = tank.track;
            ChassisComponent chassis = tank.chassis;
            float effectiveMoveAxis = chassis.EffectiveMoveAxis;
            float effectiveTurnAxis = chassis.EffectiveTurnAxis;
            bool hasCollision = tank.tankCollision.HasCollision;
            tankEngine.CollisionTimerSec = (hasCollision == tankEngine.HasValuableCollision) ? 0f : (tankEngine.CollisionTimerSec + evt.DeltaTime);
            if (tankEngine.CollisionTimerSec >= tankEngineConfig.EngineCollisionIntervalSec)
            {
                tankEngine.HasValuableCollision = hasCollision;
            }
            if (effectiveMoveAxis != 0f)
            {
                this.UpdateTankEngine(tankEngine, tankEngineConfig, tankEngine.HasValuableCollision, track, evt.DeltaTime, effectiveMoveAxis, tank.speed.Acceleration, tank.speedConfig.ReverseAcceleration, tank.speed.Speed, tankEngine.MovingBorder);
            }
            else if (effectiveTurnAxis == 0f)
            {
                tankEngine.Value = 0f;
            }
            else
            {
                this.UpdateTankEngine(tankEngine, tankEngineConfig, tankEngine.HasValuableCollision, track, evt.DeltaTime, effectiveTurnAxis, tank.speedConfig.TurnAcceleration, tank.speedConfig.ReverseTurnAcceleration, tank.speed.TurnSpeed, tank.tankEngineConfig.EngineTurningBorder);
            }
        }

        private void UpdateTankEngine(TankEngineComponent tankEngine, TankEngineConfigComponent tankEngineConfig, bool hasCollision, TrackComponent track, float dt, float currentAxis, float straightAcceleration, float reverseAcceleration, float maxSpeed, float border)
        {
            float b = border;
            if (((track.LeftTrack.numContacts + track.RightTrack.numContacts) > 0) && hasCollision)
            {
                b = tankEngineConfig.MaxEngineMovingBorder;
            }
            float num4 = (b * ((currentAxis <= 0f) ? reverseAcceleration : straightAcceleration)) / maxSpeed;
            tankEngine.Value += num4 * dt;
            tankEngine.Value = Mathf.Min(tankEngine.Value, b);
        }

        public class SpeedEffectNode : Node
        {
            public TurboSpeedEffectComponent turboSpeedEffect;
            public TankGroupComponent tankGroup;
        }

        public class TankEngineMovableNode : Node
        {
            public TankEngineConfigComponent tankEngineConfig;
            public TankCollisionComponent tankCollision;
            public TrackComponent track;
            public TankEngineComponent tankEngine;
            public TankMovableComponent tankMovable;
            public ChassisComponent chassis;
            public SpeedComponent speed;
            public SpeedConfigComponent speedConfig;
            public TankGroupComponent tankGroup;
        }

        public class TankEngineNode : Node
        {
            public TankEngineConfigComponent tankEngineConfig;
            public TankEngineComponent tankEngine;
            public SpeedComponent speed;
            public SpeedConfigComponent speedConfig;
            public TankGroupComponent tankGroup;
        }
    }
}

