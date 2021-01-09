namespace Tanks.Lobby.ClientMatchMaking.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class UserEnergyBarUIComponent : BehaviourComponent
    {
        [SerializeField]
        private float animationSpeed = 1f;
        [SerializeField]
        private Slider slider;
        [SerializeField]
        private Slider subSlider;
        [SerializeField]
        private TextMeshProUGUI energyLevel;
        private long currentValue;
        private long maxValue;
        private float mainSliderValue;
        private float subSliderValue;

        private void LerpSliderValue(Slider slider, float value)
        {
            float num = Mathf.Abs((float) (slider.value - value));
            if (num != 0f)
            {
                slider.value = Mathf.Lerp(slider.value, value, (Time.deltaTime * this.animationSpeed) / num);
            }
        }

        private void OnEnable()
        {
            this.slider.value = 0f;
            this.subSlider.value = 0f;
        }

        public void SetEnergyLevel(long currentValue, long maxValue)
        {
            this.currentValue = currentValue;
            this.maxValue = maxValue;
            this.mainSliderValue = ((float) currentValue) / ((float) maxValue);
            this.subSliderValue = 0f;
            this.SetTextValue(currentValue, maxValue);
        }

        public void SetSharedEnergyLevel(long sharedValue)
        {
            this.subSliderValue = ((float) this.currentValue) / ((float) this.maxValue);
            this.mainSliderValue = ((float) (this.currentValue - sharedValue)) / ((float) this.maxValue);
            this.SetTextValue(this.currentValue - sharedValue, this.maxValue);
        }

        private void SetTextValue(long value, long maxValue)
        {
            this.energyLevel.text = $"{value}/{maxValue}";
        }

        public void ShowAdditionalEnergyLevel(long additionalValue)
        {
            this.SetEnergyLevel(this.currentValue + additionalValue, this.maxValue);
        }

        private void Update()
        {
            this.LerpSliderValue(this.slider, this.mainSliderValue);
            this.LerpSliderValue(this.subSlider, this.subSliderValue);
        }
    }
}

