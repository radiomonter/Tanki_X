namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class AnimatedPropertyProgressBar : MonoBehaviour
    {
        private bool canStart;
        private float finalValue;
        [SerializeField]
        private Image slider;

        private void OnDisable()
        {
            this.slider.fillAmount = 0f;
        }

        private void Start()
        {
            this.finalValue = this.slider.fillAmount;
            this.slider.fillAmount = 0f;
            this.canStart = true;
        }

        private void Update()
        {
            if (this.canStart)
            {
                this.slider.fillAmount = Mathf.Lerp(this.slider.fillAmount, this.finalValue, 0.1f);
                if (this.slider.fillAmount == this.finalValue)
                {
                    this.canStart = false;
                }
            }
        }
    }
}

