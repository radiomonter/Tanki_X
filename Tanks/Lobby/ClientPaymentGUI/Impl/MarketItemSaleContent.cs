namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientGarage.Impl;
    using UnityEngine;

    public class MarketItemSaleContent : DealItemContent
    {
        [SerializeField]
        private PaletteColorField greyColor;

        protected override void FillFromEntity(Entity entity)
        {
            if (entity.HasComponent<ImageItemComponent>())
            {
                string spriteUid = entity.GetComponent<ImageItemComponent>().SpriteUid;
                base.banner.SpriteUid = spriteUid;
            }
            base.title.text = MarketItemNameLocalization.Instance.GetCategoryName(entity) + " \"";
            GarageItem item = GarageItemsRegistry.GetItem<GarageItem>(entity);
            if (item != null)
            {
                base.title.text = base.title.text + MarketItemNameLocalization.Instance.GetGarageItemName(item);
            }
            else if (entity.HasComponent<DescriptionItemComponent>())
            {
                DescriptionItemComponent component = entity.GetComponent<DescriptionItemComponent>();
                base.title.text = base.title.text + component.Name;
            }
            base.title.text = base.title.text + "\"";
            XPriceItemComponent component2 = entity.GetComponent<XPriceItemComponent>();
            string priceStr = component2.Price.ToStringSeparatedByThousands();
            if (component2.Price < component2.OldPrice)
            {
                priceStr = priceStr + $" <s><#{this.greyColor.Color.ToHexString()}>{component2.OldPrice.ToStringSeparatedByThousands()}</color></s>";
            }
            this.SetPrice(priceStr, "<sprite=9>");
            base.EndDate = entity.GetComponent<MarketItemSaleComponent>().endDate;
            if (base.EndDate.UnityTime != 0f)
            {
                TextTimerComponent component = base.GetComponent<TextTimerComponent>();
                component.EndDate = base.EndDate;
                component.enabled = true;
            }
            base.FillFromEntity(entity);
        }

        private string FormatPrice(string priceStr, string currency) => 
            this.Price.Replace("{PRICE}", priceStr).Replace("{CURRENCY}", currency);

        private void SetPrice(string priceStr, string currency)
        {
            base.price.text = this.FormatPrice(priceStr, currency);
        }

        [Inject]
        public static Tanks.Lobby.ClientGarage.API.GarageItemsRegistry GarageItemsRegistry { get; set; }

        public virtual string Price { get; set; }
    }
}

