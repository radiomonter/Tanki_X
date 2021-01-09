namespace UnityEngine.PostProcessing
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [Serializable]
    public class AntialiasingModel : PostProcessingModel
    {
        [SerializeField]
        private Settings m_Settings = Settings.defaultSettings;

        public override void Reset()
        {
            this.m_Settings = Settings.defaultSettings;
        }

        public Settings settings
        {
            get => 
                this.m_Settings;
            set => 
                this.m_Settings = value;
        }

        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct FxaaConsoleSettings
        {
            [Tooltip("The amount of spread applied to the sampling coordinates while sampling for subpixel information."), Range(0.33f, 0.5f)]
            public float subpixelSpreadAmount;
            [Tooltip("This value dictates how sharp the edges in the image are kept; a higher value implies sharper edges."), Range(2f, 8f)]
            public float edgeSharpnessAmount;
            [Tooltip("The minimum amount of local contrast required to qualify a region as containing an edge."), Range(0.125f, 0.25f)]
            public float edgeDetectionThreshold;
            [Tooltip("Local contrast adaptation value to disallow the algorithm from executing on the darker regions."), Range(0.04f, 0.06f)]
            public float minimumRequiredLuminance;
            public static AntialiasingModel.FxaaConsoleSettings[] presets;
            static FxaaConsoleSettings()
            {
                AntialiasingModel.FxaaConsoleSettings[] settingsArray1 = new AntialiasingModel.FxaaConsoleSettings[5];
                AntialiasingModel.FxaaConsoleSettings settings = new AntialiasingModel.FxaaConsoleSettings {
                    subpixelSpreadAmount = 0.33f,
                    edgeSharpnessAmount = 8f,
                    edgeDetectionThreshold = 0.25f,
                    minimumRequiredLuminance = 0.06f
                };
                settingsArray1[0] = settings;
                AntialiasingModel.FxaaConsoleSettings settings2 = new AntialiasingModel.FxaaConsoleSettings {
                    subpixelSpreadAmount = 0.33f,
                    edgeSharpnessAmount = 8f,
                    edgeDetectionThreshold = 0.125f,
                    minimumRequiredLuminance = 0.06f
                };
                settingsArray1[1] = settings2;
                AntialiasingModel.FxaaConsoleSettings settings3 = new AntialiasingModel.FxaaConsoleSettings {
                    subpixelSpreadAmount = 0.5f,
                    edgeSharpnessAmount = 8f,
                    edgeDetectionThreshold = 0.125f,
                    minimumRequiredLuminance = 0.05f
                };
                settingsArray1[2] = settings3;
                AntialiasingModel.FxaaConsoleSettings settings4 = new AntialiasingModel.FxaaConsoleSettings {
                    subpixelSpreadAmount = 0.5f,
                    edgeSharpnessAmount = 4f,
                    edgeDetectionThreshold = 0.125f,
                    minimumRequiredLuminance = 0.04f
                };
                settingsArray1[3] = settings4;
                AntialiasingModel.FxaaConsoleSettings settings5 = new AntialiasingModel.FxaaConsoleSettings {
                    subpixelSpreadAmount = 0.5f,
                    edgeSharpnessAmount = 2f,
                    edgeDetectionThreshold = 0.125f,
                    minimumRequiredLuminance = 0.04f
                };
                settingsArray1[4] = settings5;
                presets = settingsArray1;
            }
        }

        public enum FxaaPreset
        {
            ExtremePerformance,
            Performance,
            Default,
            Quality,
            ExtremeQuality
        }

        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct FxaaQualitySettings
        {
            [Tooltip("The amount of desired sub-pixel aliasing removal. Effects the sharpeness of the output."), Range(0f, 1f)]
            public float subpixelAliasingRemovalAmount;
            [Tooltip("The minimum amount of local contrast required to qualify a region as containing an edge."), Range(0.063f, 0.333f)]
            public float edgeDetectionThreshold;
            [Tooltip("Local contrast adaptation value to disallow the algorithm from executing on the darker regions."), Range(0f, 0.0833f)]
            public float minimumRequiredLuminance;
            public static AntialiasingModel.FxaaQualitySettings[] presets;
            static FxaaQualitySettings()
            {
                AntialiasingModel.FxaaQualitySettings[] settingsArray1 = new AntialiasingModel.FxaaQualitySettings[5];
                AntialiasingModel.FxaaQualitySettings settings = new AntialiasingModel.FxaaQualitySettings {
                    subpixelAliasingRemovalAmount = 0f,
                    edgeDetectionThreshold = 0.333f,
                    minimumRequiredLuminance = 0.0833f
                };
                settingsArray1[0] = settings;
                AntialiasingModel.FxaaQualitySettings settings2 = new AntialiasingModel.FxaaQualitySettings {
                    subpixelAliasingRemovalAmount = 0.25f,
                    edgeDetectionThreshold = 0.25f,
                    minimumRequiredLuminance = 0.0833f
                };
                settingsArray1[1] = settings2;
                AntialiasingModel.FxaaQualitySettings settings3 = new AntialiasingModel.FxaaQualitySettings {
                    subpixelAliasingRemovalAmount = 0.75f,
                    edgeDetectionThreshold = 0.166f,
                    minimumRequiredLuminance = 0.0833f
                };
                settingsArray1[2] = settings3;
                AntialiasingModel.FxaaQualitySettings settings4 = new AntialiasingModel.FxaaQualitySettings {
                    subpixelAliasingRemovalAmount = 1f,
                    edgeDetectionThreshold = 0.125f,
                    minimumRequiredLuminance = 0.0625f
                };
                settingsArray1[3] = settings4;
                AntialiasingModel.FxaaQualitySettings settings5 = new AntialiasingModel.FxaaQualitySettings {
                    subpixelAliasingRemovalAmount = 1f,
                    edgeDetectionThreshold = 0.063f,
                    minimumRequiredLuminance = 0.0312f
                };
                settingsArray1[4] = settings5;
                presets = settingsArray1;
            }
        }

        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct FxaaSettings
        {
            public AntialiasingModel.FxaaPreset preset;
            public static AntialiasingModel.FxaaSettings defaultSettings =>
                new AntialiasingModel.FxaaSettings { preset=AntialiasingModel.FxaaPreset.Default };
        }

        public enum Method
        {
            Fxaa,
            Taa
        }

        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct Settings
        {
            public AntialiasingModel.Method method;
            public AntialiasingModel.FxaaSettings fxaaSettings;
            public AntialiasingModel.TaaSettings taaSettings;
            public static AntialiasingModel.Settings defaultSettings =>
                new AntialiasingModel.Settings { 
                    method=AntialiasingModel.Method.Fxaa,
                    fxaaSettings=AntialiasingModel.FxaaSettings.defaultSettings,
                    taaSettings=AntialiasingModel.TaaSettings.defaultSettings
                };
        }

        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct TaaSettings
        {
            [Tooltip("The diameter (in texels) inside which jitter samples are spread. Smaller values result in crisper but more aliased output, while larger values result in more stable but blurrier output."), Range(0.1f, 1f)]
            public float jitterSpread;
            [Tooltip("Controls the amount of sharpening applied to the color buffer."), Range(0f, 3f)]
            public float sharpen;
            [Tooltip("The blend coefficient for a stationary fragment. Controls the percentage of history sample blended into the final color."), Range(0f, 0.99f)]
            public float stationaryBlending;
            [Tooltip("The blend coefficient for a fragment with significant motion. Controls the percentage of history sample blended into the final color."), Range(0f, 0.99f)]
            public float motionBlending;
            public static AntialiasingModel.TaaSettings defaultSettings =>
                new AntialiasingModel.TaaSettings { 
                    jitterSpread=0.75f,
                    sharpen=0.3f,
                    stationaryBlending=0.95f,
                    motionBlending=0.85f
                };
        }
    }
}

