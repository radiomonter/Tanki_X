namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using System.Text;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;

    public class TextTimerComponent : LocalizedControl
    {
        private Date endDate = new Date(float.NegativeInfinity);
        [SerializeField]
        private TextMeshProUGUI timerText;
        [SerializeField]
        private int timeUnitNumber = 2;

        private void AppendTime(StringBuilder builder, int time, string unit)
        {
            if (builder.Length > 0)
            {
                builder.Append(" ");
            }
            builder.Append(time);
            builder.Append(unit);
        }

        protected void OnDisable()
        {
            this.EndDate = new Date(float.NegativeInfinity);
        }

        private void Update()
        {
            if (float.IsInfinity(this.EndDate.UnityTime))
            {
                base.gameObject.SetActive(false);
            }
            else
            {
                float num = this.EndDate.UnityTime - Date.Now.UnityTime;
                if (num <= 0f)
                {
                    base.gameObject.SetActive(this.ActiveWhenTimeIsUp);
                    this.timerText.text = string.Empty;
                }
                else
                {
                    int num2 = 0;
                    TimeSpan span = TimeSpan.FromSeconds((double) num);
                    StringBuilder builder = new StringBuilder();
                    if ((num2 < this.timeUnitNumber) && (span.Days > 0))
                    {
                        num2++;
                        this.AppendTime(builder, span.Days, this.Day);
                    }
                    if ((num2 < this.timeUnitNumber) && (span.Hours > 0))
                    {
                        num2++;
                        this.AppendTime(builder, span.Hours, this.Hour);
                    }
                    if ((num2 < this.timeUnitNumber) && (span.Minutes > 0))
                    {
                        num2++;
                        this.AppendTime(builder, span.Minutes, this.Minute);
                    }
                    if (num2 < this.timeUnitNumber)
                    {
                        num2++;
                        this.AppendTime(builder, span.Seconds, this.Second);
                    }
                    this.timerText.text = this.TimerText.Replace("{TIME}", builder.ToString());
                }
            }
        }

        public Date EndDate
        {
            get => 
                this.endDate;
            set
            {
                this.endDate = value;
                if (!float.IsInfinity(this.endDate.UnityTime))
                {
                    base.gameObject.SetActive(true);
                }
            }
        }

        public string TimerText { private get; set; }

        public string Day { private get; set; }

        public string Hour { private get; set; }

        public string Minute { private get; set; }

        public string Second { private get; set; }

        public bool ActiveWhenTimeIsUp { private get; set; }
    }
}

