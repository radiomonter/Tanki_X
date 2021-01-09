namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using System;
    using Tanks.Lobby.ClientNavigation.API;
    using TMPro;
    using UnityEngine;

    public class ForceEnterEmailScreenComponent : LocalizedScreenComponent
    {
        [SerializeField]
        private TextMeshProUGUI confirmButton;
        [SerializeField]
        private TextMeshProUGUI rightPanelHint;
        [SerializeField]
        private TextMeshProUGUI placeholder;

        public string ConfirmButton
        {
            set => 
                this.confirmButton.text = value;
        }

        public string RightPanelHint
        {
            set => 
                this.rightPanelHint.text = value;
        }

        public string Placeholder
        {
            set => 
                this.placeholder.text = value;
        }
    }
}

