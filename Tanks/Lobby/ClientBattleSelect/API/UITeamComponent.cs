namespace Tanks.Lobby.ClientBattleSelect.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class UITeamComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private Tanks.Battle.ClientCore.API.TeamColor teamColor;

        public Tanks.Battle.ClientCore.API.TeamColor TeamColor
        {
            get => 
                this.teamColor;
            set => 
                this.teamColor = value;
        }
    }
}

