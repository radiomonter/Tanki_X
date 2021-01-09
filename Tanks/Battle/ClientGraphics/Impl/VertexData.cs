namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using System.Runtime.InteropServices;
    using Tanks.Battle.ClientGraphics.API;
    using UnityEngine;

    [StructLayout(LayoutKind.Sequential)]
    public struct VertexData
    {
        public Vector3 vertex;
        public Vector3 normal;
        public SurfaceType surfaceType;
        public VertexData(Vector3 vertex, Vector3 normal, SurfaceType surfaceType)
        {
            this.vertex = vertex;
            this.normal = normal;
            this.surfaceType = surfaceType;
        }

        public static VertexData Lerp(VertexData from, VertexData to, float lerpFactor) => 
            new VertexData { 
                vertex = Vector3.LerpUnclamped(from.vertex, to.vertex, lerpFactor),
                normal = Vector3.LerpUnclamped(from.normal, to.normal, lerpFactor),
                surfaceType = from.surfaceType
            };
    }
}

