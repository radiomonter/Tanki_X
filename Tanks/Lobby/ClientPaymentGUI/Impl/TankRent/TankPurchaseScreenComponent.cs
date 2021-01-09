namespace Tanks.Lobby.ClientPaymentGUI.Impl.TankRent
{
    using Lobby.ClientPayment.Impl;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using Tanks.Lobby.ClientGarage.Impl;
    using Tanks.Lobby.ClientPayment.API;
    using Tanks.Lobby.ClientPayment.Impl;
    using Tanks.Lobby.ClientPaymentGUI.Impl;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class TankPurchaseScreenComponent : PurchaseItemComponent
    {
        public TextMeshProUGUI actualPrice;
        public TextMeshProUGUI priceWithoutDiscount;
        public TextMeshProUGUI discount;
        public GameObject discountExplanationBlock;
        public Image tankImage;
        public Image backgroundImage;
        public Image[] modules;
        [Header("Support")]
        public Sprite supportTank;
        public Sprite supportTankBackgroundImage;
        public Sprite[] supportModules;
        [Header("Offensive")]
        public Sprite offensiveTank;
        public Sprite[] offensiveModules;
        public Sprite offensiveTankBackgroundImage;
        [Header("Annihilation")]
        public Sprite annihilationTank;
        public Sprite[] annihilationModules;
        public Sprite annihilationTankBackgroundImage;

        private void CloseScreen()
        {
            base.gameObject.SetActive(false);
        }

        public void InitiateScreen(GoodsPriceComponent offerGoodsPrice, DiscountComponent personalOfferDiscount, RentTankRole tankRole, ShopDialogs shopDialogs)
        {
            base.shopDialogs = shopDialogs;
            if (personalOfferDiscount.DiscountCoeff <= 0f)
            {
                this.actualPrice.text = offerGoodsPrice.Price + " " + offerGoodsPrice.Currency;
                this.SetDiscountObjects(false);
            }
            else
            {
                float num = this.RoundPrice(offerGoodsPrice.Price * (1f - personalOfferDiscount.DiscountCoeff));
                this.actualPrice.text = num + " " + offerGoodsPrice.Currency;
                this.priceWithoutDiscount.text = offerGoodsPrice.Price.ToString(CultureInfo.InvariantCulture);
                this.discount.text = $"-{personalOfferDiscount.DiscountCoeff * 100f}%";
                this.SetDiscountObjects(true);
            }
            this.SetWindowContent(tankRole);
        }

        private void OnDisable()
        {
            MainScreenComponent.Instance.ClearOnBackOverride();
            MainScreenComponent.Instance.OnPanelShow(MainScreenComponent.MainScreens.Main);
        }

        public void OpenPurchaseWindow(Entity entity, ShopDialogs dialogs = null)
        {
            if (dialogs != null)
            {
                base.shopDialogs = dialogs;
            }
            if (base.shopDialogs != null)
            {
                base.OnPackClick(entity, false);
            }
        }

        private float RoundPrice(double price) => 
            (float) (Math.Round((double) (price * 100.0)) / 100.0);

        private void SetDiscountObjects(bool state)
        {
            this.priceWithoutDiscount.gameObject.SetActive(state);
            this.discount.transform.parent.gameObject.SetActive(state);
            this.discountExplanationBlock.SetActive(state);
        }

        private void SetWindowContent(RentTankRole role)
        {
            MainScreenComponent.Instance.OverrideOnBack(new Action(this.CloseScreen));
            MainScreenComponent.Instance.OnPanelShow(MainScreenComponent.MainScreens.TankRent);
            if (role == RentTankRole.ANNIHILATION)
            {
                this.tankImage.sprite = this.annihilationTank;
                this.backgroundImage.sprite = this.annihilationTankBackgroundImage;
                for (int i = 0; i < this.modules.Length; i++)
                {
                    this.modules[i].sprite = this.annihilationModules[i];
                }
            }
            else if (role == RentTankRole.OFFENSIVE)
            {
                this.tankImage.sprite = this.offensiveTank;
                this.backgroundImage.sprite = this.offensiveTankBackgroundImage;
                for (int i = 0; i < this.modules.Length; i++)
                {
                    this.modules[i].sprite = this.offensiveModules[i];
                }
            }
            else if (role == RentTankRole.SUPPORT)
            {
                this.tankImage.sprite = this.supportTank;
                this.backgroundImage.sprite = this.supportTankBackgroundImage;
                for (int i = 0; i < this.modules.Length; i++)
                {
                    this.modules[i].sprite = this.supportModules[i];
                }
            }
        }

        private void Update()
        {
            if (InputMapping.Cancel)
            {
                this.CloseScreen();
            }
        }
    }
}

