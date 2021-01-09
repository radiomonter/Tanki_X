﻿namespace UnityEngine.PostProcessing
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [Serializable]
    public class GrainModel : PostProcessingModel
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
        public struct Settings
        {
            [Tooltip("Enable the use of colored grain.")]
            public bool colored;
            [Range(0f, 1f), Tooltip("Grain strength. Higher means more visible grain.")]
            public float intensity;
            [Range(0.3f, 3f), Tooltip("Grain particle size.")]
            public float size;
            [Range(0f, 1f), Tooltip("Controls the noisiness response curve based on scene luminance. Lower values mean less noise in dark areas.")]
            public float luminanceContribution;
            public static GrainModel.Settings defaultSettings =>
                new GrainModel.Settings { 
                    colored=true,
                    intensity=0.5f,
                    size=1f,
                    luminanceContribution=0.8f
                };
        }
    }
}

