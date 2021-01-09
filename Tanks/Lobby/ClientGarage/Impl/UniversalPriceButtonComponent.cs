namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;

    public class UniversalPriceButtonComponent : BehaviourComponent
    {
        [SerializeField]
        private GameObject price;
        [SerializeField]
        private GameObject xPrice;

        public bool PriceActivity
        {
            get => 
                this.price.activeSelf;
            set => 
                this.price.SetActive(value);
        }

        public bool XPriceActivity
        {
            get => 
                this.xPrice.activeSelf;
            set => 
                this.xPrice.SetActive(value);
        }
    }
}

