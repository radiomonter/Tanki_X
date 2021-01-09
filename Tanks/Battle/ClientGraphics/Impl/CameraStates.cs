namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public static class CameraStates
    {
        public class CameraFollowState : Node
        {
            public FollowCameraComponent followCamera;
        }

        public class CameraFreeState : Node
        {
            public FreeCameraComponent freeCamera;
        }

        public class CameraGoState : Node
        {
            public GoCameraComponent goCamera;
        }

        public class CameraOrbitState : Node
        {
            public MouseOrbitCameraComponent mouseOrbitCamera;
        }

        public class CameraTransitionState : Node
        {
            public TransitionCameraStateComponent transitionCameraState;
        }
    }
}

