namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.Impl;
    using System;
    using UnityEngine;

    public class CameraFOVUpdateSystem : ECSSystem
    {
        private const float NARROW_SCREEN = 12f;
        private const float WIDE_SCREEN = 16f;
        private const float DIVIDER = 9f;
        private const float DEFAULT_FOV_RAD = 1.570796f;

        [OnEventComplete]
        public void ApplyCalculatedFOVToCamera(ViewportResizeEvent evt, CameraFOVUpdateNode cameraNode)
        {
            cameraNode.camera.FOV = cameraNode.battleCamera.OptimalFOV;
        }

        private float CalculateCameraFovInRad()
        {
            float num = ((float) Screen.height) / 9f;
            float num2 = ((float) Screen.width) / num;
            if (num2 <= 12f)
            {
                return 1.570796f;
            }
            float num3 = num2 - 4f;
            if (num3 < 12f)
            {
                num3 = 12f;
            }
            float num5 = (Mathf.Sqrt(Mathf.Pow(num3 * num, 2f) + Mathf.Pow((float) Screen.height, 2f)) * 0.5f) / Mathf.Tan(0.7853982f);
            return (Mathf.Atan((Mathf.Sqrt(Mathf.Pow((float) Screen.width, 2f) + Mathf.Pow((float) Screen.height, 2f)) * 0.5f) / num5) * 2f);
        }

        [OnEventFire]
        public void UpdateOptimalCameraFov(NodeAddedEvent evt, CameraNode cameraNode)
        {
            this.UpdateOptimalFOV(cameraNode);
            cameraNode.camera.FOV = cameraNode.battleCamera.OptimalFOV;
        }

        [OnEventFire]
        public void UpdateOptimalCameraFov(ViewportResizeEvent evt, CameraNode cameraNode)
        {
            this.UpdateOptimalFOV(cameraNode);
        }

        private void UpdateOptimalFOV(CameraNode cameraNode)
        {
            float num = (this.CalculateCameraFovInRad() * 57.29578f) * 0.5f;
            cameraNode.battleCamera.OptimalFOV = num;
        }

        public class CameraFOVUpdateNode : Node
        {
            public CameraFOVUpdateComponent cameraFOVUpdate;
            public BattleCameraComponent battleCamera;
            public CameraComponent camera;
        }

        public class CameraNode : Node
        {
            public BattleCameraComponent battleCamera;
            public CameraComponent camera;
        }
    }
}

