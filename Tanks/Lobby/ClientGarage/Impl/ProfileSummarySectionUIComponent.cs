namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientProfile.API;
    using Tanks.Lobby.ClientUserProfile.Impl;
    using TMPro;
    using UnityEngine;

    public class ProfileSummarySectionUIComponent : BehaviourComponent
    {
        [SerializeField]
        private AnimatedProgress expProgress;
        [SerializeField]
        private TextMeshProUGUI exp;
        [SerializeField]
        private TextMeshProUGUI currentRank;
        [SerializeField]
        private TextMeshProUGUI nextRank;
        [SerializeField]
        private TextMeshProUGUI winStats;
        [SerializeField]
        private TextMeshProUGUI totalMatches;
        [SerializeField]
        private LocalizedField expLocalizedField;
        [SerializeField]
        private LocalizedField totalMatchesLocalizedField;
        [SerializeField]
        private RankUI rank;
        [SerializeField]
        private Color winColor;
        [SerializeField]
        private Color lossColor;
        public GameObject showRewardsButton;

        public void SetLevelInfo(LevelInfo levelInfo, string rankName)
        {
            bool isMaxLevel = levelInfo.IsMaxLevel;
            this.nextRank.gameObject.SetActive(!isMaxLevel);
            this.expProgress.NormalizedValue = levelInfo.Progress;
            this.currentRank.text = (levelInfo.Level + 1).ToString();
            this.nextRank.text = (levelInfo.Level + 2).ToString();
            this.exp.text = !isMaxLevel ? string.Format(this.expLocalizedField.Value, levelInfo.Experience, levelInfo.MaxExperience) : levelInfo.Experience.ToString();
            this.rank.SetRank(levelInfo.Level, rankName);
        }

        public void SetWinLossStatistics(long winCount, long lossCount, long battlesCount)
        {
            object[] objArray1 = new object[] { "<color=#", this.winColor.ToHexString(), ">", winCount, "/<color=#", this.lossColor.ToHexString(), ">", lossCount };
            this.winStats.text = string.Concat(objArray1);
            this.totalMatches.text = this.totalMatchesLocalizedField.Value + "\n" + battlesCount;
        }
    }
}

