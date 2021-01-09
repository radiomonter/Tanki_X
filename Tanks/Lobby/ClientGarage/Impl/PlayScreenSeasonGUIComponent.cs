namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;

    public class PlayScreenSeasonGUIComponent : TextTimerComponent, Component
    {
        [SerializeField]
        private LocalizedField seasonNumberTextLocalization;
        [SerializeField]
        private TextMeshProUGUI seasonNumberText;

        public void SetSeasonName(string seasonName)
        {
            this.seasonNumberText.text = seasonName;
        }

        public void SetSeasonNameFromNumber(long number)
        {
            this.seasonNumberText.text = string.Format(this.seasonNumberTextLocalization.Value, number);
        }
    }
}

