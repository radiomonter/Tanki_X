namespace UnityEngine.PostProcessing
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [Serializable]
    public class UserLutModel : PostProcessingModel
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
            [Tooltip("Custom lookup texture (strip format, e.g. 256x16).")]
            public Texture2D lut;
            [Range(0f, 1f), Tooltip("Blending factor.")]
            public float contribution;
            public static UserLutModel.Settings defaultSettings =>
                new UserLutModel.Settings { 
                    lut=null,
                    contribution=1f
                };
        }
    }
}

