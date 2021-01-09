namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;
    using UnityEngine.UI;

    public class XCrystalsMethodSaleLabelComponent : LocalizedControl, Component
    {
        [SerializeField]
        private UnityEngine.UI.Text timerText;

        public string Text
        {
            set => 
                this.timerText.text = value;
        }
    }
}

