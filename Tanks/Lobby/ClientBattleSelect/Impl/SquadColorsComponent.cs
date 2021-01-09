namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using UnityEngine;

    public class SquadColorsComponent : BehaviourComponent
    {
        [SerializeField]
        private Color selfSquadColor;
        [SerializeField]
        private Color[] friendlyColor;
        [SerializeField]
        private Color[] enemyColor;

        public Color SelfSquadColor =>
            this.selfSquadColor;

        public Color[] FriendlyColor =>
            this.friendlyColor;

        public Color[] EnemyColor =>
            this.enemyColor;
    }
}

