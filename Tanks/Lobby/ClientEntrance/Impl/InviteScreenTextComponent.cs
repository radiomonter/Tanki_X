namespace Tanks.Lobby.ClientEntrance.Impl
{
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientNavigation.API;
    using TMPro;
    using UnityEngine;

    public class InviteScreenTextComponent : LocalizedScreenComponent
    {
        [SerializeField]
        private TextMeshProUGUI inputHint;
        [SerializeField]
        private TextMeshProUGUI continueButton;

        public string InputHint
        {
            set => 
                this.inputHint.text = value;
        }

        public string Continue
        {
            set => 
                this.continueButton.text = value;
        }

        public string Error { get; set; }
    }
}

