namespace Tanks.Lobby.ClientHangar.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientGraphics.Impl;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;

    public class HangarCameraControlSystem : ECSSystem
    {
        private const float MIN_ROTATION_ANGLE = 0.1f;
        private const float MAX_ROTATION_SPEED = 1080f;

        [OnEventFire]
        public void DecelerationRotate(UpdateEvent e, HangarCameraTankViewDecelerationRotateNode hangar)
        {
            hangar.hangarCameraDecelerationRotate.Speed *= Mathf.Exp(-hangar.hangarConfig.DecelerationRotateFactor * Time.deltaTime);
            float f = hangar.hangarCameraDecelerationRotate.Speed * Time.deltaTime;
            if (Mathf.Abs(f) < 0.1f)
            {
                hangar.Entity.RemoveComponent<HangarCameraDecelerationRotateComponent>();
            }
            else
            {
                base.ScheduleEvent(new HangarCameraRotateEvent(f), hangar);
            }
        }

        [OnEventFire]
        public void MouseRotate(EventSystemOnDragEvent e, ScreenForegroundNode screenForeground, [JoinAll] HangarCameraTankViewRotateNode hangar)
        {
            HangarCameraRotateEvent eventInstance = new HangarCameraRotateEvent();
            float num = e.PointerEventData.delta.x * hangar.hangarConfig.MouseRotateFactor;
            eventInstance.Angle = num;
            hangar.hangarCameraDecelerationRotate.Speed = num / Time.deltaTime;
            hangar.hangarCameraDecelerationRotate.LastUpdateFrame = Time.frameCount;
            base.ScheduleEvent(eventInstance, hangar);
        }

        [OnEventFire]
        public void MouseRotateBegin(EventSystemOnBeginDragEvent e, ScreenForegroundNode screenForeground, [JoinAll] HangarCameraTankViewIdleNode hangar)
        {
            hangar.Entity.AddComponent<HangarCameraDecelerationRotateComponent>();
        }

        [OnEventFire]
        public void MouseRotateBegin(EventSystemOnBeginDragEvent e, ScreenForegroundNode screenForeground, [JoinAll] HangarCameraTankViewNonDragNode hangar)
        {
            hangar.Entity.AddComponent<HangarCameraDragComponent>();
        }

        [OnEventFire]
        public void MouseRotateEnd(EventSystemOnEndDragEvent e, ScreenForegroundNode screenForeground, [JoinAll] HangarCameraTankViewRotateNode hangar)
        {
            hangar.Entity.RemoveComponent<HangarCameraDragComponent>();
            if ((Time.frameCount - hangar.hangarCameraDecelerationRotate.LastUpdateFrame) > 1)
            {
                hangar.Entity.RemoveComponent<HangarCameraDecelerationRotateComponent>();
            }
            else if (hangar.hangarCameraDecelerationRotate.Speed > 1080f)
            {
                hangar.hangarCameraDecelerationRotate.Speed = 1080f;
            }
        }

        [OnEventFire]
        public void StopDecelerationRotate(NodeRemoveEvent e, HangarCameraTankViewStateNode hangar, [JoinAll] HangarCameraTankViewDecelerationRotateNode hangarRotate)
        {
            hangarRotate.Entity.RemoveComponent<HangarCameraDecelerationRotateComponent>();
        }

        [OnEventFire]
        public void StopDecelerationRotate(EventSystemOnPointerDownEvent e, ScreenForegroundNode screenForeground, [JoinAll] HangarCameraTankViewDecelerationRotateNode hangar)
        {
            hangar.Entity.RemoveComponent<HangarCameraDecelerationRotateComponent>();
        }

        [Not(typeof(HangarCameraDragComponent))]
        public class HangarCameraTankViewDecelerationRotateNode : HangarCameraControlSystem.HangarCameraTankViewStateNode
        {
            public HangarCameraDecelerationRotateComponent hangarCameraDecelerationRotate;
        }

        [Not(typeof(HangarCameraDecelerationRotateComponent))]
        public class HangarCameraTankViewIdleNode : HangarCameraControlSystem.HangarCameraTankViewStateNode
        {
        }

        [Not(typeof(HangarCameraDragComponent))]
        public class HangarCameraTankViewNonDragNode : HangarCameraControlSystem.HangarCameraTankViewStateNode
        {
        }

        public class HangarCameraTankViewRotateNode : HangarCameraControlSystem.HangarCameraTankViewStateNode
        {
            public HangarCameraDragComponent hangarCameraDrag;
            public HangarCameraDecelerationRotateComponent hangarCameraDecelerationRotate;
        }

        public class HangarCameraTankViewStateNode : Node
        {
            public HangarCameraComponent hangarCamera;
            public CameraComponent camera;
            public HangarConfigComponent hangarConfig;
            public HangarTankPositionComponent hangarTankPosition;
            public HangarCameraTankViewComponent hangarCameraTankView;
        }

        public class ScreenForegroundNode : Node
        {
            public ScreenForegroundComponent screenForeground;
            public EventSystemProviderComponent eventSystemProvider;
        }
    }
}

