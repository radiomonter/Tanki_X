namespace Tanks.Lobby.ClientBattleSelect.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;
    using UnityEngine.UI;

    [SerialVersionUID(0x15114b157a7L)]
    public class BattleTimeIndicatorComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private Text timeText;
        [SerializeField]
        private ProgressBar timeProgressBar;

        public string Time
        {
            get => 
                this.timeText.text;
            set => 
                this.timeText.text = value;
        }

        public float Progress
        {
            get => 
                this.timeProgressBar.ProgressValue;
            set => 
                this.timeProgressBar.ProgressValue = value;
        }
    }
}

