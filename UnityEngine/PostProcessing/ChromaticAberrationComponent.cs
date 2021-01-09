namespace UnityEngine.PostProcessing
{
    using System;
    using UnityEngine;

    public sealed class ChromaticAberrationComponent : PostProcessingComponentRenderTexture<ChromaticAberrationModel>
    {
        private Texture2D m_SpectrumLut;

        public override void OnDisable()
        {
            GraphicsUtils.Destroy(this.m_SpectrumLut);
            this.m_SpectrumLut = null;
        }

        public override void Prepare(Material uberMaterial)
        {
            ChromaticAberrationModel.Settings settings = base.model.settings;
            Texture2D spectralTexture = settings.spectralTexture;
            if (spectralTexture == null)
            {
                if (this.m_SpectrumLut == null)
                {
                    Texture2D textured2 = new Texture2D(3, 1, TextureFormat.RGB24, false) {
                        name = "Chromatic Aberration Spectrum Lookup",
                        filterMode = FilterMode.Bilinear,
                        wrapMode = TextureWrapMode.Clamp,
                        anisoLevel = 0,
                        hideFlags = HideFlags.DontSave
                    };
                    this.m_SpectrumLut = textured2;
                    Color[] colors = new Color[] { new Color(1f, 0f, 0f), new Color(0f, 1f, 0f), new Color(0f, 0f, 1f) };
                    this.m_SpectrumLut.SetPixels(colors);
                    this.m_SpectrumLut.Apply();
                }
                spectralTexture = this.m_SpectrumLut;
            }
            uberMaterial.EnableKeyword("CHROMATIC_ABERRATION");
            uberMaterial.SetFloat(Uniforms._ChromaticAberration_Amount, settings.intensity * 0.03f);
            uberMaterial.SetTexture(Uniforms._ChromaticAberration_Spectrum, spectralTexture);
        }

        public override bool active =>
            (base.model.enabled && (base.model.settings.intensity > 0f)) && !base.context.interrupted;

        private static class Uniforms
        {
            internal static readonly int _ChromaticAberration_Amount = Shader.PropertyToID("_ChromaticAberration_Amount");
            internal static readonly int _ChromaticAberration_Spectrum = Shader.PropertyToID("_ChromaticAberration_Spectrum");
        }
    }
}

