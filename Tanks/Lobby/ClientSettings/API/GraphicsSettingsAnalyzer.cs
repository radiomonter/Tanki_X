namespace Tanks.Lobby.ClientSettings.API
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class GraphicsSettingsAnalyzer : MonoBehaviour
    {
        protected GraphicsSettingsAnalyzer()
        {
        }

        public abstract Quality GetDefaultQuality();
        public abstract Resolution GetDefaultResolution(List<Resolution> resolutions);
        public abstract void Init();
    }
}

