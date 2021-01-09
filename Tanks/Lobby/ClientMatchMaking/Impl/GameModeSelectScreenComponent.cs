namespace Tanks.Lobby.ClientMatchMaking.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using TMPro;
    using UnityEngine;

    public class GameModeSelectScreenComponent : BehaviourComponent
    {
        [SerializeField]
        private GameObject gameModeItemPrefab;
        [SerializeField]
        private GameObject gameModesContainer;
        [SerializeField]
        private GameObject mainGameModeContainer;
        [SerializeField]
        private TextMeshProUGUI mmLevel;

        public GameObject GameModeItemPrefab =>
            this.gameModeItemPrefab;

        public GameObject GameModesContainer =>
            this.gameModesContainer;

        public GameObject MainGameModeContainer =>
            this.mainGameModeContainer;

        public int MMLevel
        {
            set => 
                this.mmLevel.text = value.ToString();
        }
    }
}

