namespace Tanks.Tool.TankViewer.API
{
    using System;
    using UnityEngine;

    public class MaskMode : CameraMode
    {
        private Camera camera;
        private GameObject plane;

        public MaskMode(Camera camera, GameObject plane)
        {
            this.camera = camera;
            this.plane = plane;
        }

        public void SwitchOn()
        {
            this.camera.gameObject.SetActive(true);
            this.plane.SetActive(false);
        }

        public void SwithOff()
        {
            this.camera.gameObject.SetActive(false);
            this.plane.SetActive(true);
        }
    }
}

