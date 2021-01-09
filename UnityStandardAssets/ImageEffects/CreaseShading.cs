namespace UnityStandardAssets.ImageEffects
{
    using System;
    using UnityEngine;

    [ExecuteInEditMode, RequireComponent(typeof(Camera)), AddComponentMenu("Image Effects/Edge Detection/Crease Shading")]
    internal class CreaseShading : PostEffectsBase
    {
        public float intensity = 0.5f;
        public int softness = 1;
        public float spread = 1f;
        public Shader blurShader;
        private Material blurMaterial;
        public Shader depthFetchShader;
        private Material depthFetchMaterial;
        public Shader creaseApplyShader;
        private Material creaseApplyMaterial;

        public override bool CheckResources()
        {
            base.CheckSupport(true);
            this.blurMaterial = base.CheckShaderAndCreateMaterial(this.blurShader, this.blurMaterial);
            this.depthFetchMaterial = base.CheckShaderAndCreateMaterial(this.depthFetchShader, this.depthFetchMaterial);
            this.creaseApplyMaterial = base.CheckShaderAndCreateMaterial(this.creaseApplyShader, this.creaseApplyMaterial);
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
                int width = source.width;
                int height = source.height;
                float num3 = (1f * width) / (1f * height);
                float num4 = 0.001953125f;
                RenderTexture dest = RenderTexture.GetTemporary(width, height, 0);
                RenderTexture texture2 = RenderTexture.GetTemporary(width / 2, height / 2, 0);
                Graphics.Blit(source, dest, this.depthFetchMaterial);
                Graphics.Blit(dest, texture2);
                for (int i = 0; i < this.softness; i++)
                {
                    RenderTexture texture3 = RenderTexture.GetTemporary(width / 2, height / 2, 0);
                    this.blurMaterial.SetVector("offsets", new Vector4(0f, this.spread * num4, 0f, 0f));
                    Graphics.Blit(texture2, texture3, this.blurMaterial);
                    RenderTexture.ReleaseTemporary(texture2);
                    texture2 = texture3;
                    texture3 = RenderTexture.GetTemporary(width / 2, height / 2, 0);
                    this.blurMaterial.SetVector("offsets", new Vector4((this.spread * num4) / num3, 0f, 0f, 0f));
                    Graphics.Blit(texture2, texture3, this.blurMaterial);
                    RenderTexture.ReleaseTemporary(texture2);
                    texture2 = texture3;
                }
                this.creaseApplyMaterial.SetTexture("_HrDepthTex", dest);
                this.creaseApplyMaterial.SetTexture("_LrDepthTex", texture2);
                this.creaseApplyMaterial.SetFloat("intensity", this.intensity);
                Graphics.Blit(source, destination, this.creaseApplyMaterial);
                RenderTexture.ReleaseTemporary(dest);
                RenderTexture.ReleaseTemporary(texture2);
            }
        }
    }
}

