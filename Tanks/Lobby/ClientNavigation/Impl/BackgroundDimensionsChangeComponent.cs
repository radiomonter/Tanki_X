namespace Tanks.Lobby.ClientNavigation.Impl
{
    using System;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class BackgroundDimensionsChangeComponent : UIBehaviour
    {
        [SerializeField]
        private Image backgroundImage;

        protected override void OnEnable()
        {
            this.OnRectTransformDimensionsChange();
        }

        protected override void OnRectTransformDimensionsChange()
        {
            if ((this.backgroundImage != null) && (this.backgroundImage.overrideSprite != null))
            {
                Rect rect = this.backgroundImage.overrideSprite.rect;
                float num = rect.width / rect.height;
                RectTransform transform = (RectTransform) base.transform;
                ((RectTransform) this.backgroundImage.transform).sizeDelta = ((transform.rect.width / transform.rect.height) >= num) ? new Vector2(transform.rect.width, transform.rect.width / num) : new Vector2(num * transform.rect.height, transform.rect.height);
            }
        }
    }
}

