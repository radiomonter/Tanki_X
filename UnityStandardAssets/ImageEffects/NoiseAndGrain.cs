namespace UnityStandardAssets.ImageEffects
{
    using System;
    using UnityEngine;

    [ExecuteInEditMode, RequireComponent(typeof(Camera)), AddComponentMenu("Image Effects/Noise/Noise And Grain (Filmic)")]
    public class NoiseAndGrain : PostEffectsBase
    {
        public float intensityMultiplier = 0.25f;
        public float generalIntensity = 0.5f;
        public float blackIntensity = 1f;
        public float whiteIntensity = 1f;
        public float midGrey = 0.2f;
        public bool dx11Grain;
        public float softness;
        public bool monochrome;
        public Vector3 intensities = new Vector3(1f, 1f, 1f);
        public Vector3 tiling = new Vector3(64f, 64f, 64f);
        public float monochromeTiling = 64f;
        public FilterMode filterMode = FilterMode.Bilinear;
        public Texture2D noiseTexture;
        public Shader noiseShader;
        private Material noiseMaterial;
        public Shader dx11NoiseShader;
        private Material dx11NoiseMaterial;
        private static float TILE_AMOUNT = 64f;

        public override bool CheckResources()
        {
            base.CheckSupport(false);
            this.noiseMaterial = base.CheckShaderAndCreateMaterial(this.noiseShader, this.noiseMaterial);
            if (this.dx11Grain && base.supportDX11)
            {
                this.dx11NoiseMaterial = base.CheckShaderAndCreateMaterial(this.dx11NoiseShader, this.dx11NoiseMaterial);
            }
            if (!base.isSupported)
            {
                base.ReportAutoDisable();
            }
            return base.isSupported;
        }

        private static void DrawNoiseQuadGrid(RenderTexture source, RenderTexture dest, Material fxMaterial, Texture2D noise, int passNr)
        {
            RenderTexture.active = dest;
            float num = noise.width * 1f;
            fxMaterial.SetTexture("_MainTex", source);
            GL.PushMatrix();
            GL.LoadOrtho();
            float num4 = 1f / ((1f * source.width) / TILE_AMOUNT);
            float num5 = num4 * ((1f * source.width) / (1f * source.height));
            float num6 = num / (noise.width * 1f);
            fxMaterial.SetPass(passNr);
            GL.Begin(7);
            float x = 0f;
            while (x < 1f)
            {
                float y = 0f;
                while (true)
                {
                    if (y >= 1f)
                    {
                        x += num4;
                        break;
                    }
                    float num9 = Mathf.Floor(Random.Range((float) 0f, (float) 1f) * num) / num;
                    float num10 = Mathf.Floor(Random.Range((float) 0f, (float) 1f) * num) / num;
                    float num11 = 1f / num;
                    GL.MultiTexCoord2(0, num9, num10);
                    GL.MultiTexCoord2(1, 0f, 0f);
                    GL.Vertex3(x, y, 0.1f);
                    GL.MultiTexCoord2(0, num9 + (num6 * num11), num10);
                    GL.MultiTexCoord2(1, 1f, 0f);
                    GL.Vertex3(x + num4, y, 0.1f);
                    GL.MultiTexCoord2(0, num9 + (num6 * num11), num10 + (num6 * num11));
                    GL.MultiTexCoord2(1, 1f, 1f);
                    GL.Vertex3(x + num4, y + num5, 0.1f);
                    GL.MultiTexCoord2(0, num9, num10 + (num6 * num11));
                    GL.MultiTexCoord2(1, 0f, 1f);
                    GL.Vertex3(x, y + num5, 0.1f);
                    y += num5;
                }
            }
            GL.End();
            GL.PopMatrix();
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (!this.CheckResources() || (null == this.noiseTexture))
            {
                Graphics.Blit(source, destination);
                if (null == this.noiseTexture)
                {
                    Debug.LogWarning("Noise & Grain effect failing as noise texture is not assigned. please assign.", base.transform);
                }
            }
            else
            {
                this.softness = Mathf.Clamp(this.softness, 0f, 0.99f);
                if (this.dx11Grain && base.supportDX11)
                {
                    this.dx11NoiseMaterial.SetFloat("_DX11NoiseTime", (float) Time.frameCount);
                    this.dx11NoiseMaterial.SetTexture("_NoiseTex", this.noiseTexture);
                    this.dx11NoiseMaterial.SetVector("_NoisePerChannel", !this.monochrome ? this.intensities : Vector3.one);
                    this.dx11NoiseMaterial.SetVector("_MidGrey", new Vector3(this.midGrey, 1f / (1f - this.midGrey), -1f / this.midGrey));
                    this.dx11NoiseMaterial.SetVector("_NoiseAmount", new Vector3(this.generalIntensity, this.blackIntensity, this.whiteIntensity) * this.intensityMultiplier);
                    if (this.softness <= Mathf.Epsilon)
                    {
                        DrawNoiseQuadGrid(source, destination, this.dx11NoiseMaterial, this.noiseTexture, !this.monochrome ? 0 : 1);
                    }
                    else
                    {
                        RenderTexture temporary = RenderTexture.GetTemporary((int) (source.width * (1f - this.softness)), (int) (source.height * (1f - this.softness)));
                        DrawNoiseQuadGrid(source, temporary, this.dx11NoiseMaterial, this.noiseTexture, !this.monochrome ? 2 : 3);
                        this.dx11NoiseMaterial.SetTexture("_NoiseTex", temporary);
                        Graphics.Blit(source, destination, this.dx11NoiseMaterial, 4);
                        RenderTexture.ReleaseTemporary(temporary);
                    }
                }
                else
                {
                    if (this.noiseTexture)
                    {
                        this.noiseTexture.wrapMode = TextureWrapMode.Repeat;
                        this.noiseTexture.filterMode = this.filterMode;
                    }
                    this.noiseMaterial.SetTexture("_NoiseTex", this.noiseTexture);
                    this.noiseMaterial.SetVector("_NoisePerChannel", !this.monochrome ? this.intensities : Vector3.one);
                    this.noiseMaterial.SetVector("_NoiseTilingPerChannel", !this.monochrome ? this.tiling : (Vector3.one * this.monochromeTiling));
                    this.noiseMaterial.SetVector("_MidGrey", new Vector3(this.midGrey, 1f / (1f - this.midGrey), -1f / this.midGrey));
                    this.noiseMaterial.SetVector("_NoiseAmount", new Vector3(this.generalIntensity, this.blackIntensity, this.whiteIntensity) * this.intensityMultiplier);
                    if (this.softness <= Mathf.Epsilon)
                    {
                        DrawNoiseQuadGrid(source, destination, this.noiseMaterial, this.noiseTexture, 0);
                    }
                    else
                    {
                        RenderTexture temporary = RenderTexture.GetTemporary((int) (source.width * (1f - this.softness)), (int) (source.height * (1f - this.softness)));
                        DrawNoiseQuadGrid(source, temporary, this.noiseMaterial, this.noiseTexture, 2);
                        this.noiseMaterial.SetTexture("_NoiseTex", temporary);
                        Graphics.Blit(source, destination, this.noiseMaterial, 1);
                        RenderTexture.ReleaseTemporary(temporary);
                    }
                }
            }
        }
    }
}

