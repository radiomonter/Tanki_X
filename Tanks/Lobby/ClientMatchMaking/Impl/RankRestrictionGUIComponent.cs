namespace Tanks.Lobby.ClientMatchMaking.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;

    public class RankRestrictionGUIComponent : BehaviourComponent
    {
        [SerializeField]
        private TextMeshProUGUI rankName;
        [SerializeField]
        private ImageListSkin imageListSkin;

        public void SetRank(int rank)
        {
            this.imageListSkin.SelectSprite(rank.ToString());
        }

        public string RankName
        {
            set => 
                this.rankName.text = value;
        }
    }
}

