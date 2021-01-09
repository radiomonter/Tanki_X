namespace Tanks.Lobby.ClientSettings.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using UnityEngine;

    [SerialVersionUID(0x8d2e6e0f268c01aL)]
    public class SelectLocaleScreenComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private GameObject applyButton;
        [SerializeField]
        private GameObject cancelButton;

        public void DisableButtons()
        {
            if (this.applyButton.activeSelf)
            {
                this.applyButton.SetActive(false);
                this.cancelButton.SetActive(false);
            }
        }

        public void EnableButtons()
        {
            this.applyButton.SetActive(true);
        }
    }
}

