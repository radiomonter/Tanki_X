namespace UnityEngine.PostProcessing
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [Serializable]
    public class MotionBlurModel : PostProcessingModel
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
            [Range(0f, 360f), Tooltip("The angle of rotary shutter. Larger values give longer exposure.")]
            public float shutterAngle;
            [Range(4f, 32f), Tooltip("The amount of sample points, which affects quality and performances.")]
            public int sampleCount;
            [Range(0f, 1f), Tooltip("The strength of multiple frame blending. The opacity of preceding frames are determined from this coefficient and time differences.")]
            public float frameBlending;
            public static MotionBlurModel.Settings defaultSettings =>
                new MotionBlurModel.Settings { 
                    shutterAngle=270f,
                    sampleCount=10,
                    frameBlending=0f
                };
        }
    }
}

