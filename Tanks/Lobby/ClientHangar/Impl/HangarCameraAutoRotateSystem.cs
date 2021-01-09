namespace Tanks.Lobby.ClientHangar.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientGraphics.Impl;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.API;

    public class HangarCameraAutoRotateSystem : ECSSystem
    {
        [OnEventFire]
        public void CheckUserActionOnAnyPointerEvent(EventSystemPointerEvent e, SingleNode<ScreenForegroundComponent> foreground, [JoinAll] HangarCameraRotateScheduledTankViewStateNode hangar)
        {
            base.ScheduleEvent<HangarCameraDelayAutoRotateEvent>(hangar);
        }

        [OnEventFire]
        public void DelayScheduledEvent(HangarCameraDelayAutoRotateEvent e, Node any, [JoinAll] HangarCameraRotateScheduledTankViewStateNode hangar)
        {
            hangar.hangarCameraRotateScheduled.ScheduledEvent.Manager().Cancel();
            hangar.hangarCameraRotateScheduled.ScheduledEvent = base.NewEvent<HangarCameraStartAutoRotateEvent>().Attach(hangar).ScheduleDelayed(hangar.hangarConfig.AutoRotateDelay);
        }

        [OnEventFire]
        public void DisableAutoRotation(NodeRemoveEvent e, HangarCameraTankViewStateRotationEnabledNode nr, [JoinSelf] HangarCameraAutoRotateNode hangar)
        {
            hangar.hangarCameraRotateScheduled.ScheduledEvent.Manager().Cancel();
            hangar.Entity.RemoveComponent<HangarCameraAutoRotateComponent>();
        }

        [OnEventFire]
        public void DisableSchedule(NodeRemoveEvent e, HangarCameraTankViewStateRotationEnabledNode nr, [JoinSelf] HangarCameraRotateScheduledTankViewStateNode hangar)
        {
            hangar.hangarCameraRotateScheduled.ScheduledEvent.Manager().Cancel();
            hangar.Entity.RemoveComponent<HangarCameraRotateScheduledComponent>();
        }

        [OnEventFire]
        public void RotateCamera(TimeUpdateEvent e, HangarCameraAutoRotateNode hangar)
        {
            base.ScheduleEvent(new HangarCameraRotateEvent(e.DeltaTime * hangar.hangarConfig.AutoRotateSpeed), hangar);
        }

        [OnEventFire]
        public void StartRotate(HangarCameraStartAutoRotateEvent e, HangarCameraRotateScheduledTankViewStateNode hangar)
        {
            hangar.hangarCameraRotateScheduled.ScheduledEvent.Manager().Cancel();
            hangar.Entity.AddComponent<HangarCameraAutoRotateComponent>();
        }

        [OnEventFire]
        public void StartSchedule(NodeAddedEvent e, HangarCameraTankViewStateRotationEnabledNode hangar)
        {
            ScheduledEvent scheduledEvent = base.NewEvent<HangarCameraStartAutoRotateEvent>().Attach(hangar).ScheduleDelayed(hangar.hangarConfig.AutoRotateDelay);
            hangar.Entity.AddComponent(new HangarCameraRotateScheduledComponent(scheduledEvent));
        }

        [OnEventFire]
        public void StopRotate(HangarCameraDelayAutoRotateEvent e, Node any, [JoinAll] HangarCameraAutoRotateNode hangar)
        {
            hangar.Entity.RemoveComponent<HangarCameraAutoRotateComponent>();
        }

        public class HangarCameraAutoRotateNode : HangarCameraAutoRotateSystem.HangarCameraRotateScheduledTankViewStateNode
        {
            public HangarCameraAutoRotateComponent hangarCameraAutoRotate;
        }

        public class HangarCameraRotateScheduledTankViewStateNode : HangarCameraAutoRotateSystem.HangarCameraTankViewStateRotationEnabledNode
        {
            public HangarCameraRotateScheduledComponent hangarCameraRotateScheduled;
        }

        public class HangarCameraTankViewStateNode : Node
        {
            public HangarCameraComponent hangarCamera;
            public CameraComponent camera;
            public HangarConfigComponent hangarConfig;
            public HangarTankPositionComponent hangarTankPosition;
            public HangarCameraTankViewComponent hangarCameraTankView;
        }

        public class HangarCameraTankViewStateRotationEnabledNode : HangarCameraAutoRotateSystem.HangarCameraTankViewStateNode
        {
            public HangarCameraRotationEnabledComponent hangarCameraRotationEnabled;
        }
    }
}

