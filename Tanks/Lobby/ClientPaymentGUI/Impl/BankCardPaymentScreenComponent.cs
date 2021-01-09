namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using Lobby.ClientPayment.Impl;
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    public class BankCardPaymentScreenComponent : BasePaymentScreenComponent
    {
        [SerializeField]
        private Text cardRequisitesLabel;
        [SerializeField]
        private Text cardNumberLabel;
        [SerializeField]
        private Text cardExpirationDateLabel;
        [SerializeField]
        private Text cardHolderLabel;
        [SerializeField]
        private Text cardCVVLabel;
        [SerializeField]
        private Text cardCVVHint;
        [SerializeField]
        private InputField number;
        [SerializeField]
        private InputField mm;
        [SerializeField]
        private InputField yy;
        [SerializeField]
        private InputField cardHolder;
        [SerializeField]
        private InputField cvc;

        protected override void Awake()
        {
            base.Awake();
            this.cvc.onValueChanged.AddListener(new UnityAction<string>(this.ValidateInput));
            this.cardHolder.onValueChanged.AddListener(new UnityAction<string>(this.ValidateInput));
            this.number.onValueChanged.AddListener(new UnityAction<string>(this.ValidateInput));
            this.mm.onValueChanged.AddListener(new UnityAction<string>(this.ValidateInput));
            this.yy.onValueChanged.AddListener(new UnityAction<string>(this.ValidateInput));
        }

        private void OnEnable()
        {
            this.cvc.text = string.Empty;
            this.cardHolder.text = string.Empty;
            this.number.text = string.Empty;
            this.mm.text = string.Empty;
            this.yy.text = string.Empty;
            this.ValidateInput(string.Empty);
        }

        private void ValidateInput(string input = "")
        {
            bool flag = (((this.cvc.text.Length == this.cvc.characterLimit) && BankCardUtils.IsBankCard(this.number.text)) && (this.yy.text.Length == this.yy.characterLimit)) && !string.IsNullOrEmpty(this.cardHolder.text);
            if (flag)
            {
                int num = int.Parse(this.mm.text);
                flag = (flag && (num >= 1)) && (num <= 12);
            }
            base.continueButton.interactable = flag;
        }

        public virtual string CardRequisitesLabel
        {
            set => 
                this.cardRequisitesLabel.text = value;
        }

        public virtual string CardNumberLabel
        {
            set => 
                this.cardNumberLabel.text = value;
        }

        public virtual string CardExpirationDateLabel
        {
            set => 
                this.cardExpirationDateLabel.text = value;
        }

        public virtual string CardHolderLabel
        {
            set => 
                this.cardHolderLabel.text = value;
        }

        public virtual string CardCVVLabel
        {
            set => 
                this.cardCVVLabel.text = value;
        }

        public virtual string CardCVVHint
        {
            set => 
                this.cardCVVHint.text = value;
        }

        public string Number =>
            this.number.text;

        public string MM =>
            this.mm.text;

        public string YY =>
            this.yy.text;

        public string CardHolder =>
            this.cardHolder.text;

        public string CVC =>
            this.cvc.text;
    }
}

