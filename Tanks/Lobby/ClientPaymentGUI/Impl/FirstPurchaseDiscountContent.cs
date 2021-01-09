namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using System;
    using System.Runtime.CompilerServices;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class FirstPurchaseDiscountContent : DealItemContent
    {
        public Image bannerImage;
        public Sprite bigBanner;
        public Sprite smallBanner;
        public GameObject bigDesc;
        public GameObject smallDesc;
        public TextMeshProUGUI BigFirstLine;
        public TextMeshProUGUI BigSecondLine;
        public TextMeshProUGUI BigThirdLine;
        public TextMeshProUGUI BigValue;
        public TextMeshProUGUI SmallFirstLine;
        public TextMeshProUGUI SmallSecondLine;

        private void OnEnable()
        {
            this.SmallSecondLine.text = this.DescriptionFirstLine + " " + this.DescriptionSecondLine;
            this.BigFirstLine.text = this.Header;
            this.BigSecondLine.text = this.DescriptionFirstLine;
            this.BigThirdLine.text = this.DescriptionSecondLine;
        }

        public override void SetParent(Transform parent)
        {
            base.SetParent(parent);
            bool flag = parent.name == "Top";
            this.bannerImage.sprite = !flag ? this.smallBanner : this.bigBanner;
            this.bigDesc.SetActive(flag);
            this.smallDesc.SetActive(!flag);
        }

        public string Header { get; set; }

        public string DescriptionFirstLine { get; set; }

        public string DescriptionSecondLine { get; set; }

        public string Footer { get; set; }

        public double Discount
        {
            set
            {
                int num = Mathf.RoundToInt(((float) value) * 100f);
                this.BigValue.text = num + "%";
                object[] objArray1 = new object[] { this.Header, " ", num, "%" };
                this.SmallFirstLine.text = string.Concat(objArray1);
            }
        }
    }
}

