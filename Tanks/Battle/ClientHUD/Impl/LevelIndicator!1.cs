namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientProfile.API;
    using UnityEngine;
    using UnityEngine.UI;

    public class LevelIndicator<T> : AnimatedIndicatorWithFinishComponent<T> where T: Component, new()
    {
        [SerializeField]
        private ColoredProgressBar levelProgress;
        [SerializeField]
        private Text levelValue;
        [SerializeField]
        private Text deltaLevelValue;
        [SerializeField]
        private ExperienceIndicator exp;
        private long fromExp;
        private long toExp;
        private NormalizedAnimatedValue animation;
        private int level;
        private int initialLevel;
        private int[] levels;

        private void Awake()
        {
            this.animation = base.GetComponent<NormalizedAnimatedValue>();
        }

        public void Init(Entity screenEntity, long fromExp, long toExp, int[] levels)
        {
            base.SetEntity(screenEntity);
            LevelInfo info = LevelInfo.Get(fromExp, levels);
            if (info.IsMaxLevel)
            {
                base.gameObject.SetActive(false);
            }
            else
            {
                base.gameObject.SetActive(true);
                this.level = info.Level;
                this.levels = levels;
                this.fromExp = fromExp;
                this.toExp = toExp;
                this.initialLevel = info.Level;
                this.exp.Init(info);
                this.levelProgress.InitialProgress = ((float) info.Level) / ((float) levels.Length);
                this.levelProgress.ColoredProgress = this.levelProgress.InitialProgress;
                this.levelValue.text = info.Level.ToString();
                base.GetComponent<Animator>().SetTrigger("Start");
                this.deltaLevelValue.gameObject.SetActive(false);
            }
        }

        public void Update()
        {
            float num = this.animation.value * (this.toExp - this.fromExp);
            LevelInfo info = LevelInfo.Get(this.fromExp + ((long) num), this.levels);
            if (info.Level != this.level)
            {
                base.GetComponent<Animator>().SetTrigger("Up");
                this.levelValue.text = info.Level.ToString();
                this.exp.LevelUp();
                this.levelProgress.ColoredProgress = ((float) info.Level) / ((float) this.levels.Length);
                this.level = info.Level;
                this.deltaLevelValue.gameObject.SetActive(true);
                this.deltaLevelValue.text = "+" + (info.Level - this.initialLevel);
            }
            info.ClampExp();
            this.exp.Change(info);
            base.TryToSetAnimationFinished((float) info.AbsolutExperience, (float) this.toExp);
        }
    }
}

