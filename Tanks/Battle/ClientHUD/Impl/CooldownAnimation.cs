namespace Tanks.Battle.ClientHUD.Impl
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(Text)), RequireComponent(typeof(NormalizedAnimatedValue))]
    public class CooldownAnimation : MonoBehaviour
    {
        private Text _text;
        private NormalizedAnimatedValue _animatedValue;
        private float cooldown;

        private void Awake()
        {
            this.text.text = string.Empty;
        }

        private void OnDisable()
        {
            this.text.text = string.Empty;
        }

        private void Update()
        {
            float num = this.animatedValue.value * this.cooldown;
            if (num > 0f)
            {
                this.text.text = $"{num:0}";
            }
        }

        private Text text
        {
            get
            {
                if (this._text == null)
                {
                    this._text = base.GetComponent<Text>();
                }
                return this._text;
            }
        }

        private NormalizedAnimatedValue animatedValue
        {
            get
            {
                if (this._animatedValue == null)
                {
                    this._animatedValue = base.GetComponent<NormalizedAnimatedValue>();
                }
                return this._animatedValue;
            }
        }

        public float Cooldown
        {
            get => 
                this.cooldown;
            set
            {
                this.cooldown = value;
                this.text.text = $"{value:0}";
            }
        }
    }
}

