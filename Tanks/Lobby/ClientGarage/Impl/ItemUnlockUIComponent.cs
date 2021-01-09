namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System.Collections.Generic;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;

    public class ItemUnlockUIComponent : MonoBehaviour, Component
    {
        public AnimatedProgress experienceProgressBar;
        public TextMeshProUGUI text;
        public GameObject rewardPrefab;
        public GameObject rewardContainer;
        public List<GameObject> rewardPreviews = new List<GameObject>();
        public LocalizedField recievedText;
        public LocalizedField levelText;
        public LocalizedField maxText;
    }
}

