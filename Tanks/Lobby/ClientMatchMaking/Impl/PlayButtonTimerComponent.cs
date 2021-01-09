namespace Tanks.Lobby.ClientMatchMaking.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;

    public class PlayButtonTimerComponent : MonoBehaviour
    {
        public TimerExpired onTimerExpired;
        [SerializeField]
        private TextMeshProUGUI timerTitleLabel;
        [SerializeField]
        private TextMeshProUGUI timerTextLabel;
        [SerializeField]
        private LocalizedField matchBeginInTitle;
        [SerializeField]
        private LocalizedField matchBeginIn;
        [SerializeField]
        private LocalizedField matchBeginingTitle;
        private Date startTime;
        private float _ticks;
        public bool isOn;
        private bool matchBeginning;

        private void OnDestroy()
        {
            this.onTimerExpired = null;
        }

        public void RunTheTimer(Date startTime, bool matchBeginnig)
        {
            this.matchBeginning = matchBeginnig;
            this.timerTitleLabel.text = !matchBeginnig ? this.matchBeginInTitle.Value : this.matchBeginingTitle.Value;
            this.startTime = startTime;
            this.ticks = (float) (startTime - Date.Now);
            this.isOn = true;
        }

        public void StopTheTimer()
        {
            this.isOn = false;
        }

        private void Update()
        {
            if (this.isOn && (this.timerTextLabel != null))
            {
                this.ticks = (float) (this.startTime - Date.Now);
                if (this.ticks <= 0f)
                {
                    this.ticks = 0f;
                    this.isOn = false;
                    if (this.onTimerExpired != null)
                    {
                        this.onTimerExpired();
                    }
                }
            }
        }

        private float ticks
        {
            get => 
                this._ticks;
            set
            {
                this._ticks = value;
                string str = (!this.matchBeginning ? (this.matchBeginIn.Value + " ") : string.Empty) + TimeToStringsConverter.SecondsToTimerFormat((double) value);
                if (!str.Equals(this.timerTextLabel.text))
                {
                    this.timerTextLabel.text = str;
                }
            }
        }

        public delegate void TimerExpired();
    }
}

