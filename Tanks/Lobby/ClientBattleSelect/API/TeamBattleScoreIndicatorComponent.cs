namespace Tanks.Lobby.ClientBattleSelect.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;
    using UnityEngine.UI;

    public class TeamBattleScoreIndicatorComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private Text blueTeamScoreText;
        [SerializeField]
        private Text redTeamScoreText;
        [SerializeField]
        private ProgressBar blueScoreProgress;
        [SerializeField]
        private ProgressBar redScoreProgress;

        public void UpdateScore(int blueScore, int redScore, int scoreLimit)
        {
            this.blueTeamScoreText.text = blueScore.ToString();
            this.redTeamScoreText.text = redScore.ToString();
            this.blueScoreProgress.ProgressValue = (scoreLimit <= 0) ? 0f : (((float) blueScore) / ((float) scoreLimit));
            this.redScoreProgress.ProgressValue = (scoreLimit <= 0) ? 0f : (((float) redScore) / ((float) scoreLimit));
        }
    }
}

