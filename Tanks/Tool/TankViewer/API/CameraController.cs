namespace Tanks.Tool.TankViewer.API
{
    using System;
    using Tanks.Battle.ClientGraphics.API;
    using UnityEngine;

    public class CameraController : MonoBehaviour
    {
        public CameraModeController cameraModeController;
        public TargetCameraController targetCameraController;
        public FreeCameraController freeCameraController;

        public void Awake()
        {
            this.targetCameraController.enabled = true;
            this.freeCameraController.enabled = false;
        }

        public void ChangeController()
        {
            if (this.targetCameraController.enabled)
            {
                this.targetCameraController.enabled = false;
                this.freeCameraController.enabled = true;
            }
            else
            {
                this.targetCameraController.enabled = true;
                this.freeCameraController.enabled = false;
            }
        }

        public void ChangeMode()
        {
            this.cameraModeController.ChangeMode();
        }
    }
}

