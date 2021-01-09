namespace UnityEngine.PostProcessing
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [Serializable]
    public class DitheringModel : PostProcessingModel
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

        [Serializable, StructLayout(LayoutKind.Sequential, Size=1)]
        public struct Settings
        {
            public static DitheringModel.Settings defaultSettings =>
                new DitheringModel.Settings();
        }
    }
}

