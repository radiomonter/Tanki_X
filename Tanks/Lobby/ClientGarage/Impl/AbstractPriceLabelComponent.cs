namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;

    public abstract class AbstractPriceLabelComponent : BehaviourComponent
    {
        public UnityEngine.Color shortageColor = UnityEngine.Color.red;
        [SerializeField]
        private GameObject oldPrice;
        [SerializeField]
        private UnityEngine.UI.Text oldPriceText;

        protected AbstractPriceLabelComponent()
        {
        }

        private void Awake()
        {
            this.DefaultColor = this.Text.color;
        }

        public UnityEngine.Color DefaultColor { get; set; }

        public long Price { get; set; }

        public long OldPrice { get; set; }

        public UnityEngine.UI.Text Text =>
            base.GetComponent<UnityEngine.UI.Text>();

        public bool OldPriceVisible
        {
            set
            {
                if (this.oldPrice != null)
                {
                    this.oldPrice.SetActive(value);
                }
            }
        }

        public string OldPriceText
        {
            set
            {
                if (this.oldPriceText != null)
                {
                    this.oldPriceText.text = value;
                }
            }
        }

        public UnityEngine.Color Color
        {
            get => 
                this.Text.color;
            set
            {
                if (this.Text.color == this.DefaultColor)
                {
                    this.Text.color = value;
                }
                this.DefaultColor = value;
            }
        }
    }
}

