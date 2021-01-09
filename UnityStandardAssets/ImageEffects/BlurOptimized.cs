namespace UnityStandardAssets.ImageEffects
{
    using System;
    using UnityEngine;

    [ExecuteInEditMode, RequireComponent(typeof(Camera)), AddComponentMenu("Image Effects/Blur/Blur (Optimized)")]
    public class BlurOptimized : PostEffectsBase
    {
        [Range(0f, 2f)]
        public int downsample = 1;
        [Range(0f, 10f)]
        public float blurSize = 3f;
        [Range(1f, 4f)]
        public int blurIterations = 2;
        public BlurType blurType;
        public Shader blurShader;
        private Material blurMaterial;

        public override bool CheckResources()
        {
            base.CheckSupport(false);
            this.blurMaterial = base.CheckShaderAndCreateMaterial(this.blurShader, this.blurMaterial);
            if (!base.isSupported)
            {
                base.ReportAutoDisable();
            }
            return base.isSupported;
        }

        public void OnDisable()
        {
            if (this.blurMaterial)
            {
                DestroyImmediate(this.blurMaterial);
            }
        }

        public void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (!this.CheckResources())
            {
                Graphics.Blit(source, destination);
            }
            else
            {
                float num = 1f / (1f * (1 << (this.downsample & 0x1f)));
                this.blurMaterial.SetVector("_Parameter", new Vector4(this.blurSize * num, -this.blurSize * num, 0f, 0f));
                source.filterMode = FilterMode.Bilinear;
                int width = source.width >> (this.downsample & 0x1f);
                int height = source.height >> (this.downsample & 0x1f);
                RenderTexture dest = RenderTexture.GetTemporary(width, height, 0, source.format);
                dest.filterMode = FilterMode.Bilinear;
                Graphics.Blit(source, dest, this.blurMaterial, 0);
                int num4 = (this.blurType != BlurType.StandardGauss) ? 2 : 0;
                for (int i = 0; i < this.blurIterations; i++)
                {
                    float num6 = i * 1f;
                    this.blurMaterial.SetVector("_Parameter", new Vector4((this.blurSize * num) + num6, (-this.blurSize * num) - num6, 0f, 0f));
                    RenderTexture texture2 = RenderTexture.GetTemporary(width, height, 0, source.format);
                    texture2.filterMode = FilterMode.Bilinear;
                    Graphics.Blit(dest, texture2, this.blurMaterial, 1 + num4);
                    RenderTexture.ReleaseTemporary(dest);
                    dest = texture2;
                    texture2 = RenderTexture.GetTemporary(width, height, 0, source.format);
                    texture2.filterMode = FilterMode.Bilinear;
                    Graphics.Blit(dest, texture2, this.blurMaterial, 2 + num4);
                    RenderTexture.ReleaseTemporary(dest);
                    dest = texture2;
                }
                Graphics.Blit(dest, destination);
                RenderTexture.ReleaseTemporary(dest);
            }
        }

        public enum BlurType
        {
            StandardGauss,
            SgxGauss
        }
    }
}

