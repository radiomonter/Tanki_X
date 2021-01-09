namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using TMPro;
    using UnityEngine;

    public class UpgradePropertyUI : MonoBehaviour
    {
        [SerializeField]
        protected TextMeshProUGUI valueLabel;
        [SerializeField]
        protected TextMeshProUGUI nextValueLabel;
        [SerializeField]
        protected TextMeshProUGUI name;
        [SerializeField]
        protected GameObject arrow;
        private int level;

        public void DisableNextValueAndArrow()
        {
            this.nextValueLabel.gameObject.SetActive(false);
            this.arrow.gameObject.SetActive(false);
        }

        private string FormatValue(float value, string format)
        {
            float num = ((float) Mathf.RoundToInt(value * 100f)) / 100f;
            return string.Format("{0:" + format + "}", value);
        }

        public virtual void SetUpgradableValue(string name, string unit, float minValue, float maxValue, float currentValue, float nextValue, string format)
        {
            this.SetUpgradableValue(name, unit, this.FormatValue(currentValue, format), this.FormatValue(nextValue, format), currentValue, nextValue, minValue, maxValue, format);
        }

        public virtual void SetUpgradableValue(string name, string unit, string currentValueStr, string nextValueStr, float currentValue, float nextValue, float minValue, float maxValue, string format)
        {
            this.name.text = name;
            this.valueLabel.text = currentValueStr + " " + unit;
            this.nextValueLabel.text = nextValueStr + " " + unit;
        }

        public virtual void SetValue(string name, string unit, string currentValueStr)
        {
            this.name.text = name;
            this.valueLabel.text = currentValueStr + " " + unit;
            Color clear = Color.clear;
            this.nextValueLabel.color = clear;
            this.arrow.GetComponent<Image>().color = clear;
        }

        public virtual void SetValue(string name, string unit, float currentValue, string format)
        {
            this.SetValue(name, unit, this.FormatValue(currentValue, format));
        }
    }
}

