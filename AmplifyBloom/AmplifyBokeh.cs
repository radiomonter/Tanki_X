namespace AmplifyBloom
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [Serializable]
    public sealed class AmplifyBokeh : IAmplifyItem, ISerializationCallbackReceiver
    {
        private const int PerPassSampleCount = 8;
        [SerializeField]
        private bool m_isActive;
        [SerializeField]
        private bool m_applyOnBloomSource;
        [SerializeField]
        private float m_bokehSampleRadius = 0.5f;
        [SerializeField]
        private Vector4 m_bokehCameraProperties = new Vector4(0.05f, 0.018f, 1.34f, 0.18f);
        [SerializeField]
        private float m_offsetRotation;
        [SerializeField]
        private AmplifyBloom.ApertureShape m_apertureShape = AmplifyBloom.ApertureShape.Hexagon;
        private List<AmplifyBokehData> m_bokehOffsets = new List<AmplifyBokehData>();

        public AmplifyBokeh()
        {
            this.CreateBokehOffsets(AmplifyBloom.ApertureShape.Hexagon);
        }

        public void ApplyBokehFilter(RenderTexture source, Material material)
        {
            for (int i = 0; i < this.m_bokehOffsets.Count; i++)
            {
                this.m_bokehOffsets[i].BokehRenderTexture = AmplifyUtils.GetTempRenderTarget(source.width, source.height);
            }
            material.SetVector(AmplifyUtils.BokehParamsId, this.m_bokehCameraProperties);
            int num2 = 0;
            while (num2 < this.m_bokehOffsets.Count)
            {
                int index = 0;
                while (true)
                {
                    if (index >= 8)
                    {
                        Graphics.Blit(source, this.m_bokehOffsets[num2].BokehRenderTexture, material, 0x1b);
                        num2++;
                        break;
                    }
                    material.SetVector(AmplifyUtils.AnamorphicGlareWeightsStr[index], this.m_bokehOffsets[num2].Offsets[index]);
                    index++;
                }
            }
            for (int j = 0; j < (this.m_bokehOffsets.Count - 1); j++)
            {
                material.SetTexture(AmplifyUtils.AnamorphicRTS[j], this.m_bokehOffsets[j].BokehRenderTexture);
            }
            source.DiscardContents();
            Graphics.Blit(this.m_bokehOffsets[this.m_bokehOffsets.Count - 1].BokehRenderTexture, source, material, 0x1c + (this.m_bokehOffsets.Count - 2));
            for (int k = 0; k < this.m_bokehOffsets.Count; k++)
            {
                AmplifyUtils.ReleaseTempRenderTarget(this.m_bokehOffsets[k].BokehRenderTexture);
                this.m_bokehOffsets[k].BokehRenderTexture = null;
            }
        }

        private unsafe Vector4[] CalculateBokehSamples(int sampleCount, float angle)
        {
            Vector4[] vectorArray = new Vector4[sampleCount];
            float f = 0.01745329f * angle;
            float num2 = ((float) Screen.width) / ((float) Screen.height);
            Vector4 b = new Vector4(this.m_bokehSampleRadius * Mathf.Cos(f), this.m_bokehSampleRadius * Mathf.Sin(f));
            Vector4* vectorPtr1 = &b;
            vectorPtr1->x /= num2;
            for (int i = 0; i < sampleCount; i++)
            {
                float t = ((float) i) / (sampleCount - 1f);
                vectorArray[i] = Vector4.Lerp(-b, b, t);
            }
            return vectorArray;
        }

        private void CreateBokehOffsets(AmplifyBloom.ApertureShape shape)
        {
            this.m_bokehOffsets.Clear();
            if (shape == AmplifyBloom.ApertureShape.Square)
            {
                this.m_bokehOffsets.Add(new AmplifyBokehData(this.CalculateBokehSamples(8, this.m_offsetRotation)));
                this.m_bokehOffsets.Add(new AmplifyBokehData(this.CalculateBokehSamples(8, this.m_offsetRotation + 90f)));
            }
            else if (shape == AmplifyBloom.ApertureShape.Hexagon)
            {
                this.m_bokehOffsets.Add(new AmplifyBokehData(this.CalculateBokehSamples(8, this.m_offsetRotation)));
                this.m_bokehOffsets.Add(new AmplifyBokehData(this.CalculateBokehSamples(8, this.m_offsetRotation - 75f)));
                this.m_bokehOffsets.Add(new AmplifyBokehData(this.CalculateBokehSamples(8, this.m_offsetRotation + 75f)));
            }
            else if (shape == AmplifyBloom.ApertureShape.Octagon)
            {
                this.m_bokehOffsets.Add(new AmplifyBokehData(this.CalculateBokehSamples(8, this.m_offsetRotation)));
                this.m_bokehOffsets.Add(new AmplifyBokehData(this.CalculateBokehSamples(8, this.m_offsetRotation + 65f)));
                this.m_bokehOffsets.Add(new AmplifyBokehData(this.CalculateBokehSamples(8, this.m_offsetRotation + 90f)));
                this.m_bokehOffsets.Add(new AmplifyBokehData(this.CalculateBokehSamples(8, this.m_offsetRotation + 115f)));
            }
        }

        public void Destroy()
        {
            for (int i = 0; i < this.m_bokehOffsets.Count; i++)
            {
                this.m_bokehOffsets[i].Destroy();
            }
        }

        public void OnAfterDeserialize()
        {
            this.CreateBokehOffsets(this.m_apertureShape);
        }

        public void OnBeforeSerialize()
        {
        }

        public AmplifyBloom.ApertureShape ApertureShape
        {
            get => 
                this.m_apertureShape;
            set
            {
                if (this.m_apertureShape != value)
                {
                    this.m_apertureShape = value;
                    this.CreateBokehOffsets(value);
                }
            }
        }

        public bool ApplyBokeh
        {
            get => 
                this.m_isActive;
            set => 
                this.m_isActive = value;
        }

        public bool ApplyOnBloomSource
        {
            get => 
                this.m_applyOnBloomSource;
            set => 
                this.m_applyOnBloomSource = value;
        }

        public float BokehSampleRadius
        {
            get => 
                this.m_bokehSampleRadius;
            set => 
                this.m_bokehSampleRadius = value;
        }

        public float OffsetRotation
        {
            get => 
                this.m_offsetRotation;
            set => 
                this.m_offsetRotation = value;
        }

        public Vector4 BokehCameraProperties
        {
            get => 
                this.m_bokehCameraProperties;
            set => 
                this.m_bokehCameraProperties = value;
        }

        public float Aperture
        {
            get => 
                this.m_bokehCameraProperties.x;
            set => 
                this.m_bokehCameraProperties.x = value;
        }

        public float FocalLength
        {
            get => 
                this.m_bokehCameraProperties.y;
            set => 
                this.m_bokehCameraProperties.y = value;
        }

        public float FocalDistance
        {
            get => 
                this.m_bokehCameraProperties.z;
            set => 
                this.m_bokehCameraProperties.z = value;
        }

        public float MaxCoCDiameter
        {
            get => 
                this.m_bokehCameraProperties.w;
            set => 
                this.m_bokehCameraProperties.w = value;
        }
    }
}

