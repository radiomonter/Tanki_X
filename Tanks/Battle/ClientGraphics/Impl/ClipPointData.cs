namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [StructLayout(LayoutKind.Sequential)]
    public struct ClipPointData
    {
        public Vector2 point2D;
        public VertexData vertexData;
        public int index;
        public ClipPointData(VertexData vertexData)
        {
            this.point2D = vertexData.vertex;
            this.vertexData = vertexData;
            this.index = -1;
        }

        public ClipPointData ToDepthSpace()
        {
            this.point2D.x = this.vertexData.vertex.z;
            return this;
        }

        public static ClipPointData Lerp(ClipPointData from, ClipPointData to, float lerpFactor) => 
            new ClipPointData { 
                point2D = Vector2.LerpUnclamped(from.point2D, to.point2D, lerpFactor),
                vertexData = VertexData.Lerp(from.vertexData, to.vertexData, lerpFactor)
            };
    }
}

