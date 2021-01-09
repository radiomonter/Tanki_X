namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;

    public class BuyContainerButton : MonoBehaviour
    {
        [SerializeField]
        private LocalizedField buyText;
        [SerializeField]
        private TextMeshProUGUI text;
        [SerializeField]
        private GaragePrice price;

        public void Init(int amount, int oldPrice, int price)
        {
            this.Amount = amount;
            this.Price = price;
            this.price.SetPrice(oldPrice, price);
            this.text.text = this.buyText.Value + " " + amount;
        }

        public int Amount { get; private set; }

        public int Price { get; private set; }
    }
}

