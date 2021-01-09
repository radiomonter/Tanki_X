namespace UnityStandardAssets.ImageEffects
{
    using System;
    using UnityEngine;

    [ExecuteInEditMode, RequireComponent(typeof(Camera)), AddComponentMenu("Image Effects/Camera/Depth of Field (deprecated)")]
    public class DepthOfFieldDeprecated : PostEffectsBase
    {
        private static int SMOOTH_DOWNSAMPLE_PASS = 6;
        private static float BOKEH_EXTRA_BLUR = 2f;
        public Dof34QualitySetting quality = Dof34QualitySetting.OnlyBackground;
        public DofResolution resolution = DofResolution.Low;
        public bool simpleTweakMode = true;
        public float focalPoint = 1f;
        public float smoothness = 0.5f;
        public float focalZDistance;
        public float focalZStartCurve = 1f;
        public float focalZEndCurve = 1f;
        private float focalStartCurve = 2f;
        private float focalEndCurve = 2f;
        private float focalDistance01 = 0.1f;
        public Transform objectFocus;
        public float focalSize;
        public DofBlurriness bluriness = DofBlurriness.High;
        public float maxBlurSpread = 1.75f;
        public float foregroundBlurExtrude = 1.15f;
        public Shader dofBlurShader;
        private Material dofBlurMaterial;
        public Shader dofShader;
        private Material dofMaterial;
        public bool visualize;
        public BokehDestination bokehDestination = BokehDestination.Background;
        private float widthOverHeight = 1.25f;
        private float oneOverBaseSize = 0.001953125f;
        public bool bokeh;
        public bool bokehSupport = true;
        public Shader bokehShader;
        public Texture2D bokehTexture;
        public float bokehScale = 2.4f;
        public float bokehIntensity = 0.15f;
        public float bokehThresholdContrast = 0.1f;
        public float bokehThresholdLuminance = 0.55f;
        public int bokehDownsample = 1;
        private Material bokehMaterial;
        private Camera _camera;
        private RenderTexture foregroundTexture;
        private RenderTexture mediumRezWorkTexture;
        private RenderTexture finalDefocus;
        private RenderTexture lowRezWorkTexture;
        private RenderTexture bokehSource;
        private RenderTexture bokehSource2;

        private void AddBokeh(RenderTexture bokehInfo, RenderTexture tempTex, RenderTexture finalTarget)
        {
            if (this.bokehMaterial)
            {
                Mesh[] meshes = Quads.GetMeshes(tempTex.width, tempTex.height);
                RenderTexture.active = tempTex;
                GL.Clear(false, true, new Color(0f, 0f, 0f, 0f));
                GL.PushMatrix();
                GL.LoadIdentity();
                bokehInfo.filterMode = FilterMode.Point;
                float num = (bokehInfo.width * 1f) / (bokehInfo.height * 1f);
                float x = (2f / (1f * bokehInfo.width)) + (((this.bokehScale * this.maxBlurSpread) * BOKEH_EXTRA_BLUR) * this.oneOverBaseSize);
                this.bokehMaterial.SetTexture("_Source", bokehInfo);
                this.bokehMaterial.SetTexture("_MainTex", this.bokehTexture);
                this.bokehMaterial.SetVector("_ArScale", new Vector4(x, x * num, 0.5f, 0.5f * num));
                this.bokehMaterial.SetFloat("_Intensity", this.bokehIntensity);
                this.bokehMaterial.SetPass(0);
                Mesh[] meshArray2 = meshes;
                int index = 0;
                while (true)
                {
                    if (index >= meshArray2.Length)
                    {
                        GL.PopMatrix();
                        Graphics.Blit(tempTex, finalTarget, this.dofMaterial, 8);
                        bokehInfo.filterMode = FilterMode.Bilinear;
                        break;
                    }
                    Mesh mesh = meshArray2[index];
                    if (mesh)
                    {
                        Graphics.DrawMeshNow(mesh, Matrix4x4.identity);
                    }
                    index++;
                }
            }
        }

        private void AllocateTextures(bool blurForeground, RenderTexture source, int divider, int lowTexDivider)
        {
            this.foregroundTexture = null;
            if (blurForeground)
            {
                this.foregroundTexture = RenderTexture.GetTemporary(source.width, source.height, 0);
            }
            this.mediumRezWorkTexture = RenderTexture.GetTemporary(source.width / divider, source.height / divider, 0);
            this.finalDefocus = RenderTexture.GetTemporary(source.width / divider, source.height / divider, 0);
            this.lowRezWorkTexture = RenderTexture.GetTemporary(source.width / lowTexDivider, source.height / lowTexDivider, 0);
            this.bokehSource = null;
            this.bokehSource2 = null;
            if (this.bokeh)
            {
                this.bokehSource = RenderTexture.GetTemporary(source.width / (lowTexDivider * this.bokehDownsample), source.height / (lowTexDivider * this.bokehDownsample), 0, RenderTextureFormat.ARGBHalf);
                this.bokehSource2 = RenderTexture.GetTemporary(source.width / (lowTexDivider * this.bokehDownsample), source.height / (lowTexDivider * this.bokehDownsample), 0, RenderTextureFormat.ARGBHalf);
                this.bokehSource.filterMode = FilterMode.Bilinear;
                this.bokehSource2.filterMode = FilterMode.Bilinear;
                RenderTexture.active = this.bokehSource2;
                GL.Clear(false, true, new Color(0f, 0f, 0f, 0f));
            }
            source.filterMode = FilterMode.Bilinear;
            this.finalDefocus.filterMode = FilterMode.Bilinear;
            this.mediumRezWorkTexture.filterMode = FilterMode.Bilinear;
            this.lowRezWorkTexture.filterMode = FilterMode.Bilinear;
            if (this.foregroundTexture)
            {
                this.foregroundTexture.filterMode = FilterMode.Bilinear;
            }
        }

        private void Blur(RenderTexture from, RenderTexture to, DofBlurriness iterations, int blurPass, float spread)
        {
            RenderTexture temporary = RenderTexture.GetTemporary(to.width, to.height);
            if (iterations <= DofBlurriness.Low)
            {
                this.dofBlurMaterial.SetVector("offsets", new Vector4(0f, spread * this.oneOverBaseSize, 0f, 0f));
                Graphics.Blit(from, temporary, this.dofBlurMaterial, blurPass);
                this.dofBlurMaterial.SetVector("offsets", new Vector4((spread / this.widthOverHeight) * this.oneOverBaseSize, 0f, 0f, 0f));
                Graphics.Blit(temporary, to, this.dofBlurMaterial, blurPass);
            }
            else
            {
                this.BlurHex(from, to, blurPass, spread, temporary);
                if (iterations > DofBlurriness.High)
                {
                    this.dofBlurMaterial.SetVector("offsets", new Vector4(0f, spread * this.oneOverBaseSize, 0f, 0f));
                    Graphics.Blit(to, temporary, this.dofBlurMaterial, blurPass);
                    this.dofBlurMaterial.SetVector("offsets", new Vector4((spread / this.widthOverHeight) * this.oneOverBaseSize, 0f, 0f, 0f));
                    Graphics.Blit(temporary, to, this.dofBlurMaterial, blurPass);
                }
            }
            RenderTexture.ReleaseTemporary(temporary);
        }

        private void BlurFg(RenderTexture from, RenderTexture to, DofBlurriness iterations, int blurPass, float spread)
        {
            this.dofBlurMaterial.SetTexture("_TapHigh", from);
            RenderTexture temporary = RenderTexture.GetTemporary(to.width, to.height);
            if (iterations <= DofBlurriness.Low)
            {
                this.dofBlurMaterial.SetVector("offsets", new Vector4(0f, spread * this.oneOverBaseSize, 0f, 0f));
                Graphics.Blit(from, temporary, this.dofBlurMaterial, blurPass);
                this.dofBlurMaterial.SetVector("offsets", new Vector4((spread / this.widthOverHeight) * this.oneOverBaseSize, 0f, 0f, 0f));
                Graphics.Blit(temporary, to, this.dofBlurMaterial, blurPass);
            }
            else
            {
                this.BlurHex(from, to, blurPass, spread, temporary);
                if (iterations > DofBlurriness.High)
                {
                    this.dofBlurMaterial.SetVector("offsets", new Vector4(0f, spread * this.oneOverBaseSize, 0f, 0f));
                    Graphics.Blit(to, temporary, this.dofBlurMaterial, blurPass);
                    this.dofBlurMaterial.SetVector("offsets", new Vector4((spread / this.widthOverHeight) * this.oneOverBaseSize, 0f, 0f, 0f));
                    Graphics.Blit(temporary, to, this.dofBlurMaterial, blurPass);
                }
            }
            RenderTexture.ReleaseTemporary(temporary);
        }

        private void BlurHex(RenderTexture from, RenderTexture to, int blurPass, float spread, RenderTexture tmp)
        {
            this.dofBlurMaterial.SetVector("offsets", new Vector4(0f, spread * this.oneOverBaseSize, 0f, 0f));
            Graphics.Blit(from, tmp, this.dofBlurMaterial, blurPass);
            this.dofBlurMaterial.SetVector("offsets", new Vector4((spread / this.widthOverHeight) * this.oneOverBaseSize, 0f, 0f, 0f));
            Graphics.Blit(tmp, to, this.dofBlurMaterial, blurPass);
            this.dofBlurMaterial.SetVector("offsets", new Vector4((spread / this.widthOverHeight) * this.oneOverBaseSize, spread * this.oneOverBaseSize, 0f, 0f));
            Graphics.Blit(to, tmp, this.dofBlurMaterial, blurPass);
            this.dofBlurMaterial.SetVector("offsets", new Vector4((spread / this.widthOverHeight) * this.oneOverBaseSize, -spread * this.oneOverBaseSize, 0f, 0f));
            Graphics.Blit(tmp, to, this.dofBlurMaterial, blurPass);
        }

        public override bool CheckResources()
        {
            base.CheckSupport(true);
            this.dofBlurMaterial = base.CheckShaderAndCreateMaterial(this.dofBlurShader, this.dofBlurMaterial);
            this.dofMaterial = base.CheckShaderAndCreateMaterial(this.dofShader, this.dofMaterial);
            this.bokehSupport = this.bokehShader.isSupported;
            if (this.bokeh && (this.bokehSupport && this.bokehShader))
            {
                this.bokehMaterial = base.CheckShaderAndCreateMaterial(this.bokehShader, this.bokehMaterial);
            }
            if (!base.isSupported)
            {
                base.ReportAutoDisable();
            }
            return base.isSupported;
        }

        private void CreateMaterials()
        {
            this.dofBlurMaterial = base.CheckShaderAndCreateMaterial(this.dofBlurShader, this.dofBlurMaterial);
            this.dofMaterial = base.CheckShaderAndCreateMaterial(this.dofShader, this.dofMaterial);
            this.bokehSupport = this.bokehShader.isSupported;
            if (this.bokeh && (this.bokehSupport && this.bokehShader))
            {
                this.bokehMaterial = base.CheckShaderAndCreateMaterial(this.bokehShader, this.bokehMaterial);
            }
        }

        private void Downsample(RenderTexture from, RenderTexture to)
        {
            this.dofMaterial.SetVector("_InvRenderTargetSize", new Vector4(1f / (1f * to.width), 1f / (1f * to.height), 0f, 0f));
            Graphics.Blit(from, to, this.dofMaterial, SMOOTH_DOWNSAMPLE_PASS);
        }

        private float FocalDistance01(float worldDist) => 
            this._camera.WorldToViewportPoint(((Vector3) ((worldDist - this._camera.nearClipPlane) * this._camera.transform.forward)) + this._camera.transform.position).z / (this._camera.farClipPlane - this._camera.nearClipPlane);

        private int GetDividerBasedOnQuality()
        {
            int num = 1;
            if (this.resolution == DofResolution.Medium)
            {
                num = 2;
            }
            else if (this.resolution == DofResolution.Low)
            {
                num = 2;
            }
            return num;
        }

        private int GetLowResolutionDividerBasedOnQuality(int baseDivider)
        {
            int num = baseDivider;
            if (this.resolution == DofResolution.High)
            {
                num *= 2;
            }
            if (this.resolution == DofResolution.Low)
            {
                num *= 2;
            }
            return num;
        }

        private void OnDisable()
        {
            Quads.Cleanup();
        }

        private void OnEnable()
        {
            this._camera = base.GetComponent<Camera>();
            this._camera.depthTextureMode |= DepthTextureMode.Depth;
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (!this.CheckResources())
            {
                Graphics.Blit(source, destination);
            }
            else
            {
                if (this.smoothness < 0.1f)
                {
                    this.smoothness = 0.1f;
                }
                this.bokeh = this.bokeh && this.bokehSupport;
                float num = !this.bokeh ? 1f : BOKEH_EXTRA_BLUR;
                bool blurForeground = this.quality > Dof34QualitySetting.OnlyBackground;
                float num2 = this.focalSize / (this._camera.farClipPlane - this._camera.nearClipPlane);
                if (this.simpleTweakMode)
                {
                    this.focalDistance01 = !this.objectFocus ? this.FocalDistance01(this.focalPoint) : (this._camera.WorldToViewportPoint(this.objectFocus.position).z / this._camera.farClipPlane);
                    this.focalStartCurve = this.focalDistance01 * this.smoothness;
                    this.focalEndCurve = this.focalStartCurve;
                    blurForeground = blurForeground && (this.focalPoint > (this._camera.nearClipPlane + Mathf.Epsilon));
                }
                else
                {
                    if (!this.objectFocus)
                    {
                        this.focalDistance01 = this.FocalDistance01(this.focalZDistance);
                    }
                    else
                    {
                        Vector3 vector2 = this._camera.WorldToViewportPoint(this.objectFocus.position);
                        vector2.z /= this._camera.farClipPlane;
                        this.focalDistance01 = vector2.z;
                    }
                    this.focalStartCurve = this.focalZStartCurve;
                    this.focalEndCurve = this.focalZEndCurve;
                    blurForeground = blurForeground && (this.focalPoint > (this._camera.nearClipPlane + Mathf.Epsilon));
                }
                this.widthOverHeight = (1f * source.width) / (1f * source.height);
                this.oneOverBaseSize = 0.001953125f;
                this.dofMaterial.SetFloat("_ForegroundBlurExtrude", this.foregroundBlurExtrude);
                this.dofMaterial.SetVector("_CurveParams", new Vector4(!this.simpleTweakMode ? this.focalStartCurve : (1f / this.focalStartCurve), !this.simpleTweakMode ? this.focalEndCurve : (1f / this.focalEndCurve), num2 * 0.5f, this.focalDistance01));
                this.dofMaterial.SetVector("_InvRenderTargetSize", new Vector4(1f / (1f * source.width), 1f / (1f * source.height), 0f, 0f));
                int dividerBasedOnQuality = this.GetDividerBasedOnQuality();
                this.AllocateTextures(blurForeground, source, dividerBasedOnQuality, this.GetLowResolutionDividerBasedOnQuality(dividerBasedOnQuality));
                Graphics.Blit(source, source, this.dofMaterial, 3);
                this.Downsample(source, this.mediumRezWorkTexture);
                this.Blur(this.mediumRezWorkTexture, this.mediumRezWorkTexture, DofBlurriness.Low, 4, this.maxBlurSpread);
                if (!this.bokeh || ((BokehDestination.Foreground & this.bokehDestination) == ((BokehDestination) 0)))
                {
                    this.Downsample(this.mediumRezWorkTexture, this.lowRezWorkTexture);
                    this.Blur(this.lowRezWorkTexture, this.lowRezWorkTexture, this.bluriness, 0, this.maxBlurSpread);
                }
                else
                {
                    this.dofMaterial.SetVector("_Threshhold", new Vector4(this.bokehThresholdContrast, this.bokehThresholdLuminance, 0.95f, 0f));
                    Graphics.Blit(this.mediumRezWorkTexture, this.bokehSource2, this.dofMaterial, 11);
                    Graphics.Blit(this.mediumRezWorkTexture, this.lowRezWorkTexture);
                    this.Blur(this.lowRezWorkTexture, this.lowRezWorkTexture, this.bluriness, 0, this.maxBlurSpread * num);
                }
                this.dofBlurMaterial.SetTexture("_TapLow", this.lowRezWorkTexture);
                this.dofBlurMaterial.SetTexture("_TapMedium", this.mediumRezWorkTexture);
                Graphics.Blit(null, this.finalDefocus, this.dofBlurMaterial, 3);
                if (this.bokeh && ((BokehDestination.Foreground & this.bokehDestination) != ((BokehDestination) 0)))
                {
                    this.AddBokeh(this.bokehSource2, this.bokehSource, this.finalDefocus);
                }
                this.dofMaterial.SetTexture("_TapLowBackground", this.finalDefocus);
                this.dofMaterial.SetTexture("_TapMedium", this.mediumRezWorkTexture);
                Graphics.Blit(source, !blurForeground ? destination : this.foregroundTexture, this.dofMaterial, !this.visualize ? 0 : 2);
                if (blurForeground)
                {
                    Graphics.Blit(this.foregroundTexture, source, this.dofMaterial, 5);
                    this.Downsample(source, this.mediumRezWorkTexture);
                    this.BlurFg(this.mediumRezWorkTexture, this.mediumRezWorkTexture, DofBlurriness.Low, 2, this.maxBlurSpread);
                    if (!this.bokeh || ((BokehDestination.Foreground & this.bokehDestination) == ((BokehDestination) 0)))
                    {
                        this.BlurFg(this.mediumRezWorkTexture, this.lowRezWorkTexture, this.bluriness, 1, this.maxBlurSpread);
                    }
                    else
                    {
                        this.dofMaterial.SetVector("_Threshhold", new Vector4(this.bokehThresholdContrast * 0.5f, this.bokehThresholdLuminance, 0f, 0f));
                        Graphics.Blit(this.mediumRezWorkTexture, this.bokehSource2, this.dofMaterial, 11);
                        Graphics.Blit(this.mediumRezWorkTexture, this.lowRezWorkTexture);
                        this.BlurFg(this.lowRezWorkTexture, this.lowRezWorkTexture, this.bluriness, 1, this.maxBlurSpread * num);
                    }
                    Graphics.Blit(this.lowRezWorkTexture, this.finalDefocus);
                    this.dofMaterial.SetTexture("_TapLowForeground", this.finalDefocus);
                    Graphics.Blit(source, destination, this.dofMaterial, !this.visualize ? 4 : 1);
                    if (this.bokeh && ((BokehDestination.Foreground & this.bokehDestination) != ((BokehDestination) 0)))
                    {
                        this.AddBokeh(this.bokehSource2, this.bokehSource, destination);
                    }
                }
                this.ReleaseTextures();
            }
        }

        private void ReleaseTextures()
        {
            if (this.foregroundTexture)
            {
                RenderTexture.ReleaseTemporary(this.foregroundTexture);
            }
            if (this.finalDefocus)
            {
                RenderTexture.ReleaseTemporary(this.finalDefocus);
            }
            if (this.mediumRezWorkTexture)
            {
                RenderTexture.ReleaseTemporary(this.mediumRezWorkTexture);
            }
            if (this.lowRezWorkTexture)
            {
                RenderTexture.ReleaseTemporary(this.lowRezWorkTexture);
            }
            if (this.bokehSource)
            {
                RenderTexture.ReleaseTemporary(this.bokehSource);
            }
            if (this.bokehSource2)
            {
                RenderTexture.ReleaseTemporary(this.bokehSource2);
            }
        }

        public enum BokehDestination
        {
            Background = 1,
            Foreground = 2,
            BackgroundAndForeground = 3
        }

        public enum Dof34QualitySetting
        {
            OnlyBackground = 1,
            BackgroundAndForeground = 2
        }

        public enum DofBlurriness
        {
            Low = 1,
            High = 2,
            VeryHigh = 4
        }

        public enum DofResolution
        {
            High = 2,
            Medium = 3,
            Low = 4
        }
    }
}

