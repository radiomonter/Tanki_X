namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class UpgradeModuleBaseButtonComponent : BehaviourComponent
    {
        [SerializeField]
        protected TextMeshProUGUI titleText;
        [SerializeField]
        protected LocalizedField activate;
        [SerializeField]
        protected LocalizedField upgrade;
        [SerializeField]
        protected LocalizedField fullUpgraded;
        [SerializeField]
        protected Image border;
        [SerializeField]
        protected Image fill;
        [SerializeField]
        protected Color notEnoughColor;
        [SerializeField]
        protected Color notEnoughFillColor;
        [SerializeField]
        protected Color enoughColor;
        [SerializeField]
        protected Color notEnoughTextColor;
        [SerializeField]
        protected Color enoughTextColor;
        [SerializeField]
        protected GameObject notEnoughText;
        [SerializeField]
        protected LocalizedField notEnoughTextStart;

        public void Activate()
        {
        }

        public void FullUpgraded()
        {
            this.titleText.text = this.fullUpgraded.Value;
            Color notEnoughColor = this.notEnoughColor;
            this.titleText.color = notEnoughColor;
            this.fill.color = notEnoughColor;
            this.border.color = notEnoughColor;
        }

        public virtual void Setup(int moduleLevel, int cardsCount, int maxCardCount, int price, int priceXCry, int userCryCount, int userXCryCount)
        {
            throw new NotImplementedException();
        }

        public string TitleTextUpgrade
        {
            get => 
                this.titleText.text;
            set => 
                this.titleText.text = this.upgrade.Value + " " + value;
        }

        public string BuyCrystal
        {
            set => 
                this.titleText.text = value;
        }

        public bool NotEnoughTextEnable
        {
            set => 
                this.notEnoughText.SetActive(value);
        }

        public long NotEnoughText
        {
            set => 
                this.notEnoughText.GetComponent<TextMeshProUGUI>().text = string.Format((string) this.notEnoughTextStart, value);
        }

        public string TitleTextActivate
        {
            get => 
                this.titleText.text;
            set => 
                this.titleText.text = this.activate.Value + " " + value;
        }
    }
}

