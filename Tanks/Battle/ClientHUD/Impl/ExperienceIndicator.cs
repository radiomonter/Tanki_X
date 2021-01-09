namespace Tanks.Battle.ClientHUD.Impl
{
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientProfile.API;
    using UnityEngine;
    using UnityEngine.UI;

    public class ExperienceIndicator : MonoBehaviour
    {
        [SerializeField]
        private Text expValue;
        [SerializeField]
        private Text maxExpValue;
        [SerializeField]
        private Text deltaExpValue;
        [SerializeField]
        private ColoredProgressBar progressBar;
        private long initialExp;
        private LevelInfo currentInfo = new LevelInfo(-1);

        public void Change(LevelInfo info)
        {
            if (this.currentInfo != info)
            {
                this.Set(info);
                this.progressBar.ColoredProgress = info.Progress;
                this.currentInfo = info;
            }
        }

        public void Init(LevelInfo info)
        {
            this.initialExp = info.AbsolutExperience;
            this.progressBar.InitialProgress = info.Progress;
            this.progressBar.ColoredProgress = info.Progress;
            this.Set(info);
        }

        public void LevelUp()
        {
            this.progressBar.InitialProgress = 0f;
        }

        private void Set(LevelInfo info)
        {
            this.expValue.text = info.Experience.ToStringSeparatedByThousands();
            this.maxExpValue.text = info.MaxExperience.ToStringSeparatedByThousands();
            this.deltaExpValue.text = ((info.AbsolutExperience - this.initialExp) <= 0L) ? string.Empty : ("+" + ((long) (info.AbsolutExperience - this.initialExp)).ToStringSeparatedByThousands());
        }
    }
}

