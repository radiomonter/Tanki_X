namespace AmplifyBloom
{
    using System;
    using UnityEngine;

    [Serializable]
    public class GlareDefData
    {
        public bool FoldoutValue;
        [SerializeField]
        private StarLibType m_starType;
        [SerializeField]
        private float m_starInclination;
        [SerializeField]
        private float m_chromaticAberration;
        [SerializeField]
        private StarDefData m_customStarData;

        public GlareDefData()
        {
            this.FoldoutValue = true;
            this.m_customStarData = new StarDefData();
        }

        public GlareDefData(StarLibType starType, float starInclination, float chromaticAberration)
        {
            this.FoldoutValue = true;
            this.m_starType = starType;
            this.m_starInclination = starInclination;
            this.m_chromaticAberration = chromaticAberration;
        }

        public StarLibType StarType
        {
            get => 
                this.m_starType;
            set => 
                this.m_starType = value;
        }

        public float StarInclination
        {
            get => 
                this.m_starInclination;
            set => 
                this.m_starInclination = value;
        }

        public float StarInclinationDeg
        {
            get => 
                this.m_starInclination * 57.29578f;
            set => 
                this.m_starInclination = value * 0.01745329f;
        }

        public float ChromaticAberration
        {
            get => 
                this.m_chromaticAberration;
            set => 
                this.m_chromaticAberration = value;
        }

        public StarDefData CustomStarData
        {
            get => 
                this.m_customStarData;
            set => 
                this.m_customStarData = value;
        }
    }
}

