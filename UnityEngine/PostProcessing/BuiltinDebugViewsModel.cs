namespace UnityEngine.PostProcessing
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [Serializable]
    public class BuiltinDebugViewsModel : PostProcessingModel
    {
        [SerializeField]
        private Settings m_Settings = Settings.defaultSettings;

        public bool IsModeActive(Mode mode) => 
            this.m_Settings.mode == mode;

        public override void Reset()
        {
            this.settings = Settings.defaultSettings;
        }

        public Settings settings
        {
            get => 
                this.m_Settings;
            set => 
                this.m_Settings = value;
        }

        public bool willInterrupt =>
            (!this.IsModeActive(Mode.None) && (!this.IsModeActive(Mode.EyeAdaptation) && (!this.IsModeActive(Mode.PreGradingLog) && !this.IsModeActive(Mode.LogLut)))) && !this.IsModeActive(Mode.UserLut);

        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct DepthSettings
        {
            [Range(0f, 1f), Tooltip("Scales the camera far plane before displaying the depth map.")]
            public float scale;
            public static BuiltinDebugViewsModel.DepthSettings defaultSettings =>
                new BuiltinDebugViewsModel.DepthSettings { scale=1f };
        }

        public enum Mode
        {
            None,
            Depth,
            Normals,
            MotionVectors,
            AmbientOcclusion,
            EyeAdaptation,
            FocusPlane,
            PreGradingLog,
            LogLut,
            UserLut
        }

        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct MotionVectorsSettings
        {
            [Range(0f, 1f), Tooltip("Opacity of the source render.")]
            public float sourceOpacity;
            [Range(0f, 1f), Tooltip("Opacity of the per-pixel motion vector colors.")]
            public float motionImageOpacity;
            [Min(0f), Tooltip("Because motion vectors are mainly very small vectors, you can use this setting to make them more visible.")]
            public float motionImageAmplitude;
            [Range(0f, 1f), Tooltip("Opacity for the motion vector arrows.")]
            public float motionVectorsOpacity;
            [Range(8f, 64f), Tooltip("The arrow density on screen.")]
            public int motionVectorsResolution;
            [Min(0f), Tooltip("Tweaks the arrows length.")]
            public float motionVectorsAmplitude;
            public static BuiltinDebugViewsModel.MotionVectorsSettings defaultSettings =>
                new BuiltinDebugViewsModel.MotionVectorsSettings { 
                    sourceOpacity=1f,
                    motionImageOpacity=0f,
                    motionImageAmplitude=16f,
                    motionVectorsOpacity=1f,
                    motionVectorsResolution=0x18,
                    motionVectorsAmplitude=64f
                };
        }

        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct Settings
        {
            public BuiltinDebugViewsModel.Mode mode;
            public BuiltinDebugViewsModel.DepthSettings depth;
            public BuiltinDebugViewsModel.MotionVectorsSettings motionVectors;
            public static BuiltinDebugViewsModel.Settings defaultSettings =>
                new BuiltinDebugViewsModel.Settings { 
                    mode=BuiltinDebugViewsModel.Mode.None,
                    depth=BuiltinDebugViewsModel.DepthSettings.defaultSettings,
                    motionVectors=BuiltinDebugViewsModel.MotionVectorsSettings.defaultSettings
                };
        }
    }
}

