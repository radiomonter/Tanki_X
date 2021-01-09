namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientNavigation.API;
    using UnityEngine;
    using UnityEngine.UI;

    public class FirstPurchaseConfirmScreenComponent : LocalizedScreenComponent, PaymentScreen
    {
        private long compensation;
        [SerializeField]
        private Text info;
        [SerializeField]
        private Text confirmButton;
        [SerializeField]
        private PaletteColorField color;
        [SerializeField]
        private GameObject overlay;
        [SerializeField]
        private CanvasGroup content;

        private void OnDisable()
        {
            this.info.text = string.Empty;
            this.content.interactable = false;
            this.overlay.SetActive(true);
        }

        public string ConfirmationText { private get; set; }

        public long Compensation
        {
            get => 
                this.compensation;
            set
            {
                this.compensation = value;
                this.info.text = string.Format(this.ConfirmationText, $"<color=#{this.color.Color.ToHexString()}>{this.compensation}</color>");
                this.content.interactable = true;
                this.overlay.SetActive(false);
            }
        }

        public string ConfirmButton
        {
            set => 
                this.confirmButton.text = value;
        }
    }
}

