namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Battle.ClientGraphics.API;
    using UnityEngine;

    public class TrackDustSystem : ECSSystem
    {
        private const float MAX_WORK_DISTANCE = 30f;
        private const int EMISSION_RAY_NUMBER = 2;

        [OnEventFire]
        public void InitTrackDustSystem(NodeAddedEvent evt, [Combine] TrackDustInitNode tank, SingleNode<MapDustComponent> mapDust)
        {
            TrackComponent track = tank.track;
            TrackDustComponent trackDust = tank.trackDust;
            trackDust.leftTrackDustDelay = new float[track.LeftTrack.rays.Length];
            trackDust.rightTrackDustDelay = new float[track.RightTrack.rays.Length];
            CollisionDustBehaviour collisionDustBehaviour = tank.hullInstance.HullInstance.AddComponent<CollisionDustBehaviour>();
            collisionDustBehaviour.mapDust = mapDust.component;
            if (tank.Entity.HasComponent<CollisionDustComponent>())
            {
                tank.Entity.GetComponent<CollisionDustComponent>().CollisionDustBehaviour = collisionDustBehaviour;
            }
            else
            {
                tank.Entity.AddComponent(new CollisionDustComponent(collisionDustBehaviour));
            }
        }

        private void TryEmitFromSuspensionRay(float maxCompression, Track track, MapDustComponent mapDust, float[] delays, int i)
        {
            SuspensionRay ray = track.rays[i];
            float num = delays[i] - UnityTime.deltaTime;
            if (!ray.hasCollision)
            {
                delays[i] = num;
            }
            else
            {
                RaycastHit rayHit = ray.rayHit;
                Transform target = rayHit.transform;
                DustEffectBehaviour effectByTag = mapDust.GetEffectByTag(target, rayHit.textureCoord);
                if (effectByTag == null)
                {
                    delays[i] = num;
                }
                else
                {
                    Vector3 point = rayHit.point;
                    if (num <= 0f)
                    {
                        num = 1f / effectByTag.movementEmissionRate.RandomValue;
                        effectByTag.TryEmitParticle(point, ray.velocity);
                        delays[i] = num;
                    }
                    else
                    {
                        if (!ray.hadPreviousCollision)
                        {
                            float num2 = Mathf.Clamp01(ray.compression / maxCompression);
                            if (num2 > effectByTag.landingCompressionThreshold)
                            {
                                effectByTag.TryEmitParticle(point, Vector3.up * (effectByTag.movementSpeedThreshold.max * num2));
                            }
                        }
                        delays[i] = num;
                    }
                }
            }
        }

        [OnEventFire]
        public void TryEmitFromTracks(UpdateEvent evt, TrackDustUpdateNode tankNode, [JoinAll] SingleNode<MapDustComponent> mapDustNode)
        {
            if (tankNode.cameraVisibleTrigger.IsVisibleAtRange(30f))
            {
                TrackComponent component = tankNode.track;
                TrackDustComponent trackDust = tankNode.trackDust;
                ChassisConfigComponent chassisConfig = tankNode.chassisConfig;
                float maxRayLength = chassisConfig.MaxRayLength;
                Track leftTrack = component.LeftTrack;
                Track rightTrack = component.RightTrack;
                MapDustComponent mapDust = mapDustNode.component;
                float[] leftTrackDustDelay = trackDust.leftTrackDustDelay;
                float[] rightTrackDustDelay = trackDust.rightTrackDustDelay;
                int numRaysPerTrack = chassisConfig.NumRaysPerTrack;
                for (int i = 0; i < numRaysPerTrack; i += 2)
                {
                    this.TryEmitFromSuspensionRay(maxRayLength, leftTrack, mapDust, leftTrackDustDelay, i);
                    this.TryEmitFromSuspensionRay(maxRayLength, rightTrack, mapDust, rightTrackDustDelay, i);
                }
            }
        }

        [Inject]
        public static Platform.Library.ClientUnityIntegration.API.UnityTime UnityTime { get; set; }

        public class TrackDustInitNode : Node
        {
            public TankVisualRootComponent tankVisualRoot;
            public HullInstanceComponent hullInstance;
            public ChassisConfigComponent chassisConfig;
            public TrackComponent track;
            public TrackDustComponent trackDust;
        }

        public class TrackDustUpdateNode : Node
        {
            public TankActiveStateComponent tankActiveState;
            public CameraVisibleTriggerComponent cameraVisibleTrigger;
            public ChassisConfigComponent chassisConfig;
            public TrackComponent track;
            public TrackDustComponent trackDust;
        }
    }
}

