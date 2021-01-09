namespace Tanks.Battle.ClientBattleSelect.Impl
{
    using System;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;

    public class NextLeagueTooltipContent : MonoBehaviour, ITooltipContent
    {
        [SerializeField]
        private TextMeshProUGUI text;
        [SerializeField]
        private ImageSkin leagueIcon;
        [SerializeField]
        private TextMeshProUGUI leagueName;
        [SerializeField]
        private LocalizedField leaguePointsText;
        [SerializeField]
        private LocalizedField leagueNameText;

        public void Init(object data)
        {
            NextLeagueTooltipData data2 = data as NextLeagueTooltipData;
            if (!string.IsNullOrEmpty(data2.unfairMM))
            {
                this.text.text = data2.unfairMM + "\n";
            }
            this.text.text = this.text.text + string.Format((string) this.leaguePointsText, data2.points);
            this.leagueIcon.SpriteUid = data2.icon;
            this.leagueName.text = string.Format((string) this.leagueNameText, data2.name);
        }
    }
}

