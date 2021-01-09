namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientGarage.Impl;

    public class TutorialTankControlsHideTriggerComponent : TutorialHideTriggerComponent
    {
        private static readonly string RIGHT_AXIS = "MoveRight";
        private static readonly string LEFT_AXIS = "MoveLeft";
        private static readonly string FORWARD_AXIS = "MoveForward";
        private static readonly string BACKWARD_AXIS = "MoveBackward";

        protected void Update()
        {
            if (!base.triggered && ((InputManager.GetUnityAxis(RIGHT_AXIS) != 0f) || ((InputManager.GetUnityAxis(LEFT_AXIS) != 0f) || ((InputManager.GetUnityAxis(FORWARD_AXIS) != 0f) || (InputManager.GetUnityAxis(BACKWARD_AXIS) != 0f)))))
            {
                this.Triggered();
            }
        }

        [Inject]
        public static Tanks.Battle.ClientCore.Impl.InputManager InputManager { get; set; }
    }
}

