namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    public class MobilePaymentScreenComponent : BasePaymentScreenComponent, PaymentScreen
    {
        [SerializeField]
        private Text mobilePhoneLabel;
        [SerializeField]
        private Text phoneCountryCode;
        [SerializeField]
        private InputField phoneNumber;

        protected override void Awake()
        {
            base.Awake();
            this.phoneNumber.onValueChanged.AddListener(new UnityAction<string>(this.ValidateInput));
        }

        private void OnEnable()
        {
            this.phoneNumber.text = string.Empty;
            this.ValidateInput(string.Empty);
        }

        private void ValidateInput(string input = "")
        {
            base.continueButton.interactable = this.phoneNumber.text.Length == this.phoneNumber.characterLimit;
        }

        public virtual string MobilePhoneLabel
        {
            set => 
                this.mobilePhoneLabel.text = value;
        }

        public virtual string PhoneCountryCode
        {
            get => 
                this.phoneCountryCode.text;
            set => 
                this.phoneCountryCode.text = value;
        }

        public virtual string PhoneNumber
        {
            get => 
                this.phoneNumber.text;
            set => 
                this.phoneNumber.text = value;
        }
    }
}

