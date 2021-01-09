namespace Tanks.Tool.TankViewer.API
{
    using System;
    using UnityEngine;

    public class DefaultMode : CameraMode
    {
        private Camera camera;

        public DefaultMode(Camera camera)
        {
            this.camera = camera;
        }

        public void SwitchOn()
        {
            this.camera.gameObject.SetActive(true);
        }

        public void SwithOff()
        {
            this.camera.gameObject.SetActive(false);
        }
    }
}

