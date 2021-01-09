namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using System;
    using Tanks.Lobby.ClientNavigation.API;
    using TMPro;
    using UnityEngine;

    public class EnterUserEmailScreenComponent : LocalizedScreenComponent, NoScaleScreen
    {
        [SerializeField]
        private TextMeshProUGUI rightPanelHint;
        [SerializeField]
        private TextMeshProUGUI continueButton;

        public virtual string RightPanelHint
        {
            set => 
                this.rightPanelHint.text = value;
        }

        public virtual string ContinueButton
        {
            set => 
                this.continueButton.text = value.ToUpper();
        }
    }
}

