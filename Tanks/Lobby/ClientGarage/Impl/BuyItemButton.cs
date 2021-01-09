namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class BuyItemButton : MonoBehaviour
    {
        [SerializeField]
        private GaragePrice enabledPrice;
        [SerializeField]
        private GaragePrice disabledPrice;
        [SerializeField]
        private UnityEngine.UI.Button button;

        public void SetPrice(int oldPrice, int price)
        {
            this.enabledPrice.NeedUpdateColor = true;
            this.disabledPrice.NeedUpdateColor = false;
            this.enabledPrice.SetPrice(oldPrice, price);
            this.disabledPrice.SetPrice(oldPrice, price);
        }

        public UnityEngine.UI.Button Button =>
            this.button;
    }
}

