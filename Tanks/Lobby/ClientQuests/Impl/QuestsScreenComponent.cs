namespace Tanks.Lobby.ClientQuests.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using UnityEngine;

    public class QuestsScreenComponent : BehaviourComponent
    {
        [SerializeField]
        private GameObject questPrefab;
        [SerializeField]
        private GameObject questCellPrefab;
        [SerializeField]
        private GameObject questsContainer;

        public GameObject QuestPrefab =>
            this.questPrefab;

        public GameObject QuestCellPrefab =>
            this.questCellPrefab;

        public GameObject QuestsContainer =>
            this.questsContainer;
    }
}

