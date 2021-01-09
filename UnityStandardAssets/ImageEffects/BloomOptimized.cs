namespace UnityStandardAssets.ImageEffects
{
    using System;
    using UnityEngine;

    [ExecuteInEditMode, RequireComponent(typeof(Camera)), AddComponentMenu("Image Effects/Bloom and Glow/Bloom (Optimized)")]
    public class BloomOptimized : PostEffectsBase
    {
        [Range(0f, 1.5f)]
        public float threshold = 0.25f;
        [Range(0f, 2.5f)]
        public float intensity = 0.75f;
        [Range(0.25f, 5.5f)]
        public float blurSize = 1f;
        private Resolution resolution;
        [Range(1f, 4f)]
        public int blurIterations = 1;
        public BlurType blurType;
        public Shader fastBloomShader;
        private Material fastBloomMaterial;

        public override bool CheckResources()
        {
            base.CheckSupport(false);
            this.fastBloomMaterial = base.CheckShaderAndCreateMaterial(this.fastBloomShader, this.fastBloomMaterial);
            if (!base.isSupported)
            {
                base.ReportAutoDisable();
            }
            return base.isSupported;
        }

        private void OnDisable()
        {
            if (this.fastBloomMaterial)
            {
                DestroyImmediate(this.fastBloomMaterial);
            }
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (!this.CheckResources())
            {
                Graphics.Blit(source, destination);
            }
            else
            {
                int num = (this.resolution != Resolution.Low) ? 2 : 4;
                float num2 = (this.resolution != Resolution.Low) ? 1f : 0.5f;
                this.fastBloomMaterial.SetVector("_Parameter", new Vector4(this.blurSize * num2, 0f, this.threshold, this.intensity));
                source.filterMode = FilterMode.Bilinear;
                int width = source.width / num;
                int height = source.height / num;
                RenderTexture dest = RenderTexture.GetTemporary(width, height, 0, source.format);
                dest.filterMode = FilterMode.Bilinear;
                Graphics.Blit(source, dest, this.fastBloomMaterial, 1);
                int num5 = (this.blurType != BlurType.Standard) ? 2 : 0;
                for (int i = 0; i < this.blurIterations; i++)
                {
                    this.fastBloomMaterial.SetVector("_Parameter", new Vector4((this.blurSize * num2) + (i * 1f), 0f, this.threshold, this.intensity));
                    RenderTexture texture2 = RenderTexture.GetTemporary(width, height, 0, source.format);
                    texture2.filterMode = FilterMode.Bilinear;
                    Graphics.Blit(dest, texture2, this.fastBloomMaterial, 2 + num5);
                    RenderTexture.ReleaseTemporary(dest);
                    dest = texture2;
                    texture2 = RenderTexture.GetTemporary(width, height, 0, source.format);
                    texture2.filterMode = FilterMode.Bilinear;
                    Graphics.Blit(dest, texture2, this.fastBloomMaterial, 3 + num5);
                    RenderTexture.ReleaseTemporary(dest);
                    dest = texture2;
                }
                this.fastBloomMaterial.SetTexture("_Bloom", dest);
                Graphics.Blit(source, destination, this.fastBloomMaterial, 0);
                RenderTexture.ReleaseTemporary(dest);
            }
        }

        public enum BlurType
        {
            Standard,
            Sgx
        }

        public enum Resolution
        {
            Low,
            High
        }
    }
}

