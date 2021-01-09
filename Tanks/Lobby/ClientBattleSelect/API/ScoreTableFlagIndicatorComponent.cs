namespace Tanks.Lobby.ClientBattleSelect.API
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;

    public class ScoreTableFlagIndicatorComponent : BehaviourComponent
    {
        [SerializeField]
        private GameObject flagIcon;

        public void SetFlagIconActivity(bool activity)
        {
            this.flagIcon.SetActive(activity);
        }
    }
}

