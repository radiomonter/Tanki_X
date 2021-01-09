namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using Lobby.ClientPayment.Impl;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientPayment.Impl;
    using TMPro;
    using UnityEngine;

    public class XCrystalPackage : MonoBehaviour
    {
        [SerializeField]
        private ImageSkin[] preview;
        [SerializeField]
        private TextMeshProUGUI amount;
        [SerializeField]
        private TextMeshProUGUI price;
        [SerializeField]
        private TextMeshProUGUI totalAmount;
        [SerializeField]
        private LocalizedField forFree;
        [SerializeField]
        private PaletteColorField greyColor;
        [SerializeField]
        private GameObject giftLabel;
        [SerializeField]
        private ImageSkin giftPreview;
        [SerializeField]
        private int xCrySpriteIndex = 9;
        [SerializeField]
        private LocalizedField _commonString;
        [SerializeField]
        private LocalizedField _rareString;
        [SerializeField]
        private LocalizedField _epicString;
        [SerializeField]
        private LocalizedField _legendaryString;

        public void Init(Platform.Kernel.ECS.ClientEntitySystem.API.Entity entity, List<string> images = null)
        {
            this.Entity = entity;
            if (images != null)
            {
                for (int i = 0; i < this.preview.Length; i++)
                {
                    this.preview[i].SpriteUid = images[i];
                }
            }
        }

        public void UpdateData()
        {
            string text;
            if (!this.Entity.HasComponent<PaymentGiftComponent>())
            {
                this.giftLabel.SetActive(false);
            }
            else
            {
                PaymentGiftComponent component = this.Entity.GetComponent<PaymentGiftComponent>();
                Platform.Kernel.ECS.ClientEntitySystem.API.Entity marketItem = Flow.Current.EntityRegistry.GetEntity(component.Gift);
                this.giftPreview.SpriteUid = marketItem.GetComponent<ImageItemComponent>().SpriteUid;
                this.giftLabel.SetActive(true);
                this.giftPreview.GetComponent<TooltipShowBehaviour>().TipText = MarketItemNameLocalization.GetFullItemDescription(marketItem, (string) this._commonString, (string) this._rareString, (string) this._epicString, (string) this._legendaryString);
            }
            XCrystalsPackComponent component2 = this.Entity.GetComponent<XCrystalsPackComponent>();
            SaleState saleState = this.Entity.GetComponent<GoodsComponent>().SaleState;
            string str = "<b>  ";
            long amount = component2.Amount;
            long num2 = (long) Math.Round((double) (component2.Amount * saleState.AmountMultiplier));
            long bonus = component2.Bonus;
            if (saleState.AmountMultiplier <= 1.0)
            {
                str = str + amount.ToStringSeparatedByThousands();
                object[] objArray2 = new object[] { ((long) (amount + bonus)).ToStringSeparatedByThousands(), "<sprite=", this.xCrySpriteIndex, ">" };
                this.totalAmount.text = string.Concat(objArray2);
            }
            else
            {
                str = str + num2.ToStringSeparatedByThousands() + $" <s><#{this.greyColor.Color.ToHexString()}>{amount.ToStringSeparatedByThousands()}</color></s>";
                this.totalAmount.text = ((long) (num2 + bonus)).ToStringSeparatedByThousands();
                this.totalAmount.text = this.totalAmount.text + $" <s><#{this.greyColor.Color.ToHexString()}>{((long) (amount + bonus)).ToStringSeparatedByThousands()}</color></s>";
                text = this.totalAmount.text;
                object[] objArray1 = new object[] { text, "<sprite=", this.xCrySpriteIndex, ">" };
                this.totalAmount.text = string.Concat(objArray1);
            }
            text = str;
            object[] objArray3 = new object[] { text, "</b><sprite=", this.xCrySpriteIndex, ">\n" };
            str = string.Concat(objArray3);
            str = (bonus <= 0L) ? (str + "\n") : (str + string.Format("<size=17><#{2}>+{0} {1}<sprite=" + this.xCrySpriteIndex + "></color>", bonus.ToStringSeparatedByThousands(), this.forFree.Value, this.greyColor.Color.ToHexString()));
            this.amount.text = str;
            GoodsPriceComponent component4 = this.Entity.GetComponent<GoodsPriceComponent>();
            str = component4.Round(this.Entity.GetComponent<GoodsComponent>().SaleState.PriceMultiplier * component4.Price).ToStringSeparatedByThousands();
            if (saleState.PriceMultiplier < 1.0)
            {
                str = str + $" <s><#{this.greyColor.Color.ToHexString()}>{component4.Price.ToStringSeparatedByThousands()}</color></s>";
            }
            str = str + " " + component4.Currency;
            this.price.text = str;
        }

        [Inject]
        public static EngineServiceInternal EngineService { get; set; }

        public Platform.Kernel.ECS.ClientEntitySystem.API.Entity Entity { get; private set; }
    }
}

