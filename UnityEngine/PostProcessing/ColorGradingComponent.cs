namespace UnityEngine.PostProcessing
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public sealed class ColorGradingComponent : PostProcessingComponentRenderTexture<ColorGradingModel>
    {
        private const int k_InternalLogLutSize = 0x20;
        private const int k_CurvePrecision = 0x80;
        private const float k_CurveStep = 0.0078125f;
        private Texture2D m_GradingCurves;
        private Color[] m_pixels = new Color[0x100];

        private Vector3 CalculateColorBalance(float temperature, float tint)
        {
            float num = temperature / 55f;
            float num2 = tint / 55f;
            float x = 0.31271f - (num * ((num >= 0f) ? 0.05f : 0.1f));
            Vector3 vector = new Vector3(0.949237f, 1.03542f, 1.08728f);
            Vector3 vector2 = this.CIExyToLMS(x, this.StandardIlluminantY(x) + (num2 * 0.05f));
            return new Vector3(vector.x / vector2.x, vector.y / vector2.y, vector.z / vector2.z);
        }

        public static void CalculateLiftGammaGain(Color lift, Color gamma, Color gain, out Vector3 outLift, out Vector3 outGamma, out Vector3 outGain)
        {
            outLift = GetLiftValue(lift);
            outGamma = GetGammaValue(gamma);
            outGain = GetGainValue(gain);
        }

        public static void CalculateSlopePowerOffset(Color slope, Color power, Color offset, out Vector3 outSlope, out Vector3 outPower, out Vector3 outOffset)
        {
            outSlope = GetSlopeValue(slope);
            outPower = GetPowerValue(power);
            outOffset = GetOffsetValue(offset);
        }

        private Vector3 CIExyToLMS(float x, float y)
        {
            float num = 1f;
            float num2 = (num * x) / y;
            float num3 = (num * ((1f - x) - y)) / y;
            return new Vector3(((0.7328f * num2) + (0.4296f * num)) - (0.1624f * num3), ((-0.7036f * num2) + (1.6975f * num)) + (0.0061f * num3), ((0.003f * num2) + (0.0136f * num)) + (0.9834f * num3));
        }

        private static Vector3 ClampVector(Vector3 v, float min, float max) => 
            new Vector3(Mathf.Clamp(v.x, min, max), Mathf.Clamp(v.y, min, max), Mathf.Clamp(v.z, min, max));

        private void GenerateLut()
        {
            Vector3 vector;
            Vector3 vector2;
            Vector3 vector3;
            Vector3 vector4;
            Vector3 vector5;
            Vector3 vector6;
            ColorGradingModel.Settings settings = base.model.settings;
            if (!this.IsLogLutValid(base.model.bakedLut))
            {
                GraphicsUtils.Destroy(base.model.bakedLut);
                RenderTexture texture = new RenderTexture(0x400, 0x20, 0, RenderTextureFormat.ARGBHalf) {
                    name = "Color Grading Log LUT",
                    hideFlags = HideFlags.DontSave,
                    filterMode = FilterMode.Bilinear,
                    wrapMode = TextureWrapMode.Clamp,
                    anisoLevel = 0
                };
                base.model.bakedLut = texture;
            }
            Material mat = base.context.materialFactory.Get("Hidden/Post FX/Lut Generator");
            mat.SetVector(Uniforms._LutParams, new Vector4(32f, 0.0004882813f, 0.015625f, 1.032258f));
            mat.shaderKeywords = null;
            ColorGradingModel.TonemappingSettings tonemapping = settings.tonemapping;
            ColorGradingModel.Tonemapper tonemapper = tonemapping.tonemapper;
            if (tonemapper != ColorGradingModel.Tonemapper.Neutral)
            {
                if (tonemapper == ColorGradingModel.Tonemapper.ACES)
                {
                    mat.EnableKeyword("TONEMAPPING_FILMIC");
                }
            }
            else
            {
                mat.EnableKeyword("TONEMAPPING_NEUTRAL");
                float t = ((tonemapping.neutralBlackIn * 20f) + 1f) / ((tonemapping.neutralBlackOut * 10f) + 1f);
                float y = Mathf.Max(0f, Mathf.LerpUnclamped(0.57f, 0.37f, t));
                mat.SetVector(Uniforms._NeutralTonemapperParams1, new Vector4(0.2f, y, Mathf.LerpUnclamped(0.01f, 0.24f, (tonemapping.neutralWhiteIn / 20f) / (1f - (tonemapping.neutralWhiteOut / 20f))), Mathf.Max(0f, Mathf.LerpUnclamped(0.02f, 0.2f, t))));
                mat.SetVector(Uniforms._NeutralTonemapperParams2, new Vector4(0.02f, 0.3f, tonemapping.neutralWhiteLevel, tonemapping.neutralWhiteClip / 10f));
            }
            mat.SetFloat(Uniforms._HueShift, settings.basic.hueShift / 360f);
            mat.SetFloat(Uniforms._Saturation, settings.basic.saturation);
            mat.SetFloat(Uniforms._Contrast, settings.basic.contrast);
            mat.SetVector(Uniforms._Balance, this.CalculateColorBalance(settings.basic.temperature, settings.basic.tint));
            CalculateLiftGammaGain(settings.colorWheels.linear.lift, settings.colorWheels.linear.gamma, settings.colorWheels.linear.gain, out vector, out vector2, out vector3);
            mat.SetVector(Uniforms._Lift, vector);
            mat.SetVector(Uniforms._InvGamma, vector2);
            mat.SetVector(Uniforms._Gain, vector3);
            CalculateSlopePowerOffset(settings.colorWheels.log.slope, settings.colorWheels.log.power, settings.colorWheels.log.offset, out vector4, out vector5, out vector6);
            mat.SetVector(Uniforms._Slope, vector4);
            mat.SetVector(Uniforms._Power, vector5);
            mat.SetVector(Uniforms._Offset, vector6);
            mat.SetVector(Uniforms._ChannelMixerRed, settings.channelMixer.red);
            mat.SetVector(Uniforms._ChannelMixerGreen, settings.channelMixer.green);
            mat.SetVector(Uniforms._ChannelMixerBlue, settings.channelMixer.blue);
            mat.SetTexture(Uniforms._Curves, this.GetCurveTexture());
            Graphics.Blit(null, base.model.bakedLut, mat, 0);
        }

        private Texture2D GetCurveTexture()
        {
            if (this.m_GradingCurves == null)
            {
                Texture2D textured = new Texture2D(0x80, 2, TextureFormat.RGBAHalf, false, true) {
                    name = "Internal Curves Texture",
                    hideFlags = HideFlags.DontSave,
                    anisoLevel = 0,
                    wrapMode = TextureWrapMode.Clamp,
                    filterMode = FilterMode.Bilinear
                };
                this.m_GradingCurves = textured;
            }
            ColorGradingModel.CurvesSettings curves = base.model.settings.curves;
            curves.hueVShue.Cache();
            curves.hueVSsat.Cache();
            for (int i = 0; i < 0x80; i++)
            {
                float t = i * 0.0078125f;
                float r = curves.hueVShue.Evaluate(t);
                float g = curves.hueVSsat.Evaluate(t);
                float b = curves.satVSsat.Evaluate(t);
                float a = curves.lumVSsat.Evaluate(t);
                this.m_pixels[i] = new Color(r, g, b, a);
                float num7 = curves.master.Evaluate(t);
                float num8 = curves.red.Evaluate(t);
                float num9 = curves.green.Evaluate(t);
                float num10 = curves.blue.Evaluate(t);
                this.m_pixels[i + 0x80] = new Color(num8, num9, num10, num7);
            }
            this.m_GradingCurves.SetPixels(this.m_pixels);
            this.m_GradingCurves.Apply(false, false);
            return this.m_GradingCurves;
        }

        public static unsafe Vector3 GetGainValue(Color gain)
        {
            Color color = NormalizeColor(gain);
            float num = ((color.r + color.g) + color.b) / 3f;
            Color* colorPtr1 = &gain;
            colorPtr1->a *= (gain.a <= 0f) ? 1f : 3f;
            return ClampVector(new Vector3(Mathf.Pow(2f, (color.r - num) * 0.5f) + gain.a, Mathf.Pow(2f, (color.g - num) * 0.5f) + gain.a, Mathf.Pow(2f, (color.b - num) * 0.5f) + gain.a), 0f, 4f);
        }

        public static unsafe Vector3 GetGammaValue(Color gamma)
        {
            Color color = NormalizeColor(gamma);
            float num = ((color.r + color.g) + color.b) / 3f;
            Color* colorPtr1 = &gamma;
            colorPtr1->a *= (gamma.a >= 0f) ? 5f : 0.8f;
            return ClampVector(new Vector3(1f / Mathf.Max((float) 0.01f, (float) (Mathf.Pow(2f, (color.r - num) * 0.5f) + gamma.a)), 1f / Mathf.Max((float) 0.01f, (float) (Mathf.Pow(2f, (color.g - num) * 0.5f) + gamma.a)), 1f / Mathf.Max((float) 0.01f, (float) (Mathf.Pow(2f, (color.b - num) * 0.5f) + gamma.a))), 0f, 5f);
        }

        public static Vector3 GetLiftValue(Color lift)
        {
            Color color = NormalizeColor(lift);
            float num = ((color.r + color.g) + color.b) / 3f;
            return ClampVector(new Vector3(((color.r - num) * 0.1f) + lift.a, ((color.g - num) * 0.1f) + lift.a, ((color.b - num) * 0.1f) + lift.a), -1f, 1f);
        }

        public static unsafe Vector3 GetOffsetValue(Color offset)
        {
            Color color = NormalizeColor(offset);
            float num = ((color.r + color.g) + color.b) / 3f;
            Color* colorPtr1 = &offset;
            colorPtr1->a *= 0.5f;
            return ClampVector(new Vector3(((color.r - num) * 0.05f) + offset.a, ((color.g - num) * 0.05f) + offset.a, ((color.b - num) * 0.05f) + offset.a), -0.8f, 0.8f);
        }

        public static unsafe Vector3 GetPowerValue(Color power)
        {
            Color color = NormalizeColor(power);
            float num = ((color.r + color.g) + color.b) / 3f;
            Color* colorPtr1 = &power;
            colorPtr1->a *= 0.5f;
            return ClampVector(new Vector3(1f / Mathf.Max((float) 0.01f, (float) ((((color.r - num) * 0.1f) + power.a) + 1f)), 1f / Mathf.Max((float) 0.01f, (float) ((((color.g - num) * 0.1f) + power.a) + 1f)), 1f / Mathf.Max((float) 0.01f, (float) ((((color.b - num) * 0.1f) + power.a) + 1f))), 0.5f, 2.5f);
        }

        public static unsafe Vector3 GetSlopeValue(Color slope)
        {
            Color color = NormalizeColor(slope);
            float num = ((color.r + color.g) + color.b) / 3f;
            Color* colorPtr1 = &slope;
            colorPtr1->a *= 0.5f;
            return ClampVector(new Vector3((((color.r - num) * 0.1f) + slope.a) + 1f, (((color.g - num) * 0.1f) + slope.a) + 1f, (((color.b - num) * 0.1f) + slope.a) + 1f), 0f, 2f);
        }

        private bool IsLogLutValid(RenderTexture lut) => 
            ((lut != null) && lut.IsCreated()) && (lut.height == 0x20);

        private static Color NormalizeColor(Color c)
        {
            float a = ((c.r + c.g) + c.b) / 3f;
            if (Mathf.Approximately(a, 0f))
            {
                return new Color(1f, 1f, 1f, c.a);
            }
            return new Color { 
                r = c.r / a,
                g = c.g / a,
                b = c.b / a,
                a = c.a
            };
        }

        public override void OnDisable()
        {
            GraphicsUtils.Destroy(this.m_GradingCurves);
            GraphicsUtils.Destroy(base.model.bakedLut);
            this.m_GradingCurves = null;
            base.model.bakedLut = null;
        }

        public void OnGUI()
        {
            RenderTexture bakedLut = base.model.bakedLut;
            Rect position = new Rect((base.context.viewport.x * Screen.width) + 8f, 8f, (float) bakedLut.width, (float) bakedLut.height);
            GUI.DrawTexture(position, bakedLut);
        }

        public override void Prepare(Material uberMaterial)
        {
            if (base.model.isDirty || !this.IsLogLutValid(base.model.bakedLut))
            {
                this.GenerateLut();
                base.model.isDirty = false;
            }
            uberMaterial.EnableKeyword(!base.context.profile.debugViews.IsModeActive(BuiltinDebugViewsModel.Mode.PreGradingLog) ? "COLOR_GRADING" : "COLOR_GRADING_LOG_VIEW");
            RenderTexture bakedLut = base.model.bakedLut;
            uberMaterial.SetTexture(Uniforms._LogLut, bakedLut);
            uberMaterial.SetVector(Uniforms._LogLut_Params, new Vector3(1f / ((float) bakedLut.width), 1f / ((float) bakedLut.height), bakedLut.height - 1f));
            float num = Mathf.Exp(base.model.settings.basic.postExposure * 0.6931472f);
            uberMaterial.SetFloat(Uniforms._ExposureEV, num);
        }

        private float StandardIlluminantY(float x) => 
            ((2.87f * x) - ((3f * x) * x)) - 0.2750951f;

        public override bool active =>
            base.model.enabled && !base.context.interrupted;

        private static class Uniforms
        {
            internal static readonly int _LutParams = Shader.PropertyToID("_LutParams");
            internal static readonly int _NeutralTonemapperParams1 = Shader.PropertyToID("_NeutralTonemapperParams1");
            internal static readonly int _NeutralTonemapperParams2 = Shader.PropertyToID("_NeutralTonemapperParams2");
            internal static readonly int _HueShift = Shader.PropertyToID("_HueShift");
            internal static readonly int _Saturation = Shader.PropertyToID("_Saturation");
            internal static readonly int _Contrast = Shader.PropertyToID("_Contrast");
            internal static readonly int _Balance = Shader.PropertyToID("_Balance");
            internal static readonly int _Lift = Shader.PropertyToID("_Lift");
            internal static readonly int _InvGamma = Shader.PropertyToID("_InvGamma");
            internal static readonly int _Gain = Shader.PropertyToID("_Gain");
            internal static readonly int _Slope = Shader.PropertyToID("_Slope");
            internal static readonly int _Power = Shader.PropertyToID("_Power");
            internal static readonly int _Offset = Shader.PropertyToID("_Offset");
            internal static readonly int _ChannelMixerRed = Shader.PropertyToID("_ChannelMixerRed");
            internal static readonly int _ChannelMixerGreen = Shader.PropertyToID("_ChannelMixerGreen");
            internal static readonly int _ChannelMixerBlue = Shader.PropertyToID("_ChannelMixerBlue");
            internal static readonly int _Curves = Shader.PropertyToID("_Curves");
            internal static readonly int _LogLut = Shader.PropertyToID("_LogLut");
            internal static readonly int _LogLut_Params = Shader.PropertyToID("_LogLut_Params");
            internal static readonly int _ExposureEV = Shader.PropertyToID("_ExposureEV");
        }
    }
}

