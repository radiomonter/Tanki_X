namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class UpdateRankEffectIconMaterialBehaviour : MonoBehaviour
    {
        [SerializeField]
        private Image img;
        [SerializeField]
        private Image sourceImage;
        private Material mat;
        private bool materialUpdate;
        public float opacity;

        private void Awake()
        {
            this.CopyFromSkinnedImage();
            this.mat = Instantiate<Material>(this.img.material);
            this.img.material = this.mat;
            this.materialUpdate = true;
            this.img.preserveAspect = true;
        }

        private void CopyFromSkinnedImage()
        {
            if (this.img.sprite != this.sourceImage.sprite)
            {
                this.img.sprite = this.sourceImage.sprite;
            }
            if (this.img.overrideSprite != this.sourceImage.overrideSprite)
            {
                this.img.overrideSprite = this.sourceImage.overrideSprite;
            }
            if (this.img.type != this.sourceImage.type)
            {
                this.img.type = this.sourceImage.type;
            }
        }

        private void Update()
        {
            this.CopyFromSkinnedImage();
            if (this.materialUpdate)
            {
                if (this.opacity < 1f)
                {
                    this.mat.SetFloat("_Opacity", this.opacity);
                }
                else
                {
                    this.mat.SetFloat("_Opacity", 1f);
                    this.materialUpdate = false;
                }
            }
        }
    }
}

