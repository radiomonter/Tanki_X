namespace UnityEngine.PostProcessing
{
    using System;
    using UnityEngine;
    using UnityEngine.Rendering;

    public sealed class ScreenSpaceReflectionComponent : PostProcessingComponentCommandBuffer<ScreenSpaceReflectionModel>
    {
        private bool k_HighlightSuppression;
        private bool k_TraceBehindObjects = true;
        private bool k_TreatBackfaceHitAsMiss;
        private bool k_BilateralUpsample = true;
        private readonly int[] m_ReflectionTextures = new int[5];

        public override CameraEvent GetCameraEvent() => 
            CameraEvent.AfterFinalPass;

        public override DepthTextureMode GetCameraFlags() => 
            DepthTextureMode.Depth;

        public override string GetName() => 
            "Screen Space Reflection";

        public override void OnEnable()
        {
            this.m_ReflectionTextures[0] = Shader.PropertyToID("_ReflectionTexture0");
            this.m_ReflectionTextures[1] = Shader.PropertyToID("_ReflectionTexture1");
            this.m_ReflectionTextures[2] = Shader.PropertyToID("_ReflectionTexture2");
            this.m_ReflectionTextures[3] = Shader.PropertyToID("_ReflectionTexture3");
            this.m_ReflectionTextures[4] = Shader.PropertyToID("_ReflectionTexture4");
        }

        public override void PopulateCommandBuffer(CommandBuffer cb)
        {
            ScreenSpaceReflectionModel.Settings settings = base.model.settings;
            Camera camera = base.context.camera;
            int num = (settings.reflection.reflectionQuality != ScreenSpaceReflectionModel.SSRResolution.High) ? 2 : 1;
            int width = base.context.width / num;
            int height = base.context.height / num;
            float x = base.context.width;
            float y = base.context.height;
            float num6 = x / 2f;
            float num7 = y / 2f;
            Material mat = base.context.materialFactory.Get("Hidden/Post FX/Screen Space Reflection");
            mat.SetInt(Uniforms._RayStepSize, settings.reflection.stepSize);
            mat.SetInt(Uniforms._AdditiveReflection, (settings.reflection.blendType != ScreenSpaceReflectionModel.SSRReflectionBlendType.Additive) ? 0 : 1);
            mat.SetInt(Uniforms._BilateralUpsampling, !this.k_BilateralUpsample ? 0 : 1);
            mat.SetInt(Uniforms._TreatBackfaceHitAsMiss, !this.k_TreatBackfaceHitAsMiss ? 0 : 1);
            mat.SetInt(Uniforms._AllowBackwardsRays, !settings.reflection.reflectBackfaces ? 0 : 1);
            mat.SetInt(Uniforms._TraceBehindObjects, !this.k_TraceBehindObjects ? 0 : 1);
            mat.SetInt(Uniforms._MaxSteps, settings.reflection.iterationCount);
            mat.SetInt(Uniforms._FullResolutionFiltering, 0);
            mat.SetInt(Uniforms._HalfResolution, (settings.reflection.reflectionQuality == ScreenSpaceReflectionModel.SSRResolution.High) ? 0 : 1);
            mat.SetInt(Uniforms._HighlightSuppression, !this.k_HighlightSuppression ? 0 : 1);
            float num8 = x / (-2f * Mathf.Tan(((camera.fieldOfView / 180f) * 3.141593f) * 0.5f));
            mat.SetFloat(Uniforms._PixelsPerMeterAtOneMeter, num8);
            mat.SetFloat(Uniforms._ScreenEdgeFading, settings.screenEdgeMask.intensity);
            mat.SetFloat(Uniforms._ReflectionBlur, settings.reflection.reflectionBlur);
            mat.SetFloat(Uniforms._MaxRayTraceDistance, settings.reflection.maxDistance);
            mat.SetFloat(Uniforms._FadeDistance, settings.intensity.fadeDistance);
            mat.SetFloat(Uniforms._LayerThickness, settings.reflection.widthModifier);
            mat.SetFloat(Uniforms._SSRMultiplier, settings.intensity.reflectionMultiplier);
            mat.SetFloat(Uniforms._FresnelFade, settings.intensity.fresnelFade);
            mat.SetFloat(Uniforms._FresnelFadePower, settings.intensity.fresnelFadePower);
            Matrix4x4 projectionMatrix = camera.projectionMatrix;
            Vector4 vector = new Vector4(-2f / (x * projectionMatrix[0]), -2f / (y * projectionMatrix[5]), (1f - projectionMatrix[2]) / projectionMatrix[0], (1f + projectionMatrix[6]) / projectionMatrix[5]);
            Vector3 vector2 = !float.IsPositiveInfinity(camera.farClipPlane) ? new Vector3(camera.nearClipPlane * camera.farClipPlane, camera.nearClipPlane - camera.farClipPlane, camera.farClipPlane) : new Vector3(camera.nearClipPlane, -1f, 1f);
            mat.SetVector(Uniforms._ReflectionBufferSize, new Vector2((float) width, (float) height));
            mat.SetVector(Uniforms._ScreenSize, new Vector2(x, y));
            mat.SetVector(Uniforms._InvScreenSize, new Vector2(1f / x, 1f / y));
            mat.SetVector(Uniforms._ProjInfo, vector);
            mat.SetVector(Uniforms._CameraClipInfo, vector2);
            Matrix4x4 matrixx2 = new Matrix4x4();
            matrixx2.SetRow(0, new Vector4(num6, 0f, 0f, num6));
            matrixx2.SetRow(1, new Vector4(0f, num7, 0f, num7));
            matrixx2.SetRow(2, new Vector4(0f, 0f, 1f, 0f));
            matrixx2.SetRow(3, new Vector4(0f, 0f, 0f, 1f));
            Matrix4x4 matrixx3 = matrixx2 * projectionMatrix;
            mat.SetMatrix(Uniforms._ProjectToPixelMatrix, matrixx3);
            mat.SetMatrix(Uniforms._WorldToCameraMatrix, camera.worldToCameraMatrix);
            mat.SetMatrix(Uniforms._CameraToWorldMatrix, camera.worldToCameraMatrix.inverse);
            RenderTextureFormat format = !base.context.isHdr ? RenderTextureFormat.ARGB32 : RenderTextureFormat.ARGBHalf;
            int nameID = Uniforms._NormalAndRoughnessTexture;
            int num10 = Uniforms._HitPointTexture;
            int num11 = Uniforms._BlurTexture;
            int num12 = Uniforms._FilteredReflections;
            int num13 = Uniforms._FinalReflectionTexture;
            int num14 = Uniforms._TempTexture;
            cb.GetTemporaryRT(nameID, -1, -1, 0, FilterMode.Point, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
            cb.GetTemporaryRT(num10, width, height, 0, FilterMode.Bilinear, RenderTextureFormat.ARGBHalf, RenderTextureReadWrite.Linear);
            for (int i = 0; i < 5; i++)
            {
                cb.GetTemporaryRT(this.m_ReflectionTextures[i], width >> (i & 0x1f), height >> (i & 0x1f), 0, FilterMode.Bilinear, format);
            }
            cb.GetTemporaryRT(num12, width, height, 0, !this.k_BilateralUpsample ? FilterMode.Bilinear : FilterMode.Point, format);
            cb.GetTemporaryRT(num13, width, height, 0, FilterMode.Point, format);
            cb.Blit(2, nameID, mat, 6);
            cb.Blit(2, num10, mat, 0);
            cb.Blit(2, num12, mat, 5);
            cb.Blit(num12, this.m_ReflectionTextures[0], mat, 8);
            for (int j = 1; j < 5; j++)
            {
                int source = this.m_ReflectionTextures[j - 1];
                int num18 = j;
                cb.GetTemporaryRT(num11, width >> (num18 & 0x1f), height >> (num18 & 0x1f), 0, FilterMode.Bilinear, format);
                cb.SetGlobalVector(Uniforms._Axis, new Vector4(1f, 0f, 0f, 0f));
                cb.SetGlobalFloat(Uniforms._CurrentMipLevel, j - 1f);
                cb.Blit(source, num11, mat, 2);
                cb.SetGlobalVector(Uniforms._Axis, new Vector4(0f, 1f, 0f, 0f));
                source = this.m_ReflectionTextures[j];
                cb.Blit(num11, source, mat, 2);
                cb.ReleaseTemporaryRT(num11);
            }
            cb.Blit(this.m_ReflectionTextures[0], num13, mat, 3);
            cb.GetTemporaryRT(num14, camera.pixelWidth, camera.pixelHeight, 0, FilterMode.Bilinear, format);
            cb.Blit(2, num14, mat, 1);
            cb.Blit(num14, 2);
            cb.ReleaseTemporaryRT(num14);
        }

        public override bool active =>
            (base.model.enabled && base.context.isGBufferAvailable) && !base.context.interrupted;

        private enum PassIndex
        {
            RayTraceStep,
            CompositeFinal,
            Blur,
            CompositeSSR,
            MinMipGeneration,
            HitPointToReflections,
            BilateralKeyPack,
            BlitDepthAsCSZ,
            PoissonBlur
        }

        private static class Uniforms
        {
            internal static readonly int _RayStepSize = Shader.PropertyToID("_RayStepSize");
            internal static readonly int _AdditiveReflection = Shader.PropertyToID("_AdditiveReflection");
            internal static readonly int _BilateralUpsampling = Shader.PropertyToID("_BilateralUpsampling");
            internal static readonly int _TreatBackfaceHitAsMiss = Shader.PropertyToID("_TreatBackfaceHitAsMiss");
            internal static readonly int _AllowBackwardsRays = Shader.PropertyToID("_AllowBackwardsRays");
            internal static readonly int _TraceBehindObjects = Shader.PropertyToID("_TraceBehindObjects");
            internal static readonly int _MaxSteps = Shader.PropertyToID("_MaxSteps");
            internal static readonly int _FullResolutionFiltering = Shader.PropertyToID("_FullResolutionFiltering");
            internal static readonly int _HalfResolution = Shader.PropertyToID("_HalfResolution");
            internal static readonly int _HighlightSuppression = Shader.PropertyToID("_HighlightSuppression");
            internal static readonly int _PixelsPerMeterAtOneMeter = Shader.PropertyToID("_PixelsPerMeterAtOneMeter");
            internal static readonly int _ScreenEdgeFading = Shader.PropertyToID("_ScreenEdgeFading");
            internal static readonly int _ReflectionBlur = Shader.PropertyToID("_ReflectionBlur");
            internal static readonly int _MaxRayTraceDistance = Shader.PropertyToID("_MaxRayTraceDistance");
            internal static readonly int _FadeDistance = Shader.PropertyToID("_FadeDistance");
            internal static readonly int _LayerThickness = Shader.PropertyToID("_LayerThickness");
            internal static readonly int _SSRMultiplier = Shader.PropertyToID("_SSRMultiplier");
            internal static readonly int _FresnelFade = Shader.PropertyToID("_FresnelFade");
            internal static readonly int _FresnelFadePower = Shader.PropertyToID("_FresnelFadePower");
            internal static readonly int _ReflectionBufferSize = Shader.PropertyToID("_ReflectionBufferSize");
            internal static readonly int _ScreenSize = Shader.PropertyToID("_ScreenSize");
            internal static readonly int _InvScreenSize = Shader.PropertyToID("_InvScreenSize");
            internal static readonly int _ProjInfo = Shader.PropertyToID("_ProjInfo");
            internal static readonly int _CameraClipInfo = Shader.PropertyToID("_CameraClipInfo");
            internal static readonly int _ProjectToPixelMatrix = Shader.PropertyToID("_ProjectToPixelMatrix");
            internal static readonly int _WorldToCameraMatrix = Shader.PropertyToID("_WorldToCameraMatrix");
            internal static readonly int _CameraToWorldMatrix = Shader.PropertyToID("_CameraToWorldMatrix");
            internal static readonly int _Axis = Shader.PropertyToID("_Axis");
            internal static readonly int _CurrentMipLevel = Shader.PropertyToID("_CurrentMipLevel");
            internal static readonly int _NormalAndRoughnessTexture = Shader.PropertyToID("_NormalAndRoughnessTexture");
            internal static readonly int _HitPointTexture = Shader.PropertyToID("_HitPointTexture");
            internal static readonly int _BlurTexture = Shader.PropertyToID("_BlurTexture");
            internal static readonly int _FilteredReflections = Shader.PropertyToID("_FilteredReflections");
            internal static readonly int _FinalReflectionTexture = Shader.PropertyToID("_FinalReflectionTexture");
            internal static readonly int _TempTexture = Shader.PropertyToID("_TempTexture");
        }
    }
}

