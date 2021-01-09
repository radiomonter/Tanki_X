namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class UpgradeBarPropertyUI : UpgradePropertyUI
    {
        [SerializeField]
        protected GameObject barContent;
        [SerializeField]
        protected Slider currentValueSlider;
        [SerializeField]
        protected Slider nextValueSlider;
        [SerializeField]
        protected RectTransform currentValueFill;
        [SerializeField]
        protected RectTransform nextValueFill;

        public override void SetUpgradableValue(string name, string unit, string currentValueStr, string nextValueStr, float minValue, float maxValue, float currentValue, float nextValue, string format)
        {
            base.SetUpgradableValue(name, unit, currentValueStr, nextValueStr, minValue, maxValue, currentValue, nextValue, format);
            float num = Mathf.Abs(minValue);
            this.nextValueSlider.minValue = num;
            this.currentValueSlider.minValue = num;
            num = Mathf.Abs(maxValue);
            this.nextValueSlider.maxValue = num;
            this.currentValueSlider.maxValue = num;
            this.currentValueSlider.value = Mathf.Abs(currentValue);
            this.nextValueSlider.value = Mathf.Abs(nextValue);
        }

        public override void SetValue(string name, string unit, string currentValueStr)
        {
            base.SetValue(name, unit, currentValueStr);
            this.barContent.SetActive(false);
        }

        private void Update()
        {
            if (this.barContent != null)
            {
                this.nextValueFill.offsetMin = new Vector2(this.currentValueFill.rect.width, this.nextValueFill.offsetMin.y);
            }
        }
    }
}

