namespace UnityEngine.PostProcessing
{
    using System;
    using UnityEngine;

    public sealed class BloomComponent : PostProcessingComponentRenderTexture<BloomModel>
    {
        private const int k_MaxPyramidBlurLevel = 0x10;
        private readonly RenderTexture[] m_BlurBuffer1 = new RenderTexture[0x10];
        private readonly RenderTexture[] m_BlurBuffer2 = new RenderTexture[0x10];

        public void Prepare(RenderTexture source, Material uberMaterial, Texture autoExposure)
        {
            BloomModel.BloomSettings bloom = base.model.settings.bloom;
            BloomModel.LensDirtSettings lensDirt = base.model.settings.lensDirt;
            Material mat = base.context.materialFactory.Get("Hidden/Post FX/Bloom");
            mat.shaderKeywords = null;
            mat.SetTexture(Uniforms._AutoExposure, autoExposure);
            int width = base.context.width / 2;
            int height = base.context.height / 2;
            RenderTextureFormat format = !Application.isMobilePlatform ? RenderTextureFormat.DefaultHDR : RenderTextureFormat.Default;
            float num3 = (Mathf.Log((float) height, 2f) + bloom.radius) - 8f;
            int num4 = (int) num3;
            int num5 = Mathf.Clamp(num4, 1, 0x10);
            float thresholdLinear = bloom.thresholdLinear;
            mat.SetFloat(Uniforms._Threshold, thresholdLinear);
            float num7 = (thresholdLinear * bloom.softKnee) + 1E-05f;
            Vector3 vector = new Vector3(thresholdLinear - num7, num7 * 2f, 0.25f / num7);
            mat.SetVector(Uniforms._Curve, vector);
            mat.SetFloat(Uniforms._PrefilterOffs, !bloom.antiFlicker ? 0f : -0.5f);
            float num8 = (0.5f + num3) - num4;
            mat.SetFloat(Uniforms._SampleScale, num8);
            if (bloom.antiFlicker)
            {
                mat.EnableKeyword("ANTI_FLICKER");
            }
            RenderTexture dest = base.context.renderTextureFactory.Get(width, height, 0, format, RenderTextureReadWrite.Default, FilterMode.Bilinear, TextureWrapMode.Clamp, "FactoryTempTexture");
            Graphics.Blit(source, dest, mat, 0);
            RenderTexture texture2 = dest;
            for (int i = 0; i < num5; i++)
            {
                this.m_BlurBuffer1[i] = base.context.renderTextureFactory.Get(texture2.width / 2, texture2.height / 2, 0, format, RenderTextureReadWrite.Default, FilterMode.Bilinear, TextureWrapMode.Clamp, "FactoryTempTexture");
                Graphics.Blit(texture2, this.m_BlurBuffer1[i], mat, (i != 0) ? 2 : 1);
                texture2 = this.m_BlurBuffer1[i];
            }
            for (int j = num5 - 2; j >= 0; j--)
            {
                RenderTexture texture3 = this.m_BlurBuffer1[j];
                mat.SetTexture(Uniforms._BaseTex, texture3);
                this.m_BlurBuffer2[j] = base.context.renderTextureFactory.Get(texture3.width, texture3.height, 0, format, RenderTextureReadWrite.Default, FilterMode.Bilinear, TextureWrapMode.Clamp, "FactoryTempTexture");
                Graphics.Blit(texture2, this.m_BlurBuffer2[j], mat, 3);
                texture2 = this.m_BlurBuffer2[j];
            }
            RenderTexture texture4 = texture2;
            for (int k = 0; k < 0x10; k++)
            {
                if (this.m_BlurBuffer1[k] != null)
                {
                    base.context.renderTextureFactory.Release(this.m_BlurBuffer1[k]);
                }
                if ((this.m_BlurBuffer2[k] != null) && (this.m_BlurBuffer2[k] != texture4))
                {
                    base.context.renderTextureFactory.Release(this.m_BlurBuffer2[k]);
                }
                this.m_BlurBuffer1[k] = null;
                this.m_BlurBuffer2[k] = null;
            }
            base.context.renderTextureFactory.Release(dest);
            uberMaterial.SetTexture(Uniforms._BloomTex, texture4);
            uberMaterial.SetVector(Uniforms._Bloom_Settings, new Vector2(num8, bloom.intensity));
            if ((lensDirt.intensity <= 0f) || (lensDirt.texture == null))
            {
                uberMaterial.EnableKeyword("BLOOM");
            }
            else
            {
                uberMaterial.SetTexture(Uniforms._Bloom_DirtTex, lensDirt.texture);
                uberMaterial.SetFloat(Uniforms._Bloom_DirtIntensity, lensDirt.intensity);
                uberMaterial.EnableKeyword("BLOOM_LENS_DIRT");
            }
        }

        public override bool active =>
            (base.model.enabled && (base.model.settings.bloom.intensity > 0f)) && !base.context.interrupted;

        private static class Uniforms
        {
            internal static readonly int _AutoExposure = Shader.PropertyToID("_AutoExposure");
            internal static readonly int _Threshold = Shader.PropertyToID("_Threshold");
            internal static readonly int _Curve = Shader.PropertyToID("_Curve");
            internal static readonly int _PrefilterOffs = Shader.PropertyToID("_PrefilterOffs");
            internal static readonly int _SampleScale = Shader.PropertyToID("_SampleScale");
            internal static readonly int _BaseTex = Shader.PropertyToID("_BaseTex");
            internal static readonly int _BloomTex = Shader.PropertyToID("_BloomTex");
            internal static readonly int _Bloom_Settings = Shader.PropertyToID("_Bloom_Settings");
            internal static readonly int _Bloom_DirtTex = Shader.PropertyToID("_Bloom_DirtTex");
            internal static readonly int _Bloom_DirtIntensity = Shader.PropertyToID("_Bloom_DirtIntensity");
        }
    }
}

