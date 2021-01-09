namespace Tanks.Tool.TankViewer.API
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class CameraModeController : MonoBehaviour
    {
        public Camera mainCamera;
        public Camera solidBackCamera;
        public List<Color> backColors;
        public Camera maskCamera;
        public GameObject plane;
        private List<CameraMode> cameraModes;
        private int currentModeIndex;

        private void Awake()
        {
            this.cameraModes = new List<CameraMode>();
            DefaultMode item = new DefaultMode(this.mainCamera);
            this.cameraModes.Add(item);
            for (int i = 0; i < this.backColors.Count; i++)
            {
                this.cameraModes.Add(new SolidBackMode(this.solidBackCamera, this.backColors[i]));
            }
            this.cameraModes.Add(new MaskMode(this.maskCamera, this.plane));
            item.SwitchOn();
        }

        public void ChangeMode()
        {
            this.cameraModes[this.currentModeIndex].SwithOff();
            this.currentModeIndex = (this.currentModeIndex >= (this.cameraModes.Count - 1)) ? 0 : (this.currentModeIndex + 1);
            this.cameraModes[this.currentModeIndex].SwitchOn();
        }
    }
}

