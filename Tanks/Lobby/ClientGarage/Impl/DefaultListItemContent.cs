namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;

    public class DefaultListItemContent : MonoBehaviour, ListItemContent
    {
        [SerializeField]
        private TextMeshProUGUI nameLabel;

        public void Select()
        {
        }

        public void SetDataProvider(object dataProvider)
        {
            this.nameLabel.text = (string) dataProvider;
        }
    }
}

