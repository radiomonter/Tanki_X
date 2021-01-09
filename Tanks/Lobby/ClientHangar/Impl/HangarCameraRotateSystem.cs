namespace Tanks.Lobby.ClientHangar.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientGraphics.Impl;
    using UnityEngine;

    public class HangarCameraRotateSystem : ECSSystem
    {
        [OnEventFire]
        public void RotateCamera(HangarCameraRotateEvent e, HangarCameraTankViewStateNode hangar)
        {
            hangar.cameraRootTransform.Root.LookAt(hangar.hangarTankPosition.transform.position);
            hangar.cameraRootTransform.Root.RotateAround(hangar.hangarTankPosition.transform.position, Vector3.up, e.Angle);
        }

        public class HangarCameraTankViewStateNode : Node
        {
            public HangarCameraComponent hangarCamera;
            public CameraComponent camera;
            public CameraRootTransformComponent cameraRootTransform;
            public HangarTankPositionComponent hangarTankPosition;
            public HangarCameraTankViewComponent hangarCameraTankView;
            public HangarCameraRotationEnabledComponent hangarCameraRotationEnabled;
        }
    }
}

