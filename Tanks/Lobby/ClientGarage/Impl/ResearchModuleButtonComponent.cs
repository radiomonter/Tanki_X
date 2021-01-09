namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using TMPro;
    using UnityEngine;

    public class ResearchModuleButtonComponent : UpgradeModuleBaseButtonComponent
    {
        [SerializeField]
        protected TextMeshProUGUI cardsCountText;

        public override void Setup(int moduleLevel, int cardsCount, int maxCardCount, int price, int priceXCry, int userCryCount, int userXCryCount)
        {
            if (moduleLevel != -1)
            {
                base.gameObject.SetActive(false);
            }
            else
            {
                base.gameObject.SetActive(true);
                base.Activate();
                bool flag = cardsCount >= maxCardCount;
                bool flag2 = userCryCount >= price;
                bool flag3 = flag && flag2;
                this.cardsCountText.text = cardsCount + "/" + maxCardCount;
                this.cardsCountText.color = !flag ? base.notEnoughTextColor : base.enoughTextColor;
                base.titleText.text = (string) base.activate;
                Color color = !flag3 ? base.notEnoughColor : base.enoughColor;
                base.titleText.color = color;
                base.fill.color = color;
                base.border.color = color;
            }
        }
    }
}

