namespace Tanks.Tool.TankViewer.API
{
    using System;
    using UnityEngine;

    public class SolidBackMode : CameraMode
    {
        private Camera solidBackgroundCamera;
        private Color backColor;

        public SolidBackMode(Camera solidBackgroundCamera, Color backColor)
        {
            this.solidBackgroundCamera = solidBackgroundCamera;
            this.backColor = backColor;
        }

        public void SwitchOn()
        {
            this.solidBackgroundCamera.backgroundColor = this.backColor;
            Debug.Log(this.solidBackgroundCamera.backgroundColor);
            this.solidBackgroundCamera.gameObject.SetActive(true);
        }

        public void SwithOff()
        {
            this.solidBackgroundCamera.gameObject.SetActive(false);
        }
    }
}

