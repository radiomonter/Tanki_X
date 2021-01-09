namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientNavigation.API;
    using UnityEngine;
    using UnityEngine.UI;

    public class ConfirmUserEmailScreenComponent : LocalizedScreenComponent
    {
        [SerializeField]
        private Text confirmationHintWithUserEmail;
        [SerializeField]
        private Text sendNewsText;
        [SerializeField]
        private Text confirmText;
        [SerializeField]
        private Text sendAgainText;
        [SerializeField]
        private Text rightPanelHint;
        [SerializeField]
        private Text confirmationCodeText;
        [SerializeField]
        private Color emailColor = Color.green;
        [SerializeField]
        private GameObject cancelButton;
        [SerializeField]
        private GameObject changeEmailButton;

        public void ActivateCancel()
        {
            this.cancelButton.SetActive(true);
            this.changeEmailButton.SetActive(false);
            this.rightPanelHint.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            this.cancelButton.SetActive(false);
            this.changeEmailButton.SetActive(true);
            this.rightPanelHint.gameObject.SetActive(true);
        }

        public string ConfirmationHintWithUserEmail
        {
            set => 
                this.confirmationHintWithUserEmail.text = value;
        }

        public string ConfirmationHint { get; set; }

        public string SendAgainText
        {
            set => 
                this.sendAgainText.text = value.ToUpper();
        }

        public string RightPanelHint
        {
            set => 
                this.rightPanelHint.text = value;
        }

        public string InvalidCodeMessage { get; set; }

        public Color EmailColor =>
            this.emailColor;
    }
}

