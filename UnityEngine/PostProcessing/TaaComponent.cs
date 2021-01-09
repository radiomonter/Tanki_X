namespace UnityEngine.PostProcessing
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public sealed class TaaComponent : PostProcessingComponentRenderTexture<AntialiasingModel>
    {
        private const string k_ShaderString = "Hidden/Post FX/Temporal Anti-aliasing";
        private const int k_SampleCount = 8;
        private readonly RenderBuffer[] m_MRT = new RenderBuffer[2];
        private int m_SampleIndex;
        private bool m_ResetHistory = true;
        private RenderTexture m_HistoryTexture;

        private Vector2 GenerateRandomOffset()
        {
            Vector2 vector = new Vector2(this.GetHaltonValue(this.m_SampleIndex & 0x3ff, 2), this.GetHaltonValue(this.m_SampleIndex & 0x3ff, 3));
            if (++this.m_SampleIndex >= 8)
            {
                this.m_SampleIndex = 0;
            }
            return vector;
        }

        public override DepthTextureMode GetCameraFlags() => 
            DepthTextureMode.MotionVectors | DepthTextureMode.Depth;

        private float GetHaltonValue(int index, int radix)
        {
            float num = 0f;
            for (float i = 1f / ((float) radix); index > 0; i /= (float) radix)
            {
                num += (index % radix) * i;
                index /= radix;
            }
            return num;
        }

        private unsafe Matrix4x4 GetOrthographicProjectionMatrix(Vector2 offset)
        {
            float orthographicSize = base.context.camera.orthographicSize;
            float num2 = orthographicSize * base.context.camera.aspect;
            Vector2* vectorPtr1 = &offset;
            vectorPtr1->x *= num2 / (0.5f * base.context.width);
            Vector2* vectorPtr2 = &offset;
            vectorPtr2->y *= orthographicSize / (0.5f * base.context.height);
            return Matrix4x4.Ortho(offset.x - num2, offset.x + num2, offset.y - orthographicSize, offset.y + orthographicSize, base.context.camera.nearClipPlane, base.context.camera.farClipPlane);
        }

        private unsafe Matrix4x4 GetPerspectiveProjectionMatrix(Vector2 offset)
        {
            float num = Mathf.Tan(0.008726646f * base.context.camera.fieldOfView);
            float num2 = num * base.context.camera.aspect;
            Vector2* vectorPtr1 = &offset;
            vectorPtr1->x *= num2 / (0.5f * base.context.width);
            Vector2* vectorPtr2 = &offset;
            vectorPtr2->y *= num / (0.5f * base.context.height);
            float num3 = (offset.x - num2) * base.context.camera.nearClipPlane;
            float num4 = (offset.x + num2) * base.context.camera.nearClipPlane;
            float num5 = (offset.y + num) * base.context.camera.nearClipPlane;
            float num6 = (offset.y - num) * base.context.camera.nearClipPlane;
            return new Matrix4x4 { 
                [0, 0] = (2f * base.context.camera.nearClipPlane) / (num4 - num3),
                [0, 1] = 0f,
                [0, 2] = (num4 + num3) / (num4 - num3),
                [0, 3] = 0f,
                [1, 0] = 0f,
                [1, 1] = (2f * base.context.camera.nearClipPlane) / (num5 - num6),
                [1, 2] = (num5 + num6) / (num5 - num6),
                [1, 3] = 0f,
                [2, 0] = 0f,
                [2, 1] = 0f,
                [2, 2] = -(base.context.camera.farClipPlane + base.context.camera.nearClipPlane) / (base.context.camera.farClipPlane - base.context.camera.nearClipPlane),
                [2, 3] = -((2f * base.context.camera.farClipPlane) * base.context.camera.nearClipPlane) / (base.context.camera.farClipPlane - base.context.camera.nearClipPlane),
                [3, 0] = 0f,
                [3, 1] = 0f,
                [3, 2] = -1f,
                [3, 3] = 0f
            };
        }

        public override void OnDisable()
        {
            if (this.m_HistoryTexture != null)
            {
                RenderTexture.ReleaseTemporary(this.m_HistoryTexture);
            }
            this.m_HistoryTexture = null;
            this.m_SampleIndex = 0;
            this.ResetHistory();
        }

        public void Render(RenderTexture source, RenderTexture destination)
        {
            Material mat = base.context.materialFactory.Get("Hidden/Post FX/Temporal Anti-aliasing");
            mat.shaderKeywords = null;
            AntialiasingModel.TaaSettings taaSettings = base.model.settings.taaSettings;
            if (this.m_ResetHistory || ((this.m_HistoryTexture == null) || ((this.m_HistoryTexture.width != source.width) || (this.m_HistoryTexture.height != source.height))))
            {
                if (this.m_HistoryTexture)
                {
                    RenderTexture.ReleaseTemporary(this.m_HistoryTexture);
                }
                this.m_HistoryTexture = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);
                this.m_HistoryTexture.name = "TAA History";
                Graphics.Blit(source, this.m_HistoryTexture, mat, 2);
            }
            mat.SetVector(Uniforms._SharpenParameters, new Vector4(taaSettings.sharpen, 0f, 0f, 0f));
            mat.SetVector(Uniforms._FinalBlendParameters, new Vector4(taaSettings.stationaryBlending, taaSettings.motionBlending, 6000f, 0f));
            mat.SetTexture(Uniforms._MainTex, source);
            mat.SetTexture(Uniforms._HistoryTex, this.m_HistoryTexture);
            RenderTexture texture = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);
            texture.name = "TAA History";
            this.m_MRT[0] = destination.colorBuffer;
            this.m_MRT[1] = texture.colorBuffer;
            Graphics.SetRenderTarget(this.m_MRT, source.depthBuffer);
            GraphicsUtils.Blit(mat, !base.context.camera.orthographic ? 0 : 1);
            RenderTexture.ReleaseTemporary(this.m_HistoryTexture);
            this.m_HistoryTexture = texture;
            this.m_ResetHistory = false;
        }

        public void ResetHistory()
        {
            this.m_ResetHistory = true;
        }

        public unsafe void SetProjectionMatrix(Func<Vector2, Matrix4x4> jitteredFunc)
        {
            AntialiasingModel.TaaSettings taaSettings = base.model.settings.taaSettings;
            Vector2 offset = this.GenerateRandomOffset() * taaSettings.jitterSpread;
            base.context.camera.nonJitteredProjectionMatrix = base.context.camera.projectionMatrix;
            base.context.camera.projectionMatrix = (jitteredFunc == null) ? (!base.context.camera.orthographic ? this.GetPerspectiveProjectionMatrix(offset) : this.GetOrthographicProjectionMatrix(offset)) : jitteredFunc(offset);
            base.context.camera.useJitteredProjectionMatrixForTransparentRendering = false;
            Vector2* vectorPtr1 = &offset;
            vectorPtr1->x /= (float) base.context.width;
            Vector2* vectorPtr2 = &offset;
            vectorPtr2->y /= (float) base.context.height;
            base.context.materialFactory.Get("Hidden/Post FX/Temporal Anti-aliasing").SetVector(Uniforms._Jitter, offset);
            this.jitterVector = offset;
        }

        public override bool active =>
            (base.model.enabled && ((base.model.settings.method == AntialiasingModel.Method.Taa) && (SystemInfo.supportsMotionVectors && (SystemInfo.supportedRenderTargetCount >= 2)))) && !base.context.interrupted;

        public Vector2 jitterVector { get; private set; }

        private static class Uniforms
        {
            internal static int _Jitter = Shader.PropertyToID("_Jitter");
            internal static int _SharpenParameters = Shader.PropertyToID("_SharpenParameters");
            internal static int _FinalBlendParameters = Shader.PropertyToID("_FinalBlendParameters");
            internal static int _HistoryTex = Shader.PropertyToID("_HistoryTex");
            internal static int _MainTex = Shader.PropertyToID("_MainTex");
        }
    }
}

