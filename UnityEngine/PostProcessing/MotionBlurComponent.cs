namespace UnityEngine.PostProcessing
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.Rendering;

    public sealed class MotionBlurComponent : PostProcessingComponentCommandBuffer<MotionBlurModel>
    {
        private ReconstructionFilter m_ReconstructionFilter;
        private FrameBlendingFilter m_FrameBlendingFilter;
        private bool m_FirstFrame = true;

        public override CameraEvent GetCameraEvent() => 
            CameraEvent.BeforeImageEffects;

        public override DepthTextureMode GetCameraFlags() => 
            DepthTextureMode.MotionVectors | DepthTextureMode.Depth;

        public override string GetName() => 
            "Motion Blur";

        public override void OnDisable()
        {
            if (this.m_FrameBlendingFilter != null)
            {
                this.m_FrameBlendingFilter.Dispose();
            }
        }

        public override void OnEnable()
        {
            this.m_FirstFrame = true;
        }

        public override void PopulateCommandBuffer(CommandBuffer cb)
        {
            if (this.m_FirstFrame)
            {
                this.m_FirstFrame = false;
            }
            else
            {
                Material material = base.context.materialFactory.Get("Hidden/Post FX/Motion Blur");
                Material mat = base.context.materialFactory.Get("Hidden/Post FX/Blit");
                MotionBlurModel.Settings settings = base.model.settings;
                RenderTextureFormat format = !base.context.isHdr ? RenderTextureFormat.Default : RenderTextureFormat.DefaultHDR;
                int nameID = Uniforms._TempRT;
                cb.GetTemporaryRT(nameID, base.context.width, base.context.height, 0, FilterMode.Point, format);
                if ((settings.shutterAngle > 0f) && (settings.frameBlending > 0f))
                {
                    this.reconstructionFilter.ProcessImage(base.context, cb, ref settings, 2, nameID, material);
                    this.frameBlendingFilter.BlendFrames(cb, settings.frameBlending, nameID, 2, material);
                    this.frameBlendingFilter.PushFrame(cb, nameID, base.context.width, base.context.height, material);
                }
                else if (settings.shutterAngle > 0f)
                {
                    cb.SetGlobalTexture(Uniforms._MainTex, 2);
                    cb.Blit(2, nameID, mat, 0);
                    this.reconstructionFilter.ProcessImage(base.context, cb, ref settings, nameID, 2, material);
                }
                else if (settings.frameBlending > 0f)
                {
                    cb.SetGlobalTexture(Uniforms._MainTex, 2);
                    cb.Blit(2, nameID, mat, 0);
                    this.frameBlendingFilter.BlendFrames(cb, settings.frameBlending, nameID, 2, material);
                    this.frameBlendingFilter.PushFrame(cb, nameID, base.context.width, base.context.height, material);
                }
                cb.ReleaseTemporaryRT(nameID);
            }
        }

        public void ResetHistory()
        {
            if (this.m_FrameBlendingFilter != null)
            {
                this.m_FrameBlendingFilter.Dispose();
            }
            this.m_FrameBlendingFilter = null;
        }

        public ReconstructionFilter reconstructionFilter
        {
            get
            {
                this.m_ReconstructionFilter ??= new ReconstructionFilter();
                return this.m_ReconstructionFilter;
            }
        }

        public FrameBlendingFilter frameBlendingFilter
        {
            get
            {
                this.m_FrameBlendingFilter ??= new FrameBlendingFilter();
                return this.m_FrameBlendingFilter;
            }
        }

        public override bool active
        {
            get
            {
                bool flag1;
                MotionBlurModel.Settings settings = base.model.settings;
                if ((!base.model.enabled || (((settings.shutterAngle <= 0f) || !this.reconstructionFilter.IsSupported()) && (settings.frameBlending <= 0f))) || (SystemInfo.graphicsDeviceType == GraphicsDeviceType.OpenGLES2))
                {
                    flag1 = false;
                }
                else
                {
                    flag1 = !base.context.interrupted;
                }
                return flag1;
            }
        }

        public class FrameBlendingFilter
        {
            private bool m_UseCompression = CheckSupportCompression();
            private RenderTextureFormat m_RawTextureFormat = GetPreferredRenderTextureFormat();
            private Frame[] m_FrameList = new Frame[4];
            private int m_LastFrameCount;

            public void BlendFrames(CommandBuffer cb, float strength, RenderTargetIdentifier source, RenderTargetIdentifier destination, Material material)
            {
                float time = Time.time;
                Frame frameRelative = this.GetFrameRelative(-1);
                Frame frame2 = this.GetFrameRelative(-2);
                Frame frame3 = this.GetFrameRelative(-3);
                Frame frame4 = this.GetFrameRelative(-4);
                cb.SetGlobalTexture(MotionBlurComponent.Uniforms._History1LumaTex, frameRelative.lumaTexture);
                cb.SetGlobalTexture(MotionBlurComponent.Uniforms._History2LumaTex, frame2.lumaTexture);
                cb.SetGlobalTexture(MotionBlurComponent.Uniforms._History3LumaTex, frame3.lumaTexture);
                cb.SetGlobalTexture(MotionBlurComponent.Uniforms._History4LumaTex, frame4.lumaTexture);
                cb.SetGlobalTexture(MotionBlurComponent.Uniforms._History1ChromaTex, frameRelative.chromaTexture);
                cb.SetGlobalTexture(MotionBlurComponent.Uniforms._History2ChromaTex, frame2.chromaTexture);
                cb.SetGlobalTexture(MotionBlurComponent.Uniforms._History3ChromaTex, frame3.chromaTexture);
                cb.SetGlobalTexture(MotionBlurComponent.Uniforms._History4ChromaTex, frame4.chromaTexture);
                cb.SetGlobalFloat(MotionBlurComponent.Uniforms._History1Weight, frameRelative.CalculateWeight(strength, time));
                cb.SetGlobalFloat(MotionBlurComponent.Uniforms._History2Weight, frame2.CalculateWeight(strength, time));
                cb.SetGlobalFloat(MotionBlurComponent.Uniforms._History3Weight, frame3.CalculateWeight(strength, time));
                cb.SetGlobalFloat(MotionBlurComponent.Uniforms._History4Weight, frame4.CalculateWeight(strength, time));
                cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, source);
                cb.Blit(source, destination, material, !this.m_UseCompression ? 8 : 7);
            }

            private static bool CheckSupportCompression() => 
                SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.R8) && (SystemInfo.supportedRenderTargetCount > 1);

            public void Dispose()
            {
                foreach (Frame frame in this.m_FrameList)
                {
                    frame.Release();
                }
            }

            private Frame GetFrameRelative(int offset)
            {
                int index = ((Time.frameCount + this.m_FrameList.Length) + offset) % this.m_FrameList.Length;
                return this.m_FrameList[index];
            }

            private static RenderTextureFormat GetPreferredRenderTextureFormat()
            {
                RenderTextureFormat[] formatArray1 = new RenderTextureFormat[] { RenderTextureFormat.RGB565, RenderTextureFormat.ARGB1555, RenderTextureFormat.ARGB4444 };
                foreach (RenderTextureFormat format in formatArray1)
                {
                    if (SystemInfo.SupportsRenderTextureFormat(format))
                    {
                        return format;
                    }
                }
                return RenderTextureFormat.Default;
            }

            public void PushFrame(CommandBuffer cb, RenderTargetIdentifier source, int width, int height, Material material)
            {
                int frameCount = Time.frameCount;
                if (frameCount != this.m_LastFrameCount)
                {
                    int index = frameCount % this.m_FrameList.Length;
                    if (this.m_UseCompression)
                    {
                        this.m_FrameList[index].MakeRecord(cb, source, width, height, material);
                    }
                    else
                    {
                        this.m_FrameList[index].MakeRecordRaw(cb, source, width, height, this.m_RawTextureFormat);
                    }
                    this.m_LastFrameCount = frameCount;
                }
            }

            [StructLayout(LayoutKind.Sequential)]
            private struct Frame
            {
                public RenderTexture lumaTexture;
                public RenderTexture chromaTexture;
                private float m_Time;
                private RenderTargetIdentifier[] m_MRT;
                public float CalculateWeight(float strength, float currentTime)
                {
                    if (Mathf.Approximately(this.m_Time, 0f))
                    {
                        return 0f;
                    }
                    float num = Mathf.Lerp(80f, 16f, strength);
                    return Mathf.Exp((this.m_Time - currentTime) * num);
                }

                public void Release()
                {
                    if (this.lumaTexture != null)
                    {
                        RenderTexture.ReleaseTemporary(this.lumaTexture);
                    }
                    if (this.chromaTexture != null)
                    {
                        RenderTexture.ReleaseTemporary(this.chromaTexture);
                    }
                    this.lumaTexture = null;
                    this.chromaTexture = null;
                }

                public void MakeRecord(CommandBuffer cb, RenderTargetIdentifier source, int width, int height, Material material)
                {
                    this.Release();
                    this.lumaTexture = RenderTexture.GetTemporary(width, height, 0, RenderTextureFormat.R8, RenderTextureReadWrite.Linear);
                    this.chromaTexture = RenderTexture.GetTemporary(width, height, 0, RenderTextureFormat.R8, RenderTextureReadWrite.Linear);
                    this.lumaTexture.filterMode = FilterMode.Point;
                    this.chromaTexture.filterMode = FilterMode.Point;
                    this.m_MRT ??= new RenderTargetIdentifier[] { this.lumaTexture, this.chromaTexture };
                    cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, source);
                    cb.SetRenderTarget(this.m_MRT, this.lumaTexture);
                    cb.DrawMesh(GraphicsUtils.quad, Matrix4x4.identity, material, 0, 6);
                    this.m_Time = Time.time;
                }

                public void MakeRecordRaw(CommandBuffer cb, RenderTargetIdentifier source, int width, int height, RenderTextureFormat format)
                {
                    this.Release();
                    this.lumaTexture = RenderTexture.GetTemporary(width, height, 0, format);
                    this.lumaTexture.filterMode = FilterMode.Point;
                    cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, source);
                    cb.Blit(source, this.lumaTexture);
                    this.m_Time = Time.time;
                }
            }
        }

        private enum Pass
        {
            VelocitySetup,
            TileMax1,
            TileMax2,
            TileMaxV,
            NeighborMax,
            Reconstruction,
            FrameCompression,
            FrameBlendingChroma,
            FrameBlendingRaw
        }

        public class ReconstructionFilter
        {
            private RenderTextureFormat m_VectorRTFormat = RenderTextureFormat.RGHalf;
            private RenderTextureFormat m_PackedRTFormat = RenderTextureFormat.ARGB2101010;

            public ReconstructionFilter()
            {
                this.CheckTextureFormatSupport();
            }

            private void CheckTextureFormatSupport()
            {
                if (!SystemInfo.SupportsRenderTextureFormat(this.m_PackedRTFormat))
                {
                    this.m_PackedRTFormat = RenderTextureFormat.ARGB32;
                }
            }

            public bool IsSupported() => 
                SystemInfo.supportsMotionVectors;

            public void ProcessImage(PostProcessingContext context, CommandBuffer cb, ref MotionBlurModel.Settings settings, RenderTargetIdentifier source, RenderTargetIdentifier destination, Material material)
            {
                int num = (int) ((5f * context.height) / 100f);
                int num2 = (((num - 1) / 8) + 1) * 8;
                float num3 = settings.shutterAngle / 360f;
                cb.SetGlobalFloat(MotionBlurComponent.Uniforms._VelocityScale, num3);
                cb.SetGlobalFloat(MotionBlurComponent.Uniforms._MaxBlurRadius, (float) num);
                cb.SetGlobalFloat(MotionBlurComponent.Uniforms._RcpMaxBlurRadius, 1f / ((float) num));
                int nameID = MotionBlurComponent.Uniforms._VelocityTex;
                cb.GetTemporaryRT(nameID, context.width, context.height, 0, FilterMode.Point, this.m_PackedRTFormat, RenderTextureReadWrite.Linear);
                cb.Blit((Texture) null, nameID, material, 0);
                int num5 = MotionBlurComponent.Uniforms._Tile2RT;
                cb.GetTemporaryRT(num5, context.width / 2, context.height / 2, 0, FilterMode.Point, this.m_VectorRTFormat, RenderTextureReadWrite.Linear);
                cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, nameID);
                cb.Blit(nameID, num5, material, 1);
                int num6 = MotionBlurComponent.Uniforms._Tile4RT;
                cb.GetTemporaryRT(num6, context.width / 4, context.height / 4, 0, FilterMode.Point, this.m_VectorRTFormat, RenderTextureReadWrite.Linear);
                cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, num5);
                cb.Blit(num5, num6, material, 2);
                cb.ReleaseTemporaryRT(num5);
                int num7 = MotionBlurComponent.Uniforms._Tile8RT;
                cb.GetTemporaryRT(num7, context.width / 8, context.height / 8, 0, FilterMode.Point, this.m_VectorRTFormat, RenderTextureReadWrite.Linear);
                cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, num6);
                cb.Blit(num6, num7, material, 2);
                cb.ReleaseTemporaryRT(num6);
                Vector2 vector = (Vector2.one * ((((float) num2) / 8f) - 1f)) * -0.5f;
                cb.SetGlobalVector(MotionBlurComponent.Uniforms._TileMaxOffs, vector);
                cb.SetGlobalFloat(MotionBlurComponent.Uniforms._TileMaxLoop, (float) ((int) (((float) num2) / 8f)));
                int num8 = MotionBlurComponent.Uniforms._TileVRT;
                cb.GetTemporaryRT(num8, context.width / num2, context.height / num2, 0, FilterMode.Point, this.m_VectorRTFormat, RenderTextureReadWrite.Linear);
                cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, num7);
                cb.Blit(num7, num8, material, 3);
                cb.ReleaseTemporaryRT(num7);
                int num9 = MotionBlurComponent.Uniforms._NeighborMaxTex;
                cb.GetTemporaryRT(num9, context.width / num2, context.height / num2, 0, FilterMode.Point, this.m_VectorRTFormat, RenderTextureReadWrite.Linear);
                cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, num8);
                cb.Blit(num8, num9, material, 4);
                cb.ReleaseTemporaryRT(num8);
                cb.SetGlobalFloat(MotionBlurComponent.Uniforms._LoopCount, (float) Mathf.Clamp(settings.sampleCount / 2, 1, 0x40));
                cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, source);
                cb.Blit(source, destination, material, 5);
                cb.ReleaseTemporaryRT(nameID);
                cb.ReleaseTemporaryRT(num9);
            }
        }

        private static class Uniforms
        {
            internal static readonly int _VelocityScale = Shader.PropertyToID("_VelocityScale");
            internal static readonly int _MaxBlurRadius = Shader.PropertyToID("_MaxBlurRadius");
            internal static readonly int _RcpMaxBlurRadius = Shader.PropertyToID("_RcpMaxBlurRadius");
            internal static readonly int _VelocityTex = Shader.PropertyToID("_VelocityTex");
            internal static readonly int _MainTex = Shader.PropertyToID("_MainTex");
            internal static readonly int _Tile2RT = Shader.PropertyToID("_Tile2RT");
            internal static readonly int _Tile4RT = Shader.PropertyToID("_Tile4RT");
            internal static readonly int _Tile8RT = Shader.PropertyToID("_Tile8RT");
            internal static readonly int _TileMaxOffs = Shader.PropertyToID("_TileMaxOffs");
            internal static readonly int _TileMaxLoop = Shader.PropertyToID("_TileMaxLoop");
            internal static readonly int _TileVRT = Shader.PropertyToID("_TileVRT");
            internal static readonly int _NeighborMaxTex = Shader.PropertyToID("_NeighborMaxTex");
            internal static readonly int _LoopCount = Shader.PropertyToID("_LoopCount");
            internal static readonly int _TempRT = Shader.PropertyToID("_TempRT");
            internal static readonly int _History1LumaTex = Shader.PropertyToID("_History1LumaTex");
            internal static readonly int _History2LumaTex = Shader.PropertyToID("_History2LumaTex");
            internal static readonly int _History3LumaTex = Shader.PropertyToID("_History3LumaTex");
            internal static readonly int _History4LumaTex = Shader.PropertyToID("_History4LumaTex");
            internal static readonly int _History1ChromaTex = Shader.PropertyToID("_History1ChromaTex");
            internal static readonly int _History2ChromaTex = Shader.PropertyToID("_History2ChromaTex");
            internal static readonly int _History3ChromaTex = Shader.PropertyToID("_History3ChromaTex");
            internal static readonly int _History4ChromaTex = Shader.PropertyToID("_History4ChromaTex");
            internal static readonly int _History1Weight = Shader.PropertyToID("_History1Weight");
            internal static readonly int _History2Weight = Shader.PropertyToID("_History2Weight");
            internal static readonly int _History3Weight = Shader.PropertyToID("_History3Weight");
            internal static readonly int _History4Weight = Shader.PropertyToID("_History4Weight");
        }
    }
}

