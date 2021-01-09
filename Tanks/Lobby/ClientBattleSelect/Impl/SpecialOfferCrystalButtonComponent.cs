namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using TMPro;
    using UnityEngine;

    public class SpecialOfferCrystalButtonComponent : BehaviourComponent
    {
        [SerializeField]
        private string priceRegularFormatting;
        [SerializeField]
        private string priceDiscountedFormatting;
        [SerializeField]
        private TextMeshProUGUI priceText;
        [SerializeField]
        private string blueCrystalIconString;
        [SerializeField]
        private string xCrystalIconString;

        private void AddCrystals(bool xCry)
        {
            this.priceText.text = !xCry ? (this.priceText.text + this.blueCrystalIconString) : (this.priceText.text + this.xCrystalIconString);
        }

        public void SetPrice(int price, bool xCry)
        {
            this.SetPrice(price, 0, xCry);
        }

        public void SetPrice(int price, int discount, bool xCry)
        {
            this.priceText.text = (discount == 0) ? string.Format(this.priceRegularFormatting, price) : string.Format(this.priceDiscountedFormatting, discount, price);
            this.AddCrystals(xCry);
        }
    }
}

