namespace UnityStandardAssets.ImageEffects
{
    using System;
    using UnityEngine;

    [ExecuteInEditMode, RequireComponent(typeof(Camera)), AddComponentMenu("Image Effects/Camera/Vignette and Chromatic Aberration")]
    public class VignetteAndChromaticAberration : PostEffectsBase
    {
        public AberrationMode mode;
        public float intensity = 0.375f;
        public float chromaticAberration = 0.2f;
        public float axialAberration = 0.5f;
        public float blur;
        public float blurSpread = 0.75f;
        public float luminanceDependency = 0.25f;
        public float blurDistance = 2.5f;
        public Shader vignetteShader;
        public Shader separableBlurShader;
        public Shader chromAberrationShader;
        private Material m_VignetteMaterial;
        private Material m_SeparableBlurMaterial;
        private Material m_ChromAberrationMaterial;

        public override bool CheckResources()
        {
            base.CheckSupport(false);
            this.m_VignetteMaterial = base.CheckShaderAndCreateMaterial(this.vignetteShader, this.m_VignetteMaterial);
            this.m_SeparableBlurMaterial = base.CheckShaderAndCreateMaterial(this.separableBlurShader, this.m_SeparableBlurMaterial);
            this.m_ChromAberrationMaterial = base.CheckShaderAndCreateMaterial(this.chromAberrationShader, this.m_ChromAberrationMaterial);
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
                bool flag = (Mathf.Abs(this.blur) > 0f) || (Mathf.Abs(this.intensity) > 0f);
                float num3 = (1f * width) / (1f * height);
                RenderTexture dest = null;
                RenderTexture texture2 = null;
                if (flag)
                {
                    dest = RenderTexture.GetTemporary(width, height, 0, source.format);
                    if (Mathf.Abs(this.blur) > 0f)
                    {
                        texture2 = RenderTexture.GetTemporary(width / 2, height / 2, 0, source.format);
                        Graphics.Blit(source, texture2, this.m_ChromAberrationMaterial, 0);
                        for (int i = 0; i < 2; i++)
                        {
                            this.m_SeparableBlurMaterial.SetVector("offsets", new Vector4(0f, this.blurSpread * 0.001953125f, 0f, 0f));
                            RenderTexture texture3 = RenderTexture.GetTemporary(width / 2, height / 2, 0, source.format);
                            Graphics.Blit(texture2, texture3, this.m_SeparableBlurMaterial);
                            RenderTexture.ReleaseTemporary(texture2);
                            this.m_SeparableBlurMaterial.SetVector("offsets", new Vector4((this.blurSpread * 0.001953125f) / num3, 0f, 0f, 0f));
                            texture2 = RenderTexture.GetTemporary(width / 2, height / 2, 0, source.format);
                            Graphics.Blit(texture3, texture2, this.m_SeparableBlurMaterial);
                            RenderTexture.ReleaseTemporary(texture3);
                        }
                    }
                    this.m_VignetteMaterial.SetFloat("_Intensity", this.intensity);
                    this.m_VignetteMaterial.SetFloat("_Blur", this.blur);
                    this.m_VignetteMaterial.SetTexture("_VignetteTex", texture2);
                    Graphics.Blit(source, dest, this.m_VignetteMaterial, 0);
                }
                this.m_ChromAberrationMaterial.SetFloat("_ChromaticAberration", this.chromaticAberration);
                this.m_ChromAberrationMaterial.SetFloat("_AxialAberration", this.axialAberration);
                this.m_ChromAberrationMaterial.SetVector("_BlurDistance", new Vector2(-this.blurDistance, this.blurDistance));
                this.m_ChromAberrationMaterial.SetFloat("_Luminance", 1f / Mathf.Max(Mathf.Epsilon, this.luminanceDependency));
                if (flag)
                {
                    dest.wrapMode = TextureWrapMode.Clamp;
                }
                else
                {
                    source.wrapMode = TextureWrapMode.Clamp;
                }
                Graphics.Blit(!flag ? source : dest, destination, this.m_ChromAberrationMaterial, (this.mode != AberrationMode.Advanced) ? 1 : 2);
                RenderTexture.ReleaseTemporary(dest);
                RenderTexture.ReleaseTemporary(texture2);
            }
        }

        public enum AberrationMode
        {
            Simple,
            Advanced
        }
    }
}

