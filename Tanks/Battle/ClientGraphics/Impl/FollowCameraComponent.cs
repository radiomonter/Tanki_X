namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class FollowCameraComponent : Component
    {
        public float verticalCameraSpeed = 0.5f;
        public float rollReturnSpeedDegPerSec = 60f;
        public CameraData cameraData;
    }
}

