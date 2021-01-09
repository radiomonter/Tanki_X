namespace Tanks.Battle.ClientGraphics.API
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [Serializable]
    public class GrassCell
    {
        public List<GrassPosition> grassPositions = new List<GrassPosition>();
        public Vector3 center;
        public int lightmapId;
    }
}

