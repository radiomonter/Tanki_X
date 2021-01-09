namespace Edelweiss.DecalSystem
{
    using System;
    using UnityEngine;

    public abstract class GenericDecalProjectorBase
    {
        private bool m_IsActiveProjector;
        private int m_DecalsMeshLowerVertexIndex;
        private int m_DecalsMeshUpperVertexIndex;
        private int m_DecalsMeshLowerTriangleIndex;
        private int m_DecalsMeshUpperTriangleIndex;
        private bool m_IsUV1ProjectionCalculated;
        private bool m_IsUV2ProjectionCalculated;
        private bool m_IsTangentProjectionCalculated;

        protected GenericDecalProjectorBase()
        {
        }

        public Bounds WorldBounds()
        {
            Matrix4x4 matrixx = Matrix4x4.TRS(this.Position, this.Rotation, Vector3.one);
            Vector3 vector = (Vector3) (0.5f * this.Scale);
            Vector3 vector2 = new Vector3(0f, -Mathf.Abs(vector.y), 0f);
            Bounds bounds = new Bounds(matrixx.MultiplyPoint3x4(Vector3.zero), Vector3.zero);
            bounds.Encapsulate(matrixx.MultiplyPoint3x4(vector2 + new Vector3(vector.x, vector.y, vector.z)));
            bounds.Encapsulate(matrixx.MultiplyPoint3x4(vector2 + new Vector3(vector.x, vector.y, -vector.z)));
            bounds.Encapsulate(matrixx.MultiplyPoint3x4(vector2 + new Vector3(vector.x, -vector.y, vector.z)));
            bounds.Encapsulate(matrixx.MultiplyPoint3x4(vector2 + new Vector3(vector.x, -vector.y, -vector.z)));
            bounds.Encapsulate(matrixx.MultiplyPoint3x4(vector2 + new Vector3(-vector.x, vector.y, vector.z)));
            bounds.Encapsulate(matrixx.MultiplyPoint3x4(vector2 + new Vector3(-vector.x, vector.y, -vector.z)));
            bounds.Encapsulate(matrixx.MultiplyPoint3x4(vector2 + new Vector3(-vector.x, -vector.y, vector.z)));
            bounds.Encapsulate(matrixx.MultiplyPoint3x4(vector2 + new Vector3(-vector.x, -vector.y, -vector.z)));
            return bounds;
        }

        public bool IsActiveProjector
        {
            get => 
                this.m_IsActiveProjector;
            internal set => 
                this.m_IsActiveProjector = value;
        }

        public int DecalsMeshLowerVertexIndex
        {
            get => 
                this.m_DecalsMeshLowerVertexIndex;
            internal set => 
                this.m_DecalsMeshLowerVertexIndex = value;
        }

        public int DecalsMeshUpperVertexIndex
        {
            get => 
                this.m_DecalsMeshUpperVertexIndex;
            internal set => 
                this.m_DecalsMeshUpperVertexIndex = value;
        }

        public int DecalsMeshVertexCount =>
            (this.DecalsMeshUpperVertexIndex - this.DecalsMeshLowerVertexIndex) + 1;

        public int DecalsMeshLowerTriangleIndex
        {
            get => 
                this.m_DecalsMeshLowerTriangleIndex;
            internal set => 
                this.m_DecalsMeshLowerTriangleIndex = value;
        }

        public int DecalsMeshUpperTriangleIndex
        {
            get => 
                this.m_DecalsMeshUpperTriangleIndex;
            internal set => 
                this.m_DecalsMeshUpperTriangleIndex = value;
        }

        public int DecalsMeshTriangleCount =>
            (this.DecalsMeshUpperTriangleIndex - this.DecalsMeshLowerTriangleIndex) + 1;

        public bool IsUV1ProjectionCalculated
        {
            get => 
                this.m_IsUV1ProjectionCalculated;
            internal set => 
                this.m_IsUV1ProjectionCalculated = value;
        }

        public bool IsUV2ProjectionCalculated
        {
            get => 
                this.m_IsUV2ProjectionCalculated;
            internal set => 
                this.m_IsUV2ProjectionCalculated = value;
        }

        public bool IsTangentProjectionCalculated
        {
            get => 
                this.m_IsTangentProjectionCalculated;
            internal set => 
                this.m_IsTangentProjectionCalculated = value;
        }

        public abstract Vector3 Position { get; }

        public abstract Quaternion Rotation { get; }

        public abstract Vector3 Scale { get; }

        public abstract float CullingAngle { get; }

        public abstract float MeshOffset { get; }

        public abstract int UV1RectangleIndex { get; }

        public abstract int UV2RectangleIndex { get; }

        public abstract Color VertexColor { get; }

        public abstract float VertexColorBlending { get; }

        public Matrix4x4 ProjectorToWorldMatrix =>
            Matrix4x4.TRS(this.Position, this.Rotation, this.Scale);

        public Matrix4x4 WorldToProjectorMatrix =>
            this.ProjectorToWorldMatrix.inverse;
    }
}

