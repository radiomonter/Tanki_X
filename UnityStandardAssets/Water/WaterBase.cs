namespace UnityStandardAssets.Water
{
    using System;
    using UnityEngine;

    [ExecuteInEditMode]
    public class WaterBase : MonoBehaviour
    {
        public Material sharedMaterial;
        public WaterQuality waterQuality = WaterQuality.High;
        public bool edgeBlend = true;
        private Camera _cachedCamera;

        public void Update()
        {
            if (this.sharedMaterial)
            {
                this.UpdateShader();
            }
        }

        public void UpdateShader()
        {
            this.sharedMaterial.shader.maximumLOD = (this.waterQuality <= WaterQuality.Medium) ? ((this.waterQuality <= WaterQuality.Low) ? 0xc9 : 0x12d) : 0x1f5;
            if (!SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth))
            {
                this.edgeBlend = false;
            }
            if (!this.edgeBlend)
            {
                Shader.EnableKeyword("WATER_EDGEBLEND_OFF");
                Shader.DisableKeyword("WATER_EDGEBLEND_ON");
            }
            else
            {
                Shader.EnableKeyword("WATER_EDGEBLEND_ON");
                Shader.DisableKeyword("WATER_EDGEBLEND_OFF");
                if (this.CachedCamera != null)
                {
                    Camera cachedCamera = this.CachedCamera;
                    cachedCamera.depthTextureMode |= DepthTextureMode.Depth;
                }
            }
        }

        public void WaterTileBeingRendered(Transform tr, Camera currentCam)
        {
            if (currentCam && this.edgeBlend)
            {
                currentCam.depthTextureMode |= DepthTextureMode.Depth;
            }
        }

        public Camera CachedCamera
        {
            get
            {
                if (!this._cachedCamera)
                {
                    this._cachedCamera = Camera.main;
                }
                return this._cachedCamera;
            }
        }
    }
}

