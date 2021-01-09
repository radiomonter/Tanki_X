namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientProfile.API;
    using TMPro;
    using UnityEngine;

    public class ProgressResultUI : MonoBehaviour
    {
        [SerializeField]
        private AnimatedDiffRadialProgress experienceProgress;
        [SerializeField]
        protected TextMeshProUGUI expRewardUI;
        [SerializeField]
        private Animator animator;
        protected LevelInfo currentLevelInfo;
        protected LevelInfo previousLevelInfo;
        protected int nextLevel;
        private float previousProgress;
        private bool isLevelUp;

        public void SetNewProgress()
        {
            if (!this.previousLevelInfo.IsMaxLevel)
            {
                if (this.isLevelUp)
                {
                    this.experienceProgress.Set(this.previousProgress, 1f);
                    this.animator.SetTrigger("LevelUp");
                }
                else
                {
                    float newValue = Mathf.Clamp01(((float) this.currentLevelInfo.Experience) / ((float) this.currentLevelInfo.MaxExperience));
                    this.experienceProgress.Set(this.previousProgress, newValue);
                }
            }
        }

        protected void SetProgress(float expReward, int[] levels, LevelInfo currentLevelInfo, BattleResultsTextTemplatesComponent textTemplates)
        {
            this.currentLevelInfo = currentLevelInfo;
            this.previousLevelInfo = LevelInfo.Get(currentLevelInfo.AbsolutExperience - ((long) expReward), levels);
            if (this.previousLevelInfo.IsMaxLevel)
            {
                this.experienceProgress.Set(1f, 1f);
                this.expRewardUI.text = string.Empty;
            }
            else
            {
                this.previousProgress = Mathf.Clamp01(((float) this.previousLevelInfo.Experience) / ((float) this.previousLevelInfo.MaxExperience));
                this.experienceProgress.Set(this.previousProgress, this.previousProgress);
                this.isLevelUp = currentLevelInfo.Level > this.previousLevelInfo.Level;
                this.nextLevel = this.previousLevelInfo.Level + 1;
                this.expRewardUI.text = string.Format(textTemplates.EarnedExperienceTextTemplate, expReward);
            }
        }

        protected void SetResidualProgress()
        {
            if (!this.currentLevelInfo.IsMaxLevel)
            {
                this.experienceProgress.Set(0f, 0f);
                if (this.nextLevel >= this.currentLevelInfo.Level)
                {
                    float newValue = Mathf.Clamp01(((float) this.currentLevelInfo.Experience) / ((float) this.currentLevelInfo.MaxExperience));
                    this.experienceProgress.Set(0f, newValue);
                }
                else
                {
                    this.experienceProgress.Set(0f, 1f);
                    this.animator.SetTrigger("LevelUp");
                    this.nextLevel++;
                }
            }
        }
    }
}

