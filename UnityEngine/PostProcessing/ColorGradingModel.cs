namespace UnityEngine.PostProcessing
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [Serializable]
    public class ColorGradingModel : PostProcessingModel
    {
        [SerializeField]
        private Settings m_Settings = Settings.defaultSettings;

        public override void OnValidate()
        {
            this.isDirty = true;
        }

        public override void Reset()
        {
            this.m_Settings = Settings.defaultSettings;
            this.OnValidate();
        }

        public Settings settings
        {
            get => 
                this.m_Settings;
            set
            {
                this.m_Settings = value;
                this.OnValidate();
            }
        }

        public bool isDirty { get; internal set; }

        public RenderTexture bakedLut { get; internal set; }

        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct BasicSettings
        {
            [Tooltip("Adjusts the overall exposure of the scene in EV units. This is applied after HDR effect and right before tonemapping so it won't affect previous effects in the chain.")]
            public float postExposure;
            [Range(-100f, 100f), Tooltip("Sets the white balance to a custom color temperature.")]
            public float temperature;
            [Range(-100f, 100f), Tooltip("Sets the white balance to compensate for a green or magenta tint.")]
            public float tint;
            [Range(-180f, 180f), Tooltip("Shift the hue of all colors.")]
            public float hueShift;
            [Range(0f, 2f), Tooltip("Pushes the intensity of all colors.")]
            public float saturation;
            [Range(0f, 2f), Tooltip("Expands or shrinks the overall range of tonal values.")]
            public float contrast;
            public static ColorGradingModel.BasicSettings defaultSettings =>
                new ColorGradingModel.BasicSettings { 
                    postExposure=0f,
                    temperature=0f,
                    tint=0f,
                    hueShift=0f,
                    saturation=1f,
                    contrast=1f
                };
        }

        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct ChannelMixerSettings
        {
            public Vector3 red;
            public Vector3 green;
            public Vector3 blue;
            [HideInInspector]
            public int currentEditingChannel;
            public static ColorGradingModel.ChannelMixerSettings defaultSettings =>
                new ColorGradingModel.ChannelMixerSettings { 
                    red=new Vector3(1f, 0f, 0f),
                    green=new Vector3(0f, 1f, 0f),
                    blue=new Vector3(0f, 0f, 1f),
                    currentEditingChannel=0
                };
        }

        public enum ColorWheelMode
        {
            Linear,
            Log
        }

        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct ColorWheelsSettings
        {
            public ColorGradingModel.ColorWheelMode mode;
            [TrackballGroup]
            public ColorGradingModel.LogWheelsSettings log;
            [TrackballGroup]
            public ColorGradingModel.LinearWheelsSettings linear;
            public static ColorGradingModel.ColorWheelsSettings defaultSettings =>
                new ColorGradingModel.ColorWheelsSettings { 
                    mode=ColorGradingModel.ColorWheelMode.Log,
                    log=ColorGradingModel.LogWheelsSettings.defaultSettings,
                    linear=ColorGradingModel.LinearWheelsSettings.defaultSettings
                };
        }

        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct CurvesSettings
        {
            public ColorGradingCurve master;
            public ColorGradingCurve red;
            public ColorGradingCurve green;
            public ColorGradingCurve blue;
            public ColorGradingCurve hueVShue;
            public ColorGradingCurve hueVSsat;
            public ColorGradingCurve satVSsat;
            public ColorGradingCurve lumVSsat;
            [HideInInspector]
            public int e_CurrentEditingCurve;
            [HideInInspector]
            public bool e_CurveY;
            [HideInInspector]
            public bool e_CurveR;
            [HideInInspector]
            public bool e_CurveG;
            [HideInInspector]
            public bool e_CurveB;
            public static ColorGradingModel.CurvesSettings defaultSettings
            {
                get
                {
                    ColorGradingModel.CurvesSettings settings = new ColorGradingModel.CurvesSettings();
                    Keyframe[] keys = new Keyframe[] { new Keyframe(0f, 0f, 1f, 1f), new Keyframe(1f, 1f, 1f, 1f) };
                    settings.master = new ColorGradingCurve(new AnimationCurve(keys), 0f, false, new Vector2(0f, 1f));
                    Keyframe[] keyframeArray2 = new Keyframe[] { new Keyframe(0f, 0f, 1f, 1f), new Keyframe(1f, 1f, 1f, 1f) };
                    settings.red = new ColorGradingCurve(new AnimationCurve(keyframeArray2), 0f, false, new Vector2(0f, 1f));
                    Keyframe[] keyframeArray3 = new Keyframe[] { new Keyframe(0f, 0f, 1f, 1f), new Keyframe(1f, 1f, 1f, 1f) };
                    settings.green = new ColorGradingCurve(new AnimationCurve(keyframeArray3), 0f, false, new Vector2(0f, 1f));
                    Keyframe[] keyframeArray4 = new Keyframe[] { new Keyframe(0f, 0f, 1f, 1f), new Keyframe(1f, 1f, 1f, 1f) };
                    settings.blue = new ColorGradingCurve(new AnimationCurve(keyframeArray4), 0f, false, new Vector2(0f, 1f));
                    settings.hueVShue = new ColorGradingCurve(new AnimationCurve(), 0.5f, true, new Vector2(0f, 1f));
                    settings.hueVSsat = new ColorGradingCurve(new AnimationCurve(), 0.5f, true, new Vector2(0f, 1f));
                    settings.satVSsat = new ColorGradingCurve(new AnimationCurve(), 0.5f, false, new Vector2(0f, 1f));
                    settings.lumVSsat = new ColorGradingCurve(new AnimationCurve(), 0.5f, false, new Vector2(0f, 1f));
                    settings.e_CurrentEditingCurve = 0;
                    settings.e_CurveY = true;
                    settings.e_CurveR = false;
                    settings.e_CurveG = false;
                    settings.e_CurveB = false;
                    return settings;
                }
            }
        }

        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct LinearWheelsSettings
        {
            [Trackball("GetLiftValue")]
            public Color lift;
            [Trackball("GetGammaValue")]
            public Color gamma;
            [Trackball("GetGainValue")]
            public Color gain;
            public static ColorGradingModel.LinearWheelsSettings defaultSettings =>
                new ColorGradingModel.LinearWheelsSettings { 
                    lift=Color.clear,
                    gamma=Color.clear,
                    gain=Color.clear
                };
        }

        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct LogWheelsSettings
        {
            [Trackball("GetSlopeValue")]
            public Color slope;
            [Trackball("GetPowerValue")]
            public Color power;
            [Trackball("GetOffsetValue")]
            public Color offset;
            public static ColorGradingModel.LogWheelsSettings defaultSettings =>
                new ColorGradingModel.LogWheelsSettings { 
                    slope=Color.clear,
                    power=Color.clear,
                    offset=Color.clear
                };
        }

        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct Settings
        {
            public ColorGradingModel.TonemappingSettings tonemapping;
            public ColorGradingModel.BasicSettings basic;
            public ColorGradingModel.ChannelMixerSettings channelMixer;
            public ColorGradingModel.ColorWheelsSettings colorWheels;
            public ColorGradingModel.CurvesSettings curves;
            public static ColorGradingModel.Settings defaultSettings =>
                new ColorGradingModel.Settings { 
                    tonemapping=ColorGradingModel.TonemappingSettings.defaultSettings,
                    basic=ColorGradingModel.BasicSettings.defaultSettings,
                    channelMixer=ColorGradingModel.ChannelMixerSettings.defaultSettings,
                    colorWheels=ColorGradingModel.ColorWheelsSettings.defaultSettings,
                    curves=ColorGradingModel.CurvesSettings.defaultSettings
                };
        }

        public enum Tonemapper
        {
            None,
            ACES,
            Neutral
        }

        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct TonemappingSettings
        {
            [Tooltip("Tonemapping algorithm to use at the end of the color grading process. Use \"Neutral\" if you need a customizable tonemapper or \"Filmic\" to give a standard filmic look to your scenes.")]
            public ColorGradingModel.Tonemapper tonemapper;
            [Range(-0.1f, 0.1f)]
            public float neutralBlackIn;
            [Range(1f, 20f)]
            public float neutralWhiteIn;
            [Range(-0.09f, 0.1f)]
            public float neutralBlackOut;
            [Range(1f, 19f)]
            public float neutralWhiteOut;
            [Range(0.1f, 20f)]
            public float neutralWhiteLevel;
            [Range(1f, 10f)]
            public float neutralWhiteClip;
            public static ColorGradingModel.TonemappingSettings defaultSettings =>
                new ColorGradingModel.TonemappingSettings { 
                    tonemapper=ColorGradingModel.Tonemapper.Neutral,
                    neutralBlackIn=0.02f,
                    neutralWhiteIn=10f,
                    neutralBlackOut=0f,
                    neutralWhiteOut=10f,
                    neutralWhiteLevel=5.3f,
                    neutralWhiteClip=10f
                };
        }
    }
}

