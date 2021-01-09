namespace Tanks.Lobby.ClientBattleSelect.API
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using TMPro;
    using UnityEngine;

    public class ScoreTableTurretIndicatorComponent : BehaviourComponent
    {
        [SerializeField]
        private TextMeshProUGUI turretIcon;

        public void SetTurretIcon(long id)
        {
            this.turretIcon.text = "<sprite name=\"" + id + "\">";
        }
    }
}

