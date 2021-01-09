namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class CraftModuleConfirmWindowComponent : ConfirmWindowComponent
    {
        [SerializeField]
        protected TextMeshProUGUI additionalText;
        [SerializeField]
        private LocalizedField module;
        [SerializeField]
        private LocalizedField craftFor;
        [SerializeField]
        private LocalizedField decline;
        [SerializeField]
        private LocalizedField upgradeFor;
        [SerializeField]
        private LocalizedField buyBlueprints;
        [SerializeField]
        private Color greenColor;
        [SerializeField]
        private Color whiteColor;
        [SerializeField]
        private Image highlight;
        [SerializeField]
        private Image fill;
        [SerializeField]
        protected ImageSkin icon;

        public void Setup(string moduleName, string desc, string spriteUid, double price, bool craft, string currencySpriteId = "8", bool dontenoughtcard = false)
        {
            base.HeaderText = this.module.Value + " " + moduleName;
            this.additionalText.gameObject.SetActive(craft && dontenoughtcard);
            if (craft)
            {
                base.ConfirmText = !dontenoughtcard ? this.craftFor.Value : this.buyBlueprints.Value;
            }
            else
            {
                object[] objArray1 = new object[] { price, "<sprite=", currencySpriteId, ">" };
                base.ConfirmText = string.Concat(objArray1);
            }
            base.DeclineText = this.decline.Value;
            base.MainText = desc;
            this.SpriteUid = spriteUid;
        }

        public string SpriteUid
        {
            set => 
                this.icon.SpriteUid = value;
        }

        public GameObject CardPriceLabel { get; set; }
    }
}

