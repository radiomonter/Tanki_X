namespace Tanks.Battle.ClientGraphics.API
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class DecalProjection
    {
        public DecalProjection()
        {
            int num = 1;
            this.AtlasVTilesCount = num;
            this.AtlasHTilesCount = num;
            this.SurfaceAtlasPositions = new int[5];
            this.Up = Vector3.up;
        }

        public UnityEngine.Ray Ray { get; set; }

        public float HalfSize { get; set; }

        public Vector3 Up { get; set; }

        public float Distantion { get; set; }

        public int AtlasHTilesCount { get; set; }

        public int AtlasVTilesCount { get; set; }

        public int[] SurfaceAtlasPositions { get; set; }

        public RaycastHit ProjectionHit { get; set; }
    }
}

