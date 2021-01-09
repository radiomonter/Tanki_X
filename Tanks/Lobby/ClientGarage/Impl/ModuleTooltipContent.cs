namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;

    public class ModuleTooltipContent : MonoBehaviour, ITooltipContent
    {
        [SerializeField]
        private TextMeshProUGUI title;
        [SerializeField]
        private TextMeshProUGUI description;
        [SerializeField]
        private TextMeshProUGUI upgradeLevel;
        [SerializeField]
        private LocalizedField upgradeLevelLocalization;
        [SerializeField]
        private TextMeshProUGUI currentLevel;
        [SerializeField]
        private TextMeshProUGUI nextLevel;

        public void Init(object data)
        {
            ModuleTooltipData data2 = data as ModuleTooltipData;
            this.title.text = data2.name;
            this.description.text = data2.desc;
            this.UpgradeLevel = data2.upgradeLevel + 1;
            ModulesPropertiesUIComponent component = base.GetComponent<ModulesPropertiesUIComponent>();
            if ((data2.upgradeLevel != -1) && (data2.upgradeLevel != data2.maxLevel))
            {
                this.CurrentLevel = data2.upgradeLevel + 1;
                this.NextLevel = data2.upgradeLevel + 2;
            }
            else
            {
                int num = -1;
                this.NextLevel = num;
                this.CurrentLevel = num;
            }
            for (int i = 0; i < data2.properties.Count; i++)
            {
                ModuleVisualProperty property = data2.properties[i];
                if (property.Upgradable && ((data2.upgradeLevel != data2.maxLevel) && (data2.upgradeLevel != -1)))
                {
                    float minValue = 0f;
                    float maxValue = property.CalculateModuleEffectPropertyValue(data2.maxLevel, data2.maxLevel);
                    component.AddProperty(property.Name, property.Unit, minValue, maxValue, (data2.upgradeLevel == -1) ? 0f : property.CalculateModuleEffectPropertyValue(data2.upgradeLevel, data2.maxLevel), property.CalculateModuleEffectPropertyValue(data2.upgradeLevel + 1, data2.maxLevel), property.Format);
                }
                else if (data2.upgradeLevel == -1)
                {
                    float currentValue = property.CalculateModuleEffectPropertyValue(0, data2.maxLevel);
                    component.AddProperty(property.Name, property.Unit, currentValue, property.Format);
                }
                else
                {
                    float currentValue = property.CalculateModuleEffectPropertyValue(data2.upgradeLevel, data2.maxLevel);
                    component.AddProperty(property.Name, property.Unit, currentValue, property.Format);
                }
            }
        }

        private int UpgradeLevel
        {
            set
            {
                this.upgradeLevel.gameObject.SetActive(value > 0);
                this.upgradeLevel.text = this.upgradeLevelLocalization.Value + " " + value;
            }
        }

        private int CurrentLevel
        {
            set
            {
                this.currentLevel.gameObject.SetActive(value != -1);
                this.currentLevel.text = this.upgradeLevelLocalization.Value + " " + value;
            }
        }

        private int NextLevel
        {
            set
            {
                this.nextLevel.gameObject.SetActive(value != -1);
                this.nextLevel.text = this.upgradeLevelLocalization.Value + " " + value;
            }
        }
    }
}

