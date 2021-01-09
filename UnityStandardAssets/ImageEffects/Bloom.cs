namespace UnityStandardAssets.ImageEffects
{
    using System;
    using UnityEngine;

    [ExecuteInEditMode, RequireComponent(typeof(Camera)), AddComponentMenu("Image Effects/Bloom and Glow/Bloom")]
    public class Bloom : PostEffectsBase
    {
        public TweakMode tweakMode;
        public BloomScreenBlendMode screenBlendMode = BloomScreenBlendMode.Add;
        public HDRBloomMode hdr;
        private bool doHdr;
        public float sepBlurSpread = 2.5f;
        public BloomQuality quality = BloomQuality.High;
        public float bloomIntensity = 0.5f;
        public float bloomThreshold = 0.5f;
        public Color bloomThresholdColor = Color.white;
        public int bloomBlurIterations = 2;
        public int hollywoodFlareBlurIterations = 2;
        public float flareRotation;
        public LensFlareStyle lensflareMode = LensFlareStyle.Anamorphic;
        public float hollyStretchWidth = 2.5f;
        public float lensflareIntensity;
        public float lensflareThreshold = 0.3f;
        public float lensFlareSaturation = 0.75f;
        public Color flareColorA = new Color(0.4f, 0.4f, 0.8f, 0.75f);
        public Color flareColorB = new Color(0.4f, 0.8f, 0.8f, 0.75f);
        public Color flareColorC = new Color(0.8f, 0.4f, 0.8f, 0.75f);
        public Color flareColorD = new Color(0.8f, 0.4f, 0f, 0.75f);
        public Texture2D lensFlareVignetteMask;
        public Shader lensFlareShader;
        private Material lensFlareMaterial;
        public Shader screenBlendShader;
        private Material screenBlend;
        public Shader blurAndFlaresShader;
        private Material blurAndFlaresMaterial;
        public Shader brightPassFilterShader;
        [HideInInspector]
        public Material brightPassFilterMaterial;

        private void AddTo(float intensity_, RenderTexture from, RenderTexture to)
        {
            this.screenBlend.SetFloat("_Intensity", intensity_);
            to.MarkRestoreExpected();
            Graphics.Blit(from, to, this.screenBlend, 9);
        }

        private void BlendFlares(RenderTexture from, RenderTexture to)
        {
            this.lensFlareMaterial.SetVector("colorA", new Vector4(this.flareColorA.r, this.flareColorA.g, this.flareColorA.b, this.flareColorA.a) * this.lensflareIntensity);
            this.lensFlareMaterial.SetVector("colorB", new Vector4(this.flareColorB.r, this.flareColorB.g, this.flareColorB.b, this.flareColorB.a) * this.lensflareIntensity);
            this.lensFlareMaterial.SetVector("colorC", new Vector4(this.flareColorC.r, this.flareColorC.g, this.flareColorC.b, this.flareColorC.a) * this.lensflareIntensity);
            this.lensFlareMaterial.SetVector("colorD", new Vector4(this.flareColorD.r, this.flareColorD.g, this.flareColorD.b, this.flareColorD.a) * this.lensflareIntensity);
            to.MarkRestoreExpected();
            Graphics.Blit(from, to, this.lensFlareMaterial);
        }

        private void BrightFilter(float thresh, RenderTexture from, RenderTexture to)
        {
            this.brightPassFilterMaterial.SetVector("_Threshhold", new Vector4(thresh, thresh, thresh, thresh));
            Graphics.Blit(from, to, this.brightPassFilterMaterial, 0);
        }

        private void BrightFilter(Color threshColor, RenderTexture from, RenderTexture to)
        {
            this.brightPassFilterMaterial.SetVector("_Threshhold", (Vector4) threshColor);
            Graphics.Blit(from, to, this.brightPassFilterMaterial, 1);
        }

        public override bool CheckResources()
        {
            base.CheckSupport(false);
            this.screenBlend = base.CheckShaderAndCreateMaterial(this.screenBlendShader, this.screenBlend);
            this.lensFlareMaterial = base.CheckShaderAndCreateMaterial(this.lensFlareShader, this.lensFlareMaterial);
            this.blurAndFlaresMaterial = base.CheckShaderAndCreateMaterial(this.blurAndFlaresShader, this.blurAndFlaresMaterial);
            this.brightPassFilterMaterial = base.CheckShaderAndCreateMaterial(this.brightPassFilterShader, this.brightPassFilterMaterial);
            if (!base.isSupported)
            {
                base.ReportAutoDisable();
            }
            return base.isSupported;
        }

        public void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (!this.CheckResources())
            {
                Graphics.Blit(source, destination);
            }
            else
            {
                this.doHdr = false;
                this.doHdr = (this.hdr != HDRBloomMode.Auto) ? (this.hdr == HDRBloomMode.On) : ((source.format == RenderTextureFormat.ARGBHalf) && base.GetComponent<Camera>().hdr);
                this.doHdr = this.doHdr && base.supportHDRTextures;
                BloomScreenBlendMode screenBlendMode = this.screenBlendMode;
                if (this.doHdr)
                {
                    screenBlendMode = BloomScreenBlendMode.Add;
                }
                RenderTextureFormat format = !this.doHdr ? RenderTextureFormat.Default : RenderTextureFormat.ARGBHalf;
                int width = source.width / 2;
                int height = source.height / 2;
                int num3 = source.width / 4;
                int num4 = source.height / 4;
                float num5 = (1f * source.width) / (1f * source.height);
                float num6 = 0.001953125f;
                RenderTexture dest = RenderTexture.GetTemporary(num3, num4, 0, format);
                RenderTexture texture2 = RenderTexture.GetTemporary(width, height, 0, format);
                if (this.quality <= BloomQuality.Cheap)
                {
                    Graphics.Blit(source, texture2);
                    Graphics.Blit(texture2, dest, this.screenBlend, 6);
                }
                else
                {
                    Graphics.Blit(source, texture2, this.screenBlend, 2);
                    RenderTexture texture3 = RenderTexture.GetTemporary(num3, num4, 0, format);
                    Graphics.Blit(texture2, texture3, this.screenBlend, 2);
                    Graphics.Blit(texture3, dest, this.screenBlend, 6);
                    RenderTexture.ReleaseTemporary(texture3);
                }
                RenderTexture.ReleaseTemporary(texture2);
                RenderTexture to = RenderTexture.GetTemporary(num3, num4, 0, format);
                this.BrightFilter((Color) (this.bloomThreshold * this.bloomThresholdColor), dest, to);
                if (this.bloomBlurIterations < 1)
                {
                    this.bloomBlurIterations = 1;
                }
                else if (this.bloomBlurIterations > 10)
                {
                    this.bloomBlurIterations = 10;
                }
                for (int i = 0; i < this.bloomBlurIterations; i++)
                {
                    float num8 = (1f + (i * 0.25f)) * this.sepBlurSpread;
                    RenderTexture texture5 = RenderTexture.GetTemporary(num3, num4, 0, format);
                    this.blurAndFlaresMaterial.SetVector("_Offsets", new Vector4(0f, num8 * num6, 0f, 0f));
                    Graphics.Blit(to, texture5, this.blurAndFlaresMaterial, 4);
                    RenderTexture.ReleaseTemporary(to);
                    to = texture5;
                    texture5 = RenderTexture.GetTemporary(num3, num4, 0, format);
                    this.blurAndFlaresMaterial.SetVector("_Offsets", new Vector4((num8 / num5) * num6, 0f, 0f, 0f));
                    Graphics.Blit(to, texture5, this.blurAndFlaresMaterial, 4);
                    RenderTexture.ReleaseTemporary(to);
                    to = texture5;
                    if (this.quality > BloomQuality.Cheap)
                    {
                        if (i != 0)
                        {
                            dest.MarkRestoreExpected();
                            Graphics.Blit(to, dest, this.screenBlend, 10);
                        }
                        else
                        {
                            Graphics.SetRenderTarget(dest);
                            GL.Clear(false, true, Color.black);
                            Graphics.Blit(to, dest);
                        }
                    }
                }
                if (this.quality > BloomQuality.Cheap)
                {
                    Graphics.SetRenderTarget(to);
                    GL.Clear(false, true, Color.black);
                    Graphics.Blit(dest, to, this.screenBlend, 6);
                }
                if (this.lensflareIntensity > Mathf.Epsilon)
                {
                    RenderTexture texture6 = RenderTexture.GetTemporary(num3, num4, 0, format);
                    if (this.lensflareMode == LensFlareStyle.Ghosting)
                    {
                        this.BrightFilter(this.lensflareThreshold, to, texture6);
                        if (this.quality > BloomQuality.Cheap)
                        {
                            this.blurAndFlaresMaterial.SetVector("_Offsets", new Vector4(0f, 1.5f / (1f * dest.height), 0f, 0f));
                            Graphics.SetRenderTarget(dest);
                            GL.Clear(false, true, Color.black);
                            Graphics.Blit(texture6, dest, this.blurAndFlaresMaterial, 4);
                            this.blurAndFlaresMaterial.SetVector("_Offsets", new Vector4(1.5f / (1f * dest.width), 0f, 0f, 0f));
                            Graphics.SetRenderTarget(texture6);
                            GL.Clear(false, true, Color.black);
                            Graphics.Blit(dest, texture6, this.blurAndFlaresMaterial, 4);
                        }
                        this.Vignette(0.975f, texture6, texture6);
                        this.BlendFlares(texture6, to);
                    }
                    else
                    {
                        float x = 1f * Mathf.Cos(this.flareRotation);
                        float y = 1f * Mathf.Sin(this.flareRotation);
                        float num11 = ((this.hollyStretchWidth * 1f) / num5) * num6;
                        this.blurAndFlaresMaterial.SetVector("_Offsets", new Vector4(x, y, 0f, 0f));
                        this.blurAndFlaresMaterial.SetVector("_Threshhold", new Vector4(this.lensflareThreshold, 1f, 0f, 0f));
                        this.blurAndFlaresMaterial.SetVector("_TintColor", (new Vector4(this.flareColorA.r, this.flareColorA.g, this.flareColorA.b, this.flareColorA.a) * this.flareColorA.a) * this.lensflareIntensity);
                        this.blurAndFlaresMaterial.SetFloat("_Saturation", this.lensFlareSaturation);
                        dest.DiscardContents();
                        Graphics.Blit(texture6, dest, this.blurAndFlaresMaterial, 2);
                        texture6.DiscardContents();
                        Graphics.Blit(dest, texture6, this.blurAndFlaresMaterial, 3);
                        this.blurAndFlaresMaterial.SetVector("_Offsets", new Vector4(x * num11, y * num11, 0f, 0f));
                        this.blurAndFlaresMaterial.SetFloat("_StretchWidth", this.hollyStretchWidth);
                        dest.DiscardContents();
                        Graphics.Blit(texture6, dest, this.blurAndFlaresMaterial, 1);
                        this.blurAndFlaresMaterial.SetFloat("_StretchWidth", this.hollyStretchWidth * 2f);
                        texture6.DiscardContents();
                        Graphics.Blit(dest, texture6, this.blurAndFlaresMaterial, 1);
                        this.blurAndFlaresMaterial.SetFloat("_StretchWidth", this.hollyStretchWidth * 4f);
                        dest.DiscardContents();
                        Graphics.Blit(texture6, dest, this.blurAndFlaresMaterial, 1);
                        int num12 = 0;
                        while (true)
                        {
                            if (num12 >= this.hollywoodFlareBlurIterations)
                            {
                                if (this.lensflareMode == LensFlareStyle.Anamorphic)
                                {
                                    this.AddTo(1f, dest, to);
                                }
                                else
                                {
                                    this.Vignette(1f, dest, texture6);
                                    this.BlendFlares(texture6, dest);
                                    this.AddTo(1f, dest, to);
                                }
                                break;
                            }
                            num11 = ((this.hollyStretchWidth * 2f) / num5) * num6;
                            this.blurAndFlaresMaterial.SetVector("_Offsets", new Vector4(num11 * x, num11 * y, 0f, 0f));
                            texture6.DiscardContents();
                            Graphics.Blit(dest, texture6, this.blurAndFlaresMaterial, 4);
                            this.blurAndFlaresMaterial.SetVector("_Offsets", new Vector4(num11 * x, num11 * y, 0f, 0f));
                            dest.DiscardContents();
                            Graphics.Blit(texture6, dest, this.blurAndFlaresMaterial, 4);
                            num12++;
                        }
                    }
                    RenderTexture.ReleaseTemporary(texture6);
                }
                int pass = (int) screenBlendMode;
                this.screenBlend.SetFloat("_Intensity", this.bloomIntensity);
                this.screenBlend.SetTexture("_ColorBuffer", source);
                if (this.quality <= BloomQuality.Cheap)
                {
                    Graphics.Blit(to, destination, this.screenBlend, pass);
                }
                else
                {
                    RenderTexture texture7 = RenderTexture.GetTemporary(width, height, 0, format);
                    Graphics.Blit(to, texture7);
                    Graphics.Blit(texture7, destination, this.screenBlend, pass);
                    RenderTexture.ReleaseTemporary(texture7);
                }
                RenderTexture.ReleaseTemporary(dest);
                RenderTexture.ReleaseTemporary(to);
            }
        }

        private void Vignette(float amount, RenderTexture from, RenderTexture to)
        {
            if (this.lensFlareVignetteMask)
            {
                this.screenBlend.SetTexture("_ColorBuffer", this.lensFlareVignetteMask);
                to.MarkRestoreExpected();
                Graphics.Blit((from != to) ? from : null, to, this.screenBlend, (from != to) ? 3 : 7);
            }
            else if (from != to)
            {
                Graphics.SetRenderTarget(to);
                GL.Clear(false, true, Color.black);
                Graphics.Blit(from, to);
            }
        }

        public enum BloomQuality
        {
            Cheap,
            High
        }

        public enum BloomScreenBlendMode
        {
            Screen,
            Add
        }

        public enum HDRBloomMode
        {
            Auto,
            On,
            Off
        }

        public enum LensFlareStyle
        {
            Ghosting,
            Anamorphic,
            Combined
        }

        public enum TweakMode
        {
            Basic,
            Complex
        }
    }
}

