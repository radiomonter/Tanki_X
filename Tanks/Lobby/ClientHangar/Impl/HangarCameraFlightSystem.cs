namespace Tanks.Lobby.ClientHangar.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Battle.ClientGraphics.Impl;
    using UnityEngine;

    public class HangarCameraFlightSystem : ECSSystem
    {
        [OnEventFire]
        public void ArcFlight(TimeUpdateEvent e, HangarCameraArcFlightNode hangar)
        {
            HangarCameraFlightDataComponent hangarCameraFlightData = hangar.hangarCameraFlightData;
            hangar.cameraRootTransform.Root.RotateAround(hangarCameraFlightData.ArcFlightPivotPoint, Vector3.up, e.DeltaTime * hangarCameraFlightData.ArcFlightAngleSpeed);
        }

        [OnEventFire]
        public void Deinit(NodeRemoveEvent e, HangarCameraInitNode hangar)
        {
            base.ScheduleEvent<HangarCameraStopFlightEvent>(hangar);
        }

        [OnEventFire]
        public void Init(NodeAddedEvent e, HangarCameraNode hangar)
        {
            this.SetupCameraFlightESM(hangar.Entity);
            HangarCameraFlightDataComponent hangarCameraFlightData = hangar.hangarCameraFlightData;
            base.NewEvent<HangarCameraArcToLinearFlightEvent>().Attach(hangar).ScheduleDelayed(hangarCameraFlightData.ArcFlightTime);
            base.NewEvent<HangarCameraStopFlightEvent>().Attach(hangar).ScheduleDelayed(hangarCameraFlightData.FlightTime);
        }

        [OnEventFire]
        public void LinearFlight(TimeUpdateEvent e, HangarCameraLinearFlightNode hangar)
        {
            HangarCameraFlightDataComponent hangarCameraFlightData = hangar.hangarCameraFlightData;
            float t = Mathf.Pow(Mathf.Clamp01(((UnityTime.time - hangar.hangarCameraFlightData.StartFlightTime) - hangarCameraFlightData.ArcFlightTime) / hangarCameraFlightData.LinearFlightTime), 0.3333333f);
            hangar.cameraRootTransform.Root.position = Vector3.Lerp(hangarCameraFlightData.ArcToLinearPoint, hangarCameraFlightData.DestinationCameraPosition, t);
        }

        [OnEventFire]
        public void RotateToDestination(HangarCameraRotateToDestinationEvent e, HangarCameraNode hangar)
        {
            Quaternion middleCameraRotation;
            Quaternion originCameraRotation;
            HangarCameraFlightDataComponent hangarCameraFlightData = hangar.hangarCameraFlightData;
            float t = Mathf.Pow((UnityTime.time - hangar.hangarCameraFlightData.StartFlightTime) / hangarCameraFlightData.FlightTime, 0.3333333f);
            if (t < 0.5)
            {
                originCameraRotation = hangarCameraFlightData.OriginCameraRotation;
                middleCameraRotation = hangarCameraFlightData.MiddleCameraRotation;
                t *= 2f;
            }
            else
            {
                originCameraRotation = hangarCameraFlightData.MiddleCameraRotation;
                middleCameraRotation = hangarCameraFlightData.DestinationCameraRotation;
                t = (t - 0.5f) * 2f;
            }
            hangar.cameraRootTransform.Root.SetRotationSafe(Quaternion.Slerp(originCameraRotation, middleCameraRotation, t));
        }

        private void SetupCameraFlightESM(Entity camera)
        {
            HangarCameraFlightStateComponent component = new HangarCameraFlightStateComponent();
            camera.AddComponent(component);
            EntityStateMachine esm = component.Esm;
            esm.AddState<HangarCameraFlightState.EmptyState>();
            esm.AddState<HangarCameraFlightState.ArcFlightState>();
            esm.AddState<HangarCameraFlightState.LinearFlightState>();
            esm.ChangeState<HangarCameraFlightState.ArcFlightState>();
        }

        [OnEventFire]
        public void StartLinearFlight(NodeAddedEvent e, HangarCameraLinearFlightNode hangar)
        {
            hangar.hangarCameraFlightData.ArcToLinearPoint = hangar.cameraRootTransform.Root.position;
        }

        [OnEventComplete]
        public void StopFlight(HangarCameraStopFlightEvent e, HangarCameraFlightNode hangar)
        {
            hangar.hangarCameraFlightState.Esm.ChangeState<HangarCameraFlightState.EmptyState>();
            hangar.Entity.RemoveComponent<HangarCameraFlightDataComponent>();
            hangar.Entity.RemoveComponent<HangarCameraFlightStateComponent>();
        }

        [OnEventFire]
        public void StopLinearFlight(NodeRemoveEvent e, HangarCameraLinearFlightNode hangar)
        {
            HangarCameraFlightDataComponent hangarCameraFlightData = hangar.hangarCameraFlightData;
            hangar.cameraRootTransform.Root.position = hangarCameraFlightData.DestinationCameraPosition;
        }

        [OnEventFire]
        public void StopRotateToDestination(NodeRemoveEvent e, HangarCameraNode hangar)
        {
            HangarCameraFlightDataComponent hangarCameraFlightData = hangar.hangarCameraFlightData;
            hangar.cameraRootTransform.Root.SetRotationSafe(hangarCameraFlightData.DestinationCameraRotation);
        }

        [OnEventFire]
        public void SwitchToLinearFlight(HangarCameraArcToLinearFlightEvent e, HangarCameraArcFlightNode hangar)
        {
            hangar.hangarCameraFlightState.Esm.ChangeState<HangarCameraFlightState.LinearFlightState>();
        }

        [Inject]
        public static Platform.Library.ClientUnityIntegration.API.UnityTime UnityTime { get; set; }

        public class HangarCameraArcFlightNode : HangarCameraFlightSystem.HangarCameraNode
        {
            public HangarCameraFlightStateComponent hangarCameraFlightState;
            public HangarCameraArcFlightComponent hangarCameraArcFlight;
        }

        public class HangarCameraFlightNode : HangarCameraFlightSystem.HangarCameraNode
        {
            public HangarCameraFlightStateComponent hangarCameraFlightState;
        }

        public class HangarCameraInitNode : Node
        {
            public HangarComponent hangar;
            public HangarCameraComponent hangarCamera;
            public CameraComponent camera;
            public CameraRootTransformComponent cameraRootTransform;
            public HangarConfigComponent hangarConfig;
            public HangarTankPositionComponent hangarTankPosition;
        }

        public class HangarCameraLinearFlightNode : HangarCameraFlightSystem.HangarCameraNode
        {
            public HangarCameraFlightStateComponent hangarCameraFlightState;
            public HangarCameraLinearFlightComponent hangarCameraLinearFlight;
        }

        public class HangarCameraNode : HangarCameraFlightSystem.HangarCameraInitNode
        {
            public HangarCameraFlightDataComponent hangarCameraFlightData;
        }
    }
}

