namespace UnityStandardAssets.ImageEffects
{
    using System;
    using UnityEngine;

    [ExecuteInEditMode, AddComponentMenu("Image Effects/Color Adjustments/Color Correction (Curves, Saturation)")]
    public class ColorCorrectionCurves : PostEffectsBase
    {
        public AnimationCurve redChannel;
        public AnimationCurve greenChannel;
        public AnimationCurve blueChannel;
        public bool useDepthCorrection;
        public AnimationCurve zCurve;
        public AnimationCurve depthRedChannel;
        public AnimationCurve depthGreenChannel;
        public AnimationCurve depthBlueChannel;
        private Material ccMaterial;
        private Material ccDepthMaterial;
        private Material selectiveCcMaterial;
        private Texture2D rgbChannelTex;
        private Texture2D rgbDepthChannelTex;
        private Texture2D zCurveTex;
        public float saturation;
        public bool selectiveCc;
        public Color selectiveFromColor;
        public Color selectiveToColor;
        public ColorCorrectionMode mode;
        public bool updateTextures;
        public Shader colorCorrectionCurvesShader;
        public Shader simpleColorCorrectionCurvesShader;
        public Shader colorCorrectionSelectiveShader;
        private bool updateTexturesOnStartup;

        public ColorCorrectionCurves()
        {
            Keyframe[] keys = new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(1f, 1f) };
            this.redChannel = new AnimationCurve(keys);
            Keyframe[] keyframeArray2 = new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(1f, 1f) };
            this.greenChannel = new AnimationCurve(keyframeArray2);
            Keyframe[] keyframeArray3 = new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(1f, 1f) };
            this.blueChannel = new AnimationCurve(keyframeArray3);
            Keyframe[] keyframeArray4 = new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(1f, 1f) };
            this.zCurve = new AnimationCurve(keyframeArray4);
            Keyframe[] keyframeArray5 = new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(1f, 1f) };
            this.depthRedChannel = new AnimationCurve(keyframeArray5);
            Keyframe[] keyframeArray6 = new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(1f, 1f) };
            this.depthGreenChannel = new AnimationCurve(keyframeArray6);
            Keyframe[] keyframeArray7 = new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(1f, 1f) };
            this.depthBlueChannel = new AnimationCurve(keyframeArray7);
            this.saturation = 1f;
            this.selectiveFromColor = Color.white;
            this.selectiveToColor = Color.white;
            this.updateTextures = true;
            this.updateTexturesOnStartup = true;
        }

        private void Awake()
        {
        }

        public override bool CheckResources()
        {
            base.CheckSupport(this.mode == ColorCorrectionMode.Advanced);
            this.ccMaterial = base.CheckShaderAndCreateMaterial(this.simpleColorCorrectionCurvesShader, this.ccMaterial);
            this.ccDepthMaterial = base.CheckShaderAndCreateMaterial(this.colorCorrectionCurvesShader, this.ccDepthMaterial);
            this.selectiveCcMaterial = base.CheckShaderAndCreateMaterial(this.colorCorrectionSelectiveShader, this.selectiveCcMaterial);
            if (!this.rgbChannelTex)
            {
                this.rgbChannelTex = new Texture2D(0x100, 4, TextureFormat.ARGB32, false, true);
            }
            if (!this.rgbDepthChannelTex)
            {
                this.rgbDepthChannelTex = new Texture2D(0x100, 4, TextureFormat.ARGB32, false, true);
            }
            if (!this.zCurveTex)
            {
                this.zCurveTex = new Texture2D(0x100, 1, TextureFormat.ARGB32, false, true);
            }
            this.rgbChannelTex.hideFlags = HideFlags.DontSave;
            this.rgbDepthChannelTex.hideFlags = HideFlags.DontSave;
            this.zCurveTex.hideFlags = HideFlags.DontSave;
            this.rgbChannelTex.wrapMode = TextureWrapMode.Clamp;
            this.rgbDepthChannelTex.wrapMode = TextureWrapMode.Clamp;
            this.zCurveTex.wrapMode = TextureWrapMode.Clamp;
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
                if (this.updateTexturesOnStartup)
                {
                    this.UpdateParameters();
                    this.updateTexturesOnStartup = false;
                }
                if (this.useDepthCorrection)
                {
                    Camera component = base.GetComponent<Camera>();
                    component.depthTextureMode |= DepthTextureMode.Depth;
                }
                RenderTexture dest = destination;
                if (this.selectiveCc)
                {
                    dest = RenderTexture.GetTemporary(source.width, source.height);
                }
                if (!this.useDepthCorrection)
                {
                    this.ccMaterial.SetTexture("_RgbTex", this.rgbChannelTex);
                    this.ccMaterial.SetFloat("_Saturation", this.saturation);
                    Graphics.Blit(source, dest, this.ccMaterial);
                }
                else
                {
                    this.ccDepthMaterial.SetTexture("_RgbTex", this.rgbChannelTex);
                    this.ccDepthMaterial.SetTexture("_ZCurve", this.zCurveTex);
                    this.ccDepthMaterial.SetTexture("_RgbDepthTex", this.rgbDepthChannelTex);
                    this.ccDepthMaterial.SetFloat("_Saturation", this.saturation);
                    Graphics.Blit(source, dest, this.ccDepthMaterial);
                }
                if (this.selectiveCc)
                {
                    this.selectiveCcMaterial.SetColor("selColor", this.selectiveFromColor);
                    this.selectiveCcMaterial.SetColor("targetColor", this.selectiveToColor);
                    Graphics.Blit(dest, destination, this.selectiveCcMaterial);
                    RenderTexture.ReleaseTemporary(dest);
                }
            }
        }

        private void Start()
        {
            base.Start();
            this.updateTexturesOnStartup = true;
        }

        public void UpdateParameters()
        {
            this.CheckResources();
            if ((this.redChannel != null) && ((this.greenChannel != null) && (this.blueChannel != null)))
            {
                float time = 0f;
                while (true)
                {
                    if (time > 1f)
                    {
                        this.rgbChannelTex.Apply();
                        this.rgbDepthChannelTex.Apply();
                        this.zCurveTex.Apply();
                        break;
                    }
                    float r = Mathf.Clamp(this.redChannel.Evaluate(time), 0f, 1f);
                    float num3 = Mathf.Clamp(this.greenChannel.Evaluate(time), 0f, 1f);
                    float num4 = Mathf.Clamp(this.blueChannel.Evaluate(time), 0f, 1f);
                    this.rgbChannelTex.SetPixel((int) Mathf.Floor(time * 255f), 0, new Color(r, r, r));
                    this.rgbChannelTex.SetPixel((int) Mathf.Floor(time * 255f), 1, new Color(num3, num3, num3));
                    this.rgbChannelTex.SetPixel((int) Mathf.Floor(time * 255f), 2, new Color(num4, num4, num4));
                    float num5 = Mathf.Clamp(this.zCurve.Evaluate(time), 0f, 1f);
                    this.zCurveTex.SetPixel((int) Mathf.Floor(time * 255f), 0, new Color(num5, num5, num5));
                    r = Mathf.Clamp(this.depthRedChannel.Evaluate(time), 0f, 1f);
                    num3 = Mathf.Clamp(this.depthGreenChannel.Evaluate(time), 0f, 1f);
                    num4 = Mathf.Clamp(this.depthBlueChannel.Evaluate(time), 0f, 1f);
                    this.rgbDepthChannelTex.SetPixel((int) Mathf.Floor(time * 255f), 0, new Color(r, r, r));
                    this.rgbDepthChannelTex.SetPixel((int) Mathf.Floor(time * 255f), 1, new Color(num3, num3, num3));
                    this.rgbDepthChannelTex.SetPixel((int) Mathf.Floor(time * 255f), 2, new Color(num4, num4, num4));
                    time += 0.003921569f;
                }
            }
        }

        private void UpdateTextures()
        {
            this.UpdateParameters();
        }

        public enum ColorCorrectionMode
        {
            Simple,
            Advanced
        }
    }
}

