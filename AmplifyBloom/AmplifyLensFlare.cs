namespace AmplifyBloom
{
    using System;
    using UnityEngine;

    [Serializable]
    public class AmplifyLensFlare : IAmplifyItem
    {
        private const int LUTTextureWidth = 0x100;
        [SerializeField]
        private float m_overallIntensity = 1f;
        [SerializeField]
        private float m_normalizedGhostIntensity = 0.8f;
        [SerializeField]
        private float m_normalizedHaloIntensity = 0.1f;
        [SerializeField]
        private bool m_applyLensFlare = true;
        [SerializeField]
        private int m_lensFlareGhostAmount = 3;
        [SerializeField]
        private Vector4 m_lensFlareGhostsParams = new Vector4(0.8f, 0.228f, 1f, 4f);
        [SerializeField]
        private float m_lensFlareGhostChrDistortion = 2f;
        [SerializeField]
        private Gradient m_lensGradient = new Gradient();
        [SerializeField]
        private Texture2D m_lensFlareGradTexture;
        private Color[] m_lensFlareGradColor = new Color[0x100];
        [SerializeField]
        private Vector4 m_lensFlareHaloParams = new Vector4(0.1f, 0.573f, 1f, 128f);
        [SerializeField]
        private float m_lensFlareHaloChrDistortion = 1.51f;
        [SerializeField]
        private int m_lensFlareGaussianBlurAmount = 1;

        public AmplifyLensFlare()
        {
            GradientColorKey[] colorKeys = new GradientColorKey[] { new GradientColorKey(Color.white, 0f), new GradientColorKey(Color.blue, 0.25f), new GradientColorKey(Color.green, 0.5f), new GradientColorKey(Color.yellow, 0.75f), new GradientColorKey(Color.red, 1f) };
            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(1f, 0.25f), new GradientAlphaKey(1f, 0.5f), new GradientAlphaKey(1f, 0.75f), new GradientAlphaKey(1f, 1f) };
            this.m_lensGradient.SetKeys(colorKeys, alphaKeys);
        }

        public RenderTexture ApplyFlare(Material material, RenderTexture source)
        {
            RenderTexture tempRenderTarget = AmplifyUtils.GetTempRenderTarget(source.width, source.height);
            material.SetVector(AmplifyUtils.LensFlareGhostsParamsId, this.m_lensFlareGhostsParams);
            material.SetTexture(AmplifyUtils.LensFlareLUTId, this.m_lensFlareGradTexture);
            material.SetVector(AmplifyUtils.LensFlareHaloParamsId, this.m_lensFlareHaloParams);
            material.SetFloat(AmplifyUtils.LensFlareGhostChrDistortionId, this.m_lensFlareGhostChrDistortion);
            material.SetFloat(AmplifyUtils.LensFlareHaloChrDistortionId, this.m_lensFlareHaloChrDistortion);
            Graphics.Blit(source, tempRenderTarget, material, 3 + this.m_lensFlareGhostAmount);
            return tempRenderTarget;
        }

        public void CreateLUTexture()
        {
            this.m_lensFlareGradTexture = new Texture2D(0x100, 1, TextureFormat.ARGB32, false);
            this.m_lensFlareGradTexture.filterMode = FilterMode.Bilinear;
            this.TextureFromGradient();
        }

        public void Destroy()
        {
            if (this.m_lensFlareGradTexture != null)
            {
                Object.DestroyImmediate(this.m_lensFlareGradTexture);
                this.m_lensFlareGradTexture = null;
            }
        }

        public void TextureFromGradient()
        {
            for (int i = 0; i < 0x100; i++)
            {
                this.m_lensFlareGradColor[i] = this.m_lensGradient.Evaluate(((float) i) / 255f);
            }
            this.m_lensFlareGradTexture.SetPixels(this.m_lensFlareGradColor);
            this.m_lensFlareGradTexture.Apply();
        }

        public bool ApplyLensFlare
        {
            get => 
                this.m_applyLensFlare;
            set => 
                this.m_applyLensFlare = value;
        }

        public float OverallIntensity
        {
            get => 
                this.m_overallIntensity;
            set
            {
                this.m_overallIntensity = (value >= 0f) ? value : 0f;
                this.m_lensFlareGhostsParams.x = value * this.m_normalizedGhostIntensity;
                this.m_lensFlareHaloParams.x = value * this.m_normalizedHaloIntensity;
            }
        }

        public int LensFlareGhostAmount
        {
            get => 
                this.m_lensFlareGhostAmount;
            set => 
                this.m_lensFlareGhostAmount = value;
        }

        public Vector4 LensFlareGhostsParams
        {
            get => 
                this.m_lensFlareGhostsParams;
            set => 
                this.m_lensFlareGhostsParams = value;
        }

        public float LensFlareNormalizedGhostsIntensity
        {
            get => 
                this.m_normalizedGhostIntensity;
            set
            {
                this.m_normalizedGhostIntensity = (value >= 0f) ? value : 0f;
                this.m_lensFlareGhostsParams.x = this.m_overallIntensity * this.m_normalizedGhostIntensity;
            }
        }

        public float LensFlareGhostsIntensity
        {
            get => 
                this.m_lensFlareGhostsParams.x;
            set => 
                this.m_lensFlareGhostsParams.x = (value >= 0f) ? value : 0f;
        }

        public float LensFlareGhostsDispersal
        {
            get => 
                this.m_lensFlareGhostsParams.y;
            set => 
                this.m_lensFlareGhostsParams.y = value;
        }

        public float LensFlareGhostsPowerFactor
        {
            get => 
                this.m_lensFlareGhostsParams.z;
            set => 
                this.m_lensFlareGhostsParams.z = value;
        }

        public float LensFlareGhostsPowerFalloff
        {
            get => 
                this.m_lensFlareGhostsParams.w;
            set => 
                this.m_lensFlareGhostsParams.w = value;
        }

        public Gradient LensFlareGradient
        {
            get => 
                this.m_lensGradient;
            set => 
                this.m_lensGradient = value;
        }

        public Vector4 LensFlareHaloParams
        {
            get => 
                this.m_lensFlareHaloParams;
            set => 
                this.m_lensFlareHaloParams = value;
        }

        public float LensFlareNormalizedHaloIntensity
        {
            get => 
                this.m_normalizedHaloIntensity;
            set
            {
                this.m_normalizedHaloIntensity = (value >= 0f) ? value : 0f;
                this.m_lensFlareHaloParams.x = this.m_overallIntensity * this.m_normalizedHaloIntensity;
            }
        }

        public float LensFlareHaloIntensity
        {
            get => 
                this.m_lensFlareHaloParams.x;
            set => 
                this.m_lensFlareHaloParams.x = (value >= 0f) ? value : 0f;
        }

        public float LensFlareHaloWidth
        {
            get => 
                this.m_lensFlareHaloParams.y;
            set => 
                this.m_lensFlareHaloParams.y = value;
        }

        public float LensFlareHaloPowerFactor
        {
            get => 
                this.m_lensFlareHaloParams.z;
            set => 
                this.m_lensFlareHaloParams.z = value;
        }

        public float LensFlareHaloPowerFalloff
        {
            get => 
                this.m_lensFlareHaloParams.w;
            set => 
                this.m_lensFlareHaloParams.w = value;
        }

        public float LensFlareGhostChrDistortion
        {
            get => 
                this.m_lensFlareGhostChrDistortion;
            set => 
                this.m_lensFlareGhostChrDistortion = value;
        }

        public float LensFlareHaloChrDistortion
        {
            get => 
                this.m_lensFlareHaloChrDistortion;
            set => 
                this.m_lensFlareHaloChrDistortion = value;
        }

        public int LensFlareGaussianBlurAmount
        {
            get => 
                this.m_lensFlareGaussianBlurAmount;
            set => 
                this.m_lensFlareGaussianBlurAmount = value;
        }
    }
}

