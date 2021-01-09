namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;
    using UnityEngine.UI;

    public class StaticBonusUI : LocalizedControl
    {
        [SerializeField]
        private ImageSkin image;
        [SerializeField]
        private Text valueText;
        [SerializeField]
        private Text sufixText;
        private int value;

        public string Icon
        {
            get => 
                this.image.SpriteUid;
            set => 
                this.image.SpriteUid = value;
        }

        public int Value
        {
            get => 
                this.value;
            set
            {
                this.value = value;
                this.valueText.text = string.Format(this.BonusText, value);
            }
        }

        public string BonusText { get; set; }

        public string DamageText { get; set; }

        public string ArmorText { get; set; }
    }
}

