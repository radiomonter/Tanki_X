namespace UnityEngine.PostProcessing
{
    using System;
    using UnityEngine;
    using UnityEngine.Rendering;

    public sealed class AmbientOcclusionComponent : PostProcessingComponentCommandBuffer<AmbientOcclusionModel>
    {
        private const string k_BlitShaderString = "Hidden/Post FX/Blit";
        private const string k_ShaderString = "Hidden/Post FX/Ambient Occlusion";
        private readonly RenderTargetIdentifier[] m_MRT = new RenderTargetIdentifier[] { 10, 2 };

        public override CameraEvent GetCameraEvent() => 
            (!this.ambientOnlySupported || base.context.profile.debugViews.IsModeActive(BuiltinDebugViewsModel.Mode.AmbientOcclusion)) ? CameraEvent.BeforeImageEffectsOpaque : CameraEvent.BeforeReflections;

        public override DepthTextureMode GetCameraFlags()
        {
            DepthTextureMode none = DepthTextureMode.None;
            if (this.occlusionSource == OcclusionSource.DepthTexture)
            {
                none |= DepthTextureMode.Depth;
            }
            if (this.occlusionSource != OcclusionSource.GBuffer)
            {
                none |= DepthTextureMode.DepthNormals;
            }
            return none;
        }

        public override string GetName() => 
            "Ambient Occlusion";

        public override void PopulateCommandBuffer(CommandBuffer cb)
        {
            AmbientOcclusionModel.Settings settings = base.model.settings;
            Material mat = base.context.materialFactory.Get("Hidden/Post FX/Blit");
            Material material2 = base.context.materialFactory.Get("Hidden/Post FX/Ambient Occlusion");
            material2.shaderKeywords = null;
            material2.SetFloat(Uniforms._Intensity, settings.intensity);
            material2.SetFloat(Uniforms._Radius, settings.radius);
            material2.SetFloat(Uniforms._Downsample, !settings.downsampling ? 1f : 0.5f);
            material2.SetInt(Uniforms._SampleCount, (int) settings.sampleCount);
            int width = base.context.width;
            int height = base.context.height;
            int num3 = !settings.downsampling ? 1 : 2;
            int nameID = Uniforms._OcclusionTexture1;
            cb.GetTemporaryRT(nameID, width / num3, height / num3, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
            cb.Blit((Texture) null, nameID, material2, (int) this.occlusionSource);
            int num5 = Uniforms._OcclusionTexture2;
            cb.GetTemporaryRT(num5, width, height, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
            cb.SetGlobalTexture(Uniforms._MainTex, nameID);
            cb.Blit(nameID, num5, material2, (this.occlusionSource != OcclusionSource.GBuffer) ? 3 : 4);
            cb.ReleaseTemporaryRT(nameID);
            nameID = Uniforms._OcclusionTexture;
            cb.GetTemporaryRT(nameID, width, height, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
            cb.SetGlobalTexture(Uniforms._MainTex, num5);
            cb.Blit(num5, nameID, material2, 5);
            cb.ReleaseTemporaryRT(num5);
            if (base.context.profile.debugViews.IsModeActive(BuiltinDebugViewsModel.Mode.AmbientOcclusion))
            {
                cb.SetGlobalTexture(Uniforms._MainTex, nameID);
                cb.Blit(nameID, 2, material2, 8);
                base.context.Interrupt();
            }
            else if (this.ambientOnlySupported)
            {
                cb.SetRenderTarget(this.m_MRT, 2);
                cb.DrawMesh(GraphicsUtils.quad, Matrix4x4.identity, material2, 0, 7);
            }
            else
            {
                RenderTextureFormat format = !base.context.isHdr ? RenderTextureFormat.Default : RenderTextureFormat.DefaultHDR;
                int num6 = Uniforms._TempRT;
                cb.GetTemporaryRT(num6, base.context.width, base.context.height, 0, FilterMode.Bilinear, format);
                cb.Blit(2, num6, mat, 0);
                cb.SetGlobalTexture(Uniforms._MainTex, num6);
                cb.Blit(num6, 2, material2, 6);
                cb.ReleaseTemporaryRT(num6);
            }
            cb.ReleaseTemporaryRT(nameID);
        }

        private OcclusionSource occlusionSource =>
            (!base.context.isGBufferAvailable || base.model.settings.forceForwardCompatibility) ? ((!base.model.settings.highPrecision || (base.context.isGBufferAvailable && !base.model.settings.forceForwardCompatibility)) ? OcclusionSource.DepthNormalsTexture : OcclusionSource.DepthTexture) : OcclusionSource.GBuffer;

        private bool ambientOnlySupported =>
            (base.context.isHdr && (base.model.settings.ambientOnly && base.context.isGBufferAvailable)) && !base.model.settings.forceForwardCompatibility;

        public override bool active =>
            (base.model.enabled && (base.model.settings.intensity > 0f)) && !base.context.interrupted;

        private enum OcclusionSource
        {
            DepthTexture,
            DepthNormalsTexture,
            GBuffer
        }

        private static class Uniforms
        {
            internal static readonly int _Intensity = Shader.PropertyToID("_Intensity");
            internal static readonly int _Radius = Shader.PropertyToID("_Radius");
            internal static readonly int _Downsample = Shader.PropertyToID("_Downsample");
            internal static readonly int _SampleCount = Shader.PropertyToID("_SampleCount");
            internal static readonly int _OcclusionTexture1 = Shader.PropertyToID("_OcclusionTexture1");
            internal static readonly int _OcclusionTexture2 = Shader.PropertyToID("_OcclusionTexture2");
            internal static readonly int _OcclusionTexture = Shader.PropertyToID("_OcclusionTexture");
            internal static readonly int _MainTex = Shader.PropertyToID("_MainTex");
            internal static readonly int _TempRT = Shader.PropertyToID("_TempRT");
        }
    }
}

