namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using UnityEngine;

    public class ListItemPrices : MonoBehaviour
    {
        [SerializeField]
        private GaragePrice price;
        [SerializeField]
        private GaragePrice xPrice;

        public void Set(GarageItem item)
        {
            if (item.UserItem != null)
            {
                base.gameObject.SetActive(false);
            }
            else
            {
                int price = item.Price;
                int xPrice = item.XPrice;
                this.price.transform.parent.gameObject.SetActive(price > 0);
                if (price > 0)
                {
                    this.price.SetPrice(item.OldPrice, price);
                }
                this.xPrice.transform.parent.gameObject.SetActive(xPrice > 0);
                if (xPrice > 0)
                {
                    this.xPrice.SetPrice(item.OldXPrice, xPrice);
                }
                base.gameObject.SetActive((xPrice != 0) || (price != 0));
            }
        }
    }
}

