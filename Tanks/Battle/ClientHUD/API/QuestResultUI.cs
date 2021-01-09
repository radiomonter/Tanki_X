namespace Tanks.Battle.ClientHUD.API
{
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;

    public class QuestResultUI : MonoBehaviour
    {
        [SerializeField]
        private AnimatedDiffProgress progress;
        [SerializeField]
        private TextMeshProUGUI task;
        [SerializeField]
        private TextMeshProUGUI reward;
    }
}

