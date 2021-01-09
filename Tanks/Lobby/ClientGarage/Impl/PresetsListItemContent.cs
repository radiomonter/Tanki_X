namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.API;
    using TMPro;
    using UnityEngine;

    public class PresetsListItemContent : MonoBehaviour, ListItemContent
    {
        [SerializeField]
        private TextMeshProUGUI name;
        [SerializeField]
        private TextMeshProUGUI level;

        public void Select()
        {
        }

        public void SetDataProvider(object dataProvider)
        {
            PresetItem item = (PresetItem) dataProvider;
            this.name.text = item.Name;
            object[] objArray1 = new object[] { item.hullName, " <sprite name=\"", item.hullId, "\"> ", item.turretName, "<sprite name=\"", item.weaponId, "\"> " };
            this.level.text = string.Concat(objArray1);
        }
    }
}

