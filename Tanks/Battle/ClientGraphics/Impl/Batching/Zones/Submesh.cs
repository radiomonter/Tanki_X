namespace Tanks.Battle.ClientGraphics.Impl.Batching.Zones
{
    using System;
    using UnityEngine;

    public sealed class Submesh
    {
        public bool merged;
        public int nearValue;
        public Bounds bounds;
        public Material material;
        public Mesh mesh;
        public int submesh;
        public MeshRenderer renderer;

        public int lightmapIndex =>
            this.renderer.lightmapIndex;
    }
}

