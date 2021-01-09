namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientPaymentGUI.Impl;
    using TMPro;
    using UnityEngine;

    public class PremiumPackComponent : BehaviourComponent
    {
        [SerializeField]
        private TextMeshProUGUI _daysText;
        [SerializeField]
        private TextMeshProUGUI _daysDescriptionText;
        [SerializeField]
        private TextMeshProUGUI _priceText;
        [SerializeField]
        private GameObject _xCrystals;
        [SerializeField]
        private GameObject _saleContainer;
        [SerializeField]
        private TextMeshProUGUI _salePercentText;
        [SerializeField]
        private PremiumLearnMoreButtonComponent _learnMoreButton;
        [SerializeField]
        private PurchaseButtonComponent _premiumPurchaseButton;

        public string DaysText
        {
            set => 
                this._daysText.text = value;
        }

        public string DaysDescription
        {
            set => 
                this._daysDescriptionText.text = value;
        }

        public string Price
        {
            set => 
                this._priceText.text = value;
        }

        public bool HasXCrystals
        {
            set => 
                this._xCrystals.SetActive(value);
        }

        public float Discount
        {
            set
            {
                if (value <= 0f)
                {
                    this._saleContainer.SetActive(false);
                }
                else
                {
                    this._saleContainer.SetActive(true);
                    this._salePercentText.text = $"-{value * 100f:0}%";
                }
            }
        }

        public int LearnMoreIndex
        {
            set => 
                this._learnMoreButton.idx = value;
        }

        public Entity GoodsEntity
        {
            set => 
                this._premiumPurchaseButton.GoodsEntity = value;
        }
    }
}

