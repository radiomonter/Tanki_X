namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;
    using UnityEngine.UI;

    public class GarageListItemContentPreviewComponent : BehaviourComponent
    {
        [SerializeField]
        private ImageSkin skin;
        [SerializeField]
        private UnityEngine.UI.Image image;
        [SerializeField]
        private Text count;

        public void SetEmptyImage()
        {
            this.skin.ResetSkin();
            this.image.enabled = false;
            this.skin.enabled = false;
        }

        public void SetImage(string spriteUid)
        {
            this.skin.SpriteUid = spriteUid;
            this.image.enabled = true;
            this.skin.enabled = true;
        }

        public UnityEngine.UI.Image Image =>
            this.image;

        public long Count
        {
            set
            {
                this.count.text = value.ToString();
                this.count.gameObject.SetActive(true);
            }
        }
    }
}

