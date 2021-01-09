namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;
    using UnityEngine.UI;

    public class DMBattleScoreIndicatorComponent : MonoBehaviour, Component
    {
        private const string VISIBLE_ANIMATION_PROP = "Visible";
        private const string INITIALLY_VISIBLE_ANIMATION_PROP = "InitiallyVisible";
        private const string BLINK_ANIMATION_PROP = "Blink";
        private const string NO_ANIMATION_PROP = "NoAnimation";
        private int score;
        private int scoreLimit;
        private bool limitVisible;
        private Tanks.Lobby.ClientControls.API.ProgressBar progressBar;
        [SerializeField]
        private Text scoreText;
        [SerializeField]
        private Text scoreLimitText;
        [SerializeField]
        private Animator iconAnimator;
        [SerializeField]
        private bool normallyVisible;
        [SerializeField]
        private bool noAnimation;

        public void Awake()
        {
            this.Score = 0;
            this.ScoreLimit = 0;
        }

        public void BlinkIcon()
        {
            this.iconAnimator.SetTrigger("Blink");
        }

        public void OnEnable()
        {
            this.propagateAnimationParam("Visible", this.normallyVisible);
            this.propagateAnimationParam("InitiallyVisible", this.normallyVisible);
            this.propagateAnimationParam("NoAnimation", this.noAnimation);
        }

        private Tanks.Lobby.ClientControls.API.ProgressBar ProgressBar()
        {
            if (this.progressBar == null)
            {
                this.progressBar = base.GetComponent<Tanks.Lobby.ClientControls.API.ProgressBar>();
            }
            return this.progressBar;
        }

        private void propagateAnimationParam(string paramName, bool paramValue)
        {
            this.scoreLimitText.GetComponent<Animator>().SetBool(paramName, paramValue);
        }

        public int Score
        {
            get => 
                this.score;
            set
            {
                this.score = value;
                this.scoreText.text = value.ToString();
            }
        }

        public int ScoreLimit
        {
            get => 
                this.scoreLimit;
            set
            {
                this.scoreLimit = value;
                this.scoreLimitText.text = value.ToString();
            }
        }

        public float ProgressValue
        {
            get => 
                this.ProgressBar().ProgressValue;
            set => 
                this.ProgressBar().ProgressValue = value;
        }

        public bool LimitVisible
        {
            get => 
                this.limitVisible;
            set
            {
                this.limitVisible = value;
                this.scoreLimitText.GetComponent<Animator>().SetBool("Visible", value);
            }
        }
    }
}

