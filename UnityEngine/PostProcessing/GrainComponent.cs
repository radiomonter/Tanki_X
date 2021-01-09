namespace UnityEngine.PostProcessing
{
    using System;
    using UnityEngine;

    public sealed class GrainComponent : PostProcessingComponentRenderTexture<GrainModel>
    {
        private RenderTexture m_GrainLookupRT;

        public override void OnDisable()
        {
            GraphicsUtils.Destroy(this.m_GrainLookupRT);
            this.m_GrainLookupRT = null;
        }

        public override void Prepare(Material uberMaterial)
        {
            GrainModel.Settings settings = base.model.settings;
            uberMaterial.EnableKeyword("GRAIN");
            float realtimeSinceStartup = Time.realtimeSinceStartup;
            float z = Random.value;
            float w = Random.value;
            if ((this.m_GrainLookupRT == null) || !this.m_GrainLookupRT.IsCreated())
            {
                GraphicsUtils.Destroy(this.m_GrainLookupRT);
                RenderTexture texture = new RenderTexture(0xc0, 0xc0, 0, RenderTextureFormat.ARGBHalf) {
                    filterMode = FilterMode.Bilinear,
                    wrapMode = TextureWrapMode.Repeat,
                    anisoLevel = 0,
                    name = "Grain Lookup Texture"
                };
                this.m_GrainLookupRT = texture;
                this.m_GrainLookupRT.Create();
            }
            Material mat = base.context.materialFactory.Get("Hidden/Post FX/Grain Generator");
            mat.SetFloat(Uniforms._Phase, realtimeSinceStartup / 20f);
            Graphics.Blit(null, this.m_GrainLookupRT, mat, !settings.colored ? 0 : 1);
            uberMaterial.SetTexture(Uniforms._GrainTex, this.m_GrainLookupRT);
            uberMaterial.SetVector(Uniforms._Grain_Params1, new Vector2(settings.luminanceContribution, settings.intensity * 20f));
            uberMaterial.SetVector(Uniforms._Grain_Params2, new Vector4((((float) base.context.width) / ((float) this.m_GrainLookupRT.width)) / settings.size, (((float) base.context.height) / ((float) this.m_GrainLookupRT.height)) / settings.size, z, w));
        }

        public override bool active =>
            (base.model.enabled && ((base.model.settings.intensity > 0f) && SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf))) && !base.context.interrupted;

        private static class Uniforms
        {
            internal static readonly int _Grain_Params1 = Shader.PropertyToID("_Grain_Params1");
            internal static readonly int _Grain_Params2 = Shader.PropertyToID("_Grain_Params2");
            internal static readonly int _GrainTex = Shader.PropertyToID("_GrainTex");
            internal static readonly int _Phase = Shader.PropertyToID("_Phase");
        }
    }
}

