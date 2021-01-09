namespace Edelweiss.DecalSystem
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class GenericDecalsMeshBase
    {
        protected List<Vector2> m_UVs = new List<Vector2>();
        protected List<Vector2> m_UV2s = new List<Vector2>();
        protected List<Vector3> m_Vertices = new List<Vector3>();
        protected List<Vector3> m_Normals = new List<Vector3>();
        protected List<Vector4> m_Tangents = new List<Vector4>();
        protected List<Color> m_TargetVertexColors = new List<Color>();
        protected List<Color> m_VertexColors = new List<Color>();
        protected List<int> m_Triangles = new List<int>();
        internal RemovedIndices m_RemovedIndices = new RemovedIndices();

        protected GenericDecalsMeshBase()
        {
        }

        protected abstract bool HasUV2LightmappingMode();
        protected abstract void RecalculateTangents();
        protected abstract void RecalculateUV2s();
        protected abstract void RecalculateUVs();

        public List<Vector2> UVs
        {
            get
            {
                this.RecalculateUVs();
                return this.m_UVs;
            }
        }

        public List<Vector2> UV2s
        {
            get
            {
                if (Application.isPlaying && this.HasUV2LightmappingMode())
                {
                    throw new InvalidOperationException("The lightmap for the UV2s can not be recalculated if the application is playing!");
                }
                this.RecalculateUV2s();
                return this.m_UV2s;
            }
        }

        public List<Vector3> Vertices =>
            this.m_Vertices;

        public List<Vector3> Normals =>
            this.m_Normals;

        public List<Vector4> Tangents
        {
            get
            {
                this.RecalculateTangents();
                return this.m_Tangents;
            }
        }

        public List<Color> TargetVertexColors =>
            this.m_TargetVertexColors;

        public List<Color> VertexColors =>
            this.m_VertexColors;

        public List<int> Triangles =>
            this.m_Triangles;
    }
}

