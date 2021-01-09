namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using System.Runtime.InteropServices;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;
    using UnityEngine.UI;

    public class SettingsSlotUIComponent : MonoBehaviour
    {
        [SerializeField]
        private ImageSkin moduleIconImageSkin;
        [SerializeField]
        private GameObject moduleIsPresent;
        [SerializeField]
        private GameObject whiteBack;
        [SerializeField]
        private Image moduleIconImage;
        [SerializeField]
        private Color activeModuleIconColor;
        [SerializeField]
        private Color inactiveModuleIconColor;

        public void SetIcon(string udid, bool moduleActive = true)
        {
            this.whiteBack.SetActive(string.IsNullOrEmpty(udid));
            this.moduleIsPresent.SetActive(!string.IsNullOrEmpty(udid));
            this.moduleIconImageSkin.SpriteUid = udid;
            this.moduleIconImage.color = !moduleActive ? this.inactiveModuleIconColor : this.activeModuleIconColor;
        }
    }
}

