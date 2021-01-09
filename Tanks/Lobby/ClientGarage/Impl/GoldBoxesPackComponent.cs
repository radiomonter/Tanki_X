namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientPaymentGUI.Impl;
    using TMPro;
    using UnityEngine;

    public class GoldBoxesPackComponent : BehaviourComponent
    {
        [SerializeField]
        private TextMeshProUGUI _cardNameText;
        [SerializeField]
        private TextMeshProUGUI _boxCountText;
        [SerializeField]
        private ImageSkin _imageSkin;
        [SerializeField]
        private TextMeshProUGUI _priceText;
        [SerializeField]
        private GameObject _hitMarkObject;
        [SerializeField]
        private GameObject _discountMarkObject;
        [SerializeField]
        private TextMeshProUGUI _discountMarkText;
        [SerializeField]
        private PurchaseButtonComponent _purchaseButton;

        public string CardName
        {
            set => 
                this._cardNameText.text = value;
        }

        public long BoxCount
        {
            set => 
                this._boxCountText.text = "x" + value;
        }

        public string SpriteUid
        {
            set => 
                this._imageSkin.SpriteUid = value;
        }

        public string Price
        {
            set => 
                this._priceText.text = value;
        }

        public bool HitMarkEnabled
        {
            set => 
                this._hitMarkObject.SetActive(value);
        }

        public int Discount
        {
            set
            {
                if (value <= 0)
                {
                    this._discountMarkObject.SetActive(false);
                }
                else
                {
                    this._discountMarkObject.SetActive(true);
                    this._discountMarkText.text = "-" + value + "%";
                }
            }
        }

        public Entity GoodsEntity
        {
            set => 
                this._purchaseButton.GoodsEntity = value;
        }
    }
}

