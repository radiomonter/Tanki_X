namespace Tanks.Lobby.ClientBattleSelect.API
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using TMPro;
    using UnityEngine;

    public class ScoreTablePingIndicatorComponent : BehaviourComponent
    {
        [SerializeField]
        private TextMeshProUGUI pingCount;

        public void SetPing(int ping)
        {
            this.pingCount.text = ping.ToString();
        }
    }
}

