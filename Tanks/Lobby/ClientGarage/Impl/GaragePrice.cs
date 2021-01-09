namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientEntrance.API;
    using TMPro;
    using UnityEngine;

    [RequireComponent(typeof(TextMeshProUGUI))]
    public class GaragePrice : MonoBehaviour
    {
        [SerializeField]
        private bool needUpdateColor = true;
        [SerializeField]
        private PaletteColorField redColor;
        [SerializeField]
        private PaletteColorField normalColor;
        [SerializeField]
        private PriceType priceType;
        private static HashSet<GaragePrice> visiblePrices = new HashSet<GaragePrice>();
        private int value;
        private int oldValue;
        [CompilerGenerated]
        private static Action<GaragePrice> <>f__am$cache0;

        private void OnDisable()
        {
            visiblePrices.Remove(this);
        }

        private void OnEnable()
        {
            visiblePrices.Add(this);
        }

        public void SetPrice(int oldPrice, int price)
        {
            this.oldValue = oldPrice;
            this.value = price;
            this.UpdatePrice();
        }

        private void UpdatePrice()
        {
            TextMeshProUGUI component = base.GetComponent<TextMeshProUGUI>();
            component.text = ((this.value >= this.oldValue) || (this.oldValue <= 0)) ? this.value.ToStringSeparatedByThousands() : $"{this.value.ToStringSeparatedByThousands()} <#787878><s>{this.oldValue.ToStringSeparatedByThousands()}</s></color>";
            if (this.needUpdateColor)
            {
                PriceType priceType = this.priceType;
                if (priceType == PriceType.Crystals)
                {
                    component.color = (SelfUserComponent.SelfUser.GetComponent<UserMoneyComponent>().Money >= this.value) ? this.normalColor.Apply(component.color) : this.redColor.Apply(component.color);
                }
                else if (priceType == PriceType.XCrystals)
                {
                    component.color = (SelfUserComponent.SelfUser.GetComponent<UserXCrystalsComponent>().Money >= this.value) ? this.normalColor.Apply(component.color) : this.redColor.Apply(component.color);
                }
            }
        }

        public static void UpdatePrices()
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = p => p.UpdatePrice();
            }
            visiblePrices.ForEach<GaragePrice>(<>f__am$cache0);
        }

        public bool NeedUpdateColor
        {
            get => 
                this.needUpdateColor;
            set => 
                this.needUpdateColor = value;
        }

        public enum PriceType
        {
            XCrystals,
            Crystals
        }
    }
}

