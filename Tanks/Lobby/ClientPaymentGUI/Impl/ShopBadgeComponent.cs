namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class ShopBadgeComponent : BehaviourComponent
    {
        [SerializeField]
        private Image saleIcon;
        [SerializeField]
        private Image specialIcon;
        [SerializeField]
        private Image promoIcon;
        [SerializeField]
        private List<PromoBadge> promoBadges;
        private static bool promoAvailable;
        private static bool saleAvailable;
        private static bool specialOfferAvailable;
        private static bool personalDiscountAvailable;
        private static bool notificationAvailable = true;

        private void OnEnable()
        {
            this.NotificationAvailable = notificationAvailable;
        }

        public void SetPromoAvailable(string Key, bool available)
        {
            <SetPromoAvailable>c__AnonStorey0 storey = new <SetPromoAvailable>c__AnonStorey0 {
                Key = Key
            };
            if (!available || !this.promoBadges.Exists(new Predicate<PromoBadge>(storey.<>m__0)))
            {
                promoAvailable = false;
            }
            else
            {
                promoAvailable = true;
                this.promoIcon.sprite = this.promoBadges.Find(new Predicate<PromoBadge>(storey.<>m__1)).Sprite;
            }
            this.UpdateIcons();
        }

        private void UpdateIcons()
        {
            if ((this.specialIcon != null) && ((this.saleIcon != null) && (this.promoIcon != null)))
            {
                if (promoAvailable)
                {
                    this.specialIcon.gameObject.SetActive(false);
                    this.saleIcon.gameObject.SetActive(false);
                    this.promoIcon.gameObject.SetActive(true);
                }
                else
                {
                    this.promoIcon.gameObject.SetActive(false);
                    if (personalDiscountAvailable && notificationAvailable)
                    {
                        this.specialIcon.gameObject.SetActive(true);
                        this.saleIcon.gameObject.SetActive(false);
                    }
                    else if (saleAvailable && notificationAvailable)
                    {
                        this.specialIcon.gameObject.SetActive(false);
                        this.saleIcon.gameObject.SetActive(true);
                    }
                    else
                    {
                        this.specialIcon.gameObject.SetActive(false);
                        this.saleIcon.gameObject.SetActive(false);
                    }
                }
            }
        }

        public bool PromoAvailable =>
            promoAvailable;

        public bool SaleAvailable
        {
            get => 
                saleAvailable;
            set
            {
                saleAvailable = value;
                this.UpdateIcons();
            }
        }

        public bool SpecialOfferAvailable
        {
            get => 
                specialOfferAvailable;
            set
            {
                specialOfferAvailable = value;
                this.UpdateIcons();
            }
        }

        public bool PersonalDiscountAvailable
        {
            get => 
                personalDiscountAvailable;
            set
            {
                personalDiscountAvailable = value;
                this.UpdateIcons();
            }
        }

        public bool NotificationAvailable
        {
            get => 
                notificationAvailable;
            set
            {
                notificationAvailable = value;
                this.UpdateIcons();
            }
        }

        [CompilerGenerated]
        private sealed class <SetPromoAvailable>c__AnonStorey0
        {
            internal string Key;

            internal bool <>m__0(ShopBadgeComponent.PromoBadge x) => 
                x.Key == this.Key;

            internal bool <>m__1(ShopBadgeComponent.PromoBadge x) => 
                x.Key == this.Key;
        }

        [Serializable]
        private class PromoBadge
        {
            public string Key;
            public UnityEngine.Sprite Sprite;
        }
    }
}

