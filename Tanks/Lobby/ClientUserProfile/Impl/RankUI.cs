namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using System;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;

    public class RankUI : MonoBehaviour
    {
        [SerializeField]
        private ImageListSkin rankIcon;
        [SerializeField]
        private TextMeshProUGUI rank;
        [SerializeField]
        private LocalizedField rankLocalizedField;

        public void SetRank(int rankIconIndex, string rankName)
        {
            this.rank.text = rankName;
            this.rankIcon.SelectSprite((rankIconIndex + 1).ToString());
        }
    }
}

