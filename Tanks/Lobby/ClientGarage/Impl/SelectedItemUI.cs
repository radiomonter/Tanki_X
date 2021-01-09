namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.API;
    using TMPro;
    using UnityEngine;

    public class SelectedItemUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI itemName;
        [SerializeField]
        private TextMeshProUGUI feature;
        [SerializeField]
        private MainVisualPropertyUI[] props;
        [SerializeField]
        private AnimatedNumber mastery;
        [SerializeField]
        private TextMeshProUGUI currentSkin;
        [SerializeField]
        private UpgradeStars upgradeStars;
        [SerializeField]
        private LocalizedField currentSkinLocalizedField;

        public void DisableMasteryElement()
        {
            this.mastery.gameObject.SetActive(false);
        }

        public void EnableMasteryElement()
        {
            this.mastery.gameObject.SetActive(true);
        }

        public void Set(Tanks.Lobby.ClientGarage.API.TankPartItem item, string skinName, int level)
        {
            this.TankPartItem = item;
            this.SendEvent<ListItemSelectedEvent>(item.UserItem);
            this.itemName.text = item.Name;
            this.mastery.Value = level;
            this.feature.text = item.Feature;
            this.currentSkin.text = this.currentSkinLocalizedField.Value + ": " + skinName;
            List<MainVisualProperty> mainProperties = item.MainProperties;
            for (int i = 0; i < mainProperties.Count; i++)
            {
                this.props[i].gameObject.SetActive(true);
                this.props[i].Set(mainProperties[i].Name, mainProperties[i].NormalizedValue);
            }
            for (int j = mainProperties.Count; j < this.props.Length; j++)
            {
                this.props[j].gameObject.SetActive(false);
            }
        }

        public void SetStars(float coef)
        {
            this.upgradeStars.SetPower(coef);
        }

        public Tanks.Lobby.ClientGarage.API.TankPartItem TankPartItem { get; set; }
    }
}

