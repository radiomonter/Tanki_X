namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;

    public class CantUseAllEnergyBonusDialog : ConfirmWindowComponent
    {
        [SerializeField]
        private TextMeshProUGUI question;
        [SerializeField]
        private LocalizedField questionText;

        public void SetEnergyCount(long energy)
        {
            this.question.text = string.Format(this.questionText.Value, energy);
        }
    }
}

