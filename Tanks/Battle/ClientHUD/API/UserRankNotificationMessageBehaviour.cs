namespace Tanks.Battle.ClientHUD.API
{
    using System;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;
    using UnityEngine.UI;

    public class UserRankNotificationMessageBehaviour : BaseUserNotificationMessageBehaviour
    {
        [SerializeField]
        private Image iconImage;
        [SerializeField]
        private ImageListSkin icon;
        [SerializeField]
        private Text message;

        private void OnIconFlyReady()
        {
            base.animator.SetTrigger("TextFadeIn");
        }

        public ImageListSkin Icon =>
            this.icon;

        public Image IconImage =>
            this.iconImage;

        public Text Message =>
            this.message;
    }
}

