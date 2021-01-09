namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.Impl;
    using UnityEngine;

    public class MouseOrbitCameraSystem : ECSSystem
    {
        private const int DINSTANCE_RATIO_BASE = 3;

        [OnEventComplete]
        public unsafe void UpdateMouseOrbitCamera(TimeUpdateEvent evt, OrbitCameraNode cameraNode, [JoinAll] CameraTargetNode targetNode)
        {
            float deltaTime = evt.DeltaTime;
            MouseOrbitCameraComponent mouseOrbitCamera = cameraNode.mouseOrbitCamera;
            Vector3 eulerAngles = mouseOrbitCamera.targetRotation.eulerAngles;
            float num2 = 1f;
            if (InputManager.CheckAction(SpectatorCameraActions.SlowMovement))
            {
                num2 = MouseOrbitCameraConstants.ROTATION_SLOW_RATIO;
            }
            float num3 = MouseOrbitCameraConstants.X_ROTATION_SPEED * deltaTime;
            float num4 = MouseOrbitCameraConstants.Y_ROTATION_SPEED * deltaTime;
            Vector3* vectorPtr1 = &eulerAngles;
            vectorPtr1->x -= (InputManager.GetUnityAxis(UnityInputConstants.MOUSE_Y) * num3) * num2;
            Vector3* vectorPtr2 = &eulerAngles;
            vectorPtr2->y += (InputManager.GetUnityAxis(UnityInputConstants.MOUSE_X) * num4) * num2;
            mouseOrbitCamera.targetRotation = Quaternion.Euler(MouseOrbitCameraUtils.NormalizeEuler(eulerAngles));
            mouseOrbitCamera.distance *= Mathf.Pow(3f, -InputManager.GetUnityAxis(UnityInputConstants.MOUSE_SCROLL_WHEEL));
            mouseOrbitCamera.distance = Mathf.Clamp(mouseOrbitCamera.distance, MouseOrbitCameraConstants.MIN_DISTANCE, MouseOrbitCameraConstants.MAX_DISTANCE);
            Vector3 position = targetNode.cameraTarget.TargetObject.transform.position;
            TransformData data = new TransformData {
                Position = MouseOrbitCameraUtils.GetClippedPosition(position, position + (mouseOrbitCamera.targetRotation * new Vector3(0f, 0f, -mouseOrbitCamera.distance)), MouseOrbitCameraConstants.MAX_DISTANCE),
                Rotation = mouseOrbitCamera.targetRotation
            };
            cameraNode.cameraTransformData.Data = data;
            ApplyCameraTransformEvent eventInstance = ApplyCameraTransformEvent.ResetApplyCameraTransformEvent();
            eventInstance.PositionSmoothingRatio = MouseOrbitCameraConstants.POSITION_SMOOTHING_SPEED;
            eventInstance.RotationSmoothingRatio = MouseOrbitCameraConstants.ROTATION_SMOOTHING_SPEED;
            eventInstance.DeltaTime = deltaTime;
            base.ScheduleEvent(eventInstance, cameraNode);
        }

        [Inject]
        public static Tanks.Battle.ClientCore.Impl.InputManager InputManager { get; set; }

        public class CameraTargetNode : Node
        {
            public CameraTargetComponent cameraTarget;
        }

        public class OrbitCameraNode : Node
        {
            public MouseOrbitCameraComponent mouseOrbitCamera;
            public BattleCameraComponent battleCamera;
            public CameraTransformDataComponent cameraTransformData;
        }
    }
}

