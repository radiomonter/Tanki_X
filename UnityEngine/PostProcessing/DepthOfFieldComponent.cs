namespace UnityEngine.PostProcessing
{
    using System;
    using UnityEngine;

    public sealed class DepthOfFieldComponent : PostProcessingComponentRenderTexture<DepthOfFieldModel>
    {
        private const string k_ShaderString = "Hidden/Post FX/Depth Of Field";
        private RenderTexture m_CoCHistory;
        private const float k_FilmHeight = 0.024f;

        private float CalculateFocalLength()
        {
            DepthOfFieldModel.Settings settings = base.model.settings;
            return (settings.useCameraFov ? (0.012f / Mathf.Tan(0.5f * (base.context.camera.fieldOfView * 0.01745329f))) : (settings.focalLength / 1000f));
        }

        private float CalculateMaxCoCRadius(int screenHeight) => 
            Mathf.Min((float) 0.05f, (float) (((((float) base.model.settings.kernelSize) * 4f) + 6f) / ((float) screenHeight)));

        private bool CheckHistory(int width, int height) => 
            ((this.m_CoCHistory != null) && (this.m_CoCHistory.IsCreated() && (this.m_CoCHistory.width == width))) && (this.m_CoCHistory.height == height);

        public override DepthTextureMode GetCameraFlags() => 
            DepthTextureMode.Depth;

        public override void OnDisable()
        {
            if (this.m_CoCHistory != null)
            {
                RenderTexture.ReleaseTemporary(this.m_CoCHistory);
            }
            this.m_CoCHistory = null;
        }

        public void Prepare(RenderTexture source, Material uberMaterial, bool antialiasCoC, Vector2 taaJitter, float taaBlending)
        {
            DepthOfFieldModel.Settings settings = base.model.settings;
            RenderTextureFormat aRGBHalf = RenderTextureFormat.ARGBHalf;
            RenderTextureFormat format = this.SelectFormat(RenderTextureFormat.R8, RenderTextureFormat.RHalf);
            float b = this.CalculateFocalLength();
            float num2 = Mathf.Max(settings.focusDistance, b);
            float num3 = ((float) source.width) / ((float) source.height);
            float num4 = (b * b) / (((settings.aperture * (num2 - b)) * 0.024f) * 2f);
            float num5 = this.CalculateMaxCoCRadius(source.height);
            Material mat = base.context.materialFactory.Get("Hidden/Post FX/Depth Of Field");
            mat.SetFloat(Uniforms._Distance, num2);
            mat.SetFloat(Uniforms._LensCoeff, num4);
            mat.SetFloat(Uniforms._MaxCoC, num5);
            mat.SetFloat(Uniforms._RcpMaxCoC, 1f / num5);
            mat.SetFloat(Uniforms._RcpAspect, 1f / num3);
            RenderTexture dest = base.context.renderTextureFactory.Get(base.context.width, base.context.height, 0, format, RenderTextureReadWrite.Default, FilterMode.Bilinear, TextureWrapMode.Clamp, "FactoryTempTexture");
            Graphics.Blit(null, dest, mat, 0);
            if (antialiasCoC)
            {
                mat.SetTexture(Uniforms._CoCTex, dest);
                float z = !this.CheckHistory(base.context.width, base.context.height) ? 0f : taaBlending;
                mat.SetVector(Uniforms._TaaParams, new Vector3(taaJitter.x, taaJitter.y, z));
                RenderTexture texture2 = RenderTexture.GetTemporary(base.context.width, base.context.height, 0, format);
                Graphics.Blit(this.m_CoCHistory, texture2, mat, 1);
                base.context.renderTextureFactory.Release(dest);
                if (this.m_CoCHistory != null)
                {
                    RenderTexture.ReleaseTemporary(this.m_CoCHistory);
                }
                this.m_CoCHistory = dest = texture2;
            }
            RenderTexture texture3 = base.context.renderTextureFactory.Get(base.context.width / 2, base.context.height / 2, 0, aRGBHalf, RenderTextureReadWrite.Default, FilterMode.Bilinear, TextureWrapMode.Clamp, "FactoryTempTexture");
            mat.SetTexture(Uniforms._CoCTex, dest);
            Graphics.Blit(source, texture3, mat, 2);
            RenderTexture texture4 = base.context.renderTextureFactory.Get(base.context.width / 2, base.context.height / 2, 0, aRGBHalf, RenderTextureReadWrite.Default, FilterMode.Bilinear, TextureWrapMode.Clamp, "FactoryTempTexture");
            Graphics.Blit(texture3, texture4, mat, 3 + settings.kernelSize);
            Graphics.Blit(texture4, texture3, mat, 7);
            uberMaterial.SetVector(Uniforms._DepthOfFieldParams, new Vector3(num2, num4, num5));
            if (base.context.profile.debugViews.IsModeActive(BuiltinDebugViewsModel.Mode.FocusPlane))
            {
                uberMaterial.EnableKeyword("DEPTH_OF_FIELD_COC_VIEW");
                base.context.Interrupt();
            }
            else
            {
                uberMaterial.SetTexture(Uniforms._DepthOfFieldTex, texture3);
                uberMaterial.SetTexture(Uniforms._DepthOfFieldCoCTex, dest);
                uberMaterial.EnableKeyword("DEPTH_OF_FIELD");
            }
            base.context.renderTextureFactory.Release(texture4);
        }

        private RenderTextureFormat SelectFormat(RenderTextureFormat primary, RenderTextureFormat secondary) => 
            !SystemInfo.SupportsRenderTextureFormat(primary) ? (!SystemInfo.SupportsRenderTextureFormat(secondary) ? RenderTextureFormat.Default : secondary) : primary;

        public override bool active =>
            (base.model.enabled && SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf)) && !base.context.interrupted;

        private static class Uniforms
        {
            internal static readonly int _DepthOfFieldTex = Shader.PropertyToID("_DepthOfFieldTex");
            internal static readonly int _DepthOfFieldCoCTex = Shader.PropertyToID("_DepthOfFieldCoCTex");
            internal static readonly int _Distance = Shader.PropertyToID("_Distance");
            internal static readonly int _LensCoeff = Shader.PropertyToID("_LensCoeff");
            internal static readonly int _MaxCoC = Shader.PropertyToID("_MaxCoC");
            internal static readonly int _RcpMaxCoC = Shader.PropertyToID("_RcpMaxCoC");
            internal static readonly int _RcpAspect = Shader.PropertyToID("_RcpAspect");
            internal static readonly int _MainTex = Shader.PropertyToID("_MainTex");
            internal static readonly int _CoCTex = Shader.PropertyToID("_CoCTex");
            internal static readonly int _TaaParams = Shader.PropertyToID("_TaaParams");
            internal static readonly int _DepthOfFieldParams = Shader.PropertyToID("_DepthOfFieldParams");
        }
    }
}

