namespace Tanks.Battle.ClientCore.API
{
    using MIConvexHull;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class Vertex : IVertex
    {
        private readonly Vector3 unityVertex;
        private readonly double[] position;

        public Vertex(Vector3 unityVertex)
        {
            this.unityVertex = unityVertex;
            this.position = new double[] { unityVertex.x, unityVertex.y, unityVertex.z };
        }

        public double[] Position =>
            this.position;

        public Vector3 UnityVertex =>
            this.unityVertex;

        public int Index { get; set; }
    }
}

