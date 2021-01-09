namespace UnityStandardAssets.ImageEffects
{
    using System;
    using UnityEngine;

    [ExecuteInEditMode, RequireComponent(typeof(Camera)), AddComponentMenu("Image Effects/Rendering/Sun Shafts")]
    public class SunShafts : PostEffectsBase
    {
        public SunShaftsResolution resolution = SunShaftsResolution.Normal;
        public ShaftsScreenBlendMode screenBlendMode;
        public Transform sunTransform;
        public int radialBlurIterations = 2;
        public Color sunColor = Color.white;
        public Color sunThreshold = new Color(0.87f, 0.74f, 0.65f);
        public float sunShaftBlurRadius = 2.5f;
        public float sunShaftIntensity = 1.15f;
        public float maxRadius = 0.75f;
        public bool useDepthTexture = true;
        public Shader sunShaftsShader;
        private Material sunShaftsMaterial;
        public Shader simpleClearShader;
        private Material simpleClearMaterial;

        public override bool CheckResources()
        {
            base.CheckSupport(this.useDepthTexture);
            this.sunShaftsMaterial = base.CheckShaderAndCreateMaterial(this.sunShaftsShader, this.sunShaftsMaterial);
            this.simpleClearMaterial = base.CheckShaderAndCreateMaterial(this.simpleClearShader, this.simpleClearMaterial);
            if (!base.isSupported)
            {
                base.ReportAutoDisable();
            }
            return base.isSupported;
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (!this.CheckResources())
            {
                Graphics.Blit(source, destination);
            }
            else
            {
                if (this.useDepthTexture)
                {
                    Camera component = base.GetComponent<Camera>();
                    component.depthTextureMode |= DepthTextureMode.Depth;
                }
                int num = 4;
                if (this.resolution == SunShaftsResolution.Normal)
                {
                    num = 2;
                }
                else if (this.resolution == SunShaftsResolution.High)
                {
                    num = 1;
                }
                Vector3 vector = Vector3.one * 0.5f;
                if (this.sunTransform)
                {
                    vector = base.GetComponent<Camera>().WorldToViewportPoint(this.sunTransform.position);
                }
                else
                {
                    vector = new Vector3(0.5f, 0.5f, 0f);
                }
                int width = source.width / num;
                int height = source.height / num;
                RenderTexture dest = RenderTexture.GetTemporary(width, height, 0);
                this.sunShaftsMaterial.SetVector("_BlurRadius4", new Vector4(1f, 1f, 0f, 0f) * this.sunShaftBlurRadius);
                this.sunShaftsMaterial.SetVector("_SunPosition", new Vector4(vector.x, vector.y, vector.z, this.maxRadius));
                this.sunShaftsMaterial.SetVector("_SunThreshold", (Vector4) this.sunThreshold);
                if (this.useDepthTexture)
                {
                    Graphics.Blit(source, dest, this.sunShaftsMaterial, 2);
                }
                else
                {
                    RenderTextureFormat format = !base.GetComponent<Camera>().hdr ? RenderTextureFormat.Default : RenderTextureFormat.DefaultHDR;
                    RenderTexture texture3 = RenderTexture.GetTemporary(source.width, source.height, 0, format);
                    RenderTexture.active = texture3;
                    GL.ClearWithSkybox(false, base.GetComponent<Camera>());
                    this.sunShaftsMaterial.SetTexture("_Skybox", texture3);
                    Graphics.Blit(source, dest, this.sunShaftsMaterial, 3);
                    RenderTexture.ReleaseTemporary(texture3);
                }
                base.DrawBorder(dest, this.simpleClearMaterial);
                this.radialBlurIterations = Mathf.Clamp(this.radialBlurIterations, 1, 4);
                float x = this.sunShaftBlurRadius * 0.001302083f;
                this.sunShaftsMaterial.SetVector("_BlurRadius4", new Vector4(x, x, 0f, 0f));
                this.sunShaftsMaterial.SetVector("_SunPosition", new Vector4(vector.x, vector.y, vector.z, this.maxRadius));
                for (int i = 0; i < this.radialBlurIterations; i++)
                {
                    RenderTexture texture = RenderTexture.GetTemporary(width, height, 0);
                    Graphics.Blit(dest, texture, this.sunShaftsMaterial, 1);
                    RenderTexture.ReleaseTemporary(dest);
                    x = (this.sunShaftBlurRadius * (((i * 2f) + 1f) * 6f)) / 768f;
                    this.sunShaftsMaterial.SetVector("_BlurRadius4", new Vector4(x, x, 0f, 0f));
                    dest = RenderTexture.GetTemporary(width, height, 0);
                    Graphics.Blit(texture, dest, this.sunShaftsMaterial, 1);
                    RenderTexture.ReleaseTemporary(texture);
                    x = (this.sunShaftBlurRadius * (((i * 2f) + 2f) * 6f)) / 768f;
                    this.sunShaftsMaterial.SetVector("_BlurRadius4", new Vector4(x, x, 0f, 0f));
                }
                if (vector.z >= 0f)
                {
                    this.sunShaftsMaterial.SetVector("_SunColor", new Vector4(this.sunColor.r, this.sunColor.g, this.sunColor.b, this.sunColor.a) * this.sunShaftIntensity);
                }
                else
                {
                    this.sunShaftsMaterial.SetVector("_SunColor", Vector4.zero);
                }
                this.sunShaftsMaterial.SetTexture("_ColorBuffer", dest);
                Graphics.Blit(source, destination, this.sunShaftsMaterial, (this.screenBlendMode != ShaftsScreenBlendMode.Screen) ? 4 : 0);
                RenderTexture.ReleaseTemporary(dest);
            }
        }

        public enum ShaftsScreenBlendMode
        {
            Screen,
            Add
        }

        public enum SunShaftsResolution
        {
            Low,
            Normal,
            High
        }
    }
}

