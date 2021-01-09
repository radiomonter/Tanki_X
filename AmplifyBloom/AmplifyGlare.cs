namespace AmplifyBloom
{
    using System;
    using UnityEngine;

    [Serializable]
    public sealed class AmplifyGlare : IAmplifyItem
    {
        public const int MaxLineSamples = 8;
        public const int MaxTotalSamples = 0x10;
        public const int MaxStarLines = 4;
        public const int MaxPasses = 4;
        public const int MaxCustomGlare = 0x20;
        [SerializeField]
        private GlareDefData[] m_customGlareDef;
        [SerializeField]
        private int m_customGlareDefIdx;
        [SerializeField]
        private int m_customGlareDefAmount;
        [SerializeField]
        private bool m_applyGlare = true;
        [SerializeField]
        private Color _overallTint = Color.white;
        [SerializeField]
        private Gradient m_cromaticAberrationGrad;
        [SerializeField]
        private int m_glareMaxPassCount = 4;
        private StarDefData[] m_starDefArr;
        private GlareDefData[] m_glareDefArr;
        private Matrix4x4[] m_weigthsMat;
        private Matrix4x4[] m_offsetsMat;
        private Color m_whiteReference;
        private float m_aTanFoV;
        private AmplifyGlareCache m_amplifyGlareCache;
        [SerializeField]
        private int m_currentWidth;
        [SerializeField]
        private int m_currentHeight;
        [SerializeField]
        private GlareLibType m_currentGlareType;
        [SerializeField]
        private int m_currentGlareIdx;
        [SerializeField]
        private float m_perPassDisplacement = 4f;
        [SerializeField]
        private float m_intensity = 0.17f;
        [SerializeField]
        private float m_overallStreakScale = 1f;
        private bool m_isDirty = true;
        private RenderTexture[] _rtBuffer;

        public AmplifyGlare()
        {
            this.m_currentGlareIdx = (int) this.m_currentGlareType;
            this.m_cromaticAberrationGrad = new Gradient();
            GradientColorKey[] colorKeys = new GradientColorKey[] { new GradientColorKey(Color.white, 0f), new GradientColorKey(Color.blue, 0.25f), new GradientColorKey(Color.green, 0.5f), new GradientColorKey(Color.yellow, 0.75f), new GradientColorKey(Color.red, 1f) };
            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(1f, 0.25f), new GradientAlphaKey(1f, 0.5f), new GradientAlphaKey(1f, 0.75f), new GradientAlphaKey(1f, 1f) };
            this.m_cromaticAberrationGrad.SetKeys(colorKeys, alphaKeys);
            this._rtBuffer = new RenderTexture[0x10];
            this.m_weigthsMat = new Matrix4x4[4];
            this.m_offsetsMat = new Matrix4x4[4];
            this.m_amplifyGlareCache = new AmplifyGlareCache();
            this.m_whiteReference = new Color(0.63f, 0.63f, 0.63f, 0f);
            this.m_aTanFoV = Mathf.Atan(0.3926991f);
            this.m_starDefArr = new StarDefData[] { new StarDefData(StarLibType.Cross, "Cross", 2, 4, 1f, 0.85f, 0f, 0.5f, -1f, 90f), new StarDefData(StarLibType.Cross_Filter, "CrossFilter", 2, 4, 1f, 0.95f, 0f, 0.5f, -1f, 90f), new StarDefData(StarLibType.Snow_Cross, "snowCross", 3, 4, 1f, 0.96f, 0.349f, 0.5f, -1f, -1f), new StarDefData(StarLibType.Vertical, "Vertical", 1, 4, 1f, 0.96f, 0f, 0f, -1f, -1f), new StarDefData(StarLibType.Sunny_Cross, "SunnyCross", 4, 4, 1f, 0.88f, 0f, 0f, 0.95f, 45f) };
            GlareDefData[] dataArray2 = new GlareDefData[9];
            dataArray2[0] = new GlareDefData(StarLibType.Cross, 0f, 0.5f);
            dataArray2[1] = new GlareDefData(StarLibType.Cross_Filter, 0.44f, 0.5f);
            dataArray2[2] = new GlareDefData(StarLibType.Cross_Filter, 1.22f, 1.5f);
            dataArray2[3] = new GlareDefData(StarLibType.Snow_Cross, 0.17f, 0.5f);
            dataArray2[4] = new GlareDefData(StarLibType.Snow_Cross, 0.7f, 1.5f);
            dataArray2[5] = new GlareDefData(StarLibType.Sunny_Cross, 0f, 0.5f);
            dataArray2[6] = new GlareDefData(StarLibType.Sunny_Cross, 0.79f, 1.5f);
            dataArray2[7] = new GlareDefData(StarLibType.Vertical, 1.57f, 0.5f);
            dataArray2[8] = new GlareDefData(StarLibType.Vertical, 0f, 0.5f);
            this.m_glareDefArr = dataArray2;
        }

        public void Destroy()
        {
            for (int i = 0; i < this.m_starDefArr.Length; i++)
            {
                this.m_starDefArr[i].Destroy();
            }
            this.m_glareDefArr = null;
            this.m_weigthsMat = null;
            this.m_offsetsMat = null;
            for (int j = 0; j < this._rtBuffer.Length; j++)
            {
                if (this._rtBuffer[j] != null)
                {
                    AmplifyUtils.ReleaseTempRenderTarget(this._rtBuffer[j]);
                    this._rtBuffer[j] = null;
                }
            }
            this._rtBuffer = null;
            this.m_amplifyGlareCache.Destroy();
            this.m_amplifyGlareCache = null;
        }

        public void OnRenderFromCache(RenderTexture source, RenderTexture dest, Material material, float glareIntensity, float cameraRotation)
        {
            for (int i = 0; i < this.m_amplifyGlareCache.TotalRT; i++)
            {
                this._rtBuffer[i] = AmplifyUtils.GetTempRenderTarget(source.width, source.height);
            }
            int index = 0;
            int num3 = 0;
            while (num3 < this.m_amplifyGlareCache.StarDef.StarlinesCount)
            {
                int num4 = 0;
                while (true)
                {
                    if (num4 >= this.m_amplifyGlareCache.CurrentPassCount)
                    {
                        num3++;
                        break;
                    }
                    this.UpdateMatrixesForPass(material, this.m_amplifyGlareCache.Starlines[num3].Passes[num4].Offsets, this.m_amplifyGlareCache.Starlines[num3].Passes[num4].Weights, glareIntensity, cameraRotation * this.m_amplifyGlareCache.StarDef.CameraRotInfluence);
                    if (num4 == 0)
                    {
                        Graphics.Blit(source, this._rtBuffer[index], material, 2);
                    }
                    else
                    {
                        Graphics.Blit(this._rtBuffer[index - 1], this._rtBuffer[index], material, 2);
                    }
                    index++;
                    num4++;
                }
            }
            for (int j = 0; j < this.m_amplifyGlareCache.StarDef.StarlinesCount; j++)
            {
                material.SetVector(AmplifyUtils.AnamorphicGlareWeightsStr[j], this.m_amplifyGlareCache.AverageWeight);
                int num6 = ((j + 1) * this.m_amplifyGlareCache.CurrentPassCount) - 1;
                material.SetTexture(AmplifyUtils.AnamorphicRTS[j], this._rtBuffer[num6]);
            }
            int pass = (0x13 + this.m_amplifyGlareCache.StarDef.StarlinesCount) - 1;
            dest.DiscardContents();
            Graphics.Blit(this._rtBuffer[0], dest, material, pass);
            for (index = 0; index < this._rtBuffer.Length; index++)
            {
                AmplifyUtils.ReleaseTempRenderTarget(this._rtBuffer[index]);
                this._rtBuffer[index] = null;
            }
        }

        public unsafe void OnRenderImage(Material material, RenderTexture source, RenderTexture dest, float cameraRot)
        {
            Graphics.Blit(Texture2D.blackTexture, dest);
            if (!this.m_isDirty && ((this.m_currentWidth == source.width) && (this.m_currentHeight == source.height)))
            {
                this.OnRenderFromCache(source, dest, material, this.m_intensity, cameraRot);
            }
            else
            {
                this.m_isDirty = false;
                this.m_currentWidth = source.width;
                this.m_currentHeight = source.height;
                GlareDefData data = null;
                bool flag = false;
                if (this.m_currentGlareType != GlareLibType.Custom)
                {
                    data = this.m_glareDefArr[this.m_currentGlareIdx];
                }
                else if ((this.m_customGlareDef == null) || (this.m_customGlareDef.Length <= 0))
                {
                    data = this.m_glareDefArr[0];
                }
                else
                {
                    data = this.m_customGlareDef[this.m_customGlareDefIdx];
                    flag = true;
                }
                this.m_amplifyGlareCache.GlareDef = data;
                float width = source.width;
                float height = source.height;
                StarDefData data2 = !flag ? this.m_starDefArr[(int) data.StarType] : data.CustomStarData;
                this.m_amplifyGlareCache.StarDef = data2;
                int num3 = (this.m_glareMaxPassCount >= data2.PassCount) ? data2.PassCount : this.m_glareMaxPassCount;
                this.m_amplifyGlareCache.CurrentPassCount = num3;
                float num4 = data.StarInclination + data2.Inclination;
                int num5 = 0;
                while (num5 < this.m_glareMaxPassCount)
                {
                    float t = ((float) (num5 + 1)) / ((float) this.m_glareMaxPassCount);
                    int num7 = 0;
                    while (true)
                    {
                        if (num7 >= 8)
                        {
                            num5++;
                            break;
                        }
                        Color b = this._overallTint * Color.Lerp(this.m_cromaticAberrationGrad.Evaluate(((float) num7) / 7f), this.m_whiteReference, t);
                        *(this.m_amplifyGlareCache.CromaticAberrationMat[num5, num7]) = Color.Lerp(this.m_whiteReference, b, data.ChromaticAberration);
                        num7++;
                    }
                }
                this.m_amplifyGlareCache.TotalRT = data2.StarlinesCount * num3;
                for (int i = 0; i < this.m_amplifyGlareCache.TotalRT; i++)
                {
                    this._rtBuffer[i] = AmplifyUtils.GetTempRenderTarget(source.width, source.height);
                }
                int index = 0;
                int num10 = 0;
                while (num10 < data2.StarlinesCount)
                {
                    StarLineData data3 = data2.StarLinesArr[num10];
                    float f = num4 + data3.Inclination;
                    float num12 = Mathf.Sin(f);
                    float num13 = Mathf.Cos(f);
                    Vector2 vector = new Vector2 {
                        x = (num13 / width) * (data3.SampleLength * this.m_overallStreakScale),
                        y = (num12 / height) * (data3.SampleLength * this.m_overallStreakScale)
                    };
                    float num14 = (((this.m_aTanFoV + 0.1f) * 280f) / (width + height)) * 1.2f;
                    int num15 = 0;
                    while (true)
                    {
                        if (num15 >= num3)
                        {
                            num10++;
                            break;
                        }
                        int num16 = 0;
                        while (true)
                        {
                            if (num16 >= 8)
                            {
                                int num18 = 8;
                                while (true)
                                {
                                    if (num18 >= 0x10)
                                    {
                                        this.UpdateMatrixesForPass(material, this.m_amplifyGlareCache.Starlines[num10].Passes[num15].Offsets, this.m_amplifyGlareCache.Starlines[num10].Passes[num15].Weights, this.m_intensity, data2.CameraRotInfluence * cameraRot);
                                        if (num15 == 0)
                                        {
                                            Graphics.Blit(source, this._rtBuffer[index], material, 2);
                                        }
                                        else
                                        {
                                            Graphics.Blit(this._rtBuffer[index - 1], this._rtBuffer[index], material, 2);
                                        }
                                        index++;
                                        vector *= this.m_perPassDisplacement;
                                        num14 *= this.m_perPassDisplacement;
                                        num15++;
                                        break;
                                    }
                                    this.m_amplifyGlareCache.Starlines[num10].Passes[num15].Offsets[num18] = -this.m_amplifyGlareCache.Starlines[num10].Passes[num15].Offsets[num18 - 8];
                                    this.m_amplifyGlareCache.Starlines[num10].Passes[num15].Weights[num18] = this.m_amplifyGlareCache.Starlines[num10].Passes[num15].Weights[num18 - 8];
                                    num18++;
                                }
                                break;
                            }
                            float num17 = Mathf.Pow(data3.Attenuation, num14 * num16);
                            this.m_amplifyGlareCache.Starlines[num10].Passes[num15].Weights[num16] = ((this.m_amplifyGlareCache.CromaticAberrationMat[(num3 - 1) - num15, num16] * num17) * (num15 + 1f)) * 0.5f;
                            this.m_amplifyGlareCache.Starlines[num10].Passes[num15].Offsets[num16].x = vector.x * num16;
                            this.m_amplifyGlareCache.Starlines[num10].Passes[num15].Offsets[num16].y = vector.y * num16;
                            if ((Mathf.Abs(this.m_amplifyGlareCache.Starlines[num10].Passes[num15].Offsets[num16].x) >= 0.9f) || (Mathf.Abs(this.m_amplifyGlareCache.Starlines[num10].Passes[num15].Offsets[num16].y) >= 0.9f))
                            {
                                this.m_amplifyGlareCache.Starlines[num10].Passes[num15].Offsets[num16].x = 0f;
                                this.m_amplifyGlareCache.Starlines[num10].Passes[num15].Offsets[num16].y = 0f;
                                Vector4* vectorPtr1 = &(this.m_amplifyGlareCache.Starlines[num10].Passes[num15].Weights[num16]);
                                vectorPtr1[0] *= 0f;
                            }
                            num16++;
                        }
                    }
                }
                this.m_amplifyGlareCache.AverageWeight = Vector4.one / ((float) data2.StarlinesCount);
                for (int j = 0; j < data2.StarlinesCount; j++)
                {
                    material.SetVector(AmplifyUtils.AnamorphicGlareWeightsStr[j], this.m_amplifyGlareCache.AverageWeight);
                    int num20 = ((j + 1) * num3) - 1;
                    material.SetTexture(AmplifyUtils.AnamorphicRTS[j], this._rtBuffer[num20]);
                }
                int pass = (0x13 + data2.StarlinesCount) - 1;
                dest.DiscardContents();
                Graphics.Blit(this._rtBuffer[0], dest, material, pass);
                for (index = 0; index < this._rtBuffer.Length; index++)
                {
                    AmplifyUtils.ReleaseTempRenderTarget(this._rtBuffer[index]);
                    this._rtBuffer[index] = null;
                }
            }
        }

        public void SetDirty()
        {
            this.m_isDirty = true;
        }

        public void UpdateMatrixesForPass(Material material, Vector4[] offsets, Vector4[] weights, float glareIntensity, float rotation)
        {
            float num = Mathf.Cos(rotation);
            float num2 = Mathf.Sin(rotation);
            for (int i = 0; i < 0x10; i++)
            {
                int index = i >> 2;
                int num5 = i & 3;
                this.m_offsetsMat[index][num5, 0] = (offsets[i].x * num) - (offsets[i].y * num2);
                this.m_offsetsMat[index][num5, 1] = (offsets[i].x * num2) + (offsets[i].y * num);
                this.m_weigthsMat[index][num5, 0] = glareIntensity * weights[i].x;
                this.m_weigthsMat[index][num5, 1] = glareIntensity * weights[i].y;
                this.m_weigthsMat[index][num5, 2] = glareIntensity * weights[i].z;
            }
            for (int j = 0; j < 4; j++)
            {
                material.SetMatrix(AmplifyUtils.AnamorphicGlareOffsetsMatStr[j], this.m_offsetsMat[j]);
                material.SetMatrix(AmplifyUtils.AnamorphicGlareWeightsMatStr[j], this.m_weigthsMat[j]);
            }
        }

        public GlareLibType CurrentGlare
        {
            get => 
                this.m_currentGlareType;
            set
            {
                if (this.m_currentGlareType != value)
                {
                    this.m_currentGlareType = value;
                    this.m_currentGlareIdx = (int) value;
                    this.m_isDirty = true;
                }
            }
        }

        public int GlareMaxPassCount
        {
            get => 
                this.m_glareMaxPassCount;
            set
            {
                this.m_glareMaxPassCount = value;
                this.m_isDirty = true;
            }
        }

        public float PerPassDisplacement
        {
            get => 
                this.m_perPassDisplacement;
            set
            {
                this.m_perPassDisplacement = value;
                this.m_isDirty = true;
            }
        }

        public float Intensity
        {
            get => 
                this.m_intensity;
            set
            {
                this.m_intensity = (value >= 0f) ? value : 0f;
                this.m_isDirty = true;
            }
        }

        public Color OverallTint
        {
            get => 
                this._overallTint;
            set
            {
                this._overallTint = value;
                this.m_isDirty = true;
            }
        }

        public bool ApplyLensGlare
        {
            get => 
                this.m_applyGlare;
            set => 
                this.m_applyGlare = value;
        }

        public Gradient CromaticColorGradient
        {
            get => 
                this.m_cromaticAberrationGrad;
            set
            {
                this.m_cromaticAberrationGrad = value;
                this.m_isDirty = true;
            }
        }

        public float OverallStreakScale
        {
            get => 
                this.m_overallStreakScale;
            set
            {
                this.m_overallStreakScale = value;
                this.m_isDirty = true;
            }
        }

        public GlareDefData[] CustomGlareDef
        {
            get => 
                this.m_customGlareDef;
            set => 
                this.m_customGlareDef = value;
        }

        public int CustomGlareDefIdx
        {
            get => 
                this.m_customGlareDefIdx;
            set => 
                this.m_customGlareDefIdx = value;
        }

        public int CustomGlareDefAmount
        {
            get => 
                this.m_customGlareDefAmount;
            set
            {
                if (value != this.m_customGlareDefAmount)
                {
                    if (value == 0)
                    {
                        this.m_customGlareDef = null;
                        this.m_customGlareDefIdx = 0;
                        this.m_customGlareDefAmount = 0;
                    }
                    else
                    {
                        GlareDefData[] dataArray = new GlareDefData[value];
                        int index = 0;
                        while (true)
                        {
                            if (index >= value)
                            {
                                this.m_customGlareDefIdx = Mathf.Clamp(this.m_customGlareDefIdx, 0, value - 1);
                                this.m_customGlareDef = dataArray;
                                dataArray = null;
                                this.m_customGlareDefAmount = value;
                                break;
                            }
                            dataArray[index] = (index >= this.m_customGlareDefAmount) ? new GlareDefData() : this.m_customGlareDef[index];
                            index++;
                        }
                    }
                }
            }
        }
    }
}

