namespace Tanks.Lobby.ClientHangar.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientGraphics.Impl;
    using Tanks.Lobby.ClientHangar.API;
    using UnityEngine;

    public class HangarCameraFlightToLocationSystem : ECSSystem
    {
        private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
        {
            float num = 1f - t;
            return (Vector3) ((((num * num) * p0) + (((2f * num) * t) * p1)) + ((t * t) * p2));
        }

        [OnEventFire]
        public void CalculateFlightToLocation(NodeAddedEvent e, HangarLocationScreenNode screen, [JoinAll] HangarCameraNode hangar, [JoinAll] ICollection<HangarLocationScreenNode> activeLocationScreen)
        {
            Transform transform;
            if ((activeLocationScreen.Count <= 1) && hangar.hangarLocations.Locations.TryGetValue(screen.hangarLocation.HangarLocation, out transform))
            {
                base.ScheduleEvent<HangarCameraStopFlightEvent>(hangar);
                HangarConfigComponent hangarConfig = hangar.hangarConfig;
                Vector3 position = hangar.cameraRootTransform.Root.position;
                Vector3 a = hangar.hangarTankPosition.transform.position;
                a.y = position.y;
                Vector3 b = transform.position;
                b.y = position.y;
                Vector3 from = position - a;
                from.y = 0f;
                from.Normalize();
                Vector3 to = b - a;
                to.y = 0f;
                to.Normalize();
                float num = Vector3.Distance(a, position);
                float num2 = Vector3.Angle(from, to) * 0.01745329f;
                Vector3 vector8 = Vector3.Cross(to, from);
                Vector3 vector9 = ((from + to) / 2f).normalized * (num / Mathf.Cos(num2 / 2f));
                float num4 = 3.141593f - num2;
                float num5 = (num * Mathf.Tan(num2 / 2f)) * num4;
                Vector3 vector10 = a + (to * num);
                float num6 = Vector3.Distance(vector10, b);
                if (num5 > num6)
                {
                    num5 = 0f;
                }
                float num7 = hangarConfig.FlightToLocationTime / (num5 + num6);
                Vector3 vector11 = Vector3.Cross((Vector3) (hangar.cameraRootTransform.Root.rotation * Vector3.forward), (Vector3) (transform.rotation * Vector3.forward));
                Quaternion quaternion = Quaternion.Slerp(hangar.cameraRootTransform.Root.rotation, transform.rotation, 0.5f);
                if ((MathUtil.Sign(vector8.y) > 0f) && (MathUtil.Sign(vector11.y) < 0f))
                {
                    quaternion = Quaternion.AngleAxis(180f, Vector3.up) * quaternion;
                }
                HangarCameraFlightDataComponent component = new HangarCameraFlightDataComponent {
                    FlightTime = hangarConfig.FlightToLocationTime,
                    ArcFlightPivotPoint = vector9,
                    ArcFlightTime = num5 * num7
                };
                if (num5 > 0f)
                {
                    component.ArcFlightAngleSpeed = ((num4 * 57.29578f) / component.ArcFlightTime) * MathUtil.Sign(vector8.y);
                }
                component.ArcToLinearPoint = vector10;
                component.LinearFlightTime = num6 * num7;
                component.OriginCameraRotation = hangar.cameraRootTransform.Root.rotation;
                component.OriginCameraPosition = hangar.cameraRootTransform.Root.position;
                component.MiddleCameraRotation = quaternion;
                component.DestinationCameraPosition = transform.position;
                component.DestinationCameraRotation = transform.rotation;
                component.StartFlightTime = UnityTime.time;
                if (hangar.Entity.HasComponent<HangarCameraFlightDataComponent>())
                {
                    hangar.Entity.RemoveComponent<HangarCameraFlightDataComponent>();
                }
                hangar.Entity.AddComponent(component);
                hangar.hangarCameraViewState.Esm.ChangeState<HangarCameraViewState.FlightToLocationState>();
            }
        }

        [OnEventFire]
        public void SwitchToLocationView(HangarCameraStopFlightEvent e, HangarCameraFlightToLocationNode hangar)
        {
            hangar.hangarCameraViewState.Esm.ChangeState<HangarCameraViewState.LocationViewState>();
        }

        [OnEventComplete]
        public void UpdateCameraHight(UpdateEvent e, HangarCameraFlightToLocationNode hangar)
        {
            HangarConfigComponent hangarConfig = hangar.hangarConfig;
            Vector3 position = hangar.cameraRootTransform.Root.position;
            position.y = this.CalculateBezierPoint(Mathf.Clamp01((UnityTime.time - hangar.hangarCameraFlightData.StartFlightTime) / hangarConfig.FlightToLocationTime), hangar.hangarCameraFlightData.OriginCameraPosition, hangar.hangarTankPosition.transform.position + (Vector3.up * hangarConfig.FlightToLocationHigh), hangar.hangarCameraFlightData.DestinationCameraPosition).y;
            hangar.cameraRootTransform.Root.position = position;
            base.ScheduleEvent<HangarCameraRotateToDestinationEvent>(hangar);
        }

        [Inject]
        public static Platform.Library.ClientUnityIntegration.API.UnityTime UnityTime { get; set; }

        public class HangarCameraFlightToLocationNode : HangarCameraFlightToLocationSystem.HangarCameraNode
        {
            public HangarCameraFlightDataComponent hangarCameraFlightData;
            public HangarCameraFlightToLocationComponent hangarCameraFlightToLocation;
        }

        public class HangarCameraNode : Node
        {
            public HangarComponent hangar;
            public HangarCameraComponent hangarCamera;
            public CameraComponent camera;
            public CameraRootTransformComponent cameraRootTransform;
            public HangarConfigComponent hangarConfig;
            public HangarTankPositionComponent hangarTankPosition;
            public HangarCameraViewStateComponent hangarCameraViewState;
            public HangarLocationsComponent hangarLocations;
        }

        public class HangarCameraTankViewNode : HangarCameraFlightToLocationSystem.HangarCameraNode
        {
            public HangarCameraTankViewComponent hangarCameraTankView;
        }

        public class HangarLocationScreenNode : Node
        {
            public HangarLocationComponent hangarLocation;
        }
    }
}

