namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class SpectatorCameraComponent : Component
    {
        public Dictionary<int, CameraSaveData> savedCameras = new Dictionary<int, CameraSaveData>();
        private bool cursorVisible = true;

        public bool SaveCameraModificatorKeyHasBeenPressed { get; set; }

        public int SequenceScreenshot { get; set; }

        public bool СursorVisible
        {
            get => 
                this.cursorVisible;
            set => 
                this.cursorVisible = value;
        }
    }
}

