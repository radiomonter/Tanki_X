namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;

    public class LeagueRewardItem : MonoBehaviour
    {
        [SerializeField]
        private ImageSkin itemIcon;
        [SerializeField]
        private TextMeshProUGUI info;

        public string Text
        {
            set => 
                this.info.text = value;
        }

        public string Icon
        {
            set => 
                this.itemIcon.SpriteUid = value;
        }
    }
}

