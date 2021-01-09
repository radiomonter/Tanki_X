namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using System;
    using Tanks.Lobby.ClientNavigation.API;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    public class QiwiWalletScreenComponent : LocalizedScreenComponent, PaymentScreen
    {
        [SerializeField]
        private InputField account;
        private string errorText;
        [SerializeField]
        private Tanks.Lobby.ClientPaymentGUI.Impl.Receipt receipt;
        [SerializeField]
        private Text accountText;
        [SerializeField]
        private Text continueButton;
        [SerializeField]
        private Button button;
        [SerializeField]
        private QiwiAccountFormatterComponent formatter;

        protected override void Awake()
        {
            base.Awake();
            this.account.onValueChanged.AddListener(new UnityAction<string>(this.Check));
            this.Check(this.account.text);
        }

        private void Check(string text)
        {
            this.button.interactable = this.formatter.IsValidPhoneNumber;
        }

        public void DisableContinueButton()
        {
            this.button.interactable = false;
        }

        public string Account =>
            "+" + this.account.text.Replace(" ", string.Empty);

        public string ErrorText
        {
            get => 
                this.errorText;
            set => 
                this.errorText = value;
        }

        public Tanks.Lobby.ClientPaymentGUI.Impl.Receipt Receipt =>
            this.receipt;

        public string AccountText
        {
            set => 
                this.accountText.text = value;
        }

        public string ContinueButton
        {
            set => 
                this.continueButton.text = value;
        }
    }
}

