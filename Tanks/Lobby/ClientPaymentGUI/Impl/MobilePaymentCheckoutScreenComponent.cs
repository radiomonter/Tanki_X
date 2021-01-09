namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientNavigation.API;
    using UnityEngine;
    using UnityEngine.UI;

    public class MobilePaymentCheckoutScreenComponent : LocalizedScreenComponent, PaymentScreen
    {
        [SerializeField]
        private Text paymentMethodLabel;
        [SerializeField]
        private Text paymentMethodValue;
        [SerializeField]
        private Text successLabel;
        [SerializeField]
        private Text transactionNumberLabel;
        [SerializeField]
        private Text transactionNumberValue;
        [SerializeField]
        private Text priceLabel;
        [SerializeField]
        private Text priceValue;
        [SerializeField]
        private Text crystalsAmountLabel;
        [SerializeField]
        private GameObject receiptObject;
        [SerializeField]
        private Text crystalsAmountValue;
        [SerializeField]
        private Text specialOfferText;
        [SerializeField]
        private Text phoneNumberLabel;
        [SerializeField]
        private Text phoneNumberValue;
        [SerializeField]
        private Text aboutLabel;
        [SerializeField]
        private Text rightPanelHint;

        private void OnDisable()
        {
            this.receiptObject.SetActive(false);
            this.specialOfferText.gameObject.SetActive(false);
        }

        public void SetCrystalsAmount(long amount)
        {
            this.receiptObject.SetActive(true);
            this.crystalsAmountValue.text = amount.ToStringSeparatedByThousands();
        }

        public void SetPhoneNumber(string phoneNumber)
        {
            this.phoneNumberValue.text = phoneNumber;
        }

        public void SetPrice(double price, string currency)
        {
            this.priceValue.text = price.ToStringSeparatedByThousands() + " " + currency;
        }

        public void SetSpecialOfferText(string text)
        {
            this.specialOfferText.gameObject.SetActive(true);
            this.specialOfferText.text = text;
        }

        public void SetTransactionNumber(string transactionNumber)
        {
            this.transactionNumberValue.text = transactionNumber;
        }

        public virtual string PaymentMethodLabel
        {
            set => 
                this.paymentMethodLabel.text = value;
        }

        public virtual string PaymentMethodValue
        {
            set => 
                this.paymentMethodValue.text = value;
        }

        public virtual string SuccessLabel
        {
            set => 
                this.successLabel.text = value;
        }

        public virtual string TransactionNumberLabel
        {
            set => 
                this.transactionNumberLabel.text = value;
        }

        public virtual string PriceLabel
        {
            set => 
                this.priceLabel.text = value;
        }

        public virtual string CrystalsAmountLabel
        {
            set => 
                this.crystalsAmountLabel.text = value;
        }

        public virtual string PhoneNumberLabel
        {
            set => 
                this.phoneNumberLabel.text = value;
        }

        public virtual string AboutLabel
        {
            set => 
                this.aboutLabel.text = value;
        }

        public virtual string RightPanelHint
        {
            set => 
                this.rightPanelHint.text = value;
        }
    }
}

