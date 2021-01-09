namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class WaitingAnswerUIComponent : BehaviourComponent
    {
        [SerializeField]
        protected Slider slider;
        [SerializeField]
        protected GameObject waitingIcon;
        [SerializeField]
        protected GameObject inviteButton;
        [SerializeField]
        protected GameObject alreadyInLabel;
        public float maxTimerValue = 10f;
        private float _timer;
        private bool waiting;

        private void OnDisable()
        {
            this.slider.gameObject.SetActive(false);
            this.waitingIcon.SetActive(false);
            this.inviteButton.SetActive(false);
        }

        public void SetTimer(DateTime inviteTime)
        {
            TimeSpan span = DateTime.Now - inviteTime;
            if (span.TotalSeconds > this.maxTimerValue)
            {
                this.Waiting = false;
            }
            else
            {
                this.timer = span.Seconds;
                this.Waiting = true;
            }
        }

        private void Update()
        {
            if (this.waiting)
            {
                this.timer += Time.deltaTime;
                if (this.timer > this.maxTimerValue)
                {
                    this.Waiting = false;
                }
            }
        }

        private float timer
        {
            get => 
                this._timer;
            set
            {
                this._timer = value;
                this.slider.value = 1f - (this.timer / this.maxTimerValue);
            }
        }

        public bool Waiting
        {
            set
            {
                this.waiting = value;
                if (!this.waiting)
                {
                    this.timer = 0f;
                }
                this.slider.gameObject.SetActive(this.waiting);
                this.waitingIcon.SetActive(this.waiting);
                this.inviteButton.SetActive(!this.waiting);
                this.alreadyInLabel.SetActive(false);
            }
        }
    }
}

