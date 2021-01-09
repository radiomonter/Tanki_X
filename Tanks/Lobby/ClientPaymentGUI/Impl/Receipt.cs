namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;
    using UnityEngine.UI;

    public class Receipt : LocalizedControl
    {
        [SerializeField]
        private Text price;
        [SerializeField]
        private Text total;
        private long totalValue;
        [SerializeField]
        private ReceiptItem receiptItemPrefab;
        [SerializeField]
        private RectTransform receiptItemsContainer;
        [SerializeField]
        private Text priceLabel;
        [SerializeField]
        private GameObject totalObject;
        [SerializeField]
        private Text specialOfferText;
        [SerializeField]
        private Text totalLabel;

        public void AddItem(string name, long amount)
        {
            this.totalObject.SetActive(true);
            ReceiptItem item = Instantiate<ReceiptItem>(this.receiptItemPrefab);
            item.Init(name, amount);
            item.transform.SetParent(this.receiptItemsContainer, false);
            this.totalValue += amount;
            this.total.text = this.totalValue.ToStringSeparatedByThousands();
        }

        public void AddSpecialOfferText(string text)
        {
            this.specialOfferText.gameObject.SetActive(true);
            this.specialOfferText.text = text;
        }

        private void OnDisable()
        {
            this.totalValue = 0L;
            IEnumerator enumerator = this.receiptItemsContainer.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Transform current = (Transform) enumerator.Current;
                    if (current != this.specialOfferText.transform)
                    {
                        Destroy(current.gameObject);
                    }
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
            this.totalObject.SetActive(false);
            this.specialOfferText.gameObject.SetActive(false);
            this.specialOfferText.text = string.Empty;
        }

        public void SetPrice(double price, string currency)
        {
            this.price.text = price.ToStringSeparatedByThousands() + " " + currency;
        }

        public Dictionary<object, object> Lines { get; set; }

        public virtual string PriceLabel
        {
            set => 
                this.priceLabel.text = value;
        }

        public virtual string TotalLabel
        {
            set => 
                this.totalLabel.text = value;
        }
    }
}

