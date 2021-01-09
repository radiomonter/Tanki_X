namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class GraffitiPreviewComponent : BehaviourComponent
    {
        [SerializeField]
        private RawImage preview;

        public void ResetPreview()
        {
            this.preview.gameObject.SetActive(false);
        }

        public void SetPreview(Texture texture)
        {
            this.preview.texture = texture;
            this.preview.gameObject.SetActive(true);
        }
    }
}

