namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class TimerUIComponent : MonoBehaviour
    {
        [SerializeField]
        private Text minutesText;
        [SerializeField]
        private Text secondsText;
        private float secondsLeft;
        private int previousSecondsLeft;

        private string AddLeadingZero(int seconds) => 
            ((seconds >= 10) ? string.Empty : "0") + seconds;

        public void Awake()
        {
            this.secondsLeft = 0f;
            this.previousSecondsLeft = -1;
        }

        private void UpdateTextFields()
        {
            int secondsLeft = (int) this.secondsLeft;
            if (this.ValidateSecondsChanging(secondsLeft))
            {
                if (this.minutesText == null)
                {
                    this.secondsText.text = secondsLeft.ToString();
                }
                else
                {
                    TimeSpan span = new TimeSpan(0, 0, 0, secondsLeft);
                    this.secondsText.text = this.AddLeadingZero(span.Seconds);
                    this.minutesText.text = ((int) span.TotalMinutes).ToString();
                }
            }
        }

        private bool ValidateSecondsChanging(int intSecondsLeft)
        {
            if (intSecondsLeft == this.previousSecondsLeft)
            {
                return false;
            }
            this.previousSecondsLeft = intSecondsLeft;
            return true;
        }

        public float SecondsLeft
        {
            get => 
                this.secondsLeft;
            set
            {
                this.secondsLeft = value;
                this.UpdateTextFields();
            }
        }
    }
}

