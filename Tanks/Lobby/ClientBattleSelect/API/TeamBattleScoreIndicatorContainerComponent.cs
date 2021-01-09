namespace Tanks.Lobby.ClientBattleSelect.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public class TeamBattleScoreIndicatorContainerComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private GameObject TDMScoreIndicator;
        [SerializeField]
        private GameObject CTFScoreIndicator;

        public GameObject TdmScoreIndicator
        {
            get => 
                this.TDMScoreIndicator;
            set => 
                this.TDMScoreIndicator = value;
        }

        public GameObject CtfScoreIndicator
        {
            get => 
                this.CTFScoreIndicator;
            set => 
                this.CTFScoreIndicator = value;
        }
    }
}

