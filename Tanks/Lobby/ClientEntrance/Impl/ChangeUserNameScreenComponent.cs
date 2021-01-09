namespace Tanks.Lobby.ClientEntrance.Impl
{
    using System;
    using Tanks.Lobby.ClientNavigation.API;
    using UnityEngine;
    using UnityEngine.UI;

    public class ChangeUserNameScreenComponent : LocalizedScreenComponent
    {
        [SerializeField]
        private Text inputHint;
        [SerializeField]
        private Text continueButton;
        [SerializeField]
        private Text reservedNameHint;

        public string InputHint
        {
            set => 
                this.inputHint.text = value;
        }

        public string ContinueButton
        {
            set => 
                this.continueButton.text = value;
        }

        public string ReservedNameHint
        {
            set => 
                this.reservedNameHint.text = value;
        }
    }
}

