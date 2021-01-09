namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientProfile.API;
    using UnityEngine;

    public class RankIndicator : AnimatedIndicatorWithFinishComponent<PersonalBattleResultRankIndicatorFinishedComponent>
    {
        [SerializeField]
        private ImageListSkin currentLevel;
        [SerializeField]
        private ImageListSkin nextLevel;
        [SerializeField]
        private ExperienceIndicator exp;
        private long fromExp;
        private long toExp;
        private NormalizedAnimatedValue animation;
        private int level;
        private int[] levels;

        private void Awake()
        {
            this.animation = base.GetComponent<NormalizedAnimatedValue>();
        }

        public void Init(Entity screenEntity, long fromExp, long toExp, int[] levels)
        {
            base.SetEntity(screenEntity);
            LevelInfo info = LevelInfo.Get(fromExp, levels);
            this.level = info.Level;
            this.levels = levels;
            this.fromExp = fromExp;
            this.toExp = toExp;
            this.exp.Init(info);
            this.currentLevel.SelectSprite((info.Level + 1).ToString());
            if (!info.IsMaxLevel)
            {
                this.nextLevel.SelectSprite((info.Level + 2).ToString());
            }
            base.GetComponent<Animator>().SetTrigger("Start");
        }

        public void Update()
        {
            float num = this.animation.value * (this.toExp - this.fromExp);
            LevelInfo info = LevelInfo.Get(this.fromExp + ((long) num), this.levels);
            if (info.Level != this.level)
            {
                base.GetComponent<Animator>().SetTrigger("Up");
                this.level = info.Level;
                this.exp.LevelUp();
            }
            this.exp.Change(info);
            base.TryToSetAnimationFinished((float) info.AbsolutExperience, (float) this.toExp);
        }

        private void UpdateLevel()
        {
            float num = this.animation.value * (this.toExp - this.fromExp);
            LevelInfo info = LevelInfo.Get(this.fromExp + ((long) num), this.levels);
            this.currentLevel.SelectSprite((info.Level + 1).ToString());
            if (!info.IsMaxLevel)
            {
                this.nextLevel.SelectSprite((info.Level + 2).ToString());
            }
        }
    }
}

