namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientNavigation.API;
    using UnityEngine;
    using UnityEngine.UI;

    public class DMBattleResultsScreenComponent : LocalizedScreenComponent, NoScaleScreen
    {
        [SerializeField]
        private Text selfScore;
        [SerializeField]
        private Text maxScore;
        [SerializeField]
        private ProgressBar progressBar;
        [SerializeField]
        private Text mapName;

        public void Init(int selfScore, int maxScore, string mapName)
        {
            if (this.progressBar != null)
            {
                this.progressBar.ProgressValue = (maxScore != 0) ? Mathf.Clamp01(((float) selfScore) / ((float) maxScore)) : 1f;
                this.selfScore.text = selfScore.ToString();
                this.maxScore.text = maxScore.ToString();
                this.mapName.text = mapName.ToUpper();
            }
        }
    }
}

