namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;

    public class MainVisualPropertyUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI text;
        [SerializeField]
        private AnimatedProgress progress;

        public void Set(string name, float progress)
        {
            this.progress.NormalizedValue = progress;
            this.text.text = name;
        }
    }
}

