namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using System;
    using Tanks.Lobby.ClientNavigation.API;
    using UnityEngine;
    using UnityEngine.UI;

    public class BasePaymentScreenComponent : LocalizedScreenComponent, PaymentScreen
    {
        [SerializeField]
        private Tanks.Lobby.ClientPaymentGUI.Impl.Receipt receipt;
        [SerializeField]
        private Text payButtonLabel;
        [SerializeField]
        protected Button continueButton;

        public Tanks.Lobby.ClientPaymentGUI.Impl.Receipt Receipt =>
            this.receipt;

        public virtual string PayButtonLabel
        {
            set => 
                this.payButtonLabel.text = value;
        }
    }
}

