namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.API;
    using TMPro;
    using UnityEngine;

    public class SkinsListItemContent : MonoBehaviour, ListItemContent
    {
        [SerializeField]
        private TextMeshProUGUI nameLabel;

        public void Select()
        {
        }

        public void SetDataProvider(object dataProvider)
        {
            VisualItem item = (VisualItem) dataProvider;
            this.nameLabel.text = item.Name;
        }
    }
}

