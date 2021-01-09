namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;

    public class ModulesPropertiesUIComponent : BehaviourComponent
    {
        [SerializeField]
        private UpgradePropertyUI propertyUIPreafab;
        [SerializeField]
        private RectTransform scrollContent;

        public void AddProperty(string name, string unit, float currentValue, string format)
        {
            this.GetPropertyUi().SetValue(name, unit, currentValue, format);
        }

        public void AddProperty(string name, string unit, float minValue, float maxValue, float currentValue, float nextValue, string format)
        {
            if (currentValue != nextValue)
            {
                this.GetPropertyUi().SetUpgradableValue(name, unit, minValue, maxValue, currentValue, nextValue, format);
            }
            else
            {
                this.AddProperty(name, unit, currentValue, format);
            }
        }

        public void AddProperty(string name, string unit, string currentValueStr, string nextValueStr, float minValue, float maxValue, float currentValue, float nextValue, string format)
        {
            if (currentValue != nextValue)
            {
                this.GetPropertyUi().SetUpgradableValue(name, unit, currentValueStr, nextValueStr, currentValue, nextValue, minValue, maxValue, format);
            }
            else
            {
                this.GetPropertyUi().SetValue(name, unit, currentValueStr);
            }
        }

        public void Clear()
        {
            this.scrollContent.DestroyChildren();
        }

        public UpgradePropertyUI GetPropertyUi()
        {
            UpgradePropertyUI yui = Instantiate<UpgradePropertyUI>(this.propertyUIPreafab);
            yui.gameObject.SetActive(true);
            yui.transform.SetParent(this.scrollContent, false);
            return yui;
        }
    }
}

