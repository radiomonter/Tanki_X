namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Battle.ClientGraphics.API;
    using Tanks.Lobby.ClientSettings.API;
    using UnityEngine;

    public class ChassisAnimationSystem : ECSSystem
    {
        private const float MAX_WORK_DISTANCE = 50f;
        private const float MIN_TRACK_SPEED = 0.05f;
        private const float TRACK_SPEED_COEFF = 0.5f;

        private void AnimateTrackWithContacts(Track track, AliveChassisAnimationNode node, float dt)
        {
            ChassisComponent chassis = node.chassis;
            float forwardTrackSpeed = this.GetForwardTrackSpeed(track, node.rigidbody.Rigidbody);
            if (this.RequiresSynchronizedAnimation(track, chassis, forwardTrackSpeed))
            {
                track.animationSpeed = forwardTrackSpeed;
            }
            else
            {
                float targetValue = this.GetDesiredSpeedCoeff(track, chassis) * 0.05f;
                track.SetAnimationSpeed(targetValue, node.speed.Acceleration * dt);
            }
        }

        private void AnimateTrackWithoutContacts(Track track, AliveChassisAnimationNode node, float maxSpeed, float dt)
        {
            float targetValue = Mathf.Clamp((maxSpeed * node.chassis.EffectiveMoveAxis) + (((-node.chassis.EffectiveTurnAxis * maxSpeed) / 2f) * track.side), -maxSpeed, maxSpeed);
            track.SetAnimationSpeed(targetValue, node.speed.Acceleration * dt);
        }

        [OnEventFire]
        public void AnimateWheelsAndTracksPosition(TimeUpdateEvent evt, ChassisAnimationNode node)
        {
            if (((GraphicsSettings.INSTANCE.CurrentQualityLevel > 1) || !node.Entity.HasComponent<SelfTankComponent>()) ? ((GraphicsSettings.INSTANCE.CurrentQualityLevel > 1) && node.cameraVisibleTrigger.IsVisibleAtRange(50f)) : true)
            {
                ChassisAnimationComponent chassisAnimation = node.chassisAnimation;
                TrackController leftTrack = node.chassisTrackController.LeftTrack;
                TrackController rightTrack = node.chassisTrackController.RightTrack;
                leftTrack.highDistance = chassisAnimation.highDistance;
                leftTrack.lowDistance = chassisAnimation.lowDistance;
                rightTrack.highDistance = chassisAnimation.highDistance;
                rightTrack.lowDistance = chassisAnimation.lowDistance;
                Transform transform = node.tankVisualRoot.transform;
                if (chassisAnimation.additionalRaycastsEnabled)
                {
                    leftTrack.AnimateWithAdditionalRays(transform, chassisAnimation.smoothSpeed, chassisAnimation.maxStretchingCoeff);
                    rightTrack.AnimateWithAdditionalRays(transform, chassisAnimation.smoothSpeed, chassisAnimation.maxStretchingCoeff);
                }
                else
                {
                    leftTrack.Animate(transform, chassisAnimation.smoothSpeed, chassisAnimation.maxStretchingCoeff);
                    rightTrack.Animate(transform, chassisAnimation.smoothSpeed, chassisAnimation.maxStretchingCoeff);
                }
            }
        }

        [OnEventComplete]
        public void AnimateWheelsAndTracksRotation(TimeUpdateEvent evt, AliveChassisAnimationNode node)
        {
            if (node.cameraVisibleTrigger.IsVisibleAtRange(50f))
            {
                ChassisAnimationComponent chassisAnimation = node.chassisAnimation;
                TrackComponent track = node.track;
                TrackController leftTrack = node.chassisTrackController.LeftTrack;
                TrackController rightTrack = node.chassisTrackController.RightTrack;
                float maxSpeed = node.effectiveSpeed.MaxSpeed;
                float deltaTime = evt.DeltaTime;
                this.CalculateTrackAnimationSpeed(track.LeftTrack, node, maxSpeed, deltaTime);
                chassisAnimation.LeftTrackPosition += (track.LeftTrack.animationSpeed * deltaTime) * 0.5f;
                leftTrack.UpdateWheelsRotation(chassisAnimation.LeftTrackPosition);
                this.CalculateTrackAnimationSpeed(track.RightTrack, node, maxSpeed, deltaTime);
                chassisAnimation.RightTrackPosition += (track.RightTrack.animationSpeed * deltaTime) * 0.5f;
                rightTrack.UpdateWheelsRotation(chassisAnimation.RightTrackPosition);
                if (chassisAnimation.TracksMaterial != null)
                {
                    float num4 = -chassisAnimation.RightTrackPosition * chassisAnimation.tracksMaterialSpeedMultiplyer;
                    Vector2 vector = new Vector2((-chassisAnimation.LeftTrackPosition * chassisAnimation.tracksMaterialSpeedMultiplyer) % 1f, num4 % 1f);
                    chassisAnimation.TracksMaterial.SetVector(TankMaterialPropertyNames.TRACKS_OFFSET, vector);
                }
            }
        }

        private void CalculateTrackAnimationSpeed(Track track, AliveChassisAnimationNode node, float maxSpeed, float dt)
        {
            if (this.HasCorrectContacts(track, node.rigidbody.Rigidbody))
            {
                this.AnimateTrackWithContacts(track, node, dt);
            }
            else
            {
                this.AnimateTrackWithoutContacts(track, node, maxSpeed, dt);
            }
        }

        private float GetDesiredSpeedCoeff(Track track, ChassisComponent chassis)
        {
            float effectiveMoveAxis = chassis.EffectiveMoveAxis;
            float effectiveTurnAxis = chassis.EffectiveTurnAxis;
            return ((effectiveMoveAxis != 0f) ? ((effectiveTurnAxis != 0f) ? ((effectiveMoveAxis * (3f + (track.side * effectiveTurnAxis))) / 4f) : effectiveMoveAxis) : ((track.side * effectiveTurnAxis) * 0.5f));
        }

        private float GetForwardTrackSpeed(Track track, Rigidbody rigidbody) => 
            Vector3.Dot(track.averageVelocity - track.averageSurfaceVelocity, rigidbody.transform.forward);

        private bool HasCorrectContacts(Track track, Rigidbody rigidbody) => 
            (track != null) && ((rigidbody.transform.up.y > 0f) && (track.numContacts > 0));

        private bool RequiresSynchronizedAnimation(Track track, ChassisComponent chassis, float trackSpeed)
        {
            float desiredSpeedCoeff = this.GetDesiredSpeedCoeff(track, chassis);
            return (((Mathf.Abs(trackSpeed) > 0.04f) || (desiredSpeedCoeff == 0f)) || ((MathUtil.SignEpsilon(trackSpeed, 0.01f) * MathUtil.Sign(desiredSpeedCoeff)) == -1f));
        }

        public class AliveChassisAnimationNode : Node
        {
            public ChassisComponent chassis;
            public CameraVisibleTriggerComponent cameraVisibleTrigger;
            public ChassisAnimationComponent chassisAnimation;
            public ChassisTrackControllerComponent chassisTrackController;
            public TrackComponent track;
            public EffectiveSpeedComponent effectiveSpeed;
            public SpeedComponent speed;
            public RigidbodyComponent rigidbody;
            public TankMovableComponent tankMovable;
        }

        public class ChassisAnimationNode : Node
        {
            public TankVisualRootComponent tankVisualRoot;
            public CameraVisibleTriggerComponent cameraVisibleTrigger;
            public ChassisAnimationComponent chassisAnimation;
            public ChassisTrackControllerComponent chassisTrackController;
            public SpeedComponent speed;
        }
    }
}

