﻿namespace UnityEngine.PostProcessing
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [Serializable]
    public class ChromaticAberrationModel : PostProcessingModel
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
            [Tooltip("Shift the hue of chromatic aberrations.")]
            public Texture2D spectralTexture;
            [Range(0f, 1f), Tooltip("Amount of tangential distortion.")]
            public float intensity;
            public static ChromaticAberrationModel.Settings defaultSettings =>
                new ChromaticAberrationModel.Settings { 
                    spectralTexture=null,
                    intensity=0.1f
                };
        }
    }
}

