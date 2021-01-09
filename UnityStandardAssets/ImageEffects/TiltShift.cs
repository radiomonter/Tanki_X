namespace UnityStandardAssets.ImageEffects
{
    using System;
    using UnityEngine;

    [RequireComponent(typeof(Camera)), AddComponentMenu("Image Effects/Camera/Tilt Shift (Lens Blur)")]
    internal class TiltShift : PostEffectsBase
    {
        public TiltShiftMode mode;
        public TiltShiftQuality quality = TiltShiftQuality.Normal;
        [Range(0f, 15f)]
        public float blurArea = 1f;
        [Range(0f, 25f)]
        public float maxBlurSize = 5f;
        [Range(0f, 1f)]
        public int downsample;
        public Shader tiltShiftShader;
        private Material tiltShiftMaterial;

        public override bool CheckResources()
        {
            base.CheckSupport(true);
            this.tiltShiftMaterial = base.CheckShaderAndCreateMaterial(this.tiltShiftShader, this.tiltShiftMaterial);
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
                this.tiltShiftMaterial.SetFloat("_BlurSize", (this.maxBlurSize >= 0f) ? this.maxBlurSize : 0f);
                this.tiltShiftMaterial.SetFloat("_BlurArea", this.blurArea);
                source.filterMode = FilterMode.Bilinear;
                RenderTexture dest = destination;
                if (this.downsample > 0f)
                {
                    dest = RenderTexture.GetTemporary(source.width >> (this.downsample & 0x1f), source.height >> (this.downsample & 0x1f), 0, source.format);
                    dest.filterMode = FilterMode.Bilinear;
                }
                int num = (int) (this.quality * TiltShiftQuality.High);
                Graphics.Blit(source, dest, this.tiltShiftMaterial, (this.mode != TiltShiftMode.TiltShiftMode) ? (num + 1) : num);
                if (this.downsample > 0)
                {
                    this.tiltShiftMaterial.SetTexture("_Blurred", dest);
                    Graphics.Blit(source, destination, this.tiltShiftMaterial, 6);
                }
                if (dest != destination)
                {
                    RenderTexture.ReleaseTemporary(dest);
                }
            }
        }

        public enum TiltShiftMode
        {
            TiltShiftMode,
            IrisMode
        }

        public enum TiltShiftQuality
        {
            Preview,
            Normal,
            High
        }
    }
}

