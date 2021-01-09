namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using System;
    using Tanks.Lobby.ClientNavigation.API;
    using UnityEngine;
    using UnityEngine.UI;

    public class ChangeUserEmailScreenComponent : LocalizedScreenComponent
    {
        [SerializeField]
        private Text sendButtonText;
        [SerializeField]
        private Text rightPanelHint;

        public void DeactivateHint()
        {
            this.rightPanelHint.gameObject.SetActive(false);
        }

        public void OnEnable()
        {
            this.rightPanelHint.gameObject.SetActive(true);
        }

        public string SendButton
        {
            set => 
                this.sendButtonText.text = value.ToUpper();
        }

        public string RightPanelHint
        {
            set => 
                this.rightPanelHint.text = value;
        }
    }
}

