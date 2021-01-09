namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using System;
    using Tanks.Lobby.ClientNavigation.API;
    using UnityEngine;
    using UnityEngine.UI;

    public class PaymentResultScreenComponent : LocalizedScreenComponent, PaymentScreen
    {
        [SerializeField]
        private Text message;

        public virtual string Message
        {
            set => 
                this.message.text = value;
        }
    }
}

