namespace Tanks.Lobby.ClientMatchMaking.Impl
{
    using System;
    using TMPro;
    using UnityEngine;

    public class StopWatchComponent : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI textLabel;
        private double startTicks;
        public bool isOn;

        public void RunTheStopwatch()
        {
            this.startTicks = TimeSpan.FromTicks(DateTime.Now.Ticks).TotalSeconds;
            this.isOn = true;
        }

        public void StopTheStopwatch()
        {
            this.isOn = false;
        }

        private void Update()
        {
            if (this.isOn && (this.textLabel != null))
            {
                this.ticks = TimeSpan.FromTicks(DateTime.Now.Ticks).TotalSeconds - this.startTicks;
            }
        }

        private double ticks
        {
            set
            {
                string str = TimeToStringsConverter.SecondsToTimerFormat(value);
                if (!str.Equals(this.textLabel.text))
                {
                    this.textLabel.text = str;
                }
            }
        }
    }
}

