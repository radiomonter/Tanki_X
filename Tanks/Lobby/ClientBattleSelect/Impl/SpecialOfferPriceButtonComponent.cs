namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using TMPro;
    using UnityEngine;

    public class SpecialOfferPriceButtonComponent : BehaviourComponent
    {
        [SerializeField]
        private string priceRegularFormatting;
        [SerializeField]
        private string priceDiscountedFormatting;
        [SerializeField]
        private TextMeshProUGUI priceText;

        public void SetPrice(int price, string currency)
        {
            this.SetPrice((double) price, 0, currency);
        }

        public void SetPrice(double price, int discount, string currency)
        {
            this.priceText.text = (discount == 0) ? string.Format(this.priceRegularFormatting, price, currency) : string.Format(this.priceDiscountedFormatting, discount, price, currency);
        }
    }
}

