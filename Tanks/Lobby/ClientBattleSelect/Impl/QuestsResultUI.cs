namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public class QuestsResultUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject resultsContainer;
        [SerializeField]
        private GameObject questResultPrefab;

        public void AddQuest(Entity quest)
        {
            Instantiate<GameObject>(this.questResultPrefab).transform.SetParent(this.resultsContainer.transform, false);
        }
    }
}

