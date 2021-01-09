namespace Tanks.Lobby.ClientBattleSelect.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using UnityEngine;

    public class BattleDetailsIndicatorsComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private GameObject scoreIndicator;
        [SerializeField]
        private GameObject timeIndicator;
        [SerializeField]
        private BattleLevelsIndicatorComponent battleLevelsIndicator;
        [SerializeField]
        private LevelWarningComponent levelWarning;
        [SerializeField]
        private GameObject archivedBattleIndicator;

        public GameObject ScoreIndicator =>
            this.scoreIndicator;

        public GameObject TimeIndicator =>
            this.timeIndicator;

        public BattleLevelsIndicatorComponent BattleLevelsIndicator =>
            this.battleLevelsIndicator;

        public LevelWarningComponent LevelWarning =>
            this.levelWarning;

        public GameObject ArchivedBattleIndicator =>
            this.archivedBattleIndicator;
    }
}

