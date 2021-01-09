namespace Tanks.Lobby.ClientHangar.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientGraphics.Impl;
    using Tanks.Lobby.ClientHangar.API;
    using Tanks.Lobby.ClientNavigation.API;
    using UnityEngine;

    public class HangarCameraFlightToTankSystem : ECSSystem
    {
        [OnEventFire]
        public void CalculateFlightToTank(NodeRemoveEvent e, HangarLocationScreenNode screen, [JoinAll] HangarCameraNode hangar, [JoinAll] Optional<NewHangarLocationScreenNode> newLocationScreen)
        {
            if (!newLocationScreen.IsPresent())
            {
                this.StartFlightToTank(hangar);
            }
        }

        [OnEventFire]
        public void CalculateFlightToTank(GoBackEvent e, Node any, [Combine, JoinAll] HangarLocationScreenNode screen, [Combine, JoinAll] HangarCameraNode hangar)
        {
            this.StartFlightToTank(hangar);
        }

        [OnEventFire]
        public void CalculateFlightToTank(ShowScreenEvent e, Node any, [Combine, JoinAll] HangarLocationScreenNode screen, [Combine, JoinAll] HangarCameraNode hangar)
        {
            this.StartFlightToTank(hangar);
        }

        private void StartFlightToTank(HangarCameraNode hangar)
        {
            base.ScheduleEvent<HangarCameraStopFlightEvent>(hangar);
            HangarConfigComponent hangarConfig = hangar.hangarConfig;
            Vector3 position = hangar.cameraRootTransform.Root.position;
            Vector3 a = hangar.hangarTankPosition.transform.position;
            a.y = position.y;
            Vector3 vector3 = position - a;
            vector3.y = 0f;
            vector3.Normalize();
            Vector3 b = hangar.hangarCameraStartPosition.transform.position;
            b.y = position.y;
            float num2 = Vector3.Distance(a, position);
            float num3 = Mathf.Asin(hangarConfig.FlightToTankRadius / num2) * 2f;
            Vector3 vector5 = (Vector3) (Quaternion.Euler(0f, num3 * 57.29578f, 0f) * vector3);
            vector5.Normalize();
            Vector3 vector8 = ((vector3 + vector5) / 2f).normalized * (num2 / Mathf.Cos(num3 / 2f));
            vector8.y = position.y;
            float num5 = 3.141593f + num3;
            float num6 = (num2 * Mathf.Tan(num3 / 2f)) * num5;
            Vector3 vector9 = a + (vector5 * Vector3.Distance(a, b));
            Vector3 vector10 = a + (vector5 * num2);
            float num7 = Vector3.Distance(vector10, vector9);
            if (num6 > num7)
            {
                num6 = 0f;
            }
            vector9.y = hangar.hangarCameraStartPosition.transform.position.y;
            float num8 = hangarConfig.FlightToTankTime / (num6 + num7);
            Quaternion quaternion = Quaternion.LookRotation(hangar.hangarTankPosition.transform.position - vector9, Vector3.up);
            Quaternion quaternion2 = Quaternion.Slerp(hangar.cameraRootTransform.Root.rotation, quaternion, 0.5f);
            HangarCameraFlightDataComponent component = new HangarCameraFlightDataComponent {
                FlightTime = hangarConfig.FlightToTankTime,
                ArcFlightPivotPoint = vector8,
                ArcFlightTime = num6 * num8
            };
            if (num6 > 0f)
            {
                component.ArcFlightAngleSpeed = (num5 * 57.29578f) / component.ArcFlightTime;
            }
            component.ArcToLinearPoint = vector10;
            component.LinearFlightTime = num7 * num8;
            component.OriginCameraRotation = hangar.cameraRootTransform.Root.rotation;
            component.OriginCameraPosition = hangar.cameraRootTransform.Root.position;
            component.MiddleCameraRotation = quaternion2;
            component.DestinationCameraPosition = vector9;
            component.DestinationCameraRotation = quaternion;
            component.StartFlightTime = UnityTime.time;
            if (hangar.Entity.HasComponent<HangarCameraFlightDataComponent>())
            {
                hangar.Entity.RemoveComponent<HangarCameraFlightDataComponent>();
            }
            hangar.Entity.AddComponent(component);
            hangar.hangarCameraViewState.Esm.ChangeState<HangarCameraViewState.FlightToTankState>();
        }

        [OnEventFire]
        public void SwitchToLocationView(HangarCameraStopFlightEvent e, HangarCameraFlightToTankNode hangar)
        {
            hangar.hangarCameraViewState.Esm.ChangeState<HangarCameraViewState.TankViewState>();
        }

        [OnEventComplete]
        public void UpdateCameraHight(UpdateEvent e, HangarCameraFlightToTankNode hangar)
        {
            HangarConfigComponent hangarConfig = hangar.hangarConfig;
            Vector3 position = hangar.cameraRootTransform.Root.position;
            position.y = Vector3.Lerp(hangar.hangarCameraFlightData.OriginCameraPosition, hangar.hangarCameraFlightData.DestinationCameraPosition, Mathf.Clamp01((UnityTime.time - hangar.hangarCameraFlightData.StartFlightTime) / hangarConfig.FlightToTankTime)).y;
            hangar.cameraRootTransform.Root.position = position;
            base.ScheduleEvent<HangarCameraRotateToDestinationEvent>(hangar);
        }

        [Inject]
        public static Platform.Library.ClientUnityIntegration.API.UnityTime UnityTime { get; set; }

        public class HangarCameraFlightToTankNode : HangarCameraFlightToTankSystem.HangarCameraNode
        {
            public HangarCameraFlightDataComponent hangarCameraFlightData;
            public HangarCameraFlightToTankComponent hangarCameraFlightToTank;
        }

        public class HangarCameraLocationViewNode : HangarCameraFlightToTankSystem.HangarCameraNode
        {
            public HangarCameraLocationViewComponent hangarCameraLocationView;
        }

        public class HangarCameraNode : Node
        {
            public HangarComponent hangar;
            public HangarCameraComponent hangarCamera;
            public CameraComponent camera;
            public CameraRootTransformComponent cameraRootTransform;
            public HangarConfigComponent hangarConfig;
            public HangarTankPositionComponent hangarTankPosition;
            public HangarCameraStartPositionComponent hangarCameraStartPosition;
            public HangarCameraViewStateComponent hangarCameraViewState;
        }

        [Not(typeof(HangarCameraArcFlightComponent))]
        public class HangarCameraNotFlightToTankNode : HangarCameraFlightToTankSystem.HangarCameraLocationViewNode
        {
        }

        public class HangarLocationScreenNode : Node
        {
            public HangarLocationComponent hangarLocation;
        }

        public class NewHangarLocationScreenNode : NotDeletedEntityNode
        {
            public HangarLocationComponent hangarLocation;
        }
    }
}

