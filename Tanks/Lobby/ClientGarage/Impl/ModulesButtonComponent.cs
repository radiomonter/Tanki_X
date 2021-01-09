namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;
    using UnityEngine.UI;

    public class ModulesButtonComponent : LocalizedControl, Component
    {
        [SerializeField]
        private UnityEngine.UI.Text text;

        public string Text
        {
            set => 
                this.text.text = value.ToUpper();
        }
    }
}

