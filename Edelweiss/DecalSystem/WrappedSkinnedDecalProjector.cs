namespace Edelweiss.DecalSystem
{
    using System;
    using UnityEngine;

    public class WrappedSkinnedDecalProjector : SkinnedDecalProjectorBase
    {
        private Transform m_Transform;
        private SkinnedDecalProjectorComponent m_DecalProjector;

        public WrappedSkinnedDecalProjector(SkinnedDecalProjectorComponent a_DecalProjector)
        {
            this.m_DecalProjector = a_DecalProjector;
            this.m_Transform = this.m_DecalProjector.transform;
        }

        public SkinnedDecalProjectorComponent WrappedSkinnedDecalProjectorComponent =>
            this.m_DecalProjector;

        public override Vector3 Position =>
            this.m_Transform.position;

        public override Quaternion Rotation =>
            this.m_Transform.rotation;

        public override Vector3 Scale =>
            this.m_Transform.localScale;

        public override float CullingAngle =>
            this.m_DecalProjector.cullingAngle;

        public override float MeshOffset =>
            this.m_DecalProjector.meshOffset;

        public override int UV1RectangleIndex =>
            this.m_DecalProjector.uv1RectangleIndex;

        public override int UV2RectangleIndex =>
            this.m_DecalProjector.uv2RectangleIndex;

        public override Color VertexColor =>
            this.m_DecalProjector.vertexColor;

        public override float VertexColorBlending =>
            this.m_DecalProjector.VertexColorBlending;
    }
}

