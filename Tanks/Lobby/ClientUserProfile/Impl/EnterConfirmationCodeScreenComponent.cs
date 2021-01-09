namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientNavigation.API;
    using TMPro;
    using UnityEngine;

    public class EnterConfirmationCodeScreenComponent : LocalizedScreenComponent, NoScaleScreen
    {
        [SerializeField]
        private TextMeshProUGUI confirmationHintWithUserEmail;
        [SerializeField]
        private TextMeshProUGUI confirmationCodeText;
        [SerializeField]
        private ConfirmationCodeSendAgainComponent confirmationCodeSendAgainComponent;
        [SerializeField]
        private Color emailColor = Color.green;

        public void ResetSendAgainTimer(long emailSendThresholdInSeconds)
        {
            if (this.confirmationCodeSendAgainComponent != null)
            {
                this.confirmationCodeSendAgainComponent.ShowTimer(emailSendThresholdInSeconds);
            }
        }

        public string ConfirmationHintWithUserEmail
        {
            set => 
                this.confirmationHintWithUserEmail.text = value;
        }

        public string ConfirmationHint { get; set; }

        public string ConfirmationCodeText
        {
            get => 
                this.confirmationCodeText.text;
            set => 
                this.confirmationCodeText.text = value;
        }

        public string InvalidCodeMessage { get; set; }

        public Color EmailColor =>
            this.emailColor;
    }
}

