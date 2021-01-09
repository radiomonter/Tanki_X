namespace Tanks.Lobby.ClientControls.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class ImageCropComponent : UIBehaviour, Component
    {
        [SerializeField]
        private Image image;

        protected override void OnRectTransformDimensionsChange()
        {
            if ((this.image != null) && (this.image.overrideSprite != null))
            {
                Rect rect = this.image.overrideSprite.rect;
                float num = rect.width / rect.height;
                RectTransform transform = (RectTransform) base.transform;
                ((RectTransform) this.image.transform).sizeDelta = ((transform.rect.width / transform.rect.height) >= num) ? new Vector2(transform.rect.width, transform.rect.width / num) : new Vector2(num * transform.rect.height, transform.rect.height);
            }
        }
    }
}

