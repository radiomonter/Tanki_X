﻿namespace AmplifyBloom
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [Serializable]
    public class StarDefData
    {
        [SerializeField]
        private StarLibType m_starType;
        [SerializeField]
        private string m_starName;
        [SerializeField]
        private int m_starlinesCount;
        [SerializeField]
        private int m_passCount;
        [SerializeField]
        private float m_sampleLength;
        [SerializeField]
        private float m_attenuation;
        [SerializeField]
        private float m_inclination;
        [SerializeField]
        private float m_rotation;
        [SerializeField]
        private StarLineData[] m_starLinesArr;
        [SerializeField]
        private float m_customIncrement;
        [SerializeField]
        private float m_longAttenuation;

        public StarDefData()
        {
            this.m_starName = string.Empty;
            this.m_starlinesCount = 2;
            this.m_passCount = 4;
            this.m_sampleLength = 1f;
            this.m_attenuation = 0.85f;
            this.m_customIncrement = 90f;
        }

        public StarDefData(StarLibType starType, string starName, int starLinesCount, int passCount, float sampleLength, float attenuation, float inclination, float rotation, float longAttenuation = 0f, float customIncrement = -1f)
        {
            this.m_starName = string.Empty;
            this.m_starlinesCount = 2;
            this.m_passCount = 4;
            this.m_sampleLength = 1f;
            this.m_attenuation = 0.85f;
            this.m_customIncrement = 90f;
            this.m_starType = starType;
            this.m_starName = starName;
            this.m_passCount = passCount;
            this.m_sampleLength = sampleLength;
            this.m_attenuation = attenuation;
            this.m_starlinesCount = starLinesCount;
            this.m_inclination = inclination;
            this.m_rotation = rotation;
            this.m_customIncrement = customIncrement;
            this.m_longAttenuation = longAttenuation;
            this.CalculateStarData();
        }

        public void CalculateStarData()
        {
            if (this.m_starlinesCount != 0)
            {
                this.m_starLinesArr = new StarLineData[this.m_starlinesCount];
                float num = ((this.m_customIncrement <= 0f) ? (180f / ((float) this.m_starlinesCount)) : this.m_customIncrement) * 0.01745329f;
                for (int i = 0; i < this.m_starlinesCount; i++)
                {
                    this.m_starLinesArr[i] = new StarLineData();
                    this.m_starLinesArr[i].PassCount = this.m_passCount;
                    this.m_starLinesArr[i].SampleLength = this.m_sampleLength;
                    this.m_starLinesArr[i].Attenuation = (this.m_longAttenuation <= 0f) ? this.m_attenuation : (((i % 2) != 0) ? this.m_attenuation : this.m_longAttenuation);
                    this.m_starLinesArr[i].Inclination = num * i;
                }
            }
        }

        public void Destroy()
        {
            this.m_starLinesArr = null;
        }

        public StarLibType StarType
        {
            get => 
                this.m_starType;
            set => 
                this.m_starType = value;
        }

        public string StarName
        {
            get => 
                this.m_starName;
            set => 
                this.m_starName = value;
        }

        public int StarlinesCount
        {
            get => 
                this.m_starlinesCount;
            set
            {
                this.m_starlinesCount = value;
                this.CalculateStarData();
            }
        }

        public int PassCount
        {
            get => 
                this.m_passCount;
            set
            {
                this.m_passCount = value;
                this.CalculateStarData();
            }
        }

        public float SampleLength
        {
            get => 
                this.m_sampleLength;
            set
            {
                this.m_sampleLength = value;
                this.CalculateStarData();
            }
        }

        public float Attenuation
        {
            get => 
                this.m_attenuation;
            set
            {
                this.m_attenuation = value;
                this.CalculateStarData();
            }
        }

        public float Inclination
        {
            get => 
                this.m_inclination;
            set
            {
                this.m_inclination = value;
                this.CalculateStarData();
            }
        }

        public float CameraRotInfluence
        {
            get => 
                this.m_rotation;
            set => 
                this.m_rotation = value;
        }

        public StarLineData[] StarLinesArr =>
            this.m_starLinesArr;

        public float CustomIncrement
        {
            get => 
                this.m_customIncrement;
            set
            {
                this.m_customIncrement = value;
                this.CalculateStarData();
            }
        }

        public float LongAttenuation
        {
            get => 
                this.m_longAttenuation;
            set
            {
                this.m_longAttenuation = value;
                this.CalculateStarData();
            }
        }
    }
}

