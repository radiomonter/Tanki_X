namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class NewsImageContainerComponent : UIBehaviour, Component
    {
        [SerializeField]
        private RectTransform imageContainer;
        [SerializeField]
        private AspectRatioFitter imageAspectRatioFitter;
        [SerializeField]
        private float imageAppearTime = 0.3f;
        private Graphic graphic;
        private float setImageTime;
        private float alphaBeforeAppear = float.NaN;

        private void ApplyAspectRatio(float aspectRatio)
        {
            this.imageAspectRatioFitter.aspectRatio = aspectRatio;
        }

        private void ApplyAspectRatio(Texture texture)
        {
            if (texture.height > 0)
            {
                this.ApplyAspectRatio((float) (((float) texture.width) / ((float) texture.height)));
            }
        }

        public void SetImage(Sprite sprite)
        {
            this.setImageTime = Time.time;
            Image image = this.imageContainer.gameObject.AddComponent<Image>();
            this.graphic = image;
            image.sprite = sprite;
            this.ApplyAspectRatio(sprite.texture);
        }

        public void SetImageSkin(string spriteUid, float aspectRatio)
        {
            this.setImageTime = Time.time;
            Image image = this.imageContainer.gameObject.AddComponent<Image>();
            this.graphic = image;
            this.imageContainer.gameObject.AddComponent<ImageSkin>().SpriteUid = spriteUid;
            this.ApplyAspectRatio(aspectRatio);
        }

        public void SetRawImage(Texture texture)
        {
            this.setImageTime = Time.time;
            RawImage image = this.imageContainer.gameObject.AddComponent<RawImage>();
            this.graphic = image;
            image.texture = texture;
            this.ApplyAspectRatio(texture);
        }

        private void Update()
        {
            if (this.setImageTime > 0f)
            {
                if (float.IsNaN(this.alphaBeforeAppear))
                {
                    this.alphaBeforeAppear = this.Color.a;
                }
                float num2 = Mathf.Clamp01((Time.time - this.setImageTime) / this.imageAppearTime);
                UnityEngine.Color color = this.Color;
                color.a = this.alphaBeforeAppear * num2;
                this.Color = color;
                if (num2 == 1f)
                {
                    this.setImageTime = 0f;
                }
            }
        }

        public bool FitInParent
        {
            set => 
                this.imageAspectRatioFitter.aspectMode = !value ? AspectRatioFitter.AspectMode.EnvelopeParent : AspectRatioFitter.AspectMode.FitInParent;
        }

        public UnityEngine.Color Color
        {
            get => 
                (this.graphic == null) ? UnityEngine.Color.black : this.graphic.color;
            set
            {
                if (this.graphic != null)
                {
                    this.graphic.color = value;
                }
            }
        }
    }
}

