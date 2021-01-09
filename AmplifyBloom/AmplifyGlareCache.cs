namespace AmplifyBloom
{
    using System;
    using UnityEngine;

    [Serializable]
    public class AmplifyGlareCache
    {
        [SerializeField]
        internal AmplifyStarlineCache[] Starlines = new AmplifyStarlineCache[4];
        [SerializeField]
        internal Vector4 AverageWeight;
        [SerializeField]
        internal Vector4[,] CromaticAberrationMat = new Vector4[4, 8];
        [SerializeField]
        internal int TotalRT;
        [SerializeField]
        internal GlareDefData GlareDef;
        [SerializeField]
        internal StarDefData StarDef;
        [SerializeField]
        internal int CurrentPassCount;

        public AmplifyGlareCache()
        {
            for (int i = 0; i < 4; i++)
            {
                this.Starlines[i] = new AmplifyStarlineCache();
            }
        }

        public void Destroy()
        {
            for (int i = 0; i < 4; i++)
            {
                this.Starlines[i].Destroy();
            }
            this.Starlines = null;
            this.CromaticAberrationMat = null;
        }
    }
}

