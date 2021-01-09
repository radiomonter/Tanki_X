namespace Tanks.Lobby.ClientSettings.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using TMPro;
    using UnityEngine;

    public class LocaleItemComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private TextMeshProUGUI caption;

        public void SetText(string caption, string localizedCaption)
        {
            this.caption.text = caption;
        }
    }
}

