namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using System;
    using Tanks.Lobby.ClientNavigation.API;
    using UnityEngine;
    using UnityEngine.UI;

    public class PaymentProcessingScreenComponent : LocalizedScreenComponent, PaymentScreen
    {
        [SerializeField]
        private Text info;

        public virtual string Info
        {
            set => 
                this.info.text = value;
        }
    }
}

