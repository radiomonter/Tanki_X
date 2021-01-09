namespace Tanks.Lobby.ClientBattleSelect.API
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using TMPro;
    using UnityEngine;

    public class ScoreTableHullIndicatorComponent : BehaviourComponent
    {
        [SerializeField]
        private TextMeshProUGUI hullIcon;

        public void SetHullIcon(long id)
        {
            this.hullIcon.text = "<sprite name=\"" + id + "\">";
        }
    }
}

