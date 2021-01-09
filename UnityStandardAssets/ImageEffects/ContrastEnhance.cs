namespace UnityStandardAssets.ImageEffects
{
    using System;
    using UnityEngine;

    [ExecuteInEditMode, RequireComponent(typeof(Camera)), AddComponentMenu("Image Effects/Color Adjustments/Contrast Enhance (Unsharp Mask)")]
    internal class ContrastEnhance : PostEffectsBase
    {
        public float intensity = 0.5f;
        public float threshold;
        private Material separableBlurMaterial;
        private Material contrastCompositeMaterial;
        public float blurSpread = 1f;
        public Shader separableBlurShader;
        public Shader contrastCompositeShader;

        public override bool CheckResources()
        {
            base.CheckSupport(false);
            this.contrastCompositeMaterial = base.CheckShaderAndCreateMaterial(this.contrastCompositeShader, this.contrastCompositeMaterial);
            this.separableBlurMaterial = base.CheckShaderAndCreateMaterial(this.separableBlurShader, this.separableBlurMaterial);
            if (!base.isSupported)
            {
                base.ReportAutoDisable();
            }
            return base.isSupported;
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (!this.CheckResources())
            {
                Graphics.Blit(source, destination);
            }
            else
            {
                int width = source.width;
                int height = source.height;
                RenderTexture dest = RenderTexture.GetTemporary(width / 2, height / 2, 0);
                Graphics.Blit(source, dest);
                RenderTexture texture2 = RenderTexture.GetTemporary(width / 4, height / 4, 0);
                Graphics.Blit(dest, texture2);
                RenderTexture.ReleaseTemporary(dest);
                this.separableBlurMaterial.SetVector("offsets", new Vector4(0f, (this.blurSpread * 1f) / ((float) texture2.height), 0f, 0f));
                RenderTexture texture3 = RenderTexture.GetTemporary(width / 4, height / 4, 0);
                Graphics.Blit(texture2, texture3, this.separableBlurMaterial);
                RenderTexture.ReleaseTemporary(texture2);
                this.separableBlurMaterial.SetVector("offsets", new Vector4((this.blurSpread * 1f) / ((float) texture2.width), 0f, 0f, 0f));
                texture2 = RenderTexture.GetTemporary(width / 4, height / 4, 0);
                Graphics.Blit(texture3, texture2, this.separableBlurMaterial);
                RenderTexture.ReleaseTemporary(texture3);
                this.contrastCompositeMaterial.SetTexture("_MainTexBlurred", texture2);
                this.contrastCompositeMaterial.SetFloat("intensity", this.intensity);
                this.contrastCompositeMaterial.SetFloat("threshhold", this.threshold);
                Graphics.Blit(source, destination, this.contrastCompositeMaterial);
                RenderTexture.ReleaseTemporary(texture2);
            }
        }
    }
}

