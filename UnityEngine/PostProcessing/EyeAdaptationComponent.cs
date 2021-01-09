namespace UnityEngine.PostProcessing
{
    using System;
    using UnityEngine;

    public sealed class EyeAdaptationComponent : PostProcessingComponentRenderTexture<EyeAdaptationModel>
    {
        private ComputeShader m_EyeCompute;
        private ComputeBuffer m_HistogramBuffer;
        private readonly RenderTexture[] m_AutoExposurePool = new RenderTexture[2];
        private int m_AutoExposurePingPing;
        private RenderTexture m_CurrentAutoExposure;
        private RenderTexture m_DebugHistogram;
        private static uint[] s_EmptyHistogramBuffer;
        private bool m_FirstFrame = true;
        private const int k_HistogramBins = 0x40;
        private const int k_HistogramThreadX = 0x10;
        private const int k_HistogramThreadY = 0x10;

        private Vector4 GetHistogramScaleOffsetRes()
        {
            EyeAdaptationModel.Settings settings = base.model.settings;
            float x = 1f / ((float) (settings.logMax - settings.logMin));
            return new Vector4(x, -settings.logMin * x, Mathf.Floor(((float) base.context.width) / 2f), Mathf.Floor(((float) base.context.height) / 2f));
        }

        public override void OnDisable()
        {
            foreach (RenderTexture texture in this.m_AutoExposurePool)
            {
                GraphicsUtils.Destroy(texture);
            }
            if (this.m_HistogramBuffer != null)
            {
                this.m_HistogramBuffer.Release();
            }
            this.m_HistogramBuffer = null;
            if (this.m_DebugHistogram != null)
            {
                this.m_DebugHistogram.Release();
            }
            this.m_DebugHistogram = null;
        }

        public override void OnEnable()
        {
            this.m_FirstFrame = true;
        }

        public void OnGUI()
        {
            if ((this.m_DebugHistogram != null) && this.m_DebugHistogram.IsCreated())
            {
                Rect position = new Rect((base.context.viewport.x * Screen.width) + 8f, 8f, (float) this.m_DebugHistogram.width, (float) this.m_DebugHistogram.height);
                GUI.DrawTexture(position, this.m_DebugHistogram);
            }
        }

        public Texture Prepare(RenderTexture source, Material uberMaterial)
        {
            EyeAdaptationModel.Settings settings = base.model.settings;
            if (this.m_EyeCompute == null)
            {
                this.m_EyeCompute = Resources.Load<ComputeShader>("Shaders/EyeHistogram");
            }
            Material mat = base.context.materialFactory.Get("Hidden/Post FX/Eye Adaptation");
            mat.shaderKeywords = null;
            this.m_HistogramBuffer ??= new ComputeBuffer(0x40, 4);
            s_EmptyHistogramBuffer ??= new uint[0x40];
            Vector4 histogramScaleOffsetRes = this.GetHistogramScaleOffsetRes();
            RenderTexture dest = base.context.renderTextureFactory.Get((int) histogramScaleOffsetRes.z, (int) histogramScaleOffsetRes.w, 0, source.format, RenderTextureReadWrite.Default, FilterMode.Bilinear, TextureWrapMode.Clamp, "FactoryTempTexture");
            Graphics.Blit(source, dest);
            if ((this.m_AutoExposurePool[0] == null) || !this.m_AutoExposurePool[0].IsCreated())
            {
                this.m_AutoExposurePool[0] = new RenderTexture(1, 1, 0, RenderTextureFormat.RFloat);
            }
            if ((this.m_AutoExposurePool[1] == null) || !this.m_AutoExposurePool[1].IsCreated())
            {
                this.m_AutoExposurePool[1] = new RenderTexture(1, 1, 0, RenderTextureFormat.RFloat);
            }
            this.m_HistogramBuffer.SetData(s_EmptyHistogramBuffer);
            int kernelIndex = this.m_EyeCompute.FindKernel("KEyeHistogram");
            this.m_EyeCompute.SetBuffer(kernelIndex, "_Histogram", this.m_HistogramBuffer);
            this.m_EyeCompute.SetTexture(kernelIndex, "_Source", dest);
            this.m_EyeCompute.SetVector("_ScaleOffsetRes", histogramScaleOffsetRes);
            this.m_EyeCompute.Dispatch(kernelIndex, Mathf.CeilToInt(((float) dest.width) / 16f), Mathf.CeilToInt(((float) dest.height) / 16f), 1);
            base.context.renderTextureFactory.Release(dest);
            settings.highPercent = Mathf.Clamp(settings.highPercent, 1.01f, 99f);
            settings.lowPercent = Mathf.Clamp(settings.lowPercent, 1f, settings.highPercent - 0.01f);
            mat.SetBuffer("_Histogram", this.m_HistogramBuffer);
            mat.SetVector(Uniforms._Params, new Vector4(settings.lowPercent * 0.01f, settings.highPercent * 0.01f, Mathf.Exp(settings.minLuminance * 0.6931472f), Mathf.Exp(settings.maxLuminance * 0.6931472f)));
            mat.SetVector(Uniforms._Speed, new Vector2(settings.speedDown, settings.speedUp));
            mat.SetVector(Uniforms._ScaleOffsetRes, histogramScaleOffsetRes);
            mat.SetFloat(Uniforms._ExposureCompensation, settings.keyValue);
            if (settings.dynamicKeyValue)
            {
                mat.EnableKeyword("AUTO_KEY_VALUE");
            }
            if (this.m_FirstFrame || !Application.isPlaying)
            {
                this.m_CurrentAutoExposure = this.m_AutoExposurePool[0];
                Graphics.Blit(null, this.m_CurrentAutoExposure, mat, 1);
                Graphics.Blit(this.m_AutoExposurePool[0], this.m_AutoExposurePool[1]);
            }
            else
            {
                int autoExposurePingPing = this.m_AutoExposurePingPing;
                RenderTexture texture3 = this.m_AutoExposurePool[++autoExposurePingPing % 2];
                Graphics.Blit(this.m_AutoExposurePool[++autoExposurePingPing % 2], texture3, mat, (int) settings.adaptationType);
                this.m_AutoExposurePingPing = ++autoExposurePingPing % 2;
                this.m_CurrentAutoExposure = texture3;
            }
            if (base.context.profile.debugViews.IsModeActive(BuiltinDebugViewsModel.Mode.EyeAdaptation))
            {
                if ((this.m_DebugHistogram == null) || !this.m_DebugHistogram.IsCreated())
                {
                    RenderTexture texture4 = new RenderTexture(0x100, 0x80, 0, RenderTextureFormat.ARGB32) {
                        filterMode = FilterMode.Point,
                        wrapMode = TextureWrapMode.Clamp
                    };
                    this.m_DebugHistogram = texture4;
                }
                mat.SetFloat(Uniforms._DebugWidth, (float) this.m_DebugHistogram.width);
                Graphics.Blit(null, this.m_DebugHistogram, mat, 2);
            }
            this.m_FirstFrame = false;
            return this.m_CurrentAutoExposure;
        }

        public void ResetHistory()
        {
            this.m_FirstFrame = true;
        }

        public override bool active =>
            (base.model.enabled && SystemInfo.supportsComputeShaders) && !base.context.interrupted;

        private static class Uniforms
        {
            internal static readonly int _Params = Shader.PropertyToID("_Params");
            internal static readonly int _Speed = Shader.PropertyToID("_Speed");
            internal static readonly int _ScaleOffsetRes = Shader.PropertyToID("_ScaleOffsetRes");
            internal static readonly int _ExposureCompensation = Shader.PropertyToID("_ExposureCompensation");
            internal static readonly int _AutoExposure = Shader.PropertyToID("_AutoExposure");
            internal static readonly int _DebugWidth = Shader.PropertyToID("_DebugWidth");
        }
    }
}

