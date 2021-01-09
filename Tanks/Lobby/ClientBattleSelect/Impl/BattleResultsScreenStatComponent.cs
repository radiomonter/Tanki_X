namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using TMPro;
    using UnityEngine;

    public class BattleResultsScreenStatComponent : BehaviourComponent
    {
        [SerializeField]
        private GameObject dmMatchDetails;
        [SerializeField]
        private GameObject teamMatchDetails;
        [SerializeField]
        private TextMeshProUGUI _battleDescription;

        public void HideMatchDetails()
        {
            this.dmMatchDetails.SetActive(false);
            this.teamMatchDetails.SetActive(false);
        }

        private void OnDisable()
        {
            this.HideMatchDetails();
        }

        public void ShowDMMatchDetails()
        {
            this.dmMatchDetails.SetActive(true);
        }

        public void ShowTeamMatchDetails()
        {
            this.teamMatchDetails.SetActive(true);
        }

        public string BattleDescription
        {
            set => 
                this._battleDescription.text = value;
        }
    }
}

