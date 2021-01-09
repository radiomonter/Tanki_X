namespace UnityStandardAssets.ImageEffects
{
    using System;
    using UnityEngine;

    [ExecuteInEditMode, RequireComponent(typeof(Camera)), AddComponentMenu("Image Effects/Noise/Noise and Scratches")]
    public class NoiseAndScratches : MonoBehaviour
    {
        public bool monochrome = true;
        private bool rgbFallback;
        public float grainIntensityMin = 0.1f;
        public float grainIntensityMax = 0.2f;
        public float grainSize = 2f;
        public float scratchIntensityMin = 0.05f;
        public float scratchIntensityMax = 0.25f;
        public float scratchFPS = 10f;
        public float scratchJitter = 0.01f;
        public Texture grainTexture;
        public Texture scratchTexture;
        public Shader shaderRGB;
        public Shader shaderYUV;
        private Material m_MaterialRGB;
        private Material m_MaterialYUV;
        private float scratchTimeLeft;
        private float scratchX;
        private float scratchY;

        protected void OnDisable()
        {
            if (this.m_MaterialRGB)
            {
                DestroyImmediate(this.m_MaterialRGB);
            }
            if (this.m_MaterialYUV)
            {
                DestroyImmediate(this.m_MaterialYUV);
            }
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            this.SanitizeParameters();
            if (this.scratchTimeLeft <= 0f)
            {
                this.scratchTimeLeft = (Random.value * 2f) / this.scratchFPS;
                this.scratchX = Random.value;
                this.scratchY = Random.value;
            }
            this.scratchTimeLeft -= Time.deltaTime;
            Material mat = this.material;
            mat.SetTexture("_GrainTex", this.grainTexture);
            mat.SetTexture("_ScratchTex", this.scratchTexture);
            float num = 1f / this.grainSize;
            mat.SetVector("_GrainOffsetScale", new Vector4(Random.value, Random.value, (((float) Screen.width) / ((float) this.grainTexture.width)) * num, (((float) Screen.height) / ((float) this.grainTexture.height)) * num));
            mat.SetVector("_ScratchOffsetScale", new Vector4(this.scratchX + (Random.value * this.scratchJitter), this.scratchY + (Random.value * this.scratchJitter), ((float) Screen.width) / ((float) this.scratchTexture.width), ((float) Screen.height) / ((float) this.scratchTexture.height)));
            mat.SetVector("_Intensity", new Vector4(Random.Range(this.grainIntensityMin, this.grainIntensityMax), Random.Range(this.scratchIntensityMin, this.scratchIntensityMax), 0f, 0f));
            Graphics.Blit(source, destination, mat);
        }

        private void SanitizeParameters()
        {
            this.grainIntensityMin = Mathf.Clamp(this.grainIntensityMin, 0f, 5f);
            this.grainIntensityMax = Mathf.Clamp(this.grainIntensityMax, 0f, 5f);
            this.scratchIntensityMin = Mathf.Clamp(this.scratchIntensityMin, 0f, 5f);
            this.scratchIntensityMax = Mathf.Clamp(this.scratchIntensityMax, 0f, 5f);
            this.scratchFPS = Mathf.Clamp(this.scratchFPS, 1f, 30f);
            this.scratchJitter = Mathf.Clamp(this.scratchJitter, 0f, 1f);
            this.grainSize = Mathf.Clamp(this.grainSize, 0.1f, 50f);
        }

        protected void Start()
        {
            if (!SystemInfo.supportsImageEffects)
            {
                base.enabled = false;
            }
            else if ((this.shaderRGB == null) || (this.shaderYUV == null))
            {
                Debug.Log("Noise shaders are not set up! Disabling noise effect.");
                base.enabled = false;
            }
            else if (!this.shaderRGB.isSupported)
            {
                base.enabled = false;
            }
            else if (!this.shaderYUV.isSupported)
            {
                this.rgbFallback = true;
            }
        }

        protected Material material
        {
            get
            {
                if (this.m_MaterialRGB == null)
                {
                    this.m_MaterialRGB = new Material(this.shaderRGB);
                    this.m_MaterialRGB.hideFlags = HideFlags.HideAndDontSave;
                }
                if ((this.m_MaterialYUV == null) && !this.rgbFallback)
                {
                    this.m_MaterialYUV = new Material(this.shaderYUV);
                    this.m_MaterialYUV.hideFlags = HideFlags.HideAndDontSave;
                }
                return ((this.rgbFallback || this.monochrome) ? this.m_MaterialRGB : this.m_MaterialYUV);
            }
        }
    }
}

