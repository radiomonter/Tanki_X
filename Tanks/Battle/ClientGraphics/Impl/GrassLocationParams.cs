namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [Serializable]
    public class GrassLocationParams
    {
        public Texture2D uvMask;
        public bool whiteAsEmpty;
        public float blackThreshold;
        public List<GameObject> terrainObjects = new List<GameObject>();
        public float densityPerMeter = 1f;
        public float grassCombineWidth = 20f;

        public float GrassStep =>
            Mathf.Sqrt(1f / this.densityPerMeter) / 2f;
    }
}

