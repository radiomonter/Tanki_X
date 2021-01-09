namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using Lobby.ClientPayment.Impl;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.Impl;
    using Tanks.Lobby.ClientPayment.Impl;
    using TMPro;
    using UnityEngine;

    public class PlatboxCheckoutWindow : MonoBehaviour
    {
        private Action onForward;
        [SerializeField]
        private TextMeshProUGUI transactionNumberValue;
        [SerializeField]
        private TextMeshProUGUI priceValue;
        [SerializeField]
        private GameObject receiptObject;
        [SerializeField]
        private TextMeshProUGUI crystalsAmountValue;
        [SerializeField]
        private TextMeshProUGUI specialOfferText;
        [SerializeField]
        private TextMeshProUGUI phoneNumberValue;

        private void OnDisable()
        {
            this.receiptObject.SetActive(false);
            this.specialOfferText.gameObject.SetActive(false);
        }

        public void Proceed()
        {
            MainScreenComponent.Instance.ClearOnBackOverride();
            base.GetComponent<Animator>().SetTrigger("cancel");
            this.onForward();
        }

        private void SetCrystalsAmount(long amount)
        {
            this.receiptObject.SetActive(true);
            this.crystalsAmountValue.text = amount.ToStringSeparatedByThousands() + "<sprite=9>";
        }

        private void SetPhoneNumber(string phoneNumber)
        {
            this.phoneNumberValue.text = phoneNumber;
        }

        private void SetPrice(double price, string currency)
        {
            this.priceValue.text = price.ToStringSeparatedByThousands() + " " + currency;
        }

        private void SetSpecialOfferText(string text)
        {
            this.specialOfferText.gameObject.SetActive(true);
            this.specialOfferText.text = text;
        }

        private void SetTransactionNumber(string transactionNumber)
        {
            this.transactionNumberValue.text = transactionNumber;
        }

        public void Show(Entity item, Entity method, string transaction, string phoneNumber, Action onForward)
        {
            this.SetPhoneNumber(phoneNumber);
            this.SetTransactionNumber(transaction);
            GoodsPriceComponent component = item.GetComponent<GoodsPriceComponent>();
            GoodsComponent component2 = item.GetComponent<GoodsComponent>();
            bool flag = item.HasComponent<SpecialOfferComponent>();
            string methodName = method.GetComponent<PaymentMethodComponent>().MethodName;
            double price = component.Price;
            price = !flag ? component.Round(component2.SaleState.PriceMultiplier * price) : item.GetComponent<SpecialOfferComponent>().GetSalePrice(price);
            if (item.HasComponent<XCrystalsPackComponent>())
            {
                XCrystalsPackComponent component3 = item.GetComponent<XCrystalsPackComponent>();
                long amount = component3.Amount;
                if (!flag)
                {
                    amount = (long) Math.Round((double) (component2.SaleState.AmountMultiplier * amount));
                }
                this.SetCrystalsAmount(amount + component3.Bonus);
            }
            this.SetPrice(price, component.Currency);
            MainScreenComponent.Instance.OverrideOnBack(new Action(this.Proceed));
            this.onForward = onForward;
            base.gameObject.SetActive(true);
        }
    }
}

