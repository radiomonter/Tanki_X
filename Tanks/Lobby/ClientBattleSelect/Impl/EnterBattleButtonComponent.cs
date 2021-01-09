namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class EnterBattleButtonComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private Tanks.Battle.ClientCore.API.TeamColor teamColor;

        public Tanks.Battle.ClientCore.API.TeamColor TeamColor =>
            this.teamColor;
    }
}

