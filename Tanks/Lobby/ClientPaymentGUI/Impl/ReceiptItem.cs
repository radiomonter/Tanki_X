namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using System;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;
    using UnityEngine.UI;

    public class ReceiptItem : MonoBehaviour
    {
        [SerializeField]
        private Text name;
        [SerializeField]
        private Text amount;

        public void Init(string name, long amount)
        {
            this.name.text = name;
            this.amount.text = amount.ToStringSeparatedByThousands();
        }
    }
}

