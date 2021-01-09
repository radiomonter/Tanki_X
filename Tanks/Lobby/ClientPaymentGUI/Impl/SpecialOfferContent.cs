namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using Lobby.ClientPayment.Impl;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using System.Text;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientGarage.Impl;
    using Tanks.Lobby.ClientPayment.Impl;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class SpecialOfferContent : DealItemContent
    {
        public TextMeshProUGUI oldPrice;
        public TextMeshProUGUI items;
        public LocalizedField specialOfferEmptyRewardMessage;
        [SerializeField]
        private Image saleImage;
        [SerializeField]
        private TextMeshProUGUI saleText;
        [SerializeField]
        private Image titleStripes;
        [SerializeField]
        private TextMeshProUGUI timer;
        private string cachedCurrency;
        private double cachedPrice;

        private StringBuilder buildComment(Entity entity, ItemsPackFromConfigComponent itemsPackFromConfig)
        {
            StringBuilder builder = new StringBuilder();
            if (itemsPackFromConfig.Pack.Count > 0)
            {
                int num = 0;
                builder.Append("* —");
                bool flag = true;
                foreach (long num2 in itemsPackFromConfig.Pack)
                {
                    ItemInMarketRequestEvent evt = new ItemInMarketRequestEvent();
                    this.SendEvent<ItemInMarketRequestEvent>(evt, entity);
                    if (evt.marketItems.ContainsKey(num2))
                    {
                        if (!flag)
                        {
                            builder.Append(", ");
                        }
                        flag = false;
                        builder.Append(evt.marketItems[num2]);
                        num++;
                    }
                }
                if (num == 0)
                {
                    builder.Append((string) this.specialOfferEmptyRewardMessage);
                }
            }
            return builder;
        }

        protected override void FillFromEntity(Entity entity)
        {
            GoodsPriceComponent component = entity.GetComponent<GoodsPriceComponent>();
            if ((component.Currency != this.cachedCurrency) || (component.Price != this.cachedPrice))
            {
                this.cachedCurrency = component.Currency;
                this.cachedPrice = component.Price;
                SpecialOfferContentLocalizationComponent component2 = entity.GetComponent<SpecialOfferContentLocalizationComponent>();
                base.description.text = component2.Description;
                base.title.text = component2.Title;
                base.banner.SpriteUid = component2.SpriteUid;
                base.order = entity.GetComponent<OrderItemComponent>().Index;
                SpecialOfferContentComponent component3 = entity.GetComponent<SpecialOfferContentComponent>();
                double price = component.Price;
                if (component3.SalePercent != 0)
                {
                    this.oldPrice.gameObject.SetActive(false);
                    this.saleImage.gameObject.SetActive(true);
                    this.saleText.text = "-" + component3.SalePercent + "%";
                }
                else
                {
                    this.oldPrice.gameObject.SetActive(true);
                    this.oldPrice.text = this.FormatPrice(price, this.cachedCurrency);
                    price = component.Round((price * (100 - component3.SalePercent)) / 100.0);
                    this.saleImage.gameObject.SetActive(false);
                }
                base.price.text = this.FormatPrice(price, this.cachedCurrency);
                if (component3.HighlightTitle)
                {
                    base.title.faceColor = new Color32(0xff, 0xbc, 9, 0xff);
                    this.titleStripes.gameObject.SetActive(true);
                }
                base.EndDate = entity.GetComponent<SpecialOfferEndTimeComponent>().EndDate;
                TextTimerComponent component4 = base.GetComponent<TextTimerComponent>();
                component4.EndDate = base.EndDate;
                component4.enabled = true;
                ItemsPackFromConfigComponent itemsPackFromConfig = entity.GetComponent<ItemsPackFromConfigComponent>();
                if (component3.ShowItemsList)
                {
                    this.items.text = this.buildComment(entity, itemsPackFromConfig).ToString();
                }
                else
                {
                    Vector3 localPosition = this.timer.transform.localPosition;
                    this.timer.transform.localPosition = new Vector3(localPosition.x, this.items.transform.localPosition.y, localPosition.z);
                    this.items.gameObject.SetActive(false);
                }
                base.FillFromEntity(entity);
            }
        }

        private string FormatPrice(double price, string currency) => 
            this.Price.Replace("{PRICE}", price.ToStringSeparatedByThousands()).Replace("{CURRENCY}", currency);

        public void SetSaleText(string text)
        {
            this.saleText.text = text;
        }

        public virtual string Price { get; set; }
    }
}

