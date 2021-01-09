namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;

    public class TutorialContainerDialog : ConfirmDialogComponent
    {
        [SerializeField]
        private ImageSkin containerImage;
        [SerializeField]
        private TextMeshProUGUI message;
        [SerializeField]
        private TextMeshProUGUI chestCountText;

        public string Message
        {
            set => 
                this.message.text = value;
        }

        public string ConatinerImageUID
        {
            set => 
                this.containerImage.SpriteUid = value;
        }

        public int ChestCount
        {
            set
            {
                this.chestCountText.text = "x" + value;
                this.chestCountText.gameObject.SetActive(value > 0);
            }
        }
    }
}

