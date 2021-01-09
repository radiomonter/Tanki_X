namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using System;
    using Tanks.Lobby.ClientNavigation.API;
    using TMPro;
    using UnityEngine;

    public class TeamBattleResultsScreenComponent : LocalizedScreenComponent, NoScaleScreen
    {
        [SerializeField]
        private TextMeshProUGUI blueScore;
        [SerializeField]
        private TextMeshProUGUI redScore;
        [SerializeField]
        private TextMeshProUGUI blueTeamTitle;
        [SerializeField]
        private TextMeshProUGUI redTeamTitle;
        [SerializeField]
        private TextMeshProUGUI blueTeamTitleForSpectator;
        [SerializeField]
        private TextMeshProUGUI redTeamTitleForSpectator;

        public void Init(string mode, int blueScore, int redScore, string mapName, bool spectator)
        {
            this.blueScore.text = blueScore.ToString();
            this.redScore.text = redScore.ToString();
            this.blueTeamTitleForSpectator.gameObject.SetActive(spectator);
            this.redTeamTitleForSpectator.gameObject.SetActive(spectator);
            this.blueTeamTitle.gameObject.SetActive(!spectator);
            this.redTeamTitle.gameObject.SetActive(!spectator);
        }
    }
}

