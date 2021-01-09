namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using TMPro;
    using UnityEngine;

    public class StarterPackTimerComponent : MonoBehaviour
    {
        public TimerExpired onTimerExpired;
        [SerializeField]
        private TextMeshProUGUI timerTextLabel;
        private Date startTime;
        public bool isOn;
        private float _ticks;

        private void OnDestroy()
        {
            this.onTimerExpired = null;
        }

        public void RunTimer(float remaining)
        {
            this.startTime = Date.Now + remaining;
            this.ticks = (float) (this.startTime - Date.Now);
            this.isOn = true;
        }

        private string SecondsToHoursTimerFormat(double seconds)
        {
            int num = (int) (seconds / 60.0);
            int num2 = ((int) seconds) - (num * 60);
            int num3 = (int) (((float) num) / 60f);
            num -= num3 * 60;
            return $"{num3:0}:{num:00}:{num2:00}";
        }

        public void StopTimer()
        {
            this.ticks = 0f;
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
                string str = this.SecondsToHoursTimerFormat((double) value);
                if (!str.Equals(this.timerTextLabel.text))
                {
                    this.timerTextLabel.text = str;
                }
            }
        }

        public delegate void TimerExpired();
    }
}

