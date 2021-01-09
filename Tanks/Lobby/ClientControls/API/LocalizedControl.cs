namespace Tanks.Lobby.ClientControls.API
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;

    public abstract class LocalizedControl : FromConfigBehaviour
    {
        [SerializeField]
        private string path = "/ui/element";

        protected LocalizedControl()
        {
        }

        protected override string GetRelativeConfigPath() => 
            this.path;
    }
}

