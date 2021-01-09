namespace UnityStandardAssets.ImageEffects
{
    using System;
    using UnityEngine;

    [ExecuteInEditMode, RequireComponent(typeof(Camera)), AddComponentMenu("Image Effects/Camera/Depth of Field (Lens Blur, Scatter, DX11)")]
    public class DepthOfField : PostEffectsBase
    {
        public bool visualizeFocus;
        public float focalLength = 10f;
        public float focalSize = 0.05f;
        public float aperture = 11.5f;
        public Transform focalTransform;
        public float maxBlurSize = 2f;
        public bool highResolution;
        public BlurType blurType;
        public BlurSampleCount blurSampleCount = BlurSampleCount.High;
        public bool nearBlur;
        public float foregroundOverlap = 1f;
        public Shader dofHdrShader;
        private Material dofHdrMaterial;
        public Shader dx11BokehShader;
        private Material dx11bokehMaterial;
        public float dx11BokehThreshold = 0.5f;
        public float dx11SpawnHeuristic = 0.0875f;
        public Texture2D dx11BokehTexture;
        public float dx11BokehScale = 1.2f;
        public float dx11BokehIntensity = 2.5f;
        private float focalDistance01 = 10f;
        private ComputeBuffer cbDrawArgs;
        private ComputeBuffer cbPoints;
        private float internalBlurWidth = 1f;

        public override bool CheckResources()
        {
            base.CheckSupport(true);
            this.dofHdrMaterial = base.CheckShaderAndCreateMaterial(this.dofHdrShader, this.dofHdrMaterial);
            if (base.supportDX11 && (this.blurType == BlurType.DX11))
            {
                this.dx11bokehMaterial = base.CheckShaderAndCreateMaterial(this.dx11BokehShader, this.dx11bokehMaterial);
                this.CreateComputeResources();
            }
            if (!base.isSupported)
            {
                base.ReportAutoDisable();
            }
            return base.isSupported;
        }

        private void CreateComputeResources()
        {
            if (this.cbDrawArgs == null)
            {
                this.cbDrawArgs = new ComputeBuffer(1, 0x10, ComputeBufferType.DrawIndirect);
                int[] data = new int[] { 0, 1, 0, 0 };
                this.cbDrawArgs.SetData(data);
            }
            this.cbPoints ??= new ComputeBuffer(0x15f90, 0x1c, ComputeBufferType.Append);
        }

        private float FocalDistance01(float worldDist) => 
            base.GetComponent<Camera>().WorldToViewportPoint(((Vector3) ((worldDist - base.GetComponent<Camera>().nearClipPlane) * base.GetComponent<Camera>().transform.forward)) + base.GetComponent<Camera>().transform.position).z / (base.GetComponent<Camera>().farClipPlane - base.GetComponent<Camera>().nearClipPlane);

        private void OnDisable()
        {
            this.ReleaseComputeResources();
            if (this.dofHdrMaterial)
            {
                DestroyImmediate(this.dofHdrMaterial);
            }
            this.dofHdrMaterial = null;
            if (this.dx11bokehMaterial)
            {
                DestroyImmediate(this.dx11bokehMaterial);
            }
            this.dx11bokehMaterial = null;
        }

        private void OnEnable()
        {
            Camera component = base.GetComponent<Camera>();
            component.depthTextureMode |= DepthTextureMode.Depth;
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (!this.CheckResources())
            {
                Graphics.Blit(source, destination);
            }
            else
            {
                if (this.aperture < 0f)
                {
                    this.aperture = 0f;
                }
                if (this.maxBlurSize < 0.1f)
                {
                    this.maxBlurSize = 0.1f;
                }
                this.focalSize = Mathf.Clamp(this.focalSize, 0f, 2f);
                this.internalBlurWidth = Mathf.Max(this.maxBlurSize, 0f);
                this.focalDistance01 = !this.focalTransform ? this.FocalDistance01(this.focalLength) : (base.GetComponent<Camera>().WorldToViewportPoint(this.focalTransform.position).z / base.GetComponent<Camera>().farClipPlane);
                this.dofHdrMaterial.SetVector("_CurveParams", new Vector4(1f, this.focalSize, this.aperture / 10f, this.focalDistance01));
                RenderTexture dest = null;
                RenderTexture texture2 = null;
                RenderTexture texture3 = null;
                RenderTexture texture4 = null;
                float y = this.internalBlurWidth * this.foregroundOverlap;
                if (this.visualizeFocus)
                {
                    this.WriteCoc(source, true);
                    Graphics.Blit(source, destination, this.dofHdrMaterial, 0x10);
                }
                else if ((this.blurType != BlurType.DX11) || !this.dx11bokehMaterial)
                {
                    source.filterMode = FilterMode.Bilinear;
                    if (this.highResolution)
                    {
                        this.internalBlurWidth *= 2f;
                    }
                    this.WriteCoc(source, true);
                    dest = RenderTexture.GetTemporary(source.width >> 1, source.height >> 1, 0, source.format);
                    texture2 = RenderTexture.GetTemporary(source.width >> 1, source.height >> 1, 0, source.format);
                    int pass = ((this.blurSampleCount == BlurSampleCount.High) || (this.blurSampleCount == BlurSampleCount.Medium)) ? 0x11 : 11;
                    if (this.highResolution)
                    {
                        this.dofHdrMaterial.SetVector("_Offsets", new Vector4(0f, this.internalBlurWidth, 0.025f, this.internalBlurWidth));
                        Graphics.Blit(source, destination, this.dofHdrMaterial, pass);
                    }
                    else
                    {
                        this.dofHdrMaterial.SetVector("_Offsets", new Vector4(0f, this.internalBlurWidth, 0.1f, this.internalBlurWidth));
                        Graphics.Blit(source, dest, this.dofHdrMaterial, 6);
                        Graphics.Blit(dest, texture2, this.dofHdrMaterial, pass);
                        this.dofHdrMaterial.SetTexture("_LowRez", texture2);
                        this.dofHdrMaterial.SetTexture("_FgOverlap", null);
                        this.dofHdrMaterial.SetVector("_Offsets", (Vector4.one * ((1f * source.width) / (1f * texture2.width))) * this.internalBlurWidth);
                        Graphics.Blit(source, destination, this.dofHdrMaterial, (this.blurSampleCount != BlurSampleCount.High) ? 12 : 0x12);
                    }
                }
                else if (this.highResolution)
                {
                    this.internalBlurWidth = (this.internalBlurWidth >= 0.1f) ? this.internalBlurWidth : 0.1f;
                    y = this.internalBlurWidth * this.foregroundOverlap;
                    dest = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);
                    RenderTexture texture5 = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);
                    this.WriteCoc(source, false);
                    texture3 = RenderTexture.GetTemporary(source.width >> 1, source.height >> 1, 0, source.format);
                    texture4 = RenderTexture.GetTemporary(source.width >> 1, source.height >> 1, 0, source.format);
                    Graphics.Blit(source, texture3, this.dofHdrMaterial, 15);
                    this.dofHdrMaterial.SetVector("_Offsets", new Vector4(0f, 1.5f, 0f, 1.5f));
                    Graphics.Blit(texture3, texture4, this.dofHdrMaterial, 0x13);
                    this.dofHdrMaterial.SetVector("_Offsets", new Vector4(1.5f, 0f, 0f, 1.5f));
                    Graphics.Blit(texture4, texture3, this.dofHdrMaterial, 0x13);
                    if (this.nearBlur)
                    {
                        Graphics.Blit(source, texture4, this.dofHdrMaterial, 4);
                    }
                    this.dx11bokehMaterial.SetTexture("_BlurredColor", texture3);
                    this.dx11bokehMaterial.SetFloat("_SpawnHeuristic", this.dx11SpawnHeuristic);
                    this.dx11bokehMaterial.SetVector("_BokehParams", new Vector4(this.dx11BokehScale, this.dx11BokehIntensity, Mathf.Clamp(this.dx11BokehThreshold, 0.005f, 4f), this.internalBlurWidth));
                    this.dx11bokehMaterial.SetTexture("_FgCocMask", !this.nearBlur ? null : texture4);
                    Graphics.SetRandomWriteTarget(1, this.cbPoints);
                    Graphics.Blit(source, dest, this.dx11bokehMaterial, 0);
                    Graphics.ClearRandomWriteTargets();
                    if (this.nearBlur)
                    {
                        this.dofHdrMaterial.SetVector("_Offsets", new Vector4(0f, y, 0f, y));
                        Graphics.Blit(texture4, texture3, this.dofHdrMaterial, 2);
                        this.dofHdrMaterial.SetVector("_Offsets", new Vector4(y, 0f, 0f, y));
                        Graphics.Blit(texture3, texture4, this.dofHdrMaterial, 2);
                        Graphics.Blit(texture4, dest, this.dofHdrMaterial, 3);
                    }
                    Graphics.Blit(dest, texture5, this.dofHdrMaterial, 20);
                    this.dofHdrMaterial.SetVector("_Offsets", new Vector4(this.internalBlurWidth, 0f, 0f, this.internalBlurWidth));
                    Graphics.Blit(dest, source, this.dofHdrMaterial, 5);
                    this.dofHdrMaterial.SetVector("_Offsets", new Vector4(0f, this.internalBlurWidth, 0f, this.internalBlurWidth));
                    Graphics.Blit(source, texture5, this.dofHdrMaterial, 0x15);
                    Graphics.SetRenderTarget(texture5);
                    ComputeBuffer.CopyCount(this.cbPoints, this.cbDrawArgs, 0);
                    this.dx11bokehMaterial.SetBuffer("pointBuffer", this.cbPoints);
                    this.dx11bokehMaterial.SetTexture("_MainTex", this.dx11BokehTexture);
                    this.dx11bokehMaterial.SetVector("_Screen", new Vector3(1f / (1f * source.width), 1f / (1f * source.height), this.internalBlurWidth));
                    this.dx11bokehMaterial.SetPass(2);
                    Graphics.DrawProceduralIndirect(MeshTopology.Points, this.cbDrawArgs, 0);
                    Graphics.Blit(texture5, destination);
                    RenderTexture.ReleaseTemporary(texture5);
                    RenderTexture.ReleaseTemporary(texture3);
                    RenderTexture.ReleaseTemporary(texture4);
                }
                else
                {
                    dest = RenderTexture.GetTemporary(source.width >> 1, source.height >> 1, 0, source.format);
                    texture2 = RenderTexture.GetTemporary(source.width >> 1, source.height >> 1, 0, source.format);
                    y = this.internalBlurWidth * this.foregroundOverlap;
                    this.WriteCoc(source, false);
                    source.filterMode = FilterMode.Bilinear;
                    Graphics.Blit(source, dest, this.dofHdrMaterial, 6);
                    texture3 = RenderTexture.GetTemporary(dest.width >> 1, dest.height >> 1, 0, dest.format);
                    texture4 = RenderTexture.GetTemporary(dest.width >> 1, dest.height >> 1, 0, dest.format);
                    Graphics.Blit(dest, texture3, this.dofHdrMaterial, 15);
                    this.dofHdrMaterial.SetVector("_Offsets", new Vector4(0f, 1.5f, 0f, 1.5f));
                    Graphics.Blit(texture3, texture4, this.dofHdrMaterial, 0x13);
                    this.dofHdrMaterial.SetVector("_Offsets", new Vector4(1.5f, 0f, 0f, 1.5f));
                    Graphics.Blit(texture4, texture3, this.dofHdrMaterial, 0x13);
                    RenderTexture texture6 = null;
                    if (this.nearBlur)
                    {
                        texture6 = RenderTexture.GetTemporary(source.width >> 1, source.height >> 1, 0, source.format);
                        Graphics.Blit(source, texture6, this.dofHdrMaterial, 4);
                    }
                    this.dx11bokehMaterial.SetTexture("_BlurredColor", texture3);
                    this.dx11bokehMaterial.SetFloat("_SpawnHeuristic", this.dx11SpawnHeuristic);
                    this.dx11bokehMaterial.SetVector("_BokehParams", new Vector4(this.dx11BokehScale, this.dx11BokehIntensity, Mathf.Clamp(this.dx11BokehThreshold, 0.005f, 4f), this.internalBlurWidth));
                    this.dx11bokehMaterial.SetTexture("_FgCocMask", texture6);
                    Graphics.SetRandomWriteTarget(1, this.cbPoints);
                    Graphics.Blit(dest, texture2, this.dx11bokehMaterial, 0);
                    Graphics.ClearRandomWriteTargets();
                    RenderTexture.ReleaseTemporary(texture3);
                    RenderTexture.ReleaseTemporary(texture4);
                    if (this.nearBlur)
                    {
                        this.dofHdrMaterial.SetVector("_Offsets", new Vector4(0f, y, 0f, y));
                        Graphics.Blit(texture6, dest, this.dofHdrMaterial, 2);
                        this.dofHdrMaterial.SetVector("_Offsets", new Vector4(y, 0f, 0f, y));
                        Graphics.Blit(dest, texture6, this.dofHdrMaterial, 2);
                        Graphics.Blit(texture6, texture2, this.dofHdrMaterial, 3);
                    }
                    this.dofHdrMaterial.SetVector("_Offsets", new Vector4(this.internalBlurWidth, 0f, 0f, this.internalBlurWidth));
                    Graphics.Blit(texture2, dest, this.dofHdrMaterial, 5);
                    this.dofHdrMaterial.SetVector("_Offsets", new Vector4(0f, this.internalBlurWidth, 0f, this.internalBlurWidth));
                    Graphics.Blit(dest, texture2, this.dofHdrMaterial, 5);
                    Graphics.SetRenderTarget(texture2);
                    ComputeBuffer.CopyCount(this.cbPoints, this.cbDrawArgs, 0);
                    this.dx11bokehMaterial.SetBuffer("pointBuffer", this.cbPoints);
                    this.dx11bokehMaterial.SetTexture("_MainTex", this.dx11BokehTexture);
                    this.dx11bokehMaterial.SetVector("_Screen", new Vector3(1f / (1f * texture2.width), 1f / (1f * texture2.height), this.internalBlurWidth));
                    this.dx11bokehMaterial.SetPass(1);
                    Graphics.DrawProceduralIndirect(MeshTopology.Points, this.cbDrawArgs, 0);
                    this.dofHdrMaterial.SetTexture("_LowRez", texture2);
                    this.dofHdrMaterial.SetTexture("_FgOverlap", texture6);
                    this.dofHdrMaterial.SetVector("_Offsets", (Vector4) ((((1f * source.width) / (1f * texture2.width)) * this.internalBlurWidth) * Vector4.one));
                    Graphics.Blit(source, destination, this.dofHdrMaterial, 9);
                    if (texture6)
                    {
                        RenderTexture.ReleaseTemporary(texture6);
                    }
                }
                if (dest)
                {
                    RenderTexture.ReleaseTemporary(dest);
                }
                if (texture2)
                {
                    RenderTexture.ReleaseTemporary(texture2);
                }
            }
        }

        private void ReleaseComputeResources()
        {
            if (this.cbDrawArgs != null)
            {
                this.cbDrawArgs.Release();
            }
            this.cbDrawArgs = null;
            if (this.cbPoints != null)
            {
                this.cbPoints.Release();
            }
            this.cbPoints = null;
        }

        private void WriteCoc(RenderTexture fromTo, bool fgDilate)
        {
            this.dofHdrMaterial.SetTexture("_FgOverlap", null);
            if (!this.nearBlur || !fgDilate)
            {
                fromTo.MarkRestoreExpected();
                Graphics.Blit(fromTo, fromTo, this.dofHdrMaterial, 0);
            }
            else
            {
                int width = fromTo.width / 2;
                int height = fromTo.height / 2;
                RenderTexture dest = RenderTexture.GetTemporary(width, height, 0, fromTo.format);
                Graphics.Blit(fromTo, dest, this.dofHdrMaterial, 4);
                float y = this.internalBlurWidth * this.foregroundOverlap;
                this.dofHdrMaterial.SetVector("_Offsets", new Vector4(0f, y, 0f, y));
                RenderTexture texture2 = RenderTexture.GetTemporary(width, height, 0, fromTo.format);
                Graphics.Blit(dest, texture2, this.dofHdrMaterial, 2);
                RenderTexture.ReleaseTemporary(dest);
                this.dofHdrMaterial.SetVector("_Offsets", new Vector4(y, 0f, 0f, y));
                dest = RenderTexture.GetTemporary(width, height, 0, fromTo.format);
                Graphics.Blit(texture2, dest, this.dofHdrMaterial, 2);
                RenderTexture.ReleaseTemporary(texture2);
                this.dofHdrMaterial.SetTexture("_FgOverlap", dest);
                fromTo.MarkRestoreExpected();
                Graphics.Blit(fromTo, fromTo, this.dofHdrMaterial, 13);
                RenderTexture.ReleaseTemporary(dest);
            }
        }

        public enum BlurSampleCount
        {
            Low,
            Medium,
            High
        }

        public enum BlurType
        {
            DiscBlur,
            DX11
        }
    }
}

